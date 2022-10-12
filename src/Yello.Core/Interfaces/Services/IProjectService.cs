using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yello.Core.DTOs.Project;
using Yello.Core.Filters;


namespace Yello.Core.Interfaces.Services
{
    public interface IProjectService
    {
        Task<ProjectDto> GetByIdAsync(int id);
        Task CreateAsync(ProjectCreateDto teamDto);
        Task AddMemberAsync(int projectId, int userId);
        Task<List<ProjectDto>> ListAsync(ProjectFilter filter);

    }
}
