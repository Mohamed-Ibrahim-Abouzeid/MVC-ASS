using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystemG01.BLL.Services.Interfaces;
using GymManagementSystemG01.BLL.ViewModels.TrainerViewModels;
using GymManagementSystemG01.DAL.Repositories.Interfaces;
using GYMSystem.DAL.Models;

namespace GymManagementSystemG01.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IGenericRepository<Trainer> _trainerRepository;
        private readonly IGenericRepository<Session> _sessionRepository;

        public TrainerService(
            IGenericRepository<Trainer> trainerRepository,
            IGenericRepository<Session> sessionRepository)
        {
            _trainerRepository = trainerRepository;
            _sessionRepository = sessionRepository;
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var trainers = await _trainerRepository.GetALLAsync(ct: ct);

            return trainers.Select(t => new TrainerViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Specialties = t.Specialities.ToString()   // FIXED
            });
        }

        public async Task<TrainerViewModel?> GetTrainerDetailsAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _trainerRepository.GetByIdAsync(trainerId, ct);

            if (trainer is null) return null;

            return new TrainerViewModel
            {
                Id = trainer.Id,
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialties = trainer.Specialities.ToString()  // FIXED
            };
        }

        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            var emailExists = await _trainerRepository.AnyAsync(t => t.Email == model.Email, ct);
            var phoneExists = await _trainerRepository.AnyAsync(t => t.Phone == model.Phone, ct);

            if (emailExists || phoneExists)
                return false;

            var trainer = new Trainer
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Gender = model.Gender,
                Specialities = model.Specialties,
                CreatedAt = DateTime.Now,

                Address = new Address
                {
                    BuildingNumber = model.BuildingNumber,
                    Street = model.Street,
                    City = model.City
                }
            };
            var result = await _trainerRepository.AddAsync(trainer);
            return result > 0;
        }

        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _trainerRepository.GetByIdAsync(trainerId, ct);

            if (trainer is null) return null;

            return new TrainerToUpdateViewModel
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialties = trainer.Specialities,

                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.City
            };
        }

        public async Task<bool> RemoveTrainerAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await _trainerRepository.GetByIdAsync(trainerId, ct);

            if (trainer is null)
                return false;

            var hasFutureSessions = await _sessionRepository.AnyAsync(
                s => s.TrainerId == trainerId && s.StartDate > DateTime.Now, ct);

            if (hasFutureSessions)
                return false;

            var result = await _trainerRepository.DeleteAsync(trainer);
            return result > 0;
        }

        public async Task<bool> UpdateTrainerDetailsAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var trainer = await _trainerRepository.GetByIdAsync(trainerId, ct);

            if (trainer is null)
                return false;

            var emailExists = await _trainerRepository.AnyAsync(
                t => t.Email == model.Email && t.Id != trainerId, ct);

            var phoneExists = await _trainerRepository.AnyAsync(
                t => t.Phone == model.Phone && t.Id != trainerId, ct);

            if (emailExists || phoneExists)
                return false;

            trainer.Name = model.Name;
            trainer.Email = model.Email;
            trainer.Phone = model.Phone;
            trainer.Specialities = model.Specialties; 
            trainer.UpdatedAt = DateTime.Now;

            var result = await _trainerRepository.UpdateAsync(trainer);
            return result > 0;
        }
    }
}