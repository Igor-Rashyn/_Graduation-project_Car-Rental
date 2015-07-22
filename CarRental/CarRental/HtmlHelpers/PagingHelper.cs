using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CarRental.WebUI.Models;

namespace CarRental.HtmlHelpers
{
    /// <summary>
    /// Хэлпер для создания пэйджинга
    /// </summary>
    public static class PagingHelper
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PageInfo pagingInfo, Func<int, string> pageUrl)
        {
            // FUnc - содержит метод который принимает параметр int и возвращает результат string
            StringBuilder result = new StringBuilder();

#region Creation button <<
            TagBuilder tagAOpen = new TagBuilder("a");
            if (pagingInfo.CurrentPage != 1)
            {
                tagAOpen.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage - 1));
            }
            else
            {
                tagAOpen.MergeAttribute("href", pageUrl(1));
            }
            tagAOpen.InnerHtml = "«";
            TagBuilder tagLiOpen = new TagBuilder("li");
            tagLiOpen.InnerHtml = tagAOpen.ToString();
            if (pagingInfo.CurrentPage==1)
            {
                tagLiOpen.AddCssClass("disabled");
            }
            result.Append(tagLiOpen.ToString());
#endregion

#region Creation buttons
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder tagLi = new TagBuilder("li");
                if (pagingInfo.CurrentPage==i)
                {
                    tagLi.AddCssClass("active");
                }
                TagBuilder tag = new TagBuilder("a"); //Contains classes and properties that are used to create HTML elements. This class is used to write helpers, such as those found in the System.Web.Helpers namespace.
                tag.MergeAttribute("href", pageUrl(i)); // Adds a new attribute to the tag.
                tag.InnerHtml = i.ToString(); // Gets or sets the inner HTML value for the element.
                tagLi.InnerHtml = tag.ToString();
                result.Append(tagLi.ToString());// Appends a copy of the specified string to this instance.
            }
            #endregion

#region Creation button >>
            TagBuilder tagAClose = new TagBuilder("a");
            if (pagingInfo.CurrentPage != pagingInfo.TotalPages)
            {
                tagAClose.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage + 1));
            }
            else
            {
                tagAClose.MergeAttribute("href", pageUrl(pagingInfo.CurrentPage));
            }
            tagAClose.InnerHtml = "»";
            TagBuilder tagLiClose = new TagBuilder("li");
            tagLiClose.InnerHtml = tagAClose.ToString();
            if (pagingInfo.CurrentPage == pagingInfo.TotalPages)
            {
                tagLiClose.AddCssClass("disabled");
            }
            result.Append(tagLiClose.ToString());
#endregion

            TagBuilder tagUl = new TagBuilder("ul");
            tagUl.AddCssClass("pagination");
            tagUl.AddCssClass("pagination-sm");
            tagUl.InnerHtml = result.ToString();

            return MvcHtmlString.Create(tagUl.ToString());
        }

    }
}