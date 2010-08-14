<#ftl strip_whitespace=true>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<#-- This is a test file only, not for production purpose -->

<head>
    <title>Test page</title>
    <script type="text/javascript" src="/dwr/interface/ItemManager.js"></script>
    <script type="text/javascript" src="/dwr/interface/OrderManager.js"></script>
    <link rel="stylesheet" type="text/css" href="http://yui.yahooapis.com/2.8.0r4/build/fonts/fonts-min.css" />
<link rel="stylesheet" type="text/css" href="http://yui.yahooapis.com/2.8.0r4/build/datatable/assets/skins/sam/datatable.css" />
<script type="text/javascript" src="http://yui.yahooapis.com/2.8.0r4/build/yahoo-dom-event/yahoo-dom-event.js"></script>
<script type="text/javascript" src="http://yui.yahooapis.com/2.8.0r4/build/connection/connection-min.js"></script>
<script type="text/javascript" src="http://yui.yahooapis.com/2.8.0r4/build/element/element-min.js"></script>
<script type="text/javascript" src="http://yui.yahooapis.com/2.8.0r4/build/button/button-min.js"></script>
<script type="text/javascript" src="http://yui.yahooapis.com/2.8.0r4/build/dragdrop/dragdrop-min.js"></script>
<script type="text/javascript" src="http://yui.yahooapis.com/2.8.0r4/build/container/container-min.js"></script>


<script type="text/javascript" src="http://yui.yahooapis.com/2.8.0r4/build/dragdrop/dragdrop-min.js"></script>
<script type="text/javascript" src="http://yui.yahooapis.com/2.8.0r4/build/element/element-min.js"></script>
<script type="text/javascript" src="http://yui.yahooapis.com/2.8.0r4/build/datasource/datasource-min.js"></script>
<script type="text/javascript" src="http://yui.yahooapis.com/2.8.0r4/build/datatable/datatable-min.js"></script>

