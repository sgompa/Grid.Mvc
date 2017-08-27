Create below list of actions in a controller

----------------------------------------------------------------------
public ActionResult GetProducts(GridModel model)   /// Here action name "GetProducts" is to be replaced with appropriate action name
{
    //Logic to get the current page records and Total record count 

	//assign Current Page Rectords to lst and total rows count to model.TotalRowsCount
	
	var lst=  MethodToGetProducts....        ///based on model.PageSize,model.PageNo,model.SearchFor
	model.TotalRowsCount=<total record count>;


	List<DisplayField> fields = new List<DisplayField>()
    {

        new DisplayField() { FieldName="ProductCode", FieldHeader="Product Code" },    //fieldname and fieldheader based on list returned from business method
        new DisplayField() { FieldName="Price", FieldHeader="Price" },
		new DisplayField() { FieldName="ProductName", FieldHeader="Title", FieldType=DisplayFieldType.HyperLink, NavigateUrl="Product/Details?id={0}", NavigateUrlFields=new string[] {"ProductCode" } }
      ------


    };

	model.Id="Products";
	model.SetData(lst, fields,<UniqueFieldName>);   // uniquefieldname is the name of uniq field in list
	
	model.GridOptions.HeaderButtons = new HeaderButton[]
            {
                new PlusButton () { PopupTitle="Create Test", Url="/Admin/Test/AddTest" }

            };
   
    model.GridOptions.RowButtos = new RowButton[]
            {
                new ActivateButton() { Url ="/Product/ChangeStatus" },
                new PopupButton()    {Text="Edit",Url="/Product/Edit", PopupTitle="Editing Product" },
                new DeleteButton()   { Url="/Product/Delete"}
            };
            
			
	return PartialView("GSKGridView", model);
}


//----------------------------------------------------------------------------------------------
//Below Two Actions should be written as mentioned below only when Edit Popup is not turned off


public ActionResult Edit(long id)   
{
	try
	{
		//logic to get record
		return PartialView(<some edit model>);
	}
	catch(Exception ex)
	{
		return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed, ex.Message);

	}
}

[HttpPost]
public ActionResult Edit(<modelname> model )   
{
	try
	{
		//---logic to Validate Input and Update 
		
		 return Json(new { result = "Success", message = "Successfully Updated" });
	}
	catch(Exception ex)
	{
		//----
		return View(model);
	}

	
}

//-----------------------------------------------------------------------------


[HttpPost]
public ActionResult Delete(long id)
{
      try {

				//Logic to delete
               //.....
			   return Json(new { result = "Success", message = "Successfully Deleted" });
        }
        catch(Exception ex)
        {
                return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed, ex.Message);
        }

}


//-----------------------------------------------------------------------------------------------------
//Below Two action required only when you turn on Activate and Deactivate Links


[HttpPost]
public ActionResult ChangeStatus(long id, string value)
{
      try {

				//Logic to Activate/ Deactivate
               //.....
			   return Json(new { result = "Success", message = "Successfully Activated" });
        }
        catch(Exception ex)
        {
                return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed, ex.Message);
        }

}

//-------------------------------------------------------------------------------------------------------------------

2. Render Grid in View as below
 @{
    Html.RenderAction("GetProducts", "Product");
  }

3. add following script tag either in view or layout
  <script src="~/Scripts/gskgrid-min.js"></script>
   <script src="~/Scripts/jquery.form.min.js"></script>