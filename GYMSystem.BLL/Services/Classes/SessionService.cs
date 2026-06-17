using AutoMapper;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using GymManagementSystemG01.BLL.Common;
using GymManagementSystemG01.BLL.Services.Interfaces;
using GymManagementSystemG01.BLL.ViewModels.SessionViewModels;
using GymManagementSystemG01.DAL.Repositories.Interfaces;
using GYMSystem.DAL.Models;
using GYMSystem.DAL.Models.Enums;

namespace GymManagementSystemG01.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SessionViewModel>?> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var sessions = await _unitOfWork.SessionRepository
                .GetAllSessionsWithTrainerAndCategoryAsync(ct: ct);

            if (sessions?.Any() != true)
                return null;

            var mapped = _mapper.Map<List<SessionViewModel>>(sessions);

            foreach (var session in mapped)
            {
                session.AvailableSlots =
                    session.Capacity -
                    await _unitOfWork.SessionRepository
                        .GetCountOfBookedSlotsAsync(session.Id, ct);
            }

            return mapped;
        }

        public async Task<SessionViewModel?> GetSessionByIdAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository
                .GetSessionWithTrainerAndCategoryAsync(sessionId, ct);

            if (session is null)
                return null;

            var mapped = _mapper.Map<SessionViewModel>(session);

            mapped.AvailableSlots =
                mapped.Capacity -
                await _unitOfWork.SessionRepository
                    .GetCountOfBookedSlotsAsync(session.Id, ct);

            return mapped;
        }

        public async Task<UpdateSessionViewModel?> GetSessionToUpdateAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.GetRepository<Session>()
                .GetByIdAsync(sessionId, ct);

            if (session is null)
                return null;

            if (!await IsSessionValidForUpdatingAsync(session, ct))
                return null;

            return _mapper.Map<UpdateSessionViewModel>(session);
        }

        private async Task<bool> IsSessionValidForUpdatingAsync(Session session, CancellationToken ct = default)
        {
            if (session.StartDate <= DateTime.Now)
                return false;

            var booked = await _unitOfWork.SessionRepository
                .GetCountOfBookedSlotsAsync(session.Id, ct);

            return booked == 0;
        }

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate)
                return Result.Validation("End Date Must Be After Start Date");

            if (model.StartDate <= DateTime.Now)
                return Result.Validation("Start Date Must Be In The Future");

            var trainer = await _unitOfWork.GetRepository<Trainer>()
                .GetByIdAsync(model.TrainerId, ct);

            if (trainer is null)
                return Result.NotFound("Trainer Not Found");

            var category = await _unitOfWork.GetRepository<Category>()
                .GetByIdAsync(model.CategoryId, ct);

            if (category is null)
                return Result.NotFound("Category Not Found");

            var isValid = Enum.TryParse<Specialities>(category.CategoryName, true, out var spec);

            if (!isValid || trainer.Specialities != spec)
                return Result.Validation("Trainer Speciality Does Not Match Session Category");

            var session = _mapper.Map<Session>(model);

            await _unitOfWork.GetRepository<Session>()
                .AddAsync(session);

            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Ok();
        }

        public async Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Session>();

            var session = await repo.GetByIdAsync(id, ct);

            if (session is null)
                return Result.NotFound("Session Not Found");

            if (session.StartDate <= DateTime.Now)
                return Result.Validation("Cannot Update Session That Has Already Started");

            var booked = await _unitOfWork.SessionRepository
                .GetCountOfBookedSlotsAsync(id, ct);

            if (booked > 0)
                return Result.Validation("Cannot Update Session With Bookings");

            var trainer = await _unitOfWork.GetRepository<Trainer>()
                .GetByIdAsync(model.TrainerId, ct);

            if (trainer is null)
                return Result.NotFound("Trainer Not Found");

            var category = await _unitOfWork.GetRepository<Category>()
                .GetByIdAsync(session.CategoryId, ct);

            if (category is null)
                return Result.NotFound("Category Not Found");

            var isValid = Enum.TryParse<Specialities>(category.CategoryName, true, out var spec);

            if (!isValid || trainer.Specialities != spec)
                return Result.Validation("Trainer Speciality Does Not Match Category");

            session.StartDate = model.StartDate;
            session.EndDate = model.EndDate;
            session.TrainerId = model.TrainerId;
            session.UpdatedAt = DateTime.Now;

            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Ok();
        }

        public async Task<Result> RemoveSessionAsync(int id, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Session>();

            var session = await repo.GetByIdAsync(id, ct);

            if (session is null)
                return Result.NotFound("Session Not Found");

            if (session.EndDate >= DateTime.Now)
                return Result.Validation("Session Not Ended Yet");

            var booked = await _unitOfWork.SessionRepository
                .GetCountOfBookedSlotsAsync(id, ct);

            if (booked > 0)
                return Result.Validation("Has Bookings");

            await repo.DeleteAsync(session);

            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Ok();
        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct = default)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>()
                .GetALLAsync(ct: ct);

            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken ct = default)
        {
            var categories = await _unitOfWork.GetRepository<Category>()
                .GetALLAsync(ct: ct);

            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(categories);
        }
    }
}