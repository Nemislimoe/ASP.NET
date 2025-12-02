using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using CampusActivityHub.Models;
using System.Collections.Generic;
using System.Text;

namespace CampusActivityHub.Helpers
{
    public static class BootstrapCategorySelectHelper
    {
        public static IHtmlContent CategorySelect(this IHtmlHelper html, string name, IEnumerable<Category> categories, int? selectedId = null)
        {
            var sb = new StringBuilder();
            sb.Append($"<select class='form-select' name='{name}'>");
            foreach (var c in categories)
            {
                var sel = (c.Id == selectedId) ? " selected" : "";
                sb.Append($"<option value='{c.Id}'{sel} data-icon='{c.IconClass}'>{c.Name}</option>");
            }
            sb.Append("</select>");
            return new HtmlString(sb.ToString());
        }
    }
}
