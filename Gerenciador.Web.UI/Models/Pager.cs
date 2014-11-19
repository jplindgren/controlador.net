using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gerenciador.Web.UI.Models {
    public class Pager {
        public Pager(int count, int pageSize = 10) {
            this.Count = count;
            this.PageSize = pageSize;
        }

        public int PageSize { get; set; }
        public int Count { get; set; }
        public int NumberOfPages { 
            get {
                return (Count / PageSize) + 1;
            } 
        }
    }// class
}