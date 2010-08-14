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


Beelun.shoppro.admin.model.myGlob = {

    getMyGlob: function() {
    	var myGlob = {};
    	
    	var myGlobIdEle = YAHOO.util.Dom.get('myGlobId');
    	myGlob.id = myGlobIdEle.value;
		
		var shopNameEle = YAHOO.util.Dom.get('shopName');
		myGlob.shopName = shopNameEle.value;
		
		var sloganEle = YAHOO.util.Dom.get('slogan');
		myGlob.slogan = sloganEle.value;
		
		var phoneNumberEle = YAHOO.util.Dom.get('phoneNumber');
		myGlob.phoneNumber = phoneNumberEle.value;
		
		var logoAbsoluteUrlEle = YAHOO.util.Dom.get('logoAbsoluteUrl');
		myGlob.logoAbsoluteUrl = logoAbsoluteUrlEle.value;
		
		var addressEle = YAHOO.util.Dom.get('address');
		myGlob.address = addressEle.value;
		
		var page404Ele = YAHOO.util.Dom.get('page404');
		myGlob.page404 = page404Ele.value;
		
		var page500Ele = YAHOO.util.Dom.get('page500');
		myGlob.page500 = page500Ele.value;
		
		var pageNoSearchResultEle = YAHOO.util.Dom.get('pageNoSearchResult');
		myGlob.pageNoSearchResult = pageNoSearchResultEle.value;
		
		var footerEle = YAHOO.util.Dom.get('footer');
		myGlob.footer = footerEle.value;
		
		var googleCustSearchCodeEle = YAHOO.util.Dom.get('googleCustSearchCode');
		myGlob.googleCustSearchCode = googleCustSearchCodeEle.value;
		
		var signupAgreementEle = YAHOO.util.Dom.get('signupAgreement');
		myGlob.signupAgreement = signupAgreementEle.value;
		
		var unlockEmailTemplateEle = YAHOO.util.Dom.get('unlockEmailTemplate');
		myGlob.unlockEmailTemplate = unlockEmailTemplateEle.value;
		
		var resetPasswordMailTemplateEle = YAHOO.util.Dom.get('resetPasswordMailTemplate');
		myGlob.resetPasswordMailTemplate = resetPasswordMailTemplateEle.value;
		
		var versionEle = YAHOO.util.Dom.get('version');
		myGlob.version = versionEle.value;
		
		var siteTypeEle = YAHOO.util.Dom.get('siteType');
		myGlob.siteType = siteTypeEle.value;
		
		//var maxUploadFileSizeEle = YAHOO.util.Dom.get('maxUploadFileSize');
		//myGlob.maxUploadFileSize = maxUploadFileSizeEle.value;
		
		return myGlob;
                
    },
    
    setMyGlob: function(myGlob) {
    	
    	var myGlobId = myGlob.id;
		var shopName = myGlob.shopName;
		var slogan = myGlob.slogan;
		var phoneNumber = myGlob.phoneNumber;
		var logoAbsoluteUrl = myGlob.logoAbsoluteUrl;
		var address = myGlob.address;
		var page404 = myGlob.page404;
		var page500 = myGlob.page500;
		var pageNoSearchResult = myGlob.pageNoSearchResult;
		var footer = myGlob.footer;
		var googleCustSearchCode = myGlob.googleCustSearchCode;
		var signupAgreement = myGlob.signupAgreement;
		var unlockEmailTemplate = myGlob.unlockEmailTemplate;
		var resetPasswordMailTemplate = myGlob.resetPasswordMailTemplate;
		var maxUploadFileSize = myGlob.maxUploadFileSize;
		var version = myGlob.version;
		var siteType = myGlob.siteType;
		
		var myGlobIdEle = YAHOO.util.Dom.get('myGlobId');
    	myGlobIdEle.value = myGlobId;
		
		var shopNameEle = YAHOO.util.Dom.get('shopName');
		shopNameEle.value = shopName;
		
		var sloganEle = YAHOO.util.Dom.get('slogan');
		sloganEle.value = slogan;
		
		var phoneNumberEle = YAHOO.util.Dom.get('phoneNumber');
		phoneNumberEle.value = phoneNumber;
		
		var logoAbsoluteUrlEle = YAHOO.util.Dom.get('logoAbsoluteUrl');
		logoAbsoluteUrlEle.value = logoAbsoluteUrl;
		
		var addressEle = YAHOO.util.Dom.get('address');
		addressEle.value = address;
		
		var page404Ele = YAHOO.util.Dom.get('page404');
		page404Ele.value = page404;
		
		var page500Ele = YAHOO.util.Dom.get('page500');
		page500Ele.value = page500;
		
		var pageNoSearchResultEle = YAHOO.util.Dom.get('pageNoSearchResult');
		pageNoSearchResultEle.value = pageNoSearchResult;
		
		var footerEle = YAHOO.util.Dom.get('footer');
		footerEle.value = footer;
		
		var googleCustSearchCodeEle = YAHOO.util.Dom.get('googleCustSearchCode');
		googleCustSearchCodeEle.value = googleCustSearchCode;
		
		var signupAgreementEle = YAHOO.util.Dom.get('signupAgreement');
		signupAgreementEle.value = signupAgreement;
		
		var unlockEmailTemplateEle = YAHOO.util.Dom.get('unlockEmailTemplate');
		unlockEmailTemplateEle.value = unlockEmailTemplate;
    	
    	var resetPasswordMailTemplateEle = YAHOO.util.Dom.get('resetPasswordMailTemplate');
    	resetPasswordMailTemplateEle.value = resetPasswordMailTemplate;
    	
    	var versionEle = YAHOO.util.Dom.get('version');
    	versionEle.value = version;
		
    	var siteTypeEle = YAHOO.util.Dom.get('siteType');
		siteTypeEle.value = siteType;
		
		//var maxUploadFileSizeEle = YAHOO.util.Dom.get('maxUploadFileSize');
		//maxUploadFileSizeEle.value = maxUploadFileSize;
    },
    
    initGlobTable: function()
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
			
    	var initMyGlobTable = function()
		{
    		updatingPanel.show();
    		MyGlobManager.fetch(fnGetMyGlobHandleSuccess);    		    	
		};
		
		var fnGetMyGlobHandleSuccess = function(myGlob)
		{		
			
			updatingPanel.hide();
		 	var tabView = new YAHOO.widget.TabView('globalTabView');
			
			 
			var initGlobalTable = function(myGlob) {
				
				Beelun.shoppro.admin.model.myGlob.setMyGlob(myGlob);
			}
			
			var fnSaveButtonClick = function()
			{
				var myGlob = Beelun.shoppro.admin.model.myGlob.getMyGlob();
				
				var fnSaveBack = function(item)
				{
					updatingPanel.hide();
					window.location.reload();
				};
				MyGlobManager.save(myGlob, fnSaveBack);
				//editMyGlobDialog.hide();
				updatingPanel.show();
			}
			initGlobalTable(myGlob);

			YAHOO.util.Event.addListener("save", "click", fnSaveButtonClick);
			
			};
			
			YAHOO.util.Event.onContentReady("myGlobTable", function () {
			
				initMyGlobTable();
			});
			
    }

}
    

