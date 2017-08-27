using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq.Expressions;
using System.Web.Routing;
namespace GSK.Grid.Helpers
{
    public static class TableExtensions
    {

        public static MvcHtmlString GSKGridView(this HtmlHelper htmlHelper, GridModel model)
        {
            if (model == null) return new MvcHtmlString("");

            if (String.IsNullOrEmpty(model.SubmitUrl))
            {
                //model.SubmitUrl = htmlHelper.ViewContext.RouteData.DataTokens["area"] + "/" +
                //   htmlHelper.ViewContext.RouteData.GetRequiredString("controller") + "/" +
                //   htmlHelper.ViewContext.RouteData.GetRequiredString("action");
                model.SubmitUrl = "/"+htmlHelper.ViewContext.RouteData.DataTokens["area"] + "/" +
                  htmlHelper.ViewContext.Controller.ControllerContext.RouteData.GetRequiredString("controller") + "/" +
                 htmlHelper.ViewContext.Controller.ControllerContext.RouteData.GetRequiredString("action");
                model.SubmitUrl = model.SubmitUrl.Replace("//", "/");


            }

            StringBuilder sb = new StringBuilder();

            sb.Append("<div id='" + model.Id + "' class='gskgrid' action='"+model.SubmitUrl +"' >");

            string hiddenElements = "";
            foreach (string key in htmlHelper.ViewContext.HttpContext.Request.QueryString.Keys)
                if (key!="SearchFor" && key!="PageNo" && key!="PageSize")
                hiddenElements += htmlHelper.Hidden(key, htmlHelper.ViewContext.HttpContext.Request.QueryString[key]);
            sb.Append(hiddenElements);

           // sb.Append(String.Format("<form id='frmList' name='frmList' method='post' action='{0}'>", model.SubmitUrl));
            sb.Append(String.Format("<input type = 'hidden' value = '{0}' name = 'PageNo' id = 'PageNo' />", model.PageNo));
            //Search Bar
            sb.Append("<div class='row'>");


            
            sb.Append("<div class='col-md-6'>");
            
            //Dropdown
            if (model.GridOptions.Pagination)
            {
                sb.Append(htmlHelper.Label("Paging: "));
                sb.Append(htmlHelper.DropDownList("PageSize", new SelectList(model.PageSizes, model.PageSize), new {@class="pagesize" }));
            }
            //Buttons Bar
            if (model.GridOptions.HeaderButtons!=null)
            foreach (HeaderButton hb in model.GridOptions.HeaderButtons)
                if (hb.IsPopup)
                    //sb.Append(String.Format("<a href = '' class='hButton' style='margin-right:10px' url='{5}'  onclick = \"updateRecord('{0}',this,'{1}','{2}','{3}'); return false; \" >{4}</a>", model.Id, 1, hb.Url, hb.PopupTitle,hb.Text, hb.Url));
                    sb.Append(String.Format("<a href = '' class='hButton {3}' style='margin-right:10px' popup='1' url='{0}' title='{1}'  >{2}</a>", hb.Url,hb.PopupTitle, hb.Text, hb.CSSClass));
                else
                    sb.Append(String.Format("<a style='margin-right:10px' class='hButton {3}' popup='0'  href = '{0}/{1}?p={2}'>{3}</a>", hb.Url,1, model.PageNo,hb.Text, hb.CSSClass));

            /////////////sb.Append("<button type='button' id='hb' onclick='' style='margin-left:10px'>" + hb.Text +"</button></div>");
            sb.Append("</div>");

            //Search Box and Search Button
            if (model.GridOptions.Search)
            {
                sb.Append("<div class='col-md-6'><div class=\"pull-right\">");
                sb.Append(htmlHelper.TextBox("SearchFor", ""));
                sb.Append(String.Format("<button type = 'button' value='{0}' id='btnSearch' class=\"glyphicon glyphicon-search\" style=\"line-height:inherit\" onclick=\"onSubmit(this,'{1}')\"  />", "Search", model.Id));
                sb.Append("</div></div>");
            }
            sb.Append("</div>");

            //Table header
            sb.Append("<div class='row'><div class='col-md-12'><div  style =\"overflow-x:auto\"><table class='table table-striped'><thead><tr>");
            if (model.Data.Headers != null)
            {
                if (model.GridOptions.ShowCheckBoxes)
                sb.Append("<th><input type='checkbox' name='chkAll' id='chkAll' /></th>");
                foreach (var header in model.Data.Headers)
                    sb.Append("<th>" + header + "</th>");
                sb.Append("<th>Actions</th>");
            }
            sb.Append("</tr></thead>");

            //Table Data
            if (model.Data.Count > 0)
            {
                foreach (var r in model.Data)
                {
                    sb.Append("<tr>");
                    if (model.GridOptions.ShowCheckBoxes)
                    sb.Append("<td><input type='checkbox' name='" + r.RowId +"' /></td>");
                    //dynamic statusFieldValue = null;
                    foreach (var f in r.Fields)
                    {
                        if (!String.IsNullOrEmpty(f.NavigationUrl))
                        { 
                            string text = f.Value;
                            sb.Append(String.Format("<td><a href='{0}'>{1}</a></td>",f.NavigationUrl, f.Value ));
                        }
                        else

                            sb.Append("<td>" + f.Value + "</td>");

                        //if (f.Name == model.StatusFieldname)
                        //{
                        //    statusFieldValue = f.Value;
                        //}

                    }

                    //Row buttons
                    if (model.GridOptions.RowButtos != null)
                    {
                        sb.Append("<td>");
                        foreach (RowButton rb in model.GridOptions.RowButtos)
                        {
                            string t = rb.Text;
                            string url = rb.Url;
                            bool isDisabled = (rb.DisableWhen != null && r.Fields.FirstOrDefault(rb.DisableWhen) != null);
                            if (rb is PopupButton)
                            {
                                if (isDisabled)
                                    sb.Append("<a href='' style='opacity:0.7; pointer-events: none;' onclick='return false;' >" + t + "</a> | ");
                                else
                                {
                                    PopupButton pb = (PopupButton)rb;
                                    sb.Append(String.Format("<a href = ''  onclick = \"updateRecord('{0}','{1}','{2}','{3}'); return false; \" >{4}</a> | ", model.Id, r.RowId, url, pb.PopupTitle, t));
                                }
                            }
                            else if (rb is ConfirmButton)
                            {
                                if (isDisabled)
                                    sb.Append("<a href=''  style='opacity:0.7; pointer-events: none;' onclick='return false;' >" + t + "</a> | ");
                                else
                                   sb.Append(String.Format("<a href = '' onclick = \"doAction('{0}','{1}','{2}','{3}','{4}'); return false; \" >{5}</a> | ", model.Id, "id="+r.RowId, url, "Confirm Dialog", ((ConfirmButton)rb).ConfirmMessage, t));
                            }
                            else if (rb is ToggleButton)
                            {
                                
                                ToggleButton tb = (ToggleButton)rb;
                                string confirmMesage = tb.ConfirmMessage;
                                if (r.Fields[tb.ToggleField] == null) throw new Exception(tb.ToggleField + " field not found");
                                string value = r.Fields[tb.ToggleField].Value;
                                if (Convert.ToString(value) == Convert.ToString(tb.ToggleValue))
                                {
                                    t = tb.ToggleText;
                                    confirmMesage = tb.ToggleConfirmMessage;
                                }
                                if (isDisabled)
                                    sb.Append("<a href=''  style='opacity:0.7; pointer-events: none;' onclick='return false;' >" + t + "</a> | ");
                                else
                                    sb.Append(String.Format("<a href = '' onclick = \"doAction('{0}','{1}','{2}','{3}','{4}'); return false; \" >{5}</a> | ", model.Id, "id="+r.RowId+"&value="+ value, rb.Url, "Confirm Dailog", confirmMesage, t));
                                
                            }
                            else
                                sb.Append(String.Format("<a href = '{0}/{1}?p={2}'>{3}</a> | ", rb.Url, r.RowId, model.PageNo,t));

                            
                        }
                        sb.Remove(sb.Length - 2, 2);
                        sb.Append("</td>");
                    }
                    sb.Append("<tr>");
                }
            }
            else
                sb.Append(String.Format( "<tr><td colspan={0}>No data found</td></tr>", model.Data.Headers.Count +1));

            sb.Append("</table></div></div></div>");

            //Paging
            if (model.GridOptions.Pagination) sb.Append(htmlHelper.PagedList(model));
            sb.Append("</form>");
            

            sb.Append(String.Format("<div id = 'modalEdit_{0}' class='mx-auto'><div id = 'modalBody' ></div><div id='ErrMsg'></div></div></div>", model.Id));
             return new MvcHtmlString(sb.ToString());
        }
    }
}