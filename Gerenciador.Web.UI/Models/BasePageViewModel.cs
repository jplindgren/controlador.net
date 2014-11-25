using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gerenciador.Web.UI.Models {
    public class BasePageViewModel {
        public BasePageViewModel(PageMetadataViewModel metadata) {
            this.PageMetadataViewModel = metadata;
        }
        PageMetadataViewModel PageMetadataViewModel { get; set; }
    }
}