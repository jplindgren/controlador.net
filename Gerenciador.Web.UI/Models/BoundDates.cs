using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gerenciador.Web.UI.Models {
    public class LimitDate {
        public LimitDate(DateTime start, DateTime end, string description) {
            Start = start;
            End = end;
            Description = description;
        }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Start { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime End { get; set; }
        public string Description { get; set; }
    } //class
}