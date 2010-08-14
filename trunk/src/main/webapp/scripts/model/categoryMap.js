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


Beelun.shoppro.admin.model.categoryMap = {

    getCategoryMap: function() {
    	var categoryMap = {};
    	
    	var categoryMapIdEle = YAHOO.util.Dom.get('categoryMapId');
    	categoryMap.id = categoryMapIdEle.value;
		
		var categoryEle = YAHOO.util.Dom.get('category');
		//tabMap.category = categoryEle.value;
		
		var categoryId = categoryEle.options[categoryEle.selectedIndex].value;
		categoryMap.category = {id:categoryId};
		
		var itemEle = YAHOO.util.Dom.get('item');
		//categoryMap.item = itemEle.value;
		
		var itemId = categoryEle.options[itemEle.selectedIndex].value;
		categoryMap.item = {id:itemId};
		
		var itemOrderEle = YAHOO.util.Dom.get('itemOrder');
		categoryMap.itemOrder = itemOrderEle.value;
				
		return categoryMap;
                
    },
    
    setCategoryMap: function(categoryMap) {
    	
    	var categoryMapId = categoryMap.id;
		var category = categoryMap.category;
		var item = categoryMap.item;
		var itemOrder = categoryMap.itemOrder;
		
    	var categoryMapIdEle = YAHOO.util.Dom.get('categoryMapId');
    	categoryMapIdEle.value = categoryMapId;
		
		//var categoryEle = YAHOO.util.Dom.get('category');
		//categoryEle.value = category;
		
		var handleCategoryCallback = function(categoryList) {
	    	var categoryEle = YAHOO.util.Dom.get('category');
	    	for(var i=0; i<categoryEle.options.length; i)
	    	{
	    		categoryEle.remove(i);
    		}
	    	for(var i=0; i<categoryList.length; i++)
	    	{
	    		var op = document.createElement("option");
	    		op.text = categoryList[i].name;
	    		op.value = categoryList[i].id;
	    	    if(op.text == category.name)
	    	    {
	    	    	op.selected = "selected";
	    	    }
	    	    try
	    	    {
	    	    	categoryEle.add(op,null); // standards compliant
	    	    }
	    	    catch(ex)
	    	    {
	    	    	categoryEle.add(op); // IE only
	    	    }
	    	}
		   
	    };
	    
		CategoryManager.getAll(handleCategoryCallback);
		
		//var itemEle = YAHOO.util.Dom.get('item');
		//itemEle.value = item;
		
		var handleItemCallback = function(itemList) {
	    	var itemEle = YAHOO.util.Dom.get('item');
	    	for(var i=0; i<itemEle.options.length; i)
	    	{
	    		itemEle.remove(i);
    		}
	    	for(var i=0; i<itemList.length; i++)
	    	{
	    		var op = document.createElement("option");
	    		op.text = itemList[i].name;
	    		op.value = itemList[i].id;
	    	    if(op.text == item.name)
	    	    {
	    	    	op.selected = "selected";
	    	    }
	    	    try
	    	    {
	    	    	itemEle.add(op,null); // standards compliant
	    	    }
	    	    catch(ex)
	    	    {
	    	    	itemEle.add(op); // IE only
	    	    }
	    	}
		   
	    };
	    
		ItemManager.getAll(handleItemCallback);
		
		var itemOrderEle = YAHOO.util.Dom.get('itemOrder');
		itemOrderEle.value = itemOrder;
		
    },
    
    clearCategoryMap: function() {
    		
    	var categoryMapIdEle = YAHOO.util.Dom.get('categoryMapId');
    	categoryMapIdEle.value = "";
		
		//var categoryEle = YAHOO.util.Dom.get('category');
		//categoryEle.value = category;
		
		var handleCategoryCallback = function(categoryList) {
	    	var categoryEle = YAHOO.util.Dom.get('category');
	    	for(var i=0; i<categoryEle.options.length; i)
	    	{
	    		categoryEle.remove(i);
    		}
	    	for(var i=0; i<categoryList.length; i++)
	    	{
	    		var op = document.createElement("option");
	    		op.text = categoryList[i].name;
	    		op.value = categoryList[i].id;
	    	    try
	    	    {
	    	    	categoryEle.add(op,null); // standards compliant
	    	    }
	    	    catch(ex)
	    	    {
	    	    	categoryEle.add(op); // IE only
	    	    }
	    	}
		   
	    };
	    
		CategoryManager.getAll(handleCategoryCallback);
		
		//var itemEle = YAHOO.util.Dom.get('item');
		//itemEle.value = item;
		
		var handleItemCallback = function(itemList) {
	    	var itemEle = YAHOO.util.Dom.get('item');
	    	for(var i=0; i<itemEle.options.length; i)
	    	{
	    		itemEle.remove(i);
    		}
	    	for(var i=0; i<itemList.length; i++)
	    	{
	    		var op = document.createElement("option");
	    		op.text = itemList[i].name;
	    		op.value = itemList[i].id;
	    	    try
	    	    {
	    	    	itemEle.add(op,null); // standards compliant
	    	    }
	    	    catch(ex)
	    	    {
	    	    	itemEle.add(op); // IE only
	    	    }
	    	}
		   
	    };
	    
		ItemManager.getAll(handleItemCallback);
		
		var itemOrderEle = YAHOO.util.Dom.get('itemOrder');
		itemOrderEle.value = "";
		
    },
    
    initCategoryMapTable: function()
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
    	
    	var initCategoryMapTable = function()
		{
    		updatingPanel.show();
        	Category2ItemMapManager.getAll(fnGetCategoryMapHandleSuccess);
    		
		};
		
		var fnGetCategoryMapHandleSuccess = function(categoryMap)
		{
		
			
			updatingPanel.hide();
			/*
	         * Initial editItemDialog Dialog
	 		 */
			
	    	// Define various event handlers for Dialog
	    	var handleSubmit = function() {
	    		
	    		var categoryMap = Beelun.shoppro.admin.model.categoryMap.getCategoryMap();
	
				var fnSaveBack = function(item)
				{
					updatingPanel.hide();
					window.location.reload();
				};
				Category2ItemMapManager.saveLazy(categoryMap, fnSaveBack);
				editcategoryMapDialog.hide();
				updatingPanel.show();
	    	};
	    	var handleCancel = function() {
	    		this.cancel();
	    	};
	    	
	    	// Instantiate the Dialog
	    	var editCategoryMapDialog = new YAHOO.widget.Dialog("editCategoryMapDialog", 
	    							{ width : "600px",
									  fixedcenter : false,
									  visible : false,
									  x:300,
									  y:100,
	    							  modal: true,
	    							  constraintoviewport : true,
	    							  buttons : [ { text:"Save", handler:handleSubmit, isDefault:true },
	    								      { text:"Cancel", handler:handleCancel } ]
	    							});
	    	// Render the Dialog
	    	editCategoryMapDialog.render();
	    	YAHOO.util.Dom.removeClass("editCategoryMapDialog", "hide");
	    	var updateCategoryMapDialog = function(record){
	    		var item = record._oData;
	    		Beelun.shoppro.admin.model.categoryMap.setCategoryMap(item);
	    		editCategoryMapDialog.show();
	    	}
	    	var  editCategoryMapClick = function(o){
	    		var oRecord = this.getRecord(o.target);
	    		updateCategoryMapDialog(oRecord);

	        };
			
			/*
	         * Initial user datatable
	 		 */
			
			var categoryFormatter = function(elLiner, oRecord, oColumn, oData) {
		        elLiner.innerHTML = oRecord.getData("category").name;   
		
			};
			
			var itemFormatter = function(elLiner, oRecord, oColumn, oData) {
		        elLiner.innerHTML = oRecord.getData("item").name;   
		
			};
			
			var myColumnDefs = [
	            {key:"id", sortable:true, resizeable:true},
				{key:"category", formatter:categoryFormatter, sortable:true, resizeable:true},
				{key:"item", formatter:itemFormatter, sortable:true, resizeable:true},
				{key:"itemOrder", editor:"textbox", sortable:true, resizeable:true},
				{key:"edit", formatter: "button"}
	        ];
	
	        var myDataSource = new YAHOO.util.DataSource(categoryMap);
	        myDataSource.responseType = YAHOO.util.DataSource.TYPE_JSARRAY;
	        myDataSource.responseSchema = {
	            fields: [
	            {key: "id"},{key: "category"},{key: "item"},{key: "itemOrder"}
				]
	        };
	
	        var myDataTable = new YAHOO.widget.ScrollingDataTable("categoryMapTable",
	                myColumnDefs, myDataSource, {width:"900px"});
	        
	        myDataTable.subscribe("rowClickEvent",myDataTable.onEventSelectRow);
	        myDataTable.subscribe("cellDblclickEvent",myDataTable.onEventShowCellEditor);
	        myDataTable.subscribe("editorBlurEvent", myDataTable.onEventSaveCellEditor);
	        myDataTable.subscribe("rowMouseoverEvent", myDataTable.onEventHighlightRow);
        	myDataTable.subscribe("rowMouseoutEvent", myDataTable.onEventUnhighlightRow);
        	myDataTable.subscribe("buttonClickEvent", editCategoryMapClick);
	        
        	var onCellEdit = function(oArgs) {
            	
        		var elCell = oArgs.editor.getTdEl();
	            var oOldData = oArgs.oldData;
	            var oNewData = oArgs.newData;
	            
				var oCategoryMap = oArgs.editor._oRecord._oData;
	            var categoryMap = {
	            	id: oCategoryMap.id, 
	            	category: oCategoryMap.category, 
	            	item: oCategoryMap.item, 
	            	itemOrder: oCategoryMap.itemOrder};
		
				var fnSaveItemBack = function(item)
	        	{
	        		alert("Update Done!");
	        	};
	        	Category2ItemMapManager.save(categoryMap, fnSaveItemBack);
	        };
        	myDataTable.subscribe("editorSaveEvent", onCellEdit);
        	
        	var addCategoryMapClick = function()
	    	{
	    		Beelun.shoppro.admin.model.categoryMap.clearCategoryMap();
	    		editCategoryMapDialog.show();
	    	};
	    	
	    	YAHOO.util.Event.addListener("addCategoryMap", "click", addCategoryMapClick);

		};
		
        YAHOO.util.Event.onContentReady("categoryMapTable", function () {
            
        	initCategoryMapTable();
        });
    }

}
    

