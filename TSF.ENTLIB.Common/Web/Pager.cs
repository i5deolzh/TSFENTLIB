// ===============================================================================
// 版权    ：枢木
// 创建时间：2011-7 修改：
// 作者    ：枢木 ideal35500@qq.com
// 文件    ：
// 功能    ：
// 说明    ：
// ===============================================================================

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;

namespace TSF.ENTLIB.Common.Web
{
    #region Foundation

    public enum FirstLastType : byte
    {
        Normal = 0,
        Digit = 1
    }

    public class PagerEventArgs : EventArgs
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }

    public delegate void PagerEventHandler(Pager sender, PagerEventArgs e);

    #endregion

    /// <summary>
    /// 简易分页控件 V1.0
    /// </summary>
    [PersistChildren(false)]
    public class Pager : Control
    {
        private int _pageIndex = -1;
        public event PagerEventHandler PageIndexChanged;

        protected void OnPageIndexChanged(Pager sender, PagerEventArgs e)
        {
            if (this.PageIndexChanged != null)
                this.PageIndexChanged(sender, e);
        }

        public Pager() { }

        protected virtual void BindDefaultContent(Control control)
        {
            if (this.TotalPages <= 1)
            {
                this.AutomatedVisible = false;
            }
            else
            {
                if (this.ShowPreviousNext)
                {
                    this.BindPrevious(control);
                }
                if (this.ShowFirstLast)
                {
                    this.BindFirst(control);
                }
                if (this.ShowIndividualPages)
                {
                    this.BindPages(control);
                }
                if (this.ShowFirstLast)
                {
                    this.BindLast(control);
                }
                if (this.ShowPreviousNext)
                {
                    this.BindNext(control);
                }
                if (this.ShowTotalSummary)
                {
                    this.BindTotalSummary(control);
                }
            }
        }

        protected virtual void BindFirst(Control control)
        {
            if ((this.PageIndex > this.IndividualPagesDisplayedCount / 2) && (this.TotalPages > this.IndividualPagesDisplayedCount) && this.ShowIndividualPages)
            {
                HyperLink child = new HyperLink();
                child.Text = this.FirstButtonText;
                child.NavigateUrl = this.GetQueryStringNavigateUrl(1);
                control.Controls.Add(child);

                control.Controls.Add(new LiteralControl("&nbsp;...&nbsp;"));
            }
        }
        protected virtual void BindPrevious(Control control)
        {
            if (this.PageIndex > 0)
            {
                HyperLink child = new HyperLink();
                child.Text = this.PreviousButtonText;
                child.NavigateUrl = this.GetQueryStringNavigateUrl(this.PageIndex);
                child.CssClass = "previous";
                control.Controls.Add(child);

                control.Controls.Add(new LiteralControl("&nbsp;"));
            }
        }
        protected virtual void BindNext(Control control)
        {
            if ((this.PageIndex + 1) < this.TotalPages)
            {
                if (this.ShowIndividualPages)
                {
                    control.Controls.Add(new LiteralControl("&nbsp;"));
                }

                HyperLink child = new HyperLink();
                child.Text = this.NextButtonText;
                child.NavigateUrl = this.GetQueryStringNavigateUrl(this.PageIndex + 2);
                child.CssClass = "next";
                control.Controls.Add(child);
            }
        }
        protected virtual void BindPages(Control control)
        {
            int start = this.GetFirstIndividualPageIndex();
            int end = this.GetLastIndividualPageIndex();

            for (int i = start; i <= end; i++)
            {
                if (this.PageIndex == i)
                {
                    HyperLink child = new HyperLink();
                    child.Text = (i + 1).ToString();
                    child.NavigateUrl = this.GetQueryStringNavigateUrl(i + 1);
                    child.CssClass = "current";
                    control.Controls.Add(child);
                }
                else
                {
                    HyperLink child = new HyperLink();
                    child.Text = (i + 1).ToString();
                    child.NavigateUrl = this.GetQueryStringNavigateUrl(i + 1);
                    control.Controls.Add(child);
                }
                if (i < end)
                {
                    control.Controls.Add(new LiteralControl("&nbsp"));
                }
            }
        }
        protected virtual void BindLast(Control control)
        {
            if (((this.PageIndex + this.IndividualPagesDisplayedCount / 2 + 1) < this.TotalPages) && (this.TotalPages > this.IndividualPagesDisplayedCount) && this.ShowIndividualPages)
            {
                control.Controls.Add(new LiteralControl("&nbsp;...&nbsp;"));

                HyperLink child = new HyperLink();
                child.Text = this.LastButtonText;
                child.NavigateUrl = this.GetQueryStringNavigateUrl(this.TotalPages);
                control.Controls.Add(child);
            }
        }
        protected virtual void BindTotalSummary(Control control)
        {
            control.Controls.Add(new LiteralControl("&nbsp"));
            control.Controls.Add(new LiteralControl(string.Format("共{0}页，当前第{1}页", this.TotalPages, this.CurrentPage)));
        }

        protected virtual int GetInitialPageIndex()
        {
            int index = 0;

            if (HttpContext.Current.Request[this.QueryStringProperty] != null)
            {
                int.TryParse(HttpContext.Current.Request[this.QueryStringProperty].ToString(), out index);
            }

            return index - 1;
        }
        protected virtual int GetFirstIndividualPageIndex()
        {
            int first = 0;

            if ((this.TotalPages < this.IndividualPagesDisplayedCount) || ((this.PageIndex - (this.IndividualPagesDisplayedCount / 2)) < 0))
            {
                first = 0;
            }
            else if ((this.PageIndex + (this.IndividualPagesDisplayedCount / 2)) >= this.TotalPages)
            {
                first = (this.TotalPages - this.IndividualPagesDisplayedCount);
            }
            else
            {
                first = (this.PageIndex - (this.IndividualPagesDisplayedCount / 2));
            }

            return first;
        }
        protected virtual int GetLastIndividualPageIndex()
        {
            int add = this.IndividualPagesDisplayedCount / 2;
            if ((this.IndividualPagesDisplayedCount % 2) == 0)
            {
                add--;
            }

            int last = 0;

            if ((this.TotalPages < this.IndividualPagesDisplayedCount) || ((this.PageIndex + add) >= this.TotalPages))
            {
                last = (this.TotalPages - 1);
            }
            else if ((this.PageIndex - (this.IndividualPagesDisplayedCount / 2)) < 0)
            {
                last = (this.IndividualPagesDisplayedCount - 1);
            }
            else
            {
                last = this.PageIndex + add;
            }

            return last;
        }

        protected virtual string GetQueryStringNavigateUrl(int pageNumber)
        {
            string queryStringNavigateUrl = "";
            string queryStringField = this.QueryStringProperty;
            StringBuilder builder = new StringBuilder();
            HttpRequest request = this.Page.Request;

            builder.Append(request.Path);
            builder.Append("?");
            foreach (string str2 in request.QueryString.AllKeys)
            {
                if (!str2.Equals(queryStringField, StringComparison.OrdinalIgnoreCase))
                {
                    builder.Append(HttpUtility.UrlEncode(str2));
                    builder.Append("=");
                    builder.Append(HttpUtility.UrlEncode(request.QueryString[str2]));
                    builder.Append("&");
                }
            }
            builder.Append(queryStringField);
            builder.Append("=");
            builder.Append(pageNumber);

            queryStringNavigateUrl = builder.ToString();

            return queryStringNavigateUrl;
        }

        protected void UpdateVisible()
        {
            bool vis = true;
            if (this.ViewState["Visible"] != null)
                vis = (bool)this.ViewState["Visible"];

            base.Visible = vis && this.AutomatedVisible;
        }

        //------------------------------------------

        protected override void OnLoad(EventArgs e)
        {
            int index = this.GetInitialPageIndex();
            if (index >= 0)
            {
                this.PageIndex = index;
                this.OnPageIndexChanged(this, new PagerEventArgs() { PageSize = this.PageSize, PageIndex = index, TotalRecords = this.TotalRecords });
            }

            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.DataBind();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.WriteLine();
            writer.Write("<!-- 简易分页控件 枢木 ideal35500@163.com -->");
            writer.WriteLine();

            writer.Write("<div");
            if (!string.IsNullOrEmpty(this.ID))
            {
                writer.Write(" id=\"{0}\"", this.ID);
            }
            if (!string.IsNullOrEmpty(this.CssClass))
            {
                writer.Write(" class=\"{0}\"", this.CssClass);
            }
            writer.Write(">");

            base.Render(writer);

            writer.Write("</div>");
        }

        public override void DataBind()
        {
            if ((this.TotalRecords > 0) && ((this.TotalPages - 1) < this.PageIndex))
            {
                this.PageIndex = this.TotalPages - 1;
                this.OnPageIndexChanged(this, new PagerEventArgs() { PageSize = this.PageSize, PageIndex = this.PageIndex, TotalRecords = this.TotalRecords });
            }

            this.BindDefaultContent(this);
            base.DataBind();
        }

        //------------------------------------------       

        #region Properties


        protected virtual bool AutomatedVisible
        {
            get
            {
                if (this.ViewState["AutomatedVisible"] == null)
                    return true;
                else
                    return (bool)this.ViewState["AutomatedVisible"];
            }
            set
            {
                this.ViewState["AutomatedVisible"] = value;
                this.UpdateVisible();
            }
        }

        //------------------------------------------

        public virtual int TotalPages
        {
            get
            {
                if ((this.TotalRecords == 0) || (this.PageSize == 0))
                {
                    return 0;
                }
                int num = this.TotalRecords / this.PageSize;
                if ((this.TotalRecords % this.PageSize) > 0)
                {
                    num++;
                }
                return num;
            }
        }
        public virtual int CurrentPage
        {
            get
            {
                return (this.PageIndex + 1);
            }
        }

        //------------------------------------------

        public override string ID
        {
            get
            {
                return (((string)this.ViewState["ID"]) ?? this.UniqueID);
            }
            set
            {
                this.ViewState["ID"] = value;
            }
        }
        public virtual string CssClass
        {
            get
            {
                return (((string)this.ViewState["CssClass"]) ?? string.Empty);
            }
            set
            {
                this.ViewState["CssClass"] = value;
            }
        }
        public virtual string QueryStringProperty
        {
            get
            {
                return (((string)this.ViewState["QueryStringProperty"]) ?? "PageIndex");
            }
            set
            {
                this.ViewState["QueryStringProperty"] = value;
            }
        }
        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                this.ViewState["Visible"] = value;
                this.UpdateVisible();
            }
        }
        public virtual int IndividualPagesDisplayedCount
        {
            get
            {
                if (this.ViewState["IndividualPagesDisplayedCount"] == null)
                    return 5;
                else
                    return (int)this.ViewState["IndividualPagesDisplayedCount"];
            }
            set
            {
                this.ViewState["IndividualPagesDisplayedCount"] = value;
            }
        }

        public virtual int TotalRecords
        {
            get
            {
                if (this.ViewState["TotalRecords"] == null)
                    return 0;
                else
                    return (int)this.ViewState["TotalRecords"];
            }
            set
            {
                this.ViewState["TotalRecords"] = value;
            }
        }
        public virtual int PageSize
        {
            get
            {
                if (this.ViewState["PageSize"] == null)
                    return 5;
                else
                    return (int)this.ViewState["PageSize"];
            }
            set
            {
                this.ViewState["PageSize"] = value;
            }
        }
        public virtual int PageIndex
        {
            get
            {
                if (this._pageIndex == -1)
                {
                    int index = this.GetInitialPageIndex();
                    if (index >= 0)
                    {
                        this.PageIndex = index;
                        this.OnPageIndexChanged(this, new PagerEventArgs() { PageSize = this.PageSize, PageIndex = this.PageIndex, TotalRecords = this.TotalRecords });
                    }
                }
                if (this._pageIndex < 0)
                {
                    return 0;
                }
                return this._pageIndex;
            }
            set
            {
                this._pageIndex = value;
            }
        }

        //------------------------------------------

        public bool ShowFirstLast
        {
            get
            {
                if (this.ViewState["ShowFirstLast"] == null)
                    return true;
                else
                    return (bool)this.ViewState["ShowFirstLast"];
            }
            set
            {
                this.ViewState["ShowFirstLast"] = value;
            }
        }
        public bool ShowPreviousNext
        {
            get
            {
                if (this.ViewState["ShowPreviousNext"] == null)
                    return true;
                else
                    return (bool)this.ViewState["ShowPreviousNext"];
            }
            set
            {
                this.ViewState["ShowPreviousNext"] = value;
            }
        }
        public bool ShowIndividualPages
        {
            get
            {
                if (this.ViewState["ShowIndividualPages"] == null)
                    return true;
                else
                    return (bool)this.ViewState["ShowIndividualPages"];
            }
            set
            {
                this.ViewState["ShowIndividualPages"] = value;
            }
        }
        public bool ShowTotalSummary
        {
            get
            {
                if (this.ViewState["ShowTotalSummary"] == null)
                    return false;
                else
                    return (bool)this.ViewState["ShowTotalSummary"];
            }
            set
            {
                this.ViewState["ShowTotalSummary"] = value;
            }
        }

        public FirstLastType FirstLastType
        {
            get
            {
                return ((this.ViewState["FirstButtonText"] == null) ? FirstLastType.Normal : FirstLastType.Digit);
            }
            set
            {
                this.ViewState["FirstButtonText"] = value;
            }
        }

        public string FirstButtonText
        {
            get
            {
                if (this.FirstLastType == FirstLastType.Normal)
                {
                    return (((string)this.ViewState["LastButtonText"]) ?? "首页");
                }
                else
                {
                    return "1";
                }
            }
            set
            {
                this.ViewState["FirstButtonText"] = value;
            }
        }
        public string PreviousButtonText
        {
            get
            {
                return (((string)this.ViewState["PreviousButtonText"]) ?? "上一页");
            }
            set
            {
                this.ViewState["PreviousButtonText"] = value;
            }
        }
        public string NextButtonText
        {
            get
            {
                return (((string)this.ViewState["NextButtonText"]) ?? "下一页");
            }
            set
            {
                this.ViewState["NextButtonText"] = value;
            }
        }
        public string LastButtonText
        {
            get
            {
                if (this.FirstLastType == FirstLastType.Normal)
                {
                    return (((string)this.ViewState["LastButtonText"]) ?? "尾页");
                }
                else
                {
                    return this.TotalPages.ToString();
                }
            }
            set
            {
                this.ViewState["LastButtonText"] = value;
            }
        }

        #endregion
    }
}
