using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gerenciador.Web.UI.Helpers {
    public static class StatusHelper {
        public static MvcHtmlString DisplayStatus(this HtmlHelper htmlHelper, string id, string value, string @class) {
            var spanTag = new TagBuilder("span");

            //var cssClass = DefineStatusLabelClass(value) + " " + @class;
            spanTag.MergeAttribute("class", @class);
            spanTag.AddCssClass(DefineStatusLabelClass(value));
            if (!string.IsNullOrEmpty(id)) 
                spanTag.MergeAttribute("id", id);            
            spanTag.InnerHtml = TraduzirStatusTeporarioGambiarra(value);
            var result = MvcHtmlString.Create(spanTag.ToString(TagRenderMode.Normal));
            return result;
        }

        private static string DefineStatusLabelClass(string status) {
            if (status == "Open") {
                return "label label-warning";
            } else if(status == "Completed") {
                return "label label-success";
            } else if (status == "Cancelled") {
                return "label label-danger";
            } else if (status == "InProgress") {
                return "label label-info";
            } else {
                return "label label-default";
            }
        }

        //TODO: Remove this shit. Use some AOP in enum
        private static string TraduzirStatusTeporarioGambiarra(string status) {
            if (status == "Open") {
                return "aberta";
            } else if (status == "Completed") {
                return "completa";
            } else if (status == "Cancelled") {
                return "cancelada";
            } else if (status == "InProgress") {
                return "em andamento";
            } else {
                return "desconhecida";
            }
        }
    }// class
}