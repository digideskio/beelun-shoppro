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


Beelun.shoppro.admin.model.tab = {

    getTab: function() {
    	var tab = {};
    	
    	var tabIdEle = YAHOO.util.Dom.get('tabId');
    	tab.id = tabIdEle.value;
		
		var nameEle = YAHOO.util.Dom.get('name');
		tab.name = nameEle.value;
		
		var isShownEle = document.getElementsByName("isShown");
		if (isShownEle[0].checked)
		{
			tab.isShown = isShownEle[0].value;
		}
		else
		{
			tab.isShown = isShownEle[1].value;
		}
		
		var contentEle = YAHOO.util.Dom.get('tabContent');
		tab.content = contentEle.value;
		
		var showOrderEle = YAHOO.util.Dom.get('showOrder');
		tab.showOrder = showOrderEle.value;
		
		//var categoryMapEle = YAHOO.util.Dom.get('categoryMap');
		//tab.categoryMap = categoryMapEle.value;
		
		var pageTitleEle = YAHOO.util.Dom.get('pageTitle');
		tab.pageTitle = pageTitleEle.value;
		
		var keywordsEle = YAHOO.util.Dom.get('keywords');
		tab.keywords = keywordsEle.value;
		
		var descriptionEle = YAHOO.util.Dom.get('description');
		tab.description = descriptionEle.value;
		
		var metaTagEle = YAHOO.util.Dom.get('metaTag');
		tab.metaTag = metaTagEle.value;
		
		var urlEle = YAHOO.util.Dom.get('url');
		tab.url = urlEle.value;
		
		return tab;
                
    },
    
    setTab: function(tab) {
    	
    	var tabId = tab.id;
		var name = tab.name;
		var isShown = tab.isShown;
		var content = tab.content;
		var showOrder = tab.showOrder;
		//var categoryMap = tab.categoryMap;
		var pageTitle = tab.pageTitle;
		var keywords = tab.keywords;
		var description = tab.description;
		var metaTag = tab.metaTag;
		var url = tab.url;
		
		var tabIdEle = YAHOO.util.Dom.get('tabId');
    	tabIdEle.value = tabId;
		
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
		
		var contentEle = YAHOO.util.Dom.get('tabContent');
		contentEle.value = content;
		
		var showOrderEle = YAHOO.util.Dom.get('showOrder');
		showOrderEle.value = showOrder;
		
		//var categoryMapEle = YAHOO.util.Dom.get('categoryMap');
		//categoryMapEle.value = categoryMap;
		
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
    
    clearTab: function() {
    		
		var tabIdEle = YAHOO.util.Dom.get('tabId');
    	tabIdEle.value = "";
		
		var nameEle = YAHOO.util.Dom.get('name');
		nameEle.value = "";
		
		var isShownEle = document.getElementsByName("isShown");
		isShownEle[0].checked = "true";
		
		var contentEle = YAHOO.util.Dom.get('tabContent');
		contentEle.value = "";
		
		var showOrderEle = YAHOO.util.Dom.get('showOrder');
		showOrderEle.value = "";
		
		//var categoryMapEle = YAHOO.util.Dom.get('categoryMap');
		//categoryMapEle.value = categoryMap;
		
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
    
    initTabListTable: function()
    {
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
		
    	var initTabTable = function()
		{
    		updatingPanel.show();
        	TabManager.getAll(fnGetTabHandleSuccess);
    		
		};
		
		var fnGetTabHandleSuccess = function(tab)
		{
		
			/*
			 * Initial Loading dialog
			 */
			
			 updatingPanel.hide();

			/*
	         * Initial editItemDialog Dialog
	 		 */
			
	    	// Define various event handlers for Dialog
	    	var handleSubmit = function() {
	    		
	    		var tab = Beelun.shoppro.admin.model.tab.getTab();
	
				var fnSaveBack = function(item)
				{
					updatingPanel.hide();
					window.location.reload();
				};
				TabManager.save(tab, fnSaveBack);
				editTabDialog.hide();
				updatingPanel.show();
	    	};
	    	var handleCancel = function() {
	    		this.cancel();
	    	};
	    	
	    	// Instantiate the Dialog
	    	var editTabDialog = new YAHOO.widget.Dialog("editTabDialog", 
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
	    	editTabDialog.render();
	    	YAHOO.util.Dom.removeClass("editTabDialog", "hide");
	    	var updateTabDialog = function(record){
	    		var item = record._oData;
	    		Beelun.shoppro.admin.model.tab.setTab(item);
	    		editTabDialog.show();
	    	}
	    	var  editTabClick = function(o){
			
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
						TabManager.remove(oRecord.getData("id"), fnDeleteItemBack);
					}
				}
				else if(oColumn.key == "edit")
				{
					updateTabDialog(oRecord);
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
				{key:"isShown", editor:new YAHOO.widget.RadioCellEditor({radioOptions:["true","false"],disableBtns:false}), sortable:true, resizeable:true},
				{key:"content", editor:"textbox", hidden:true, sortable:true, resizeable:true},
				{key:"showOrder", editor:"textbox", sortable:true, resizeable:true},
				{key:"categoryMap", editor:"textbox", hidden:true, sortable:true, resizeable:true},
				{key:"pageTitle", editor:"textbox", sortable:true, resizeable:true},
				{key:"keywords", editor:"textbox", sortable:true, resizeable:true},
				{key:"description", editor:"textbox", sortable:true, resizeable:true},
				{key:"metaTag", editor:"textbox", sortable:true, resizeable:true},
				{key:"url", editor:"textbox", sortable:true, resizeable:true},
				{key:"edit", formatter: "button"},
				{key:"delete", formatter: "button"}
	        ];
	
	        var myDataSource = new YAHOO.util.DataSource(tab);
	        myDataSource.responseType = YAHOO.util.DataSource.TYPE_JSARRAY;
	        myDataSource.responseSchema = {
	            fields: [
	            {key: "id"},{key: "name"},{key: "isShown"},{key: "content"},{key: "showOrder"},
				{key: "categoryMap"},{key: "pageTitle"},{key: "keywords"},{key: "description"},{key: "metaTag"},
				{key: "url"}
				]
	        };
	
	        var myDataTable = new YAHOO.widget.ScrollingDataTable("tabTable",
	                myColumnDefs, myDataSource, {width:"900px"});
	        
	        myDataTable.subscribe("rowClickEvent",myDataTable.onEventSelectRow);
	        myDataTable.subscribe("cellDblclickEvent",myDataTable.onEventShowCellEditor);
	        myDataTable.subscribe("editorBlurEvent", myDataTable.onEventSaveCellEditor);
	        myDataTable.subscribe("rowMouseoverEvent", myDataTable.onEventHighlightRow);
        	myDataTable.subscribe("rowMouseoutEvent", myDataTable.onEventUnhighlightRow);
        	myDataTable.subscribe("buttonClickEvent", editTabClick);
	        
        	var onCellEdit = function(oArgs) {
            	
        		var elCell = oArgs.editor.getTdEl();
	            var oOldData = oArgs.oldData;
	            var oNewData = oArgs.newData;
	            
				var oTab = oArgs.editor._oRecord._oData;
	            var tab = {
	            	id: oTab.id, 
	            	name: oTab.name, 
	            	isShown: oTab.isShown, 
	            	content: oTab.content,
	            	showOrder: oTab.showOrder,
	            	//categoryMap: oTab.categoryMap,
					pageTitle: oTab.pageTitle,
	            	keywords: oTab.keywords,
	            	description: oTab.description,
	            	metaTag: oTab.metaTag,
	            	url: oTab.url};
		
				var fnSaveItemBack = function(item)
	        	{
	        		alert("Update Done!");
	        	};
	        	TabManager.save(tab, fnSaveItemBack);
	        };
        	myDataTable.subscribe("editorSaveEvent", onCellEdit);
        	
        	var addTabClick = function()
	    	{
	    		Beelun.shoppro.admin.model.tab.clearTab();
	    		editTabDialog.show();
	    	};
	    	
	    	YAHOO.util.Event.addListener("addTab", "click", addTabClick);

		};
		
        YAHOO.util.Event.onContentReady("tabTable", function () {
            
        	initTabTable();
        });
    }

}
    

