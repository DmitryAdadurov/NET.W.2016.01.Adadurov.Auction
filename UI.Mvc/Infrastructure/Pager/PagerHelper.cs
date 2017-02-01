using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Mvc.Infrastructure.Pager
{
    public static class PagerHelper
    {
        public static MvcHtmlString Pagination(this HtmlHelper html, int pagesCount, int currentPage, Func<int, string> urls)
        {
            string result = "";
            for (int i = 1; i <= pagesCount; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", urls(i));
                tag.SetInnerText(i.ToString());

                if (i == currentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                result = $"{result}{tag.ToString()}";
            }
            return MvcHtmlString.Create(result);
        }
    }
}