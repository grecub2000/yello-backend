using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yello.Core.DTOs.Card;
using Yello.Core.Enums;

namespace Yello.Core.Interfaces.Services
{
    public interface ICardService
    {

        Task<List<CardDto>> ListAsync(int userId);
        Task<CardDto> GetByIdAsync(int id);
        Task CreateAsync(CardCreateDto cardDto);
        Task<CardDto> UpdateCardProgressAsync(int cardId, int progress);
        //Task<CardDto> UpdateAsync(CardCreateDto CardDto);
    }
}
