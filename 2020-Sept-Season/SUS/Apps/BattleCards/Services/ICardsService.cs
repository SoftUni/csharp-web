using BattleCards.ViewModels.Cards;
using System.Collections.Generic;

namespace BattleCards.Services
{
    public interface ICardsService
    {
        int AddCard(AddCardInputModel input);

        IEnumerable<CardViewModel> GetAll();

        IEnumerable<CardViewModel> GetByUserId(string userId);

        void AddCardToUserCollection(string userId, int cardId);

        void RemoveCardFromUserCollection(string userId, int cardId);
    }
}
