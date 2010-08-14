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


Beelun.shoppro.admin.model.order = {

    getOrder: function() {
    	var order = {};
    	
    	var orderIdEle = YAHOO.util.Dom.get('orderId');
    	order.id = orderIdEle.value;
		
		var orderDateEle = YAHOO.util.Dom.get('orderDate');
		order.orderDate = orderDateEle.value;
		
		var userEle = YAHOO.util.Dom.get('user');
		order.user = userEle.value;
		
		var addressEle = YAHOO.util.Dom.get('address');
		order.address = addressEle.value;
		
		var shipDateEle = YAHOO.util.Dom.get('shipDate');
		order.shipDate = shipDateEle.value;
		
		var shipTimeEle = YAHOO.util.Dom.get('shipTime');
		order.shipTime = shipTimeEle.value;
		
		var specifiedShipDateEle = YAHOO.util.Dom.get('specifiedShipDate');
		order.specifiedShipDate = specifiedShipDateEle.value;
		
		var notesEle = YAHOO.util.Dom.get('notes');
		order.notes = notesEle.value;
		
		var expressCorpEle = YAHOO.util.Dom.get('expressCorp');
		order.expressCorp = expressCorpEle.value;
		
		var paymentToolEle = YAHOO.util.Dom.get('paymentTool');
		order.paymentTool = paymentToolEle.value;
		
		var statusEle = YAHOO.util.Dom.get('status');
		order.status = statusEle.value;
		
		var orderItemSetEle = YAHOO.util.Dom.get('orderItemSet');
		order.orderItemSet = orderItemSetEle.value;
		
		return order;
                
    },
    
    setOrder: function(order) {
    	
    	var orderId = order.id;
		var orderDate = order.orderDate;
		var user = order.user;
		var address = order.address;
		var shipDate = order.shipDate;
		var shipTime = order.shipTime;
		var specifiedShipDate = order.specifiedShipDate;
		var notes = order.notes;
		var expressCorp = order.expressCorp;
		var paymentTool = order.paymentTool;
		var status = order.status;
		var orderItemSet = order.orderItemSet;

		var orderIdEle = YAHOO.util.Dom.get('orderId');
    	orderIdEle.value = orderId;
		
		var orderDateEle = YAHOO.util.Dom.get('orderDate');
		orderDateEle.value = orderDate;
		
		var userEle = YAHOO.util.Dom.get('user');
		userEle.value = user;
		
		var addressEle = YAHOO.util.Dom.get('address');
		addressEle.value = address;
		
		var shipDateEle = YAHOO.util.Dom.get('shipDate');
		shipDateEle.value = shipDate;
		
		var shipTimeEle = YAHOO.util.Dom.get('shipTime');
		shipTimeEle.value = shipTime;
		
		var specifiedShipDateEle = YAHOO.util.Dom.get('specifiedShipDate');
		specifiedShipDateEle.value = specifiedShipDate;
		
		var notesEle = YAHOO.util.Dom.get('notes');
		notesEle.value = notes;
		
		var expressCorpEle = YAHOO.util.Dom.get('expressCorp');
		expressCorpEle.value = expressCorp;
		
		var paymentToolEle = YAHOO.util.Dom.get('paymentTool');
		paymentToolEle.value = paymentTool;
		
		var statusEle = YAHOO.util.Dom.get('status');
		statusEle.value = status;
		
		var orderItemSetEle = YAHOO.util.Dom.get('orderItemSet');
		orderItemSetEle.value = orderItemSet;
    },
    
    initOldOrder: function() {
    	var initOrderTable = function()
		{
    		OrderManager.getPaidOrder(fnGetOrderHandleSuccess);
    		
		};
		
		var fnGetOrderHandleSuccess = function(order)
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
		
			/*
	         * Initial editItemDialog Dialog
	 		 */
			
	    	// Define various event handlers for Dialog
	    	var handleSubmit = function() {
	    		
	    		var order = Beelun.shoppro.admin.model.order.getOrder();
	
				var fnSaveBack = function(item)
				{
					updatingPanel.hide();
					window.location.reload();
				};
				OrderManager.save(order, fnSaveBack);
				editOrderDialog.hide();
				updatingPanel.show();
	    	};
	    	var handleCancel = function() {
	    		this.cancel();
	    	};
	    	
	    	// Instantiate the Dialog
	    	var editOrderDialog = new YAHOO.widget.Dialog("editOrderDialog", 
	    							{ width : "30em",
	    							  fixedcenter : true,
	    							  visible : false, 
	    							  draggable: false,
	    							  modal: true,
	    							  constraintoviewport : true,
	    							  buttons : [ { text:"Save", handler:handleSubmit, isDefault:true },
	    								      { text:"Cancel", handler:handleCancel } ]
	    							});
	    	// Render the Dialog
	    	editOrderDialog.render();
	    	YAHOO.util.Dom.removeClass("editOrderDialog", "hide");
	    	var updateOrderDialog = function(record){
	    		var item = record._oData;
	    		Beelun.shoppro.admin.model.order.setOrder(item);
	    		editOrderDialog.show();
	    	}
	    	var  editOrderClick = function(o){
	    		var oRecord = this.getRecord(o.target);
	    		updateOrderDialog(oRecord);

	        };
			
			/*
	         * Initial user datatable
	 		 */
			
			var userFormatter = function(elLiner, oRecord, oColumn, oData) {
		        elLiner.innerHTML = oRecord.getData("user").name;   
		
			};
			var shippingAddressFormatter = function(elLiner, oRecord, oColumn, oData) {
		        elLiner.innerHTML = oRecord.getData("shippingAddress").address;   
		
			};
			var recipientFormatter = function(elLiner, oRecord, oColumn, oData) {
		        elLiner.innerHTML = oRecord.getData("shippingAddress").recipientName;   
		
			};
			var mobileFormatter = function(elLiner, oRecord, oColumn, oData) {
		        elLiner.innerHTML = oRecord.getData("shippingAddress").mobileNumber;   
		
			};
			var billingAddressFormatter = function(elLiner, oRecord, oColumn, oData) {
		        elLiner.innerHTML = oRecord.getData("billingAddress").address;   
		
			};
			var billing_recipientFormatter = function(elLiner, oRecord, oColumn, oData) {
		        elLiner.innerHTML = oRecord.getData("billingAddress").recipientName;   
		
			};
			var billing_mobileFormatter = function(elLiner, oRecord, oColumn, oData) {
		        elLiner.innerHTML = oRecord.getData("billingAddress").mobileNumber;   
		
			};
			var expressFormatter = function(elLiner, oRecord, oColumn, oData) {
		        elLiner.innerHTML = oRecord.getData("expressCorp").shortName;   
		
			};
			var paymentFormatter = function(elLiner, oRecord, oColumn, oData) {
		        elLiner.innerHTML = oRecord.getData("paymentTool").name;   
		
			};
			var itemSetFormatter = function(elLiner, oRecord, oColumn, oData) {
		        elLiner.innerHTML = oRecord.getData("orderItemSet").length;   
		
			};
			
			var myColumnDefs = [
	            {key:"id", sortable:true, resizeable:true},
				{key:"orderDate", sortable:true, resizeable:true},
				{key:"user", formatter:userFormatter, sortable:true, resizeable:true},
				{key:"shippingAddress", formatter:shippingAddressFormatter, sortable:true, resizeable:true},
				{key:"recipientName", formatter:recipientFormatter, sortable:true, resizeable:true},
				{key:"mobile", formatter:mobileFormatter, sortable:true, resizeable:true},
				{key:"billingAddress", formatter:billingAddressFormatter, sortable:true, resizeable:true},
				{key:"billing_recipientName", formatter:billing_recipientFormatter, sortable:true, resizeable:true},
				{key:"billing_mobile", formatter:billing_mobileFormatter, sortable:true, resizeable:true},
				{key:"shipDate", sortable:true, resizeable:true},
				{key:"shipTime", sortable:true, resizeable:true},
				{key:"specifiedShipDate", sortable:true, resizeable:true},
				{key:"notes", editor:"textbox", sortable:true, resizeable:true},
				{key:"expressCorp", formatter:expressFormatter, sortable:true, resizeable:true},
				{key:"paymentTool", formatter:paymentFormatter, sortable:true, resizeable:true},
				{key:"status", editor:"textbox", sortable:true, resizeable:true},
				{key:"orderItemSet", formatter:itemSetFormatter, sortable:true, resizeable:true},
				{key:"edit", hidden:true, formatter: "button"}
	        ];
	
	        var myDataSource = new YAHOO.util.DataSource(order);
	        myDataSource.responseType = YAHOO.util.DataSource.TYPE_JSARRAY;
	        myDataSource.responseSchema = {
	            fields: [
	            {key: "id"},{key: "orderDate"},{key: "user"},{key: "shippingAddress"},{key: "billingAddress"},{key: "shipDate"},
				{key: "shipTime"},{key: "specifiedShipDate"},{key: "notes"},{key: "expressCorp"},{key: "paymentTool"},
				{key: "status"},{key: "orderItemSet"}
				]
	        };
	
	        var myDataTable = new YAHOO.widget.ScrollingDataTable("orderTable",
	                myColumnDefs, myDataSource, {width:"900px"});
	        
	        myDataTable.subscribe("rowClickEvent",myDataTable.onEventSelectRow);
	        myDataTable.subscribe("cellDblclickEvent",myDataTable.onEventShowCellEditor);
	        myDataTable.subscribe("editorBlurEvent", myDataTable.onEventSaveCellEditor);
	        myDataTable.subscribe("rowMouseoverEvent", myDataTable.onEventHighlightRow);
        	myDataTable.subscribe("rowMouseoutEvent", myDataTable.onEventUnhighlightRow);
        	myDataTable.subscribe("buttonClickEvent", editOrderClick);
	        
        	var onCellEdit = function(oArgs) {
            	
        		var elCell = oArgs.editor.getTdEl();
	            var oOldData = oArgs.oldData;
	            var oNewData = oArgs.newData;
	            
				var oOrder = oArgs.editor._oRecord._oData;
	            var order = {
	            	id: oOrder.id, 
	            	orderDate: oOrder.orderDate, 
	            	user: oOrder.user, 
	            	shippingAddress: oOrder.shippingAddress,
	            	billingAddress: oOrder.billingAddress,
	            	shipDate: oOrder.shipDate,
	            	shipTime: oOrder.shipTime,
					specifiedShipDate: oOrder.specifiedShipDate,
	            	notes: oOrder.notes,
	            	expressCorp: oOrder.expressCorp,
	            	paymentTool: oOrder.paymentTool,
	            	status: oOrder.status,
					orderItemSet: oOrder.orderItemSet };
		
				var fnSaveItemBack = function(item)
	        	{
	        		alert("Update Done!");
	        	};
	            OrderManager.save(order, fnSaveItemBack);
	        };
        	myDataTable.subscribe("editorSaveEvent", onCellEdit);

		};
		
        YAHOO.util.Event.onContentReady("orderTable", function () {
            
        	initOrderTable();
        });
    	
    	
    },
    
    initNewOrder: function()
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
		
    	var initNewOrderTable = function()
		{
    		updatingPanel.show();
    		OrderManager.getNewOrder(fnGetNewOrderHandleSuccess);
    		
		};
		
		var fnGetNewOrderHandleSuccess = function(order)
		{
		
			updatingPanel.hide();
		
			/*
	         * Initial editItemDialog Dialog
	 		 */
			
	    	// Define various event handlers for Dialog
	    	var handleSubmit = function() {
	    		
	    		var order = Beelun.shoppro.admin.model.order.getOrder();
	
				var fnSaveBack = function(item)
				{
					updatingPanel.hide();
					window.location.reload();
				};
				OrderManager.save(order, fnSaveBack);
				editOrderDialog.hide();
				updatingPanel.show();
	    	};
	    	var handleCancel = function() {
	    		this.cancel();
	    	};
	    	
	    	// Instantiate the Dialog
	    	var editOrderDialog = new YAHOO.widget.Dialog("editOrderDialog", 
	    							{ width : "30em",
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
	    	editOrderDialog.render();
	    	YAHOO.util.Dom.removeClass("editOrderDialog", "hide");
	    	var updateOrderDialog = function(record){
	    		var item = record._oData;
	    		Beelun.shoppro.admin.model.order.setOrder(item);
	    		editOrderDialog.show();
	    	}
	    	var  editOrderClick = function(o){
	    		var oRecord = this.getRecord(o.target);
	    		updateOrderDialog(oRecord);

	        };
			
			/*
	         * Initial user datatable
	 		 */
			
			var userFormatter = function(elLiner, oRecord, oColumn, oData) {
		        elLiner.innerHTML = oRecord.getData("user").name;   
		
			};
			var shippingAddressFormatter = function(elLiner, oRecord, oColumn, oData) {
		        elLiner.innerHTML = oRecord.getData("shippingAddress").address;   
		
			};
			var recipientFormatter = function(elLiner, oRecord, oColumn, oData) {
		        elLiner.innerHTML = oRecord.getData("shippingAddress").recipientName;   
		
			};
			var mobileFormatter = function(elLiner, oRecord, oColumn, oData) {
		        elLiner.innerHTML = oRecord.getData("shippingAddress").mobileNumber;   
		
			};
			var billingAddressFormatter = function(elLiner, oRecord, oColumn, oData) {
		        elLiner.innerHTML = oRecord.getData("billingAddress").address;   
		
			};
			var billing_recipientFormatter = function(elLiner, oRecord, oColumn, oData) {
		        elLiner.innerHTML = oRecord.getData("billingAddress").recipientName;   
		
			};
			var billing_mobileFormatter = function(elLiner, oRecord, oColumn, oData) {
		        elLiner.innerHTML = oRecord.getData("billingAddress").mobileNumber;   
		
			};
			var expressFormatter = function(elLiner, oRecord, oColumn, oData) {
				if(oRecord.getData("expressCorp"))
				{
					elLiner.innerHTML = oRecord.getData("expressCorp").shortName;  
				}
				else
				{
					elLiner.innerHTML = "N/A";
				}
			};
			var paymentFormatter = function(elLiner, oRecord, oColumn, oData) {
				if(oRecord.getData("paymentTool"))
				{
					elLiner.innerHTML = oRecord.getData("paymentTool").name;
				}
				else
				{
					elLiner.innerHTML = "Default";
				}
		           
		
			};
			var itemSetFormatter = function(elLiner, oRecord, oColumn, oData) {
		        elLiner.innerHTML = oRecord.getData("orderItemSet").length;   
		
			};
			
			var myColumnDefs = [
	            {key:"id", sortable:true, resizeable:true},
				{key:"orderDate", editor:"textbox", sortable:true, resizeable:true},
				{key:"user", formatter:userFormatter, sortable:true, resizeable:true},
				{key:"shippingAddress", formatter:shippingAddressFormatter, sortable:true, resizeable:true},
				{key:"recipientName", formatter:recipientFormatter, sortable:true, resizeable:true},
				{key:"mobile", formatter:mobileFormatter, sortable:true, resizeable:true},
				{key:"billingAddress", formatter:billingAddressFormatter, sortable:true, resizeable:true},
				{key:"billing_recipientName", formatter:billing_recipientFormatter, sortable:true, resizeable:true},
				{key:"billing_mobile", formatter:billing_mobileFormatter, sortable:true, resizeable:true},
				{key:"shipDate", sortable:true, resizeable:true},
				{key:"shipTime", sortable:true, resizeable:true},
				{key:"specifiedShipDate", sortable:true, resizeable:true},
				{key:"notes", sortable:true, resizeable:true},
				{key:"expressCorp", formatter:expressFormatter, sortable:true, resizeable:true},
				{key:"paymentTool", formatter:paymentFormatter, sortable:true, resizeable:true},
				{key:"status", editor:"textbox", sortable:true, resizeable:true},
				{key:"orderItemSet", formatter:itemSetFormatter, sortable:true, resizeable:true},
				{key:"edit", hidden:true, formatter: "button"}
	        ];
	
	        var myDataSource = new YAHOO.util.DataSource(order);
	        myDataSource.responseType = YAHOO.util.DataSource.TYPE_JSARRAY;
	        myDataSource.responseSchema = {
	            fields: [
	            {key: "id"},{key: "orderDate"},{key: "user"},{key: "shippingAddress"},{key: "billingAddress"},{key: "shipDate"},
				{key: "shipTime"},{key: "specifiedShipDate"},{key: "notes"},{key: "expressCorp"},{key: "paymentTool"},
				{key: "status"},{key: "orderItemSet"}
				]
	        };
	
	        var myDataTable = new YAHOO.widget.ScrollingDataTable("newOrderTable",
	                myColumnDefs, myDataSource, {width:"900px"});
	        
	        myDataTable.subscribe("rowClickEvent",myDataTable.onEventSelectRow);
	        myDataTable.subscribe("cellDblclickEvent",myDataTable.onEventShowCellEditor);
	        myDataTable.subscribe("editorBlurEvent", myDataTable.onEventSaveCellEditor);
	        myDataTable.subscribe("rowMouseoverEvent", myDataTable.onEventHighlightRow);
        	myDataTable.subscribe("rowMouseoutEvent", myDataTable.onEventUnhighlightRow);
        	myDataTable.subscribe("buttonClickEvent", editOrderClick);
	        
        	var onCellEdit = function(oArgs) {
            	
        		var elCell = oArgs.editor.getTdEl();
	            var oOldData = oArgs.oldData;
	            var oNewData = oArgs.newData;
	            
				var oOrder = oArgs.editor._oRecord._oData;
	            var order = {
	            	id: oOrder.id, 
	            	orderDate: oOrder.orderDate, 
	            	user: oOrder.user, 
	            	shippingAddress: oOrder.shippingAddress,
	            	billingAddress: oOrder.billingAddress,
	            	shipDate: oOrder.shipDate,
	            	shipTime: oOrder.shipTime,
					specifiedShipDate: oOrder.specifiedShipDate,
	            	notes: oOrder.notes,
	            	expressCorp: oOrder.expressCorp,
	            	paymentTool: oOrder.paymentTool,
	            	status: oOrder.status,
					orderItemSet: oOrder.orderItemSet };
		
				var fnSaveItemBack = function(item)
	        	{
	        		alert("Update Done!");
	        	};
	            OrderManager.save(order, fnSaveItemBack);
	        };
        	myDataTable.subscribe("editorSaveEvent", onCellEdit);

		};
		
        YAHOO.util.Event.onContentReady("newOrderTable", function () {
            
        	initNewOrderTable();
        });
    }

}
    

