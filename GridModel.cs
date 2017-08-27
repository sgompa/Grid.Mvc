using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Web;

using System.Reflection;

namespace GSK.Grid
{
    public class GridOptions
    {
        public bool Pagination { get; set; }
        public HeaderButton[] HeaderButtons { get; set; }
        public bool Search { get; set; }
        public RowButton[] RowButtos { get; set; }
        public bool ShowCheckBoxes { get; set; }
    }
    public class HeaderButton
    {
        public string Text { get; set; }
        public bool IsPopup { get; set; }
        public string Url { get; set; }
        public string PopupTitle { get; set; }
        public bool IsPost { get; set; }
        public string CSSClass { get; set; }
    }
    public class PlusButton:HeaderButton
    {
        public PlusButton()
        {
            Text = "+";
            IsPopup = true;
            
        }
    }
 
    public class Fields:List<Field>
    {

        public  Field this[string name]
        {
            get
            {
                return Find(f => f.Name == name);

            }
        }
    }
    public class Row 
    {


        public string RowId { get; set; }
        public List<string> UniqueFields { get; set; }
        public Fields Fields { get; set; }
       
        public Row()
        {
            Fields = new Fields();
            UniqueFields = new List<string>();
        }
        
       
    }
    public class Field 
    {
        public DisplayFieldType FieldType
        {
            set; get;
        }

        public string Name { get; set; }

        public string NavigationUrl
        {
            get; set;
        }

        //public Type DataType { get; set; }
        //public bool IsRequired { get; set; }

        public dynamic Value { get; set; }

    }
    public enum DisplayFieldType
    {
        Label=1,
        HyperLink=2

    }
    public class DisplayField
    {
        public string FieldName { get; set; }
        public string FieldHeader { get; set; }

        public DisplayFieldType FieldType { get; set; }

