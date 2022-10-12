using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yello.Core.DTOs.Team;
using Yello.Core.Filters;


namespace Yello.Core.Interfaces.Services
{
    public interface ITeamService
    {   
        Task<TeamDto> GetByIdAsync(int id);
        Task CreateAsync(TeamCreateDto teamDto);
        Task AddMemberAsync(int teamId, int userId);
        Task<List<TeamDto>> ListAsync(TeamFilter filter);

    }
}
