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

	Beelun.shoppro.App = function() {
        this.initApp();
    };

    var proto = Beelun.shoppro.App.prototype;

    proto.initApp = function(attr) {

        /*
         * Initial Loading dialog
 		 */
        
        
        var loadingPanel = 
            new YAHOO.widget.Panel("loadingPanel",  
                                            { width: "240px", 
                                              fixedcenter: true, 
                                              close: false, 
                                              draggable: false, 
                                              zindex:4,
                                              modal: true,
                                              visible: false
                                            } 
                                        );

        loadingPanel.setHeader("Updating, please wait...");
        loadingPanel.setBody("<img src=\"/images/loading.gif\"/>");
        loadingPanel.render(document.body);
        
    	
        
        
        var fnSaveRecInfoHandleSuccess = function(o){

        	loadingPanel.hide();   
    	    var addressId = o.responseXML.getElementsByTagName('addressId').item(0).firstChild.nodeValue;
        	YAHOO.util.Dom.get('recAddressId').value = addressId;
			var msgEle = YAHOO.util.Dom.get('saveRecInfoMessage');
			msgEle.innerHTML = "Your new information has been saved!";
        	//updateCartDialog(o.responseXML);
        	
        };

        var fnSaveRecInfoHandleFailure = function(o){
        	alert('There is a error. Please contact administrator. Thank you!');
        };
        var oSaveRecInfoCallback =
        {
        	success:fnSaveRecInfoHandleSuccess,
        	failure:fnSaveRecInfoHandleFailure
        };

        var  saveRecInfoEleRequest = function(o){     	
        	var sUrl = "/customer/create-update-address.html?";
        	
        	var recAddress = YAHOO.util.Dom.get('recAddress').value;
        	var recZip = YAHOO.util.Dom.get('recZip').value;
        	var recRecipientName = YAHOO.util.Dom.get('recRecipientName').value;
        	var recMobile = YAHOO.util.Dom.get('recMobile').value;
        	var recPhone = YAHOO.util.Dom.get('recPhone').value;
        	var recAddressId = YAHOO.util.Dom.get('recAddressId').value;
        	
        	if(Beelun.shoppro.Util.trimTxt(recAddress) && Beelun.shoppro.Util.trimTxt(recZip) 
        			&& Beelun.shoppro.Util.trimTxt(recRecipientName) && Beelun.shoppro.Util.trimTxt(recMobile) 
        					&& Beelun.shoppro.Util.trimTxt(recPhone))
        	{
        		if(!Beelun.shoppro.Util.isNumber(recZip))
            	{
        			alert("Please input valid Zip code!");
        			return;
            	}
        		if(!Beelun.shoppro.Util.isNumber(recMobile))
            	{
        			alert("Please input valid mobile phone number!");
        			return;
            	}
        		if(!Beelun.shoppro.Util.isNumber(recPhone))
            	{
        			alert("Please input valid phone number!");
        			return;
            	}
        		
        		if(recAddressId)
        		{
                	sUrl = sUrl + "address=" + recAddress + "&zip=" + recZip + "&recipientName=" + recRecipientName
                	+ "&phoneNumber=" + recPhone + "&mobileNumber=" + recMobile + "&id=" + recAddressId + "&t=" + (new Date).getTime();
                	var request = YAHOO.util.Connect.asyncRequest('GET', sUrl, oSaveRecInfoCallback);
        		}
        		else
        		{
        			sUrl = sUrl + "address=" + recAddress + "&zip=" + recZip + "&recipientName=" + recRecipientName
                	+ "&phoneNumber=" + recPhone + "&mobileNumber=" + recMobile + "&t=" + (new Date).getTime();
                	var request = YAHOO.util.Connect.asyncRequest('GET', sUrl, oSaveRecInfoCallback);
        		}
        		loadingPanel.show();
        		return;
        	}
        	else
        	{
        		alert('Please input all information!');
        		return;
        	}


        };
             
        YAHOO.util.Event.addListener('saveRecInfo', "click", saveRecInfoEleRequest);
        
        
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
        YAHOO.util.Dom.removeClass("addToCartDialog", "yui-pe-content");

    	// Instantiate the Dialog
    	var addToCartDialog = new YAHOO.widget.Dialog("addToCartDialog", 
    							{ width : "30em",
    							  fixedcenter : true,
    							  visible : false, 
    							  draggable: false,
    							  modal: true,
    							  constraintoviewport : true,
    							  buttons : [ { text:"Go to Checkout", handler:handleSubmit, isDefault:true },
    								      { text:"Continue Shopping", handler:handleCancel } ]
    							});
    	// Render the Dialog
    	addToCartDialog.render();
    	
    	var updateCartDialog = function(xml) {
    		
    		var xmlEle = xml;
    		var itemNumber = xmlEle.getElementsByTagName('itemNumber').item(0).firstChild.nodeValue;    
    	    var itemValue = xmlEle.getElementsByTagName('itemValue').item(0).firstChild.nodeValue;   
    		
    		var totalArticle = YAHOO.util.Dom.get('dialogTotalArticle');
    		totalArticle.innerHTML = itemNumber;
    		var totalPrice = YAHOO.util.Dom.get('dialogTotalPrice');
    		totalPrice.innerHTML = parseInt(itemValue*100)/100;
    		addToCartDialog.show();
    		
    		// Bali: Update item number in cart
    		YAHOO.util.Dom.get('itemNumberInCart').innerHTML = itemNumber;
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

        var  addCartItemRequest = function(o){     	
        	var sUrl = "/cart/cartAjax.html?";
        	var activeEle;
        	if(o.srcElement)
        	{
        		activeEle = o.srcElement;
        	}
        	else
        	{
        		activeEle = o.currentTarget;
        	}
        	var itemId = activeEle.id
        	sUrl = sUrl + "itemId=" + itemId + "&action=add";
        	var request = YAHOO.util.Connect.asyncRequest('GET', sUrl, oAddCartCallback);

        };
       
        var addToCartElements = YAHOO.util.Dom.getElementsByClassName('addToCart_button');  
        
        YAHOO.util.Event.addListener(addToCartElements, "click", addCartItemRequest);
        
        var product_items = YAHOO.util.Dom.getElementsByClassName('product_item');
        
        /*
         * Initial for cart/ItemList
 		 */
    	var removeItemFromCart = function(xml, oActiveEle) {
    		
    		var xmlEle = xml;
    		//var itemNumber = xmlEle.getElementsByTagName('itemNumber').item(0).firstChild.nodeValue;   
    		var itemNumber = xmlEle.getElementsByTagName('itemNumber').item(0).firstChild.nodeValue;
    	    var itemValue = xmlEle.getElementsByTagName('itemValue').item(0).firstChild.nodeValue;   
    		
    		var totalValue = YAHOO.util.Dom.get('totalValue');
    		totalValue.innerHTML = "$" + itemValue;
    		
    		YAHOO.util.Dom.get('itemNumberInCart').innerHTML = itemNumber;
    		
    		var tbody = oActiveEle.parentNode.parentNode.parentNode;
    		tbody.removeChild(oActiveEle.parentNode.parentNode);
    	};
    	
        var fnDeleteCartHandleSuccess = function(o){

        	removeItemFromCart(o.responseXML, o.argument.oActiveEle);
        	
        };

        var fnDeleteCartHandleFailure = function(o){
        	alert('There is a error. Please contact administrator. Thank you!');
        };
        
        var oDeleteCartCallback =
        {
        	success:fnDeleteCartHandleSuccess,
        	failure:fnDeleteCartHandleFailure,
        	argument:{oActiveEle:null}
        };
        
        var deleteCartItemRequest = function(o){
        	
        	if(confirm('Delete this item?'))
        	{
	        	var sUrl = "/cart/cartAjax.html?";
	        	var activeEle;
	        	if(o.srcElement)
	        	{
	        		activeEle = o.srcElement;
	        	}
	        	else
	        	{
	        		activeEle = o.currentTarget;
	        	}
	        	var itemId = activeEle.parentNode.parentNode.getElementsByTagName('td')[0].innerHTML;
	        	var itemCount = activeEle.parentNode.parentNode.getElementsByTagName('td')[3].getElementsByTagName('input')[0].value;
	        	sUrl = sUrl + "itemId=" + itemId + "&action=remove&n=" + itemCount;
	        	oDeleteCartCallback.argument.oActiveEle = activeEle;
	        	var request = YAHOO.util.Connect.asyncRequest('GET', sUrl, oDeleteCartCallback);
        	}

        };
        
        var deleteItemElements = YAHOO.util.Dom.getElementsByClassName('deleteItem');
        
        YAHOO.util.Event.addListener(deleteItemElements, "click", deleteCartItemRequest);
        
    	var changeItemNumber = function(xml, oActiveEle) {
    		
    		var xmlEle = xml;
    		var itemNumber = xmlEle.getElementsByTagName('itemNumber').item(0).firstChild.nodeValue;    
    	    var itemValue = xmlEle.getElementsByTagName('itemValue').item(0).firstChild.nodeValue;   
    		
    	    YAHOO.util.Dom.get('itemNumberInCart').innerHTML = itemNumber;
    		var totalValue = YAHOO.util.Dom.get('totalValue');
    		totalValue.innerHTML = "$" + itemValue;
    		
    		var newItemCountObj = oActiveEle.parentNode.parentNode.getElementsByTagName('td')[3].getElementsByTagName('input')[0];
        	var oldItemCountObj = oActiveEle.parentNode.parentNode.getElementsByTagName('td')[3].getElementsByTagName('input')[1];
        	
        	oldItemCountObj.value = newItemCountObj.value;
    	};
    	
        var fnChangeCartHandleSuccess = function(o){

        	changeItemNumber(o.responseXML, o.argument.oActiveEle);
        	
        };

        var fnChangeCartHandleFailure = function(o){
        	alert('There is a error. Please contact administrator. Thank you!');
        };
        
        var oChangeCartCallback =
        {
        	success:fnChangeCartHandleSuccess,
        	failure:fnChangeCartHandleFailure,
        	argument:{oActiveEle:null}
        };
        
        var  itemCountInputChange = function(o){
        	
        	var activeEle;
        	if(o.srcElement)
        	{
        		activeEle = o.srcElement;
        	}
        	else
        	{
        		activeEle = o.currentTarget;
        	}
        	var newItemCountObj = activeEle.parentNode.parentNode.getElementsByTagName('td')[3].getElementsByTagName('input')[0];
        	var oldItemCountObj = activeEle.parentNode.parentNode.getElementsByTagName('td')[3].getElementsByTagName('input')[1];
        	var newItemCount = newItemCountObj.value;
        	var oldItemCount = oldItemCountObj.value;
        	
        	if(!Beelun.shoppro.Util.isNumber(newItemCount))
        	{
        		alert('Please input valid number!');
        		newItemCountObj.value = oldItemCount;
        		return;
        	}
        	if(parseInt(newItemCount) == 0)
        	{
            	if(confirm('Delete this item?'))
            	{
    	        	var sUrl = "/cart/cartAjax.html?";
    	        	var activeEle;
    	        	if(o.srcElement)
    	        	{
    	        		activeEle = o.srcElement;
    	        	}
    	        	else
    	        	{
    	        		activeEle = o.currentTarget;
    	        	}
    	        	var itemId = activeEle.parentNode.parentNode.getElementsByTagName('td')[0].innerHTML;
    	        	sUrl = sUrl + "itemId=" + itemId + "&action=remove&n=" + oldItemCount;
    	        	oDeleteCartCallback.argument.oActiveEle = activeEle;
    	        	var request = YAHOO.util.Connect.asyncRequest('GET', sUrl, oDeleteCartCallback);
    	        	return;
            	}
            	else
            	{
            		newItemCountObj.value = oldItemCount;
            		return;
            	}
        	}
        	else
        	{
	        	var sUrl = "/cart/cartAjax.html?";
	        	var activeEle;
	        	if(o.srcElement)
	        	{
	        		activeEle = o.srcElement;
	        	}
	        	else
	        	{
	        		activeEle = o.currentTarget;
	        	}
	        	var itemId = activeEle.parentNode.parentNode.getElementsByTagName('td')[0].innerHTML;
	        	sUrl = sUrl + "itemId=" + itemId + "&action=set_items&n=" + newItemCount;
	        	oChangeCartCallback.argument.oActiveEle = activeEle;
	        	var request = YAHOO.util.Connect.asyncRequest('GET', sUrl, oChangeCartCallback);
        	}
        	
        	oldItemCountObj.value = newItemCount;
        };

        
        var itemCountInputElements = YAHOO.util.Dom.getElementsByClassName('itemCountInput');
        
        YAHOO.util.Event.addListener(itemCountInputElements, "blur", itemCountInputChange);
        
        
        
        var  showRecInfo = function(o){
        	YAHOO.util.Dom.removeClass('recipientInfo', 'hide');
        };
        
        var editRecInfoElement = YAHOO.util.Dom.get('editRecInfo');
        
        YAHOO.util.Event.addListener(editRecInfoElement, "click", showRecInfo);
        
        
        var clearRecInfo = function(){
        	var recAddress = YAHOO.util.Dom.get('recAddress');
        	var recZip = YAHOO.util.Dom.get('recZip');
        	var recRecipientName = YAHOO.util.Dom.get('recRecipientName');
        	var recMobile = YAHOO.util.Dom.get('recMobile');
        	var recPhone = YAHOO.util.Dom.get('recPhone');
        	
        	recAddress.value = '';
        	recZip.value = '';
        	recRecipientName.value = '';
        	recMobile.value = '';
        	recPhone.value = '';
        	
        };
        
        var  recInfoRadioChange = function(o){
        	var activeEle;
        	if(o.srcElement)
        	{
        		activeEle = o.srcElement;
        	}
        	else
        	{
        		activeEle = o.currentTarget;
        	}
        	
        	if(activeEle.value == 0)
        	{
        		YAHOO.util.Dom.addClass('recipientInfo', 'hide');
        		
        	}
        	else if(activeEle.value == 1)
        	{
        		YAHOO.util.Dom.removeClass('recipientInfo', 'hide');
        		clearRecInfo();
        	}
        	
        	
        };
        
        var recInfoRadioElements = YAHOO.util.Dom.getElementsByClassName('recInfoRadio');
        
        YAHOO.util.Event.addListener(recInfoRadioElements, "click", recInfoRadioChange);
        
        var sameAddrChange = function()
        {
        	var sameAddr = YAHOO.util.Dom.get('sameAddress');
        	if (sameAddr.checked == true)
        	{
        		YAHOO.util.Dom.addClass('billingAddrDiv', 'hide');
        	}
        	else
        	{
        		YAHOO.util.Dom.removeClass('billingAddrDiv', 'hide');
        	}
        };
        
        YAHOO.util.Event.addListener("sameAddress", "click", sameAddrChange);
        
        
        YAHOO.util.Event.onContentReady("f", function () {
        	YAHOO.util.Dom.get('j_username').focus();
           });
        
        YAHOO.util.Event.onContentReady("userForm", function () {
			document.getElementById('name').focus();
			Beelun.shoppro.Util.userFormValidator();
        });

        YAHOO.util.Event.onContentReady("sendMePasswordForm", function () {
        	YAHOO.util.Dom.get('email').focus();
           });
        
        var shipDateCal = null;
        
        var initCal = function()
        {
        	var handleSelect = function(type,args,obj)
        	{
                    var dates = args[0]; 
                    var date = dates[0];
                    var year = date[0], month = date[1], day = date[2];
                    var shipDateInput = YAHOO.util.Dom.get('specifiedDate');
                    YAHOO.util.Dom.removeClass('specifiedDate', 'hide');
                    shipDateInput.value = year + "/" + month + "/" + day;
                    shipDateCal.hide();
        	};
        	
        	shipDateCal = new YAHOO.widget.Calendar("shipDateCal","shipDateCalContainer", { title:"Choose a date:", close:true } );
        	shipDateCal.render();
        	shipDateCal.hide();
        	shipDateCal.selectEvent.subscribe(handleSelect, shipDateCal, true);
        };
        
        YAHOO.util.Event.onContentReady("shipDate", initCal);
        
        var shipDateChange = function()
        {
        	var shipDateEle = YAHOO.util.Dom.get('shipDate');
        	var shipDate = shipDateEle.options[shipDateEle.selectedIndex].text;
            
        	if(shipDate == "Specified date")
        	{
        		shipDateCal.show();
        		YAHOO.util.Dom.removeClass('specifiedDate', 'hide');
        	}
        	else
        	{
        		shipDateCal.hide();
        		YAHOO.util.Dom.addClass('specifiedDate', 'hide');
        	}
        	
        };
        
        YAHOO.util.Event.addListener("shipDate", "change", shipDateChange);

    };
    

    
    


})();