        /// <summary>
        /// NavigationUrl can be either static or dynamic (eg: Test/Details?field1={0}&field2={1})
        /// </summary>
        public string NavigateUrl { get; set; }
        public string[] NavigateUrlFields { get; set; }


    }
    public class RowButton
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public Func<Field, bool> DisableWhen;
        

    }
    public class ConfirmButton:RowButton
    {
        public string ConfirmMessage { get; set; }
    }
    public class PopupButton :RowButton
    {
        public string PopupTitle { get; set; }
    }
    public class ActivateButton:ToggleButton
    {

        public ActivateButton()
        {
            Text = "Activate";
            ToggleText = "DeActivate";
            ConfirmMessage = "Do you want to activate (Y/N)?";
            ToggleConfirmMessage = "Do you want to Deactivate (Y/N)?";
            ToggleField = "Status";
            ToggleValue = "Active";
        }
    }
    public class DeleteButton:ConfirmButton
    {
        public DeleteButton()
        {
            ConfirmMessage = "Do you want to Delete(Y/N)?";
            Text = "Delete";
        }
    }
    public class ToggleButton :RowButton
    {

        public string ToggleField { get; set; }
        public dynamic ToggleValue { get; set; }
        public string ToggleText { get; set; }
        public string ConfirmMessage { get; set; }
        public string ToggleConfirmMessage { get; set; }
        
    }
    public class GridModel
    {


        #region "Properties"

        private string _id;
        public string Id {
            get {
                if (string.IsNullOrEmpty(_id))
                {
                    _id= "GSKGrid" + (++Sequence).ToString();
                    return _id;
                }
                else
                    return _id;
                
            }
                
            set {
                _id = value;
            }
        }
        private string _searchFor = "";
        public string SearchFor { get { return _searchFor; } set { _searchFor = value ?? ""; } }
        public int PageNo { get; set; }
        int _pageSize;
        bool _isPagesizeSet = false;
        
        public int PageSize
        {
            get { return _pageSize; }

            set
            {
              
                    _pageSize = value;
                _isPagesizeSet = true;
            }
        }

        private int[] _pageSizes;

        public int[] PageSizes
        { 

            get
            {
                return _pageSizes;
            }
            set
            {
                if (value == null ) throw new Exception("Invalid pagesizes");
                _pageSizes = value;
               if (!_isPagesizeSet)
                _pageSize = _pageSizes[0];
            }

        }
        public long TotalRowsCount { get; set; }
        public string SubmitUrl { get; set; }
        public string EditUrl { get; set; }
        public string DeleteUrl { get; set; }

        public bool IsEditPopup { get; set; }
        
        public GridOptions GridOptions { get; set; }


        private string _editTitle;
        public string EditTitle {
            get {
                if (string.IsNullOrEmpty(_editTitle))
                    return ("Edit " + Id).TrimEnd('s','S');
                else
                    return _editTitle;
                
            }
             set {
                _editTitle = value;
            }
        }

        private string _deleteTitle;
        public string DeleteTitle
        {
            get
            {
                if (string.IsNullOrEmpty(_editTitle))
                    return ("Delete " + Id).TrimEnd('s', 'S');
                else
                    return _deleteTitle;

            }
            set
            {
                _deleteTitle = value;
            }
        }

        private string _activateTitle;
        public string ActivateTitle
        {
            get
            {
                if (string.IsNullOrEmpty(_activateTitle))
                    return ("Activate " + Id).TrimEnd('s', 'S');
                else
                    return _activateTitle;

            }
            set
            {
                _activateTitle = value;
            }
        }

        private string _deactivateTitle;
        public string DeactivateTitle
        {
            get
            {
                if (string.IsNullOrEmpty(_deactivateTitle))
                    return ("De-Activate " + Id).TrimEnd('s', 'S');
                else
                    return _deactivateTitle;

            }
            set
            {
                _deactivateTitle = value;
            }
        }
      
        public string StatusFieldname { get; set; }
        public dynamic ActiveStatusValue { get; set; }
        public string  DeactivateUrl { get; set; }
        public string ActivateUrl { get; set; }


       
        private static int Sequence;

        internal GridData Data { get; private set; }
        #endregion
        public GridModel()
        {
            
            PageNo = 1;
            _pageSizes = new int[] { 30, 50, 75, 100, 200, 300 };

            GridOptions = new GridOptions() { Pagination = true, Search = true };
            _pageSize = 30;
            SearchFor = "";
            EditUrl = "Edit";
            DeleteUrl="Delete";
            ActivateUrl = "Activate";
            DeactivateUrl = "Deactivate";
            IsEditPopup = true;
        }

        public void SetData<T>(ICollection<T> collection, List<DisplayField> displayFields, string uniqueField) where T : class
        {
          
               
            Data = new GridData();
            Data.Init(collection, displayFields, uniqueField);
        }

        public void SetData(List<Row> rows, string[] headers )
        {
            Data = new GridData(rows);
            foreach (string h in headers)
                Data.Headers.Add(h);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="headers">Header labels seperated with coma (,)</param>
        /// <param name="pagesizes"></param>
        //public void SetData<T>(ICollection<T> collection, List<DisplayField> displayFields, string uniqueField, int[] pagesizes) where T : class
        //{
        //    if (pagesizes == null) throw new Exception("pagesize is empty");
        //    if (collection == null) throw new Exception("collection is empty");

        //    PageSizes = pagesizes.ToList();
        //    if (pagesizes.Count() == 0) throw new Exception("pagesizes is empty");
        //    PageSize = PageSizes[0];
        //    SetData(collection, displayFields,uniqueField);
        //}




       
        internal class GridData : List<Row>   
        {
           

            internal List<string> Headers
            {
                get; private set;
            }

            public void Init<T>(ICollection<T> collection, List<DisplayField> displayFields, string uniqueField) 
            {
                Headers = new List<string>();
                displayFields.ForEach(item => Headers.Add(item.FieldHeader));

                foreach (var item in collection)
                {
                    Row r = new Row();
                    PropertyInfo[] properties = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    displayFields.ForEach(displayField =>
                    {
                        PropertyInfo pi = properties.FirstOrDefault(property => property.Name == displayField.FieldName);
                        if (pi != null)
                        {
                            Field f = new Field() { Name = pi.Name, Value = pi.GetValue(item) };
                            if (displayField.FieldType == DisplayFieldType.HyperLink)
                            {
                                List<string> nfValues = new List<string>();
                                if (displayField.NavigateUrlFields == null && uniqueField != null)
                                {
                                    if (displayField.NavigateUrl.Contains("?"))
                                        displayField.NavigateUrl += "&id=" + uniqueField;
                                    else
                                        displayField.NavigateUrl += "?id=" + uniqueField;

                                }
                                else if (displayField.NavigateUrlFields!=null)
                                {
                                    foreach (var nfName in displayField.NavigateUrlFields)
                                    {
                                        pi = properties.FirstOrDefault(property => property.Name == nfName);
                                        nfValues.Add(Convert.ToString(pi.GetValue(item)));
                                    }
                                }
                                
                                f.NavigationUrl = string.Format(displayField.NavigateUrl, nfValues.ToArray());
                            }
                            r.Fields.Add(f);
                        }
                        else
                            throw new Exception("One or more display fields are not found in collection");
                    });
                    PropertyInfo piUnique = properties.FirstOrDefault(property => property.Name == uniqueField);
                    if (piUnique == null) throw new Exception("Invalid unique field");
                    r.RowId = piUnique.GetValue(item).ToString();
                    this.Add(r);
                }

            }
            internal GridData()
            {
                Headers = new List<string>();
            }

            internal GridData(IEnumerable<Row> rows):base(rows)
            {
                Headers = new List<string>();
            }
        }


    }
}