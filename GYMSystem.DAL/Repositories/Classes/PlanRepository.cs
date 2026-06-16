using GYMSystem.DAL.DBContexts;
using GYMSystem.DAL.Models;
using GYMSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYMSystem.DAL.Repositories.Classes
{
    public class PlanRepository : IPlanRepository
    {
        private readonly GYMDBContext _dbcontext;
        public PlanRepository(GYMDBContext dbcontext)
        {
            _dbcontext=dbcontext;
        }
        public async Task<int> AddAsync(Plan plan)
        {
                _dbcontext.Plans.Add(plan);
            return await _dbcontext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Plan plan)
        {
            _dbcontext.Plans.Remove(plan);
            return await _dbcontext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
           IQueryable<Plan> query=tracking?_dbcontext.Plans: _dbcontext.Plans.AsNoTracking();
            return await query.ToListAsync();
        }

        public async Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _dbcontext.Plans.FindAsync(id,ct);
        }

        public Task<int> UpdateAsync(Plan plan)
        {
        _dbcontext.Plans.Update(plan);
            return _dbcontext.SaveChangesAsync();
        }
    }
}
