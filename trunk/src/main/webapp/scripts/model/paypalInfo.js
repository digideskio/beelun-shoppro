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


Beelun.shoppro.admin.model.paypalInfo = {

    getPaypalInfo: function() {
    	var paypalInfo = {};
    	
    	var paypalInfoIdEle = YAHOO.util.Dom.get('paypalInfoId');
    	paypalInfo.id = paypalInfoIdEle.value;
    	
    	var apiUserNameEle = YAHOO.util.Dom.get('apiUserName');
    	paypalInfo.apiUserName = apiUserNameEle.value;
    	
    	var apiPasswordEle = YAHOO.util.Dom.get('apiPassword');
    	paypalInfo.apiPassword = apiPasswordEle.value;
    	
    	var apiSignatureEle = YAHOO.util.Dom.get('apiSignature');
    	paypalInfo.apiSignature = apiSignatureEle.value;
    	
    	var useSandboxEle = YAHOO.util.Dom.get('useSandbox');
    	paypalInfo.useSandbox = useSandboxEle.value;
		
		return paypalInfo;
                
    },
    
    setPaypalInfo: function(paypalInfo) {
    	
    	var paypalInfoId = paypalInfo.id;
    	var apiUserName = paypalInfo.apiUserName;
    	var apiPassword = paypalInfo.apiPassword;
    	var apiSignature = paypalInfo.apiSignature;
    	var useSandbox = paypalInfo.useSandbox;
    	
    	var paypalInfoIdEle = YAHOO.util.Dom.get('paypalInfoId');
    	paypalInfoIdEle.value = paypalInfoId;
    	
    	var apiUserNameEle = YAHOO.util.Dom.get('apiUserName');
    	apiUserNameEle.value = apiUserName;
    	
    	var apiPasswordEle = YAHOO.util.Dom.get('apiPassword');
    	apiPasswordEle.value = apiPassword;
    	
    	var apiSignatureEle = YAHOO.util.Dom.get('apiSignature');
    	apiSignatureEle.value = apiSignature;
    	
    	var useSandboxEle = YAHOO.util.Dom.get('useSandbox');
    	useSandboxEle.value = useSandbox;
    	
    },
    
    initPaypalInfoTable: function()
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
			
    	var initMyPaypalInfoTable = function()
		{
    		
    		updatingPanel.show();
    		PaypalAccessInfoManager.fetch(fnGetPaypalInfoHandleSuccess);    		    	
		};
		
		var fnGetPaypalInfoHandleSuccess = function(paypalInfo)
		{		
			
			updatingPanel.hide();
			var tabView = new YAHOO.widget.TabView('paypalInfoTabView');
				
			var initPaypalTable = function(paypalInfo) {
				
				Beelun.shoppro.admin.model.paypalInfo.setPaypalInfo(paypalInfo);
			}
			
			var fnSaveButtonClick = function()
			{
				var paypalInfo = Beelun.shoppro.admin.model.paypalInfo.getPaypalInfo();
				
				var fnSaveBack = function(item)
				{
					updatingPanel.hide();
					window.location.reload();
				};
				PaypalAccessInfoManager.save(paypalInfo, fnSaveBack);
				//editMyGlobDialog.hide();
				updatingPanel.show();
			}
			initPaypalTable(paypalInfo);

			YAHOO.util.Event.addListener("save", "click", fnSaveButtonClick);
			
			};
			
			YAHOO.util.Event.onContentReady("paypalInfoTable", function () {
			
				initMyPaypalInfoTable();
			});
			
    }

}
    

