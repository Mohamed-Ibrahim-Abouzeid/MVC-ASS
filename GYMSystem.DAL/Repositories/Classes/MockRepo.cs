using GYMSystem.DAL.Models;
using GYMSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYMSystem.DAL.Repositories.Classes
{
    public class MockRepo : IPlanRepository
    {
        public Task<int> DeleteAsync(Plan plan)
        {
            throw new NotImplementedException();
        }

        Task<int> IPlanRepository.AddAsync(Plan plan)
        {
            throw new NotImplementedException();
        }

     

        async Task<IEnumerable<Plan>> IPlanRepository.GetAllAsync(bool tracking, CancellationToken ct)
        {
            List<Plan> plans = new List<Plan>() {
            new(){ Name="Test"}
            };
            return await Task.FromResult(plans.AsEnumerable());
        }

        Task<Plan?> IPlanRepository.GetByIdAsync(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        Task<int> IPlanRepository.UpdateAsync(Plan plan)
        {
            throw new NotImplementedException();
        }
    }
}
