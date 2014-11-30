using Gerenciador.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gerenciador.Web.UI.Models {
    public class PageMetadataViewModel{
        public PageMetadataViewModel(UserProfile user, string pageTitle) {
            this.User = user;
            this.PageTitle = pageTitle;
        }

        protected UserProfile User { get; set; }
        protected string PageTitle { get; set; }
    } //class
}