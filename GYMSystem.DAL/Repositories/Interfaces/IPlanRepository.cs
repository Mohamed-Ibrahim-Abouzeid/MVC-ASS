using GYMSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYMSystem.DAL.Repositories.Interfaces
{
    public interface IPlanRepository
    {
        Task<IEnumerable<Plan>> GetAllAsync(bool tracking =false,CancellationToken ct =default);
        Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default);
        public Task<int> AddAsync(Plan plan);
        public Task<int> UpdateAsync(Plan plan);
        public Task<int> DeleteAsync(Plan plan);

    }
}
