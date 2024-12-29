using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using Poliak_UI_WT.Domain.Entities;
using System.Text.Encodings.Web;

namespace Poliak_UI_WT.TagHelpers
{
    /// <summary>
    /// Пейджер для навигации по спискам товаров
    /// </summary>
    public class PagerTagHelper : TagHelper
    {
        LinkGenerator _linkGenerator;
        IHttpContextAccessor _contextAccessor;


        public PagerTagHelper(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
        {
            _linkGenerator = linkGenerator;
            _contextAccessor = httpContextAccessor;        
        }

        public int CurrentPage {  get; set; }
        public int TotalPages { get; set; }
        public bool Admin {  get; set; } = false;

        public int Prev
        {
            get => CurrentPage == 1? 1: CurrentPage - 1;
        }

        public int Next
        {
            get => CurrentPage == TotalPages? TotalPages: CurrentPage + 1;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.AddClass("row", HtmlEncoder.Default);

            var nav = new TagBuilder("nav");
            nav.Attributes.Add("aria-label", "pagination");

            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");

            #region Кнопка предыдущей страницы
            ul.InnerHtml.AppendHtml(
            CreateListItem(nameof(Category), Prev, "<span ariahidden=\"true\">&laquo;</span>"));
            #endregion

            #region Разметка кнопок переключения страниц
            for (var index = 1; index <= TotalPages; index++)
            {
                ul.InnerHtml.AppendHtml(
                CreateListItem(nameof(Category), index, String.Empty));
            }
            #endregion

            #region Кнопка следующей страницы
            ul.InnerHtml.AppendHtml(
            CreateListItem(nameof(Category), Next, "<span ariahidden=\"true\">&raquo;</span>"));
            #endregion 

            nav.InnerHtml.AppendHtml(ul);
            output.Content.AppendHtml(ul);

        }

        /// <summary>
        /// Разметка одной кнопки пейджера
        /// </summary>
        /// <param name="category">имя категории</param>
        /// <param name="pageNo">номер страницы</param>
        /// <param name="innerText">текст кнопки</param>
        /// <returns></returns>
        private TagBuilder CreateListItem(string? category, int pageNo, string? innerText)
        {
            var li = new TagBuilder("li");
            li.AddCssClass("page-item");
            if (pageNo == CurrentPage && String.IsNullOrEmpty(innerText))
                li.AddCssClass("active");

            var a = new TagBuilder("a");
            a.AddCssClass("page-link");
            var routeData = new
            {
                pageno = pageNo,
                category = category
            };
            string url;

            // Для страниц администратора будет использоваться не MVC, а Razor pages
            
            if (Admin)
                url = _linkGenerator.GetPathByPage(_contextAccessor.HttpContext, page: "./Index", values: routeData);
            else
                url = _linkGenerator.GetPathByAction("index", "product", routeData);
            a.Attributes.Add("href", url);
            var text = String.IsNullOrEmpty(innerText)
            ? pageNo.ToString()
            : innerText;

            a.InnerHtml.AppendHtml(text);
            li.InnerHtml.AppendHtml(a);
            return li;

        }
    }
}

