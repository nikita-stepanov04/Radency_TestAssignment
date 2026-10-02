using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Radency_TestAssignment.Application.DTOs;
using Radency_TestAssignment.Application.IBusinessServices;
using Radency_TestAssignment.Domain.Applications;
using Radency_TestAssignment.Domain.Entities.Applications;
using Radency_TestAssignment.Domain.Entities.Identity;
using Radency_TestAssignment.Domain.Enums;
using Radency_TestAssignment.Domain.Pagination;
using Radency_TestAssignment.Infrastructure.IRepositories;

namespace Radency_TestAssignment.Application.BusinessServices
{
    public class ApplicationBS(
        IMapper _mapper,
        IApplicationRepository _applicationRepository,
        IUnitRepository _unitRepository,
        UserManager<User> _userManager) : IApplicationBS
    {
        private const string NotFoundMessage = "Application was not found.";
        private const string NotEditableMessage = "This application can no longer be edited.";
        private const string StaleMessage = "This section was changed by someone else. Reload the page.";
        private const string LeasedMessage = "The unit currently has an active lease.";


        public async Task<OpRes<int>> StartAsync(int unitID, int userID)
        {
            var unit = await _unitRepository.GetByIDAsync(unitID);
            if (unit == null)
                return OpRes.Err<int>("Unit was not found.");

            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            if (await _applicationRepository.HasActiveLeaseAsync(unitID, today))
                return OpRes.Err<int>("This unit is not available.");

            var existing = await _applicationRepository.FindDraftAsync(unitID, userID);
            if (existing != null)
                return OpRes.Success(existing.ID);

            var user = await _userManager.FindByIdAsync(userID.ToString());
            if (user == null)
                return OpRes.Err<int>("User was not found.");

            var application = new RentalApplication
            {
                UnitID = unitID,
                CreatedBy = user,
                CreatedAtUtc = DateTime.UtcNow,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                SectionStates = new List<ApplicationSectionState>
                {
                    new ApplicationSectionState { Section = ApplicationSection.ApplicantInformation },
                    new ApplicationSectionState { Section = ApplicationSection.ResidenceHistory }
                }
            };
            application.Applicants.Add(user);

            await _applicationRepository.AddAsync(application);
            await _applicationRepository.SaveChangesAsync();

            return OpRes.Success(application.ID);
        }

        public Task<ApplicationAccess?> GetAccessAsync(int id, int userID)
        {
            return _applicationRepository.GetAccessAsync(id, userID);
        }

        public async Task<ApplicationWizardDTO?> GetWizardAsync(int id, int userID, ApplicationStep step)
        {
            var application = await _applicationRepository.GetForEditAsync(id, userID);
            if (application == null) return null;

            var model = new ApplicationWizardDTO { ID = application.ID, Step = step };
            ApplyContext(model, application);

            model.ApplicantInformation.Version = GetState(application, ApplicationSection.ApplicantInformation).Version;
            model.ApplicantInformation.FullName = application.FullName;
            model.ApplicantInformation.Phone = application.Phone;
            model.ApplicantInformation.Email = application.Email;
            model.ApplicantInformation.CurrentAddress = application.CurrentAddress;

            model.ResidenceHistory.Version = GetState(application, ApplicationSection.ResidenceHistory).Version;

            if (step == ApplicationStep.Summary)
                model.Summary = await BuildSummaryAsync(application);

            return model;
        }

        public async Task FillContextAsync(ApplicationWizardDTO model, int userID)
        {
            var application = await _applicationRepository.GetForEditAsync(model.ID, userID);
            if (application == null) return;

            ApplyContext(model, application);
        }

        public Task<OpRes<bool>> SaveApplicantInfoAsync(int id, int userID, ApplicantInfoSectionDTO dto)
        {
            return SaveSectionAsync(id, userID, ApplicationSection.ApplicantInformation, dto.Version, application =>
            {
                application.FullName = dto.FullName;
                application.Phone = dto.Phone;
                application.Email = dto.Email;
                application.CurrentAddress = dto.CurrentAddress;
                return null;
            });
        }

        public Task<OpRes<bool>> SaveResidenceHistoryAsync(int id, int userID, Guid version)
        {
            return SaveSectionAsync(id, userID, ApplicationSection.ResidenceHistory, version, application =>
            {
                var errors = ApplicationRules.ValidateResidences(application.Residences);
                return errors.Count == 0 ? null : string.Join(" ", errors);
            });
        }

        public async Task<OpRes<bool>> SubmitAsync(int id, int userID)
        {
            var application = await _applicationRepository.GetForEditAsync(id, userID);
            if (application == null)
                return OpRes.Err<bool>(NotFoundMessage);

            if (!ApplicationRules.CanEdit(application.Status))
                return OpRes.Err<bool>(NotEditableMessage);

            if (!ApplicationRules.AllSectionsSaved(application.SectionStates))
                return OpRes.Err<bool>("Complete and save all sections before submitting.");

            if (await HasActiveLeaseAsync(application.UnitID))
                return OpRes.Err<bool>(LeasedMessage);

            application.Status = ApplicationStatus.Submitted;
            application.SubmittedAtUtc = DateTime.UtcNow;

            await _applicationRepository.SaveChangesAsync();
            return OpRes.Success(true);
        }

        public async Task<PagedResult<ApplicationListItemDTO>> GetListAsync(
            ApplicationListFilterDTO filter, int userID, bool isManager)
        {
            var query = new ApplicationListQuery
            {
                Status = filter.Status,
                PropertyID = filter.PropertyID
            };

            if (isManager)
                query.ExcludeDrafts = true;
            else
                query.ApplicantID = userID;

            var paged = await _applicationRepository.GetListAsync(query, new PageRequest(filter.Page, filter.PageSize));

            return new PagedResult<ApplicationListItemDTO>(
                _mapper.Map<List<ApplicationListItemDTO>>(paged.Items),
                paged.TotalCount,
                paged.Page,
                paged.PageSize);
        }

        private async Task<SummaryDTO> BuildSummaryAsync(RentalApplication application)
        {
            var summary = new SummaryDTO();

            foreach (var state in application.SectionStates.Where(s => !s.IsSaved))
                summary.BlockingReasons.Add($"{SectionTitle(state.Section)} has not been saved.");

            if (await HasActiveLeaseAsync(application.UnitID))
                summary.BlockingReasons.Add(LeasedMessage);

            summary.CanSubmit = ApplicationRules.CanEdit(application.Status) && summary.BlockingReasons.Count == 0;
            return summary;
        }

        private Task<bool> HasActiveLeaseAsync(int unitID)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            return _applicationRepository.HasActiveLeaseAsync(unitID, today);
        }

