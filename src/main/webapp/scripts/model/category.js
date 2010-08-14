//
//  Copyright (c) 2008-2009 by Beelun, Inc.
//  
// The information contained herein is confidential, proprietary to Beelun,
// Inc., and considered a trade secret as defined in section 499C of the
// penal code of the Shanghai. Use of this information by anyone
// other than authorized employees of Beelun, Inc. is granted only under a
// written non-disclosure agreement, expressly prescribing the scope and
// manner of such use.
//


Beelun.shoppro.admin.model.category = {

    getCategory: function() {
    	var category = {};
    	
    	var categoryIdEle = YAHOO.util.Dom.get('categoryId');
    	category.id = categoryIdEle.value;
		
		var nameEle = YAHOO.util.Dom.get('name');
		category.name = nameEle.value;
		
		var isShownEle = document.getElementsByName("isShown");
		if (isShownEle[0].checked)
		{
			category.isShown = isShownEle[0].value;
		}
		else
		{
			category.isShown = isShownEle[1].value;
		}
		
		//var itemMapEle = YAHOO.util.Dom.get('itemMap');
		//category.itemMap = itemMapEle.value;
		
		var pageTitleEle = YAHOO.util.Dom.get('pageTitle');
		category.pageTitle = pageTitleEle.value;
		
		var keywordsEle = YAHOO.util.Dom.get('keywords');
		category.keywords = keywordsEle.value;
		
		var descriptionEle = YAHOO.util.Dom.get('description');
		category.description = descriptionEle.value;
		
		var metaTagEle = YAHOO.util.Dom.get('metaTag');
		category.metaTag = metaTagEle.value;
		
		var urlEle = YAHOO.util.Dom.get('url');
		category.url = urlEle.value;
		
		return category;
                
    },
    
    setCategory: function(category) {
    	
    	var categoryId = category.id;
		var name = category.name;
		var isShown = category.isShown;
		var itemMap = category.itemMap;
		var pageTitle = category.pageTitle;
		var keywords = category.keywords;
		var description = category.description;
		var metaTag = category.metaTag;
		var url = category.url;
		
		var categoryIdEle = YAHOO.util.Dom.get('categoryId');
    	categoryIdEle.value = categoryId;
		
		var nameEle = YAHOO.util.Dom.get('name');
		nameEle.value = name;
		
		var isShownEle = document.getElementsByName("isShown");
		if (isShown == true || isShown == "true")
		{
			isShownEle[0].checked = "true";
		}
		else
		{
			isShownEle[1].checked = "true";
		}
		
		//var itemMapEle = YAHOO.util.Dom.get('itemMap');
		//itemMapEle.value = itemMap;
		
		var pageTitleEle = YAHOO.util.Dom.get('pageTitle');
		pageTitleEle.value = pageTitle;
		
		var keywordsEle = YAHOO.util.Dom.get('keywords');
		keywordsEle.value = keywords;
		
		var descriptionEle = YAHOO.util.Dom.get('description');
		descriptionEle.value = description;
		
		var metaTagEle = YAHOO.util.Dom.get('metaTag');
		metaTagEle.value = metaTag;
		
		var urlEle = YAHOO.util.Dom.get('url');
		urlEle.value = url;
    },
    
    clearCategory: function() {
		
		var categoryIdEle = YAHOO.util.Dom.get('categoryId');
    	categoryIdEle.value = "";
		
		var nameEle = YAHOO.util.Dom.get('name');
		nameEle.value = "";
		
		var isShownEle = document.getElementsByName("isShown");
		isShownEle[0].checked = "true";
		
		//var itemMapEle = YAHOO.util.Dom.get('itemMap');
		//itemMapEle.value = itemMap;
		
		var pageTitleEle = YAHOO.util.Dom.get('pageTitle');
		pageTitleEle.value = "";
		
		var keywordsEle = YAHOO.util.Dom.get('keywords');
		keywordsEle.value = "";
		
		var descriptionEle = YAHOO.util.Dom.get('description');
		descriptionEle.value = "";
		
		var metaTagEle = YAHOO.util.Dom.get('metaTag');
		metaTagEle.value = "";
		
		var urlEle = YAHOO.util.Dom.get('url');
		urlEle.value = "";
    },
    
    initCategoryTable: function()
    {
    	/*
		 * Initial Loading dialog
		 */
		

		
		var updatingPanel = 
			new YAHOO.widget.Panel("updatingPanel",  
											{ width: "240px", 
											  fixedcenter: true, 
											  close: false, 
											  draggable: false, 
											  zindex:4,
											  modal: true,
											  visible: false
											} 
										);

		updatingPanel.setHeader("Updating Item, please wait...");
		updatingPanel.setBody("<img src=\"/images/loading.gif\"/>");
		updatingPanel.render(document.body);
		
        var initCategoryTable = function()
		{
        	updatingPanel.show();
        	CategoryManager.getAll(fnGetCategoryHandleSuccess);
    		
		};
		
		var fnGetCategoryHandleSuccess = function(category)
		{
		
			updatingPanel.hide();
		
			/*
	         * Initial editItemDialog Dialog
	 		 */
			
	    	// Define various event handlers for Dialog
	    	var handleSubmit = function() {
	    		
	    		var category = Beelun.shoppro.admin.model.category.getCategory();
	
				var fnSaveBack = function(item)
				{
					updatingPanel.hide();
					window.location.reload();
				};
				CategoryManager.save(category, fnSaveBack);
				editCategoryDialog.hide();
				updatingPanel.show();
	    	};
	    	var handleCancel = function() {
	    		this.cancel();
	    	};
	    	
	    	// Instantiate the Dialog
	    	var editCategoryDialog = new YAHOO.widget.Dialog("editCategoryDialog", 
	    							{ width : "600px",
						    		  fixedcenter : false,
									  visible : false,
									  x:300,
									  y:100,
									  draggable: true,
	    							  modal: true,
	    							  constraintoviewport : true,
	    							  buttons : [ { text:"Save", handler:handleSubmit, isDefault:true },
	    								      { text:"Cancel", handler:handleCancel } ]
	    							});
	    	// Render the Dialog
	    	editCategoryDialog.render();
	    	YAHOO.util.Dom.removeClass("editCategoryDialog", "hide");
	    	var updateCategoryDialog = function(record){
	    		var item = record._oData;
	    		Beelun.shoppro.admin.model.category.setCategory(item);
	    		editCategoryDialog.show();
	    	}
	    	var  editCategoryClick = function(o){
				var oRecord = this.getRecord(o.target);
				var oColumn = this.getColumn(o.target);
				
				var fnDeleteItemBack = function(value)
				{
					if(value)
					{
						window.location.reload();
					}
					else
					{
						alert("You can't delete this item!");
					}
				};
				if (oColumn.key == "delete") {
					if(confirm("Do you want to delete this item?"))
					{
						CategoryManager.remove(oRecord.getData("id"), fnDeleteItemBack);
					}
				}
				else if(oColumn.key == "edit")
				{
					updateCategoryDialog(oRecord);
				}
	        };
			
			/*
	         * Initial user datatable
	 		 */
			
			var brandFormatter = function(elLiner, oRecord, oColumn, oData) {
		        elLiner.innerHTML = oRecord.getData("brand").name;   
		
			};
			
			var myColumnDefs = [
	            {key:"id", sortable:true, resizeable:true},
				{key:"name", editor:"textbox", sortable:true, resizeable:true},
				{key:"isShown", editor:new YAHOO.widget.RadioCellEditor({radioOptions:["true","false"],disableBtns:false}), editor:"textbox", sortable:true, resizeable:true},
				{key:"itemMap", editor:"itemMap", hidden:true, sortable:true, resizeable:true},
				{key:"pageTitle", editor:"textbox", sortable:true, resizeable:true},
				{key:"keywords", editor:"textbox", sortable:true, resizeable:true},
				{key:"description", editor:"textbox", sortable:true, resizeable:true},
				{key:"metaTag", editor:"textbox", sortable:true, resizeable:true},
				{key:"url", editor:"textbox", sortable:true, resizeable:true},
				{key:"edit", formatter: "button"},
				{key:"delete", formatter: "button"}
	        ];
	
	        var myDataSource = new YAHOO.util.DataSource(category);
	        myDataSource.responseType = YAHOO.util.DataSource.TYPE_JSARRAY;
	        myDataSource.responseSchema = {
	            fields: [
	            {key: "id"},{key: "name"},{key: "isShown"},{key: "itemMap"},
				{key: "pageTitle"},{key: "keywords"},{key: "description"},{key: "metaTag"},
				{key: "url"}
				]
	        };
	
	        var myDataTable = new YAHOO.widget.ScrollingDataTable("categoryTable",
	                myColumnDefs, myDataSource, {width:"900px"});
	        
	        myDataTable.subscribe("rowClickEvent",myDataTable.onEventSelectRow);
	        myDataTable.subscribe("cellDblclickEvent",myDataTable.onEventShowCellEditor);
	        myDataTable.subscribe("editorBlurEvent", myDataTable.onEventSaveCellEditor);
	        myDataTable.subscribe("rowMouseoverEvent", myDataTable.onEventHighlightRow);
        	myDataTable.subscribe("rowMouseoutEvent", myDataTable.onEventUnhighlightRow);
        	myDataTable.subscribe("buttonClickEvent", editCategoryClick);
	        
        	var onCellEdit = function(oArgs) {
            	
        		var elCell = oArgs.editor.getTdEl();
	            var oOldData = oArgs.oldData;
	            var oNewData = oArgs.newData;
	            
				var oCategory = oArgs.editor._oRecord._oData;
	            var category = {
	            	id: oCategory.id, 
	            	name: oCategory.name, 
	            	isShown: oCategory.isShown, 
	            	//itemMap: oCategory.itemMap,
					pageTitle: oCategory.pageTitle,
	            	keywords: oCategory.keywords,
	            	description: oCategory.description,
	            	metaTag: oCategory.metaTag,
	            	url: oCategory.url};
		
				var fnSaveItemBack = function(item)
	        	{
	        		alert("Update Done!");
	        	};
	        	CategoryManager.save(category, fnSaveItemBack);
	        };
        	myDataTable.subscribe("editorSaveEvent", onCellEdit);
        	
        	var addCategoryClick = function()
	    	{
	    		Beelun.shoppro.admin.model.category.clearCategory();
	    		editCategoryDialog.show();
	    	};
	    	
	    	YAHOO.util.Event.addListener("addCategory", "click", addCategoryClick);

		};
		
        YAHOO.util.Event.onContentReady("categoryTable", function () {
            
        	initCategoryTable();
        });
    }

}
    

