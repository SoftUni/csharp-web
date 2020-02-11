using IRunes.Models;
using IRunes.ViewModels.Albums;
using System;
using System.Collections.Generic;

namespace IRunes.Services
{
    public interface IAlbumsService
    {
        void Create(string name, string cover);

        IEnumerable<T> GetAll<T>(Func<Album, T> selectFunc);

        AlbumDetailsViewModel GetDetails(string id);
    }
}
