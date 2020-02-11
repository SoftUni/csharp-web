using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.ViewModels.Albums
{
    public class AllAlbumsViewModel
    {
        public IEnumerable<AlbumInfoViewModel> Albums { get; set; }
    }
}
