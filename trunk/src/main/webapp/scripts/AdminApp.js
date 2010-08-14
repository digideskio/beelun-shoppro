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


(function() {

	 Beelun.shoppro.admin.AdminApp = function() {
        this.initApp();
    };

    var proto =  Beelun.shoppro.admin.AdminApp.prototype;

    proto.initApp = function(attr) {
    	
        // Remove progressively enhanced content class, just before creating the module
        YAHOO.util.Dom.removeClass("editItemDialog", "yui-pe-content");
        YAHOO.util.Dom.removeClass("editUserDialog", "yui-pe-content");
        var brandListChange = function(o)
        {
        	var activeEle;
        	if(o.srcElement)
        	{
        		activeEle = o.srcElement;
        	}
        	else
        	{
        		activeEle = o.currentTarget;
        	}
        	
        	var funGetBrandListHandleSuccess = function(brand)
    		{
        		alert(brand.id);
        		YAHOO.util.Dom.get('brandId').value = brand.id;
        		YAHOO.util.Dom.get('brandImage').value = brand.image;
        		YAHOO.util.Dom.get('brandWebsite').value = brand.webSite;
    		};
    		
        	BrandManager.getByName(activeEle.options[activeEle.selectedIndex].text, funGetBrandListHandleSuccess);
        	
        };
        var brandSelect = YAHOO.util.Dom.get('brand');
        YAHOO.util.Event.addListener(brandSelect, "change", brandListChange);
        
        var oBrandList = null;

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
		
    	var initItemListTable = function()
		{
    		updatingPanel.show();
    		ItemManager.getAll(fnGetItemListHandleSuccess);
    		
		};
		
		var fnGetItemListHandleSuccess = function(itemList)
		{
		
			updatingPanel.hide();
		
			/*
	         * Initial editItemDialog Dialog
	 		 */
			
	    	// Define various event handlers for Dialog
	    	var handleSubmit = function() {
	    		
	    		var item = Beelun.shoppro.admin.model.item.getItem(oBrandList);
	
				var fnSaveItemBack = function(item)
				{
					updatingPanel.hide();
					window.location.reload();
				};
				ItemManager.save(item, fnSaveItemBack);
				editItemDialog.hide();
				updatingPanel.show();
	    	};
	    	var handleCancel = function() {
	    		this.cancel();
	    	};
	    	
	    	// Instantiate the Dialog
	    	var editItemDialog = new YAHOO.widget.Dialog("editItemDialog", 
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
	    	editItemDialog.render();
	    	YAHOO.util.Dom.removeClass("editItemDialog", "hide");
	    	
	    	var updateItemDialog = function(record){
	    		var item = record._oData;
	    		Beelun.shoppro.admin.model.item.setItem(item);
	    		editItemDialog.show();
	    	};
	    	var  editItemClick = function(oArgs){

	    		var oRecord = this.getRecord(oArgs.target);
	    	    var oColumn = this.getColumn(oArgs.target);
	    	    
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
	    	        	ItemManager.removeItem(oRecord.getData("id"), fnDeleteItemBack);
	    	        }
	    	    }
	    	    else if(oColumn.key == "edit")
	    	    {
	    	    	updateItemDialog(oRecord);
	    	    }
	        };
	        
	    	var addItemClick = function()
	    	{
	    		Beelun.shoppro.admin.model.item.clearItem();
	    		editItemDialog.show();
	    	};
	    	
	    	YAHOO.util.Event.addListener("addItem", "click", addItemClick);

			
			/*
	         * Initial itemList datatable
	 		 */
			
			var brandFormatter = function(elLiner, oRecord, oColumn, oData) {
				
				if(oRecord.getData("brand") != null)
				{
					elLiner.innerHTML = oRecord.getData("brand").name;   
				}
				else
				{
					elLiner.innerHTML = "NULL"; 
				}
		
			};
			
			var myColumnDefs = [
                {key:"check", formatter: "checkbox"},
	            {key:"id", sortable:true, resizeable:true},
	            {key:"netSuiteId", sortable:true, resizeable:true},
	            {key:"brand", formatter:brandFormatter},
				{key:"name", editor:"textbox", sortable:true, resizeable:true},
				{key:"serialNumber", editor:"textbox", sortable:true, resizeable:true},
				{key:"shortDescription", editor:"textbox", sortable:true, resizeable:true},
				{key:"detailedDescription", hidden:true, editor:"textbox", sortable:true, resizeable:true},
				{key:"image", editor:"textbox", hidden:true, sortable:true, resizeable:true},
				{key:"thumbNail", editor:"textbox", hidden:true, sortable:true, resizeable:true},
				{key:"listPrice", editor:"textbox", formatter:YAHOO.widget.DataTable.formatCurrency, sortable:true, resizeable:true},
				{key:"sellPrice", editor:"textbox", formatter:YAHOO.widget.DataTable.formatCurrency, sortable:true, resizeable:true},
				{key:"inventoryNumber", editor:"textbox", formatter:YAHOO.widget.DataTable.formatNumber,  sortable:true, resizeable:true},
				{key:"isShown", editor:new YAHOO.widget.RadioCellEditor({radioOptions:["true","false"],disableBtns:true}), sortable:true, resizeable:true},
				{key:"pageTitle", editor:"textbox", hidden:true, sortable:true, resizeable:true},
				{key:"keywords", editor:"textbox", sortable:true, resizeable:true},
				{key:"description", editor:"textbox", sortable:true, resizeable:true},
				{key:"metaTag", editor:"textbox", sortable:true, resizeable:true},
				{key:"url", editor:"textbox", hidden:true, sortable:true, resizeable:true},
				{key:"edit", formatter: "button"},
				{key:"delete", formatter: "button"}
	        ];
	
	        var myDataSource = new YAHOO.util.DataSource(itemList);
	        myDataSource.responseType = YAHOO.util.DataSource.TYPE_JSARRAY;
	        myDataSource.responseSchema = {
	            fields: [
	            {key: "id"},{key: "netSuiteId"},{key:"brand"}, {key: "name"},{key: "serialNumber"},{key: "shortDescription"},{key: "detailedDescription"},
				{key: "image"},{key: "thumbNail"},{key: "listPrice"},{key: "sellPrice"},{key: "inventoryNumber"},{key: "isShown"},
				{key: "pageTitle"},{key: "keywords"},{key: "description"},{key: "metaTag"},{key: "url"}]
	        };
	
	        var myDataTable = new YAHOO.widget.ScrollingDataTable("itemListTable",
	                myColumnDefs, myDataSource, {width:"900px", paginator: new YAHOO.widget.Paginator({
	                    rowsPerPage: 15
	                })});
	        
	        myDataTable.subscribe("rowClickEvent",myDataTable.onEventSelectRow);
	        myDataTable.subscribe("cellDblclickEvent",myDataTable.onEventShowCellEditor);
	        myDataTable.subscribe("editorBlurEvent", myDataTable.onEventSaveCellEditor);
	        myDataTable.subscribe("rowMouseoverEvent", myDataTable.onEventHighlightRow);
        	myDataTable.subscribe("rowMouseoutEvent", myDataTable.onEventUnhighlightRow);
        	myDataTable.subscribe("buttonClickEvent", editItemClick);
        	
        	
        	myDataTable.subscribe("checkboxClickEvent", function(oArgs){   
    			var elCheckbox = oArgs.target;   
    			var oRecord = this.getRecord(elCheckbox);   
    			oRecord.setData("check",elCheckbox.checked);
        	});

        	var onCellEdit = function(oArgs) {
            	
        		var brand = {id:0, name:"Spirent", image:"1.jpg", webSite:"http://www.spirent.com"};
				var elCell = oArgs.editor.getTdEl();
	            var oOldData = oArgs.oldData;
	            var oNewData = oArgs.newData;
	            
				var oitem = oArgs.editor._oRecord._oData;
	            var item = {
	            	id: oitem.id, 
					brand: oitem.brand,
	            	netSuiteId: oitem.netSuiteId,
	            	name: oitem.name, 
					serialNumber: oitem.serialNumber, 
	            	shortDescription: oitem.shortDescription,
	            	detailedDescription: oitem.detailedDescription,
	            	image: oitem.image,
	            	thumbNail: oitem.thumbNail,
	            	listPrice: oitem.listPrice,
	            	sellPrice: oitem.sellPrice,
					inventoryNumber: oitem.inventoryNumber,
					isShown: oitem.isShown,
					pageTitle: oitem.pageTitle, 
					keywords: oitem.keywords,
					description: oitem.description,
					metaTag: oitem.metaTag,
					url: oitem.url };
		
				var fnSaveItemBack = function(item)
	        	{
	        		alert("Update Done!");
	        	};
	            ItemManager.save(item, fnSaveItemBack);
	        };
        	myDataTable.subscribe("editorSaveEvent", onCellEdit);
        	
	    	var deleteItemClick = function()
	    	{
	    		if(confirm("Do you want to remove selected items?"))
	    		{
	    			var recordSet = myDataTable.getRecordSet()._records;
	    			var ids = new Array();
	    			for(var i=0; i<recordSet.length; i++)
	    			{
	    				if(recordSet[i].getData("check"))
	    				{
	    					ids.push(recordSet[i].getData("id"));
	    				}
	    			}
	    			var fnDeleteItemsBack = function(value)
					{
		    	    	if(value)
		    	    	{
		    	    		window.location.reload();
		    	    	}
		    	    	else
		    	    	{
		    	    		alert("You can't delete these items!");
		    	    	}
					};
	    			ItemManager.removeItems(ids, fnDeleteItemsBack);
	    		}
	    		//Beelun.shoppro.admin.model.item.clearItem();
	    		//editItemDialog.show();
	    	};
	    	YAHOO.util.Event.addListener("deleteItem", "click", deleteItemClick);
	    	
	    	var importItemsClick = function()
	    	{
	    		var uploadPanel = YAHOO.util.Dom.get('uploadPanel');
	    		var uploadButton = YAHOO.util.Dom.get('importItems');
	    		
	    		if(uploadPanel.style.display == "none")
	    		{
	    			uploadButton.innerHTML = "Import Items <<";
	    			uploadPanel.style.display='block';
	    		}
	    		else
	    		{
	    			uploadButton.innerHTML = "Import Items >>";
	    			uploadPanel.style.display='none';
	    		}
	    		
	    	};
	    	YAHOO.util.Event.addListener("importItems", "click", importItemsClick);

		};
		
        YAHOO.util.Event.onContentReady("itemListTable", function () {
            
        	initItemListTable();
        });

        
/***************************************************************************************
 * customer List Table
 ***************************************************************************************/
        
        if(Beelun.shoppro.admin.model.user)
        {
        	Beelun.shoppro.admin.model.user.initUserTable();
        }
        
        
/***************************************************************************************
 * myGlob Table
 ***************************************************************************************/
                
        if(Beelun.shoppro.admin.model.myGlob)
        {
        	Beelun.shoppro.admin.model.myGlob.initGlobTable();
        }

 /***************************************************************************************
  * myGlob Table
  ***************************************************************************************/
                 
         if(Beelun.shoppro.admin.model.paypalInfo)
         {
         	Beelun.shoppro.admin.model.paypalInfo.initPaypalInfoTable();
         }     
  
  /***************************************************************************************
   * article Table
   ***************************************************************************************/
                  
          if(Beelun.shoppro.admin.model.article)
          {
          	Beelun.shoppro.admin.model.article.initArticleListTable();
          }     
                                
/***************************************************************************************
 * Old order Table
 ***************************************************************************************/
                
		 if(Beelun.shoppro.admin.model.order)
		 {
		 	Beelun.shoppro.admin.model.order.initNewOrder();
		 	Beelun.shoppro.admin.model.order.initOldOrder();
		 }		

                
/***************************************************************************************
 * tab Table
 ***************************************************************************************/
                
                if(Beelun.shoppro.admin.model.tab)
                {
                	Beelun.shoppro.admin.model.tab.initTabListTable();
                }
                
/***************************************************************************************
 * category Table
 ***************************************************************************************/
                
 
				 if(Beelun.shoppro.admin.model.category)
				 {
				 	Beelun.shoppro.admin.model.category.initCategoryTable();
				 }
                
                
/***************************************************************************************
 * tab2category Table
 ***************************************************************************************/
 
				 if(Beelun.shoppro.admin.model.tabMap)
				 {
				 	Beelun.shoppro.admin.model.tabMap.initTabMapTable();
				 }
				                
                
/***************************************************************************************
 * category2item Table
 ***************************************************************************************/
                
 
				 if(Beelun.shoppro.admin.model.categoryMap)
				 {
				 	Beelun.shoppro.admin.model.categoryMap.initCategoryMapTable();
				 }
 
			var setDashboard = function()
			{
				var fnGetNewOrderHandleSuccess = function(order)
				{
					var newOrderNumEle = YAHOO.util.Dom.get('newOrderNum');
					newOrderNumEle.innerHTML = '<a href="newOrder.html"><u>' + order.length + '</u></a>';
					
				};
				var fnGetOrderHandleSuccess = function(order)
				{
					var oldOrderNumEle = YAHOO.util.Dom.get('oldOrderNum');
					oldOrderNumEle.innerHTML = '<a href="order.html"><u>' + order.length + '</u></a>';
				}
				
				OrderManager.getNewOrder(fnGetNewOrderHandleSuccess);
				OrderManager.getPaidOrder(fnGetOrderHandleSuccess);
				
			};
 
			 YAHOO.util.Event.onContentReady("newOrderNum", function () {
			     
			 	setDashboard();
			 });

    };
    

})();