<link rel="stylesheet" type="text/css" href="http://yui.yahooapis.com/2.8.0r4/build/fonts/fonts-min.css" />
<link rel="stylesheet" type="text/css" href="http://yui.yahooapis.com/2.8.0r4/build/button/assets/skins/sam/button.css" />
<link rel="stylesheet" type="text/css" href="http://yui.yahooapis.com/2.8.0r4/build/container/assets/skins/sam/container.css" />


    <script>
    var myDataTable = null;
    	function sendRequest()
		{
    		DWRUtil.setValue("showArea", "");
    		ItemManager.getAll(gotItems);
		}
		
		function gotItems(itemList)
		{
		
			/*
         * Initial AddToCart and cart Dialog
 		 */
            	// Define various event handlers for Dialog
    	var handleSubmit = function() {
    		window.location.href=('/cart/itemList.html')
    		//this.submit();
    	};
    	var handleCancel = function() {
    		this.cancel();
    	};

        // Remove progressively enhanced content class, just before creating the module
        YAHOO.util.Dom.removeClass("editItemDialog", "yui-pe-content");

    	// Instantiate the Dialog
    	var editItemDialog = new YAHOO.widget.Dialog("editItemDialog", 
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
    	editItemDialog.render();
    	
    	var updateCartDialog = function(xml) {
    		
    		var xmlEle = xml;
    		var itemNumber = xmlEle.getElementsByTagName('itemNumber').item(0).firstChild.nodeValue;    
    	    var itemValue = xmlEle.getElementsByTagName('itemValue').item(0).firstChild.nodeValue;   
    		
    		var totalArticle = YAHOO.util.Dom.get('dialogTotalArticle');
    		totalArticle.innerHTML = itemNumber;
    		var totalPrice = YAHOO.util.Dom.get('dialogTotalPrice');
    		totalPrice.innerHTML = itemValue;
    		addToCartDialog.show();
    	};
    	
        var fnAddCartHandleSuccess = function(o){

        	//alert(o.tId + o.responseText);
        	updateCartDialog(o.responseXML);
        	
        };

        var fnAddCartHandleFailure = function(o){
        	alert('There is a error. Please contact administrator. Thank you!');
        	if(o.responseText !== undefined){
        		//div.innerHTML = "<ul><li>Transaction id: " + o.tId + "</li>";
        		//div.innerHTML += "<li>HTTP status: " + o.status + "</li>";
        		//div.innerHTML += "<li>Status code message: " + o.statusText + "</li></ul>";
        	}
        };
        var oAddCartCallback =
        {
        	success:fnAddCartHandleSuccess,
        	failure:fnAddCartHandleFailure
        };

        var  editItemClick = function(o){   
        var item = Beelun.shoppro.admin.model.item.getItemFromPanel();
	        	alert(item.id);  	
        	addToCartDialog.show()

        };
       
        
        
        
		
			
    		//var items = "";
    		//for (var data in itemList)
    		//{
        	//	items = "<div>" + itemList[data].name + 
        	//				"</div>" + items;
    		//}
    		//DWRUtil.setValue("showArea", items);
    		
			var myCustomFormatter = function(elLiner, oRecord, oColumn, oData) {
			        elLiner.innerHTML = oRecord.getData("brand").name;   
			
	        };   
    		
			var myColumnDefs = [
	            {key:"id", sortable:true, resizeable:true},
	            {key:"netSuiteId", editor:"textbox", formatter:YAHOO.widget.DataTable.formatNumber, sortable:true, resizeable:true},
	            {key:"brand", formatter:myCustomFormatter},
				{key:"name", editor:"textbox", sortable:true, resizeable:true},
				{key:"serialNumber", editor:"textbox", sortable:true, resizeable:true},
				{key:"shortDescription", editor:"textbox", sortable:true, resizeable:true},
				{key:"detailedDescription", editor:"textarea", sortable:true, resizeable:true},
				{key:"image", editor:"textbox", sortable:true, resizeable:true},
				{key:"thumbNail", editor:"textbox", sortable:true, resizeable:true},
				{key:"listPrice", editor:"textbox", formatter:YAHOO.widget.DataTable.formatCurrency, sortable:true, resizeable:true},
				{key:"sellPrice", editor:"textbox", formatter:YAHOO.widget.DataTable.formatCurrency, sortable:true, resizeable:true},
				{key:"inventoryNumber", editor:"textbox", formatter:YAHOO.widget.DataTable.formatNumber,  sortable:true, resizeable:true},
				{key:"isShown", editor:new YAHOO.widget.RadioCellEditor({radioOptions:["true","false"],disableBtns:true}), sortable:true, resizeable:true},
				{key:"pageTitle", editor:"textbox", sortable:true, resizeable:true},
				{key:"keywords", editor:"textbox", sortable:true, resizeable:true},
				{key:"description", editor:"textbox", sortable:true, resizeable:true},
				{key:"metaTag", editor:"textbox", sortable:true, resizeable:true},
				{key:"edit", formatter: "button"}
	        ];
	        
	        var myDataSource = new YAHOO.util.DataSource(itemList);
	        myDataSource.responseType = YAHOO.util.DataSource.TYPE_JSARRAY;
	        myDataSource.responseSchema = {
	            fields: [
	            {key: "id"},{key: "netSuiteId"},{key:"brand"}, {key: "name"},{key: "serialNumber"},{key: "shortDescription"},{key: "detailedDescription"},
				{key: "image"},{key: "thumbNail"},{key: "listPrice"},{key: "sellPrice"},{key: "inventoryNumber"},{key: "isShown"},
				{key: "pageTitle"},{key: "keywords"},{key: "description"},{key: "metaTag"},{key: "url"}]
	        };
	
	        myDataTable = new YAHOO.widget.ScrollingDataTable("showArea",
	                myColumnDefs, myDataSource, {width:"500px"});
	                
	        var test = function()
	        {
	        	
	        	alert('test');
	        	var item = Beelun.shoppro.admin.model.item.getItemFromPanel();
	        	alert(item.id);
	        };
            myDataTable.subscribe("rowClickEvent",myDataTable.onEventSelectRow);
	        myDataTable.subscribe("cellDblclickEvent",myDataTable.onEventShowCellEditor);
	        myDataTable.subscribe("cellDblclickEvent",test);
	        myDataTable.subscribe("editorBlurEvent", myDataTable.onEventSaveCellEditor);
	        myDataTable.subscribe("rowMouseoverEvent", myDataTable.onEventHighlightRow);
        	myDataTable.subscribe("rowMouseoutEvent", myDataTable.onEventUnhighlightRow);
        	myDataTable.subscribe("buttonClickEvent", editItemClick);
        	
        	
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
        	
        	var callback = function(item)
        	{
        		alert(item.id);
        		//myDataTable.addRow(item);
        		myDataTable.refresh();
        		myDataTable.refreshView();
        	}
        	
        	var testfun = function()
        	{
        		var brand = {id:0, name:"Spirent", image:"1.jpg", webSite:"http://www.spirent.com"};
				var item = {name: "ab", brand: brand, sellPrice: 100, inventoryNumber: 123345, isShown:0, pageTitle:"test Save Title", url: "test.jpg" };
				
				ItemManager.save(item, callback);
        	};
        	
        	
        	
        	
        	
        	//YAHOO.util.Event.addListener("test", "click", testfun);
		}
		
		function test()
		{
    		ItemManager.getItemByUrl('item-1.html', testResponse);
		}
		
	    function testResponse(item)
		{
			DWRUtil.setValue("showArea", item.detailedDescription);
		}				   
	
	
		function test1(){
			var brand = {id:0, name:"Spirent", image:"1.jpg", webSite:"http://www.spirent.com"};
			var item = {name: "ab", brand: brand, sellPrice: 100, inventoryNumber: 123345, isShown:0, pageTitle:"test Save Title", url: "test.jpg" };
			var items = [9,10,11];
			var a = new Array();
			a.push(5);
			a.push(6);
			ItemManager.removeItems(a, callback);
			
			
					
			//OrderManager.getAll(callback);

        }
        
        function callback(value){
        	alert("item.id " + value);
        	//myDataTable.addRow(item, 4);
    		//alert(itemList.length);
        }
    </script>
</head>

<body>
	<p>Messages:</p>
	<div id="showArea"></div>
	<span>Item List Table</span>
	<div id="itemListTable"></div>
		<p>
  		<input type="button" value="Get all items" onclick="sendRequest()"/>
  		<input type="button" id="test" name="test" value="Get one item" onclick="test1()"/>
	</p>	
	
	<div id="editItemDialog">
	<div class="hd"></div>
	<div class="bd">
		<div>
			<table>
				<tr>
					<td>Item id</td>
					<td><input id="itemId" type="text" value=""></td>
				</tr>
				<tr>
					<td>Brand</td>
					<td>
						<select id="brand"></select>
						<input type="hidden" id="brandId"/>
						<input type="hidden" id="brandImage"/>
						<input type="hidden" id="brandWebsite"/>
					</td>
				</tr>
				<tr>
					<td>netSuite Id</td>
					<td><input id="netSuiteId" type="text" value=""></td>
				</tr>
				<tr>
					<td>name</td>
					<td><input id="name" type="text" value=""></td>
				</tr>
				<tr>
					<td>serialNumber</td>
					<td><input id="serialNumber" type="text" value=""></td>
				</tr>
				<tr>
					<td>shortDescription</td>
					<td><input id="shortDescription" type="text" value=""></td>
				</tr>
				<tr>
					<td>detailedDescription</td>
					<td><input id="detailedDescription" type="text" value=""></td>
				</tr>
				<tr>
					<td>image</td>
					<td><input id="image" type="text" value=""></td>
				</tr>
				<tr>
					<td>thumbNail</td>
					<td><input id="thumbNail" type="text" value=""></td>
				</tr>
				<tr>
					<td>listPrice</td>
					<td><input id="listPrice" type="text" value=""></td>
				</tr>
				<tr>
					<td>sellPrice</td>
					<td><input id="sellPrice" type="text" value=""></td>
				</tr>
				<tr>
					<td>inventoryNumber</td>
					<td><input id="inventoryNumber" type="text" value=""></td>
				</tr>
				<tr>
					<td>isShown</td>
					<td><input id="isShown" type="text" value=""></td>
				</tr>
				<tr>
					<td>pageTitle</td>
					<td><input id="pageTitle" type="text" value=""></td>
				</tr>
				<tr>
					<td>keywords</td>
					<td><input id="keywords" type="text" value=""></td>
				</tr>
				<tr>
					<td>description</td>
					<td><input id="description" type="text" value=""></td>
				</tr>
				<tr>
					<td>metaTag</td>
					<td><input id="metaTag" type="text" value=""></td>
				</tr>
				<tr>
					<td>url</td>
					<td><input id="url" type="text" value=""></td>
				</tr>
			</table>
		</div>

	</div>
	</div>
</body>