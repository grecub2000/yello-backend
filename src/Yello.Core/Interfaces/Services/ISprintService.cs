using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yello.Core.DTOs.Sprint;
using Yello.Core.DTOs.Project;

namespace Yello.Core.Interfaces.Services
{
    public interface ISprintService
    {
        Task<SprintDto> GetByIdAsync(int id);
        Task CreateAsync(SprintCreateDto sprintDto);
        //Task AddCardAsync(int cardId, int sprintId);
        Task CompleteSprint(SprintCompleteDto sprint);

        Task<List<SprintInfoDto>> GetByUserIdAsync(int id); 
    }
}
