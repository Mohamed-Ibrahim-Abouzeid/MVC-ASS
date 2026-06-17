using GymManagementSystemG01.BLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystemG01.BLL.Services.Interfaces
{
    public interface IAnalyticsService
    {
        Task<AnalyticsViewModel> GetAnalyticsDataAsync(CancellationToken ct = default);
    }
}
