using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Encodings.Web;

namespace CampusActivityHubPRO.Helpers
{
    public static class BootstrapSelectHelper
    {
        // Генерує Bootstrap dropdown з іконками категорій
        public static IHtmlContent CategorySelect(this IHtmlHelper html, string name, IEnumerable<(int Id, string Name, string? Icon)> items, int? selectedId = null)
        {
            var div = new TagBuilder("div");
            div.AddCssClass("dropdown bootstrap-select");

            var button = new TagBuilder("button");
            button.AddCssClass("btn btn-outline-secondary dropdown-toggle");
            button.Attributes["data-bs-toggle"] = "dropdown";
            button.InnerHtml.AppendHtml(selectedId.HasValue ? items.FirstOrDefault(i => i.Id == selectedId.Value).Name : "Оберіть категорію");

            var ul = new TagBuilder("ul");
            ul.AddCssClass("dropdown-menu");

            foreach (var it in items)
            {
                var li = new TagBuilder("li");
                var a = new TagBuilder("a");
                a.AddCssClass("dropdown-item");
                a.Attributes["data-value"] = it.Id.ToString();
                a.InnerHtml.AppendHtml($"<i class='{it.Icon ?? "bi-folder"} me-2'></i>{HtmlEncoder.Default.Encode(it.Name)}");
                li.InnerHtml.AppendHtml(a);
                ul.InnerHtml.AppendHtml(li);
            }

            div.InnerHtml.AppendHtml(button);
            div.InnerHtml.AppendHtml(ul);

            return div;
        }
    }
}
