#region

using System.Text;
using System.Web.Mvc;

#endregion

namespace Deployment.Extention
{
    public static class HtmlExtension
    {

        public static MvcHtmlString Paging(this HtmlHelper htmlHelper, int pageIndex, string url)
        {
            if (!url.Contains("?")) url += "?";
            else url += "&";
            var startIndex = pageIndex - 5;
            if (startIndex < 1) startIndex = 1;
            var endIndex = pageIndex + 5;
            if (endIndex < 10) endIndex = 10;

            var sb = new StringBuilder();
            sb.Append("<div class='paging-container'>");

            if (startIndex > 1)
                sb.Append(
                    string.Format("<span class='pagenav'><a href='{1}pageindex={0}' title='{0}'>&lt;&lt;</a></span>",
                                  startIndex - 1, url));

            for (var i = startIndex; i <= endIndex; i++)
            {
                if (i == pageIndex) sb.Append(string.Format("<span class='currentpage'>{0}</a></span>", pageIndex));
                else
                    sb.Append(
                        string.Format("<span class='pagenav'><a href='{1}pageindex={0}' title='{0}'>{0}</a></span>", i,
                                      url));
            }
            sb.Append(string.Format("<span class='pagenav'><a href='{1}pageindex={0}' title='{0}'>&gt;&gt;</a></span>",
                                    endIndex + 1, url));
            sb.Append("</div>");

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}