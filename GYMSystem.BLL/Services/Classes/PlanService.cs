using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystemG01.BLL.Services.Interfaces;
using GymManagementSystemG01.BLL.ViewModels.PlanViewModel;
using GymManagementSystemG01.BLL.ViewModels.PlanViewModels;
using GymManagementSystemG01.DAL.Repositories.Interfaces;
using GYMSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagementSystemG01.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IGenericRepository<Plan> _planRepository;
        private readonly IGenericRepository<MemberShip> _membershipRepository;

        public PlanService(
            IGenericRepository<Plan> planRepository,
            IGenericRepository<MemberShip> membershipRepository)
        {
            _planRepository = planRepository;
            _membershipRepository = membershipRepository;
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var plans = await _planRepository.GetALLAsync(ct: ct);

            return plans.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                DurationDays = p.DurationInDays,
                IsActive = p.IsActive
            });
        }

        public async Task<PlanViewModel?> GetPlanByIdAsync(int planId, CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(planId, ct);

            if (plan is null) return null;

            return new PlanViewModel
            {
                Id = plan.Id,
                Name = plan.Name,
                Price = plan.Price,
                DurationDays = plan.DurationInDays,
                IsActive = plan.IsActive
            };
        }

        public async Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int planId, CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(planId, ct);

            if (plan is null || !plan.IsActive) return null;

            var hasActiveMemberships = await _membershipRepository.AnyAsync(
                m => m.PlanId == planId && m.EndDate > DateTime.Now, ct);

            if (hasActiveMemberships) return null;

            return new UpdatePlanViewModel
            {
                PlanName = plan.Name,
                Price = plan.Price,
                DurationDays = plan.DurationInDays
            };
        }

        public async Task<bool> ToggleActivationAsync(int planId, CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(planId, ct);

            if (plan is null) return false;

            var hasActiveMemberships = await _membershipRepository.AnyAsync(
                m => m.PlanId == planId && m.EndDate > DateTime.Now, ct);

            if (plan.IsActive && hasActiveMemberships)
                return false;

            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.Now;

            var result = await _planRepository.UpdateAsync(plan);
            return result > 0;
        }

        public async Task<bool> UpdatePlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAsync(id, ct);

            if (plan is null) return false;

            var hasActiveMemberships = await _membershipRepository.AnyAsync(
                m => m.PlanId == id && m.EndDate > DateTime.Now, ct);

            if (hasActiveMemberships)
                return false;

            plan.Name = model.PlanName;
            plan.Price = model.Price;
            plan.DurationInDays = model.DurationDays;
            plan.UpdatedAt = DateTime.Now;

            var result = await _planRepository.UpdateAsync(plan);
            return result > 0;
        }
    }
}