        private static string SectionTitle(ApplicationSection section)
        {
            return section switch
            {
                ApplicationSection.ApplicantInformation => "Applicant information",
                ApplicationSection.ResidenceHistory => "Residence history",
                _ => section.ToString()
            };
        }

        private async Task<OpRes<bool>> SaveSectionAsync(
            int id,
            int userID,
            ApplicationSection section,
            Guid version,
            Func<RentalApplication, string?> apply)
        {
            var application = await _applicationRepository.GetForEditAsync(id, userID);
            if (application == null)
                return OpRes.Err<bool>(NotFoundMessage);

            if (!ApplicationRules.CanEdit(application.Status))
                return OpRes.Err<bool>(NotEditableMessage);

            var state = GetState(application, section);
            if (state.Version != version)
                return OpRes.Err<bool>(StaleMessage);

            var error = apply(application);
            if (error != null)
                return OpRes.Err<bool>(error);

            state.IsSaved = true;
            state.Version = Guid.NewGuid();
            state.UpdatedAtUtc = DateTime.UtcNow;
            state.UpdatedByID = userID;

            return await _applicationRepository.TrySaveChangesAsync()
                ? OpRes.Success(true)
                : OpRes.Err<bool>(StaleMessage);
        }

        private static void ApplyContext(ApplicationWizardDTO model, RentalApplication application)
        {
            model.IsEditable = ApplicationRules.CanEdit(application.Status);
            model.Status = application.Status;
            model.UnitTitle = $"{application.Unit.Property.Name}, unit {application.Unit.UnitNumber}";
            model.SavedSteps = new List<bool>
            {
                GetState(application, ApplicationSection.ApplicantInformation).IsSaved,
                GetState(application, ApplicationSection.ResidenceHistory).IsSaved,
                false
            };

            model.ApplicantInformation.IsReadOnly = !model.IsEditable;
            model.ResidenceHistory.IsReadOnly = !model.IsEditable;
            model.ResidenceHistory.ApplicationID = application.ID;
        }

        private static ApplicationSectionState GetState(RentalApplication application, ApplicationSection section)
        {
            return application.SectionStates.Single(s => s.Section == section);
        }
    }
}
