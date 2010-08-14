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


Beelun.shoppro.admin.model.tabMap = {

    getTabMap: function() {
    	var tabMap = {};
    	
    	var tabMapIdEle = YAHOO.util.Dom.get('tabMapId');
    	tabMap.id = tabMapIdEle.value;
		
		var tabEle = YAHOO.util.Dom.get('tab');
		//tabMap.tab = tabEle.value;
		//var tabName = tabEle.options[tabEle.selectedIndex].text;
		var tabId = tabEle.options[tabEle.selectedIndex].value;
		tabMap.tab = {id:tabId};
		
		var categoryEle = YAHOO.util.Dom.get('category');
		//tabMap.category = categoryEle.value;
		
		var categoryId = categoryEle.options[categoryEle.selectedIndex].value;
		tabMap.category = {id:categoryId};
		
		var categoryOrderEle = YAHOO.util.Dom.get('categoryOrder');
		tabMap.categoryOrder = categoryOrderEle.value;
				
		return tabMap;
                
    },
    
    setTabMap: function(tabMap) {
    	
    	var tabMapId = tabMap.id;
		var tab = tabMap.tab;
		var category = tabMap.category;
		var categoryOrder = tabMap.categoryOrder;
		
    	var tabMapIdEle = YAHOO.util.Dom.get('tabMapId');
    	tabMapIdEle.value = tabMapId;
		
		//var tabEle = YAHOO.util.Dom.get('tab');
		//tabEle.value = tab;
		
		var handleTabCallback = function(tabList) {
	    	var tabEle = YAHOO.util.Dom.get('tab');
	    	for(var i=0; i<tabEle.options.length; i)
	    	{
	    		tabEle.remove(i);
    		}
	    	for(var i=0; i<tabList.length; i++)
	    	{
	    		var op = document.createElement("option");
	    		op.text = tabList[i].name;
	    		op.value = tabList[i].id;
	    	    if(op.text == tab.name)
	    	    {
	    	    	op.selected = "selected";
	    	    }
	    	    try
	    	    {
	    	    	tabEle.add(op,null); // standards compliant
	    	    }
	    	    catch(ex)
	    	    {
	    	    	tabEle.add(op); // IE only
	    	    }
	    	}
		   
	    };
	    
		TabManager.getAll(handleTabCallback);
		
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
		
		var categoryOrderEle = YAHOO.util.Dom.get('categoryOrder');
		categoryOrderEle.value = categoryOrder;
		
    },
    
    clearTabMap: function() {
    	
    	var tabMapIdEle = YAHOO.util.Dom.get('tabMapId');
    	tabMapIdEle.value = "";
		
		//var tabEle = YAHOO.util.Dom.get('tab');
		//tabEle.value = tab;
		
		var handleTabCallback = function(tabList) {
	    	var tabEle = YAHOO.util.Dom.get('tab');
	    	for(var i=0; i<tabEle.options.length; i)
	    	{
	    		tabEle.remove(i);
    		}
	    	for(var i=0; i<tabList.length; i++)
	    	{
	    		var op = document.createElement("option");
	    		op.text = tabList[i].name;
	    		op.value = tabList[i].id;
	    	    try
	    	    {
	    	    	tabEle.add(op,null); // standards compliant
	    	    }
	    	    catch(ex)
	    	    {
	    	    	tabEle.add(op); // IE only
	    	    }
	    	}
		   
	    };
	    
		TabManager.getAll(handleTabCallback);
		
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
		
		var categoryOrderEle = YAHOO.util.Dom.get('categoryOrder');
		categoryOrderEle.value = "";
		
    },
    
    initTabMapTable: function()
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
		
    	var initTabMapTable = function()
		{
    		updatingPanel.show();
        	Tab2CategoryMapManager.getAll(fnGetTabMapHandleSuccess);
    		
		};
		
		var fnGetTabMapHandleSuccess = function(tabMap)
		{
		
			updatingPanel.hide();
		
			/*
	         * Initial editItemDialog Dialog
	 		 */
			
	    	// Define various event handlers for Dialog
	    	var handleSubmit = function() {
	    		
	    		var tabMap = Beelun.shoppro.admin.model.tabMap.getTabMap();
	
				var fnSaveBack = function(item)
				{
					updatingPanel.hide();
					window.location.reload();
				};
				Tab2CategoryMapManager.saveLazy(tabMap, fnSaveBack);
				editTabMapDialog.hide();
				updatingPanel.show();
	    	};
	    	var handleCancel = function() {
	    		this.cancel();
	    	};
	    	
	    	// Instantiate the Dialog
	    	var editTabMapDialog = new YAHOO.widget.Dialog("editTabMapDialog", 
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
	    	editTabMapDialog.render();
	    	YAHOO.util.Dom.removeClass("editTabMapDialog", "hide");
	    	var updateTabMapDialog = function(record){
	    		var item = record._oData;
	    		Beelun.shoppro.admin.model.tabMap.setTabMap(item);
	    		editTabMapDialog.show();
	    	}
	    	var  editTabMapClick = function(o){
	    		var oRecord = this.getRecord(o.target);
	    		updateTabMapDialog(oRecord);

	        };
			
			/*
	         * Initial user datatable
	 		 */
			
			var tabFormatter = function(elLiner, oRecord, oColumn, oData) {
		        elLiner.innerHTML = oRecord.getData("tab").name;   
		
			};
			
			var categoryFormatter = function(elLiner, oRecord, oColumn, oData) {
		        elLiner.innerHTML = oRecord.getData("category").name;   
		
			};
			
			var myColumnDefs = [
	            {key:"id", sortable:true, resizeable:true},
				{key:"tab", formatter: tabFormatter, sortable:true, resizeable:true},
				{key:"category", formatter: categoryFormatter, sortable:true, resizeable:true},
				{key:"categoryOrder", editor:"textbox", sortable:true, resizeable:true},
				{key:"edit", formatter: "button"}
	        ];
	
	        var myDataSource = new YAHOO.util.DataSource(tabMap);
	        myDataSource.responseType = YAHOO.util.DataSource.TYPE_JSARRAY;
	        myDataSource.responseSchema = {
	            fields: [
	            {key: "id"},{key: "tab"},{key: "category"},{key: "categoryOrder"}
				]
	        };
	
	        var myDataTable = new YAHOO.widget.ScrollingDataTable("tabMapTable",
	                myColumnDefs, myDataSource, {width:"900px"});
	        
	        myDataTable.subscribe("rowClickEvent",myDataTable.onEventSelectRow);
	        myDataTable.subscribe("cellDblclickEvent",myDataTable.onEventShowCellEditor);
	        myDataTable.subscribe("editorBlurEvent", myDataTable.onEventSaveCellEditor);
	        myDataTable.subscribe("rowMouseoverEvent", myDataTable.onEventHighlightRow);
        	myDataTable.subscribe("rowMouseoutEvent", myDataTable.onEventUnhighlightRow);
        	myDataTable.subscribe("buttonClickEvent", editTabMapClick);
	        
        	var onCellEdit = function(oArgs) {
            	
        		var elCell = oArgs.editor.getTdEl();
	            var oOldData = oArgs.oldData;
	            var oNewData = oArgs.newData;
	            
				var oTabMap = oArgs.editor._oRecord._oData;
	            var tabMap = {
	            	id: oTabMap.id, 
	            	tab: oTabMap.tab, 
	            	category: oTabMap.category, 
	            	categoryOrder: oTabMap.categoryOrder};
		
				var fnSaveItemBack = function(item)
	        	{
	        		alert("Update Done!");
	        	};
	        	Tab2CategoryMapManager.save(tabMap, fnSaveItemBack);
	        };
        	myDataTable.subscribe("editorSaveEvent", onCellEdit);
        	
        	var addTabMapClick = function()
	    	{
	    		Beelun.shoppro.admin.model.tabMap.clearTabMap();
	    		editTabMapDialog.show();
	    	};
	    	
	    	YAHOO.util.Event.addListener("addTabMap", "click", addTabMapClick);

		};
		
        YAHOO.util.Event.onContentReady("tabMapTable", function () {
            
        	initTabMapTable();
        });
    }

}
    

