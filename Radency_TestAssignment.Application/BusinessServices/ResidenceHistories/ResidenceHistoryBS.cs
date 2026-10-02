using AutoMapper;
using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Application.IBusinessServices;
using Radency_TestAssignment.Domain.Applications;
using Radency_TestAssignment.Domain.Entities.Applications;
using Radency_TestAssignment.Domain.Enums;
using Radency_TestAssignment.Infrastructure.IRepositories;

namespace Radency_TestAssignment.Application.BusinessServices
{
    public class ResidenceHistoryBS(
        IMapper _mapper,
        IApplicationRepository _applicationRep,
        IResidenceHistoryRepository _residenceHistoryRep) : IResidenceHistoryBS
    {
        private const string ApplicationNotFound = "Application was not found.";
        private const string ResidenceNotFound = "Residence was not found.";
        private const string NotEditable = "This application can no longer be edited.";

        public async Task<OpRes<List<ResidenceDTO>>> GetListAsync(int applicationID, int userID)
        {
            var access = await _applicationRep.GetAccessAsync(applicationID, userID);
            if (access == null)
                return OpRes.Err<List<ResidenceDTO>>(ApplicationNotFound);

            var residences = await _residenceHistoryRep.GetByApplicationAsync(applicationID);

            return OpRes.Success(_mapper.Map<List<ResidenceDTO>>(residences));
        }

        public async Task<OpRes<ResidenceDTO>> GetForEditAsync(int id, int userID)
        {
            var loaded = await LoadResidenceAsync(id, userID);
            if (loaded.HasError)
                return OpRes.Err<ResidenceDTO>(loaded.ErrorMessage);

            return OpRes.Success(_mapper.Map<ResidenceDTO>(loaded.Result.Residence));
        }

        public async Task<OpRes<bool>> CreateAsync(int userID, ResidenceDTO dto)
        {
            var loaded = await LoadEditableApplicationAsync(dto.ApplicationID, userID);
            if (loaded.HasError)
                return OpRes.Err<bool>(loaded.ErrorMessage);

            var application = loaded.Result;

            var residence = _mapper.Map<ResidenceHistory>(dto);
            application.Residences.Add(residence);

            MarkSectionUnsaved(application, userID);
            await _applicationRep.SaveChangesAsync();

            return OpRes.Success(true);
        }

        public async Task<OpRes<bool>> UpdateAsync(int userID, ResidenceDTO dto)
        {
            var loaded = await LoadResidenceAsync(dto.ID, userID);
            if (loaded.HasError)
                return OpRes.Err<bool>(loaded.ErrorMessage);

            _mapper.Map(dto, loaded.Result.Residence);

            MarkSectionUnsaved(loaded.Result.Application, userID);
            await _applicationRep.SaveChangesAsync();

            return OpRes.Success(true);
        }

        public async Task<OpRes<bool>> DeleteAsync(int userID, int id)
        {
            var loaded = await LoadResidenceAsync(id, userID);
            if (loaded.HasError)
                return OpRes.Err<bool>(loaded.ErrorMessage);

            _residenceHistoryRep.Delete(loaded.Result.Residence);

            MarkSectionUnsaved(loaded.Result.Application, userID);
            await _applicationRep.SaveChangesAsync();

            return OpRes.Success(true);
        }

        private async Task<OpRes<RentalApplication>> LoadEditableApplicationAsync(int applicationID, int userID)
        {
            var application = await _applicationRep.GetForEditAsync(applicationID, userID);
            if (application == null)
                return OpRes.Err<RentalApplication>(ApplicationNotFound);

            if (!ApplicationRules.CanEdit(application.Status))
                return OpRes.Err<RentalApplication>(NotEditable);

            return OpRes.Success(application);
        }

        private async Task<OpRes<(RentalApplication Application, ResidenceHistory Residence)>> LoadResidenceAsync(int id, int userID)
        {
            var applicationID = await _residenceHistoryRep.GetApplicationIDAsync(id);
            if (applicationID == null)
                return OpRes.Err<(RentalApplication, ResidenceHistory)>(ResidenceNotFound);

            var loaded = await LoadEditableApplicationAsync(applicationID.Value, userID);
            if (loaded.HasError)
                return OpRes.Err<(RentalApplication, ResidenceHistory)>(loaded.ErrorMessage);

            var residence = loaded.Result.Residences.SingleOrDefault(r => r.ID == id);
            if (residence == null)
                return OpRes.Err<(RentalApplication, ResidenceHistory)>(ResidenceNotFound);

            return OpRes.Success((loaded.Result, residence));
        }

        private void MarkSectionUnsaved(RentalApplication application, int userID)
        {
            var state = application.SectionStates.Single(s => s.Section == ApplicationSection.ResidenceHistory);
            state.IsSaved = false;
            state.UpdatedAtUtc = DateTime.UtcNow;
            state.UpdatedByID = userID;
        }
    }
}
