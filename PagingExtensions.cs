
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GSK.Grid.Helpers
{
    public static class PagingExtensions
    {
        public static MvcHtmlString PagedList(this HtmlHelper helper, int pageSize, long totalRecords, int curentPage,string url)
        {
            int totalPages = (int)Math.Ceiling(Convert.ToDouble(totalRecords) / pageSize);
            int startPage = curentPage / 6 * 5 + 1;
            int endPage = startPage + 4;

            bool prevExists = false;
            if (curentPage > 5) prevExists = true;


            bool nextExists = false;
            if (endPage < totalPages) nextExists = true;

            string s = "<div class='dataTables_paginate paging_simple_numbers'><ul class='pagination'>";
            if (prevExists) s += string.Format("<li class='paginate_button'><a href='" + url + "?page={0}'>{1}</a></li>", curentPage - 1, "Prev");

            for (int i = startPage; i <= endPage; i++)
            {
                if (i == curentPage)
                {
                    s += string.Format("<li class='paginate_button active'><a style='padding-right:10px' href='" + url + "?page={0}' >{1}</a></li>", i, i);
                }
                else
                {
                    s += string.Format("<li class='paginate_button'><a style='padding-right:10px' href='" + url + "?page={0}' >{1}</a></li>", i, i);
                }
            }
            if (nextExists) s += string.Format("<li class='paginate_button'><a href='" + url+"?page={0}'>{1}</a></li>", curentPage - 1, "Next");
            s = s + "</ul></div>";
            return new MvcHtmlString(s);
        }


        public static MvcHtmlString PagedList(this HtmlHelper helper, GridModel model)
        {
            int totalPages = (int)Math.Ceiling(Convert.ToDouble(model.TotalRowsCount) / model.PageSize);
            int startPage = model.PageNo / 6 * 5 + 1;
            int endPage = 0;
            if (totalPages > 5)
                endPage = startPage + 4;
            else
                endPage = totalPages;
            bool prevExists = false;
            if (model.PageNo > 5) prevExists = true;


            bool nextExists = false;
            if (endPage<totalPages)      nextExists = true;

           
            string s = "<div class='dataTables_paginate paging_simple_numbers'><ul class='pagination'>";
            if (prevExists) s += string.Format("<li class='paginate_button'><a  pageno='{0}'>{1}</a></li>",  model.PageNo - 1, "Prev");

            for (int i = startPage; i<=endPage && endPage>1; i++)
            {
                if (i == model.PageNo)
                {
                    s += string.Format("<li class='paginate_button active'><a style='padding-right:10px'  pageno='{0}'>{1}</a></li>", i, i);
                }
                else
                {
                    s += string.Format("<li class='paginate_button'><a style='padding-right:10px'  pageno='{0}'>{1}</a></li>", i, i);
                }
            }
            if (nextExists) s += string.Format("<li class='paginate_button'><a  pageno='{0}'>{1}</a></li>", model.PageNo+1, "Next");
            s = s + "</ul></div>";
            return new MvcHtmlString(s);
        }

    }
}