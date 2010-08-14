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


Beelun.shoppro.admin.model.user = {

    getUser: function() {
    	var user = {};
    	
    	var userIdEle = YAHOO.util.Dom.get('userId');
		user.id = userIdEle.value;
		
		var nameEle = YAHOO.util.Dom.get('name');
		user.name = nameEle.value;
		
		var emailEle = YAHOO.util.Dom.get('email');
		user.email = emailEle.value;
		
		var passwordEle = YAHOO.util.Dom.get('password');
		user.password = passwordEle.value;
		
		//var securityQuestionEle = YAHOO.util.Dom.get('securityQuestion');
		//user.securityQuestion = securityQuestionEle.value;
		
		//var securityQuestionAnswerEle = YAHOO.util.Dom.get('securityQuestionAnswer');
		//user.securityQuestionAnswer = securityQuestionAnswerEle.value;
		
		var enabledEle = YAHOO.util.Dom.get('enabled');
		user.enabled = enabledEle.value;
		
		var accountExpiredEle = YAHOO.util.Dom.get('accountExpired');
		user.accountExpired = accountExpiredEle.value;
		
		var accountLockedEle = YAHOO.util.Dom.get('accountLocked');
		user.accountLocked = accountLockedEle.value;
		
		var credentialsExpiredEle = YAHOO.util.Dom.get('credentialsExpired');
		user.credentialsExpired = credentialsExpiredEle.value;
		
		var shippingAddressEle = YAHOO.util.Dom.get('shippingAddress');
		user.shippingAddress = shippingAddressEle.value;
		
		var billingAddressEle = YAHOO.util.Dom.get('billingAddress');
		user.billingAddressAddress = billingAddressEle.value;
		
		var createdWhenEle = YAHOO.util.Dom.get('createdWhen');
		user.createdWhen = createdWhenEle.value;
		
		var lastLoginEle = YAHOO.util.Dom.get('lastLogin');
		user.lastLogin = lastLoginEle.value;
		
		return user;
                
    },
    
    setUser: function(user) {
    	
    	var userId = user.id;
		var name = user.name;
		var email = user.email;
		var password = user.password;
		var securityQuestion = user.securityQuestion;
		var securityQuestionAnswer = user.securityQuestionAnswer;
		var enabled = user.enabled;
		var accountExpired = user.accountExpired;
		var accountLocked = user.accountLocked;
		var credentialsExpired = user.credentialsExpired;
		var shippingAddress = user.shippingAddress;
		var billingAddress = user.billingAddress;
		var createdWhen = user.createdWhen;
		var lastLogin = user.lastLogin;
		
    	var userIdEle = YAHOO.util.Dom.get('userId');
		userIdEle.value = userId;
		
		var nameEle = YAHOO.util.Dom.get('name');
		nameEle.value = name;
		
		var emailEle = YAHOO.util.Dom.get('email');
		emailEle.value = email;
		
		var passwordEle = YAHOO.util.Dom.get('password');
		passwordEle.value = password;
		
		//var securityQuestionEle = YAHOO.util.Dom.get('securityQuestion');
		//securityQuestionEle.value = securityQuestion;
		
		//var securityQuestionAnswerEle = YAHOO.util.Dom.get('securityQuestionAnswer');
		//securityQuestionAnswerEle.value = securityQuestionAnswer;
		
		var enabledEle = YAHOO.util.Dom.get('enabled');
		enabledEle.value = enabled;
		
		var accountExpiredEle = YAHOO.util.Dom.get('accountExpired');
		accountExpiredEle.value = accountExpired;
		
		var accountLockedEle = YAHOO.util.Dom.get('accountLocked');
		accountLockedEle.value = accountLocked;
		
		var credentialsExpiredEle = YAHOO.util.Dom.get('credentialsExpired');
		credentialsExpiredEle.value = credentialsExpired;
		
		var shippingAddressEle = YAHOO.util.Dom.get('shippingAddress');
		shippingAddressEle.value = shippingAddress;
		
		var billingAddressEle = YAHOO.util.Dom.get('billingAddress');
		billingAddressEle.value = billingAddress;
		
		var createdWhenEle = YAHOO.util.Dom.get('createdWhen');
		createdWhenEle.value = createdWhen;
		
		var lastLoginEle = YAHOO.util.Dom.get('lastLogin');
		lastLoginEle.value = lastLogin;
    },
    
    initUserTable: function()
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
		
    	var initUserListTable = function()
		{
    		updatingPanel.show();
    		UserManager.getUsers(fnGetUserListHandleSuccess);
    		
		};
		
		var fnGetUserListHandleSuccess = function(userList)
		{
		
			updatingPanel.hide();
		
			/*
	         * Initial editItemDialog Dialog
	 		 */
			
	    	// Define various event handlers for Dialog
	    	var handleSubmit = function() {
	    		
	    		var user = Beelun.shoppro.admin.model.user.getUser();
	
				var fnSaveUserBack = function(item)
				{
					updatingPanel.hide();
				};
				UserManager.save(user, fnSaveUserBack);
				editUserDialog.hide();
				updatingPanel.show();
	    	};
	    	var handleCancel = function() {
	    		this.cancel();
	    	};
	    	
	    	// Instantiate the Dialog
	    	var editUserDialog = new YAHOO.widget.Dialog("editUserDialog", 
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
	    	editUserDialog.render();
	    	YAHOO.util.Dom.removeClass("editUserDialog", "hide");
	    	var updateUserDialog = function(record){
	    		var item = record._oData;
	    		Beelun.shoppro.admin.model.user.setUser(item);
	    		editUserDialog.show();
	    	}
	    	var  editItemClick = function(o){
	    		var oRecord = this.getRecord(o.target);
	    		updateUserDialog(oRecord);

	        };
			
			/*
	         * Initial user datatable
	 		 */
			
			var shippingAddressFormatter = function(elLiner, oRecord, oColumn, oData) {
				if(oRecord.getData("shippingAddress"))
				{
					elLiner.innerHTML = oRecord.getData("shippingAddress").name;
				}
				else
				{
					elLiner.innerHTML = "N/A";
				}
		
			};
			
			var billingAddressFormatter = function(elLiner, oRecord, oColumn, oData) {
				if(oRecord.getData("billingAddress"))
				{
					elLiner.innerHTML = oRecord.getData("billingAddress").name;
				}
				else
				{
					elLiner.innerHTML = "N/A";
				}
		
			};
			
			var recipientFormatter = function(elLiner, oRecord, oColumn, oData) {
				if(oRecord.getData("shippingAddress"))
				{
					elLiner.innerHTML = oRecord.getData("shippingAddress").recipientName;
				}
				else
				{
					elLiner.innerHTML = "N/A";
				}
			};
			
			var mobileFormatter = function(elLiner, oRecord, oColumn, oData) {
				if(oRecord.getData("shippingAddress"))
				{
					elLiner.innerHTML = oRecord.getData("shippingAddress").mobileNumber;
				}
				else
				{
					elLiner.innerHTML = "N/A";
				}
			};
			
			var myColumnDefs = [
	            {key:"id", sortable:true, resizeable:true},
				{key:"name",sortable:true, resizeable:true},
				{key:"email", sortable:true, resizeable:true},
				{key:"password", hidden:true, sortable:true, resizeable:true},
				//{key:"securityQuestion", editor:"textbox", sortable:true, resizeable:true},
				//{key:"securityQuestionAnswer", editor:"textbox", sortable:true, resizeable:true},
				{key:"enabled", editor:new YAHOO.widget.RadioCellEditor({radioOptions:["true","false"],disableBtns:false}), sortable:true, resizeable:true},
				{key:"accountExpired", sortable:true, resizeable:true},
				{key:"accountLocked", /*editor:new YAHOO.widget.RadioCellEditor({radioOptions:["true","false"],disableBtns:true}),*/ sortable:true, resizeable:true},
				{key:"credentialsExpired", sortable:true, resizeable:true},
				{key:"shippingAddress", formatter: shippingAddressFormatter, sortable:true, resizeable:true},
				{key:"recipientName", formatter: recipientFormatter, sortable:true, resizeable:true},
				{key:"mobile", formatter: mobileFormatter, sortable:true, resizeable:true},
				{key:"billingAddress", formatter: billingAddressFormatter, sortable:true, resizeable:true},
				{key:"memberships", hidden:true, sortable:true, resizeable:true},
				{key:"unlockToken", hidden:true, sortable:true, resizeable:true},
				{key:"resetPswdToken", hidden:true, sortable:true, resizeable:true},
				{key:"createdWhen", hidden:true, sortable:true, resizeable:true},
				{key:"lastLogin", hidden:true, sortable:true, resizeable:true},
				{key:"edit", hidden:true, formatter: "button"}
	        ];
	
	        var myDataSource = new YAHOO.util.DataSource(userList);
	        myDataSource.responseType = YAHOO.util.DataSource.TYPE_JSARRAY;
	        myDataSource.responseSchema = {
	            fields: [
	            {key: "id"},{key: "name"},{key: "email"},{key: "password"},
	            //{key: "securityQuestion"},{key: "securityQuestionAnswer"},
	            {key: "enabled"},{key: "accountExpired"},
				{key: "accountLocked"},{key: "credentialsExpired"},{key: "shippingAddress"},{key: "billingAddress"},
				{key: "memberships"},{key: "unlockToken"},{key: "resetPswdToken"},
				{key: "createdWhen"},{key: "lastLogin"}]
	        };
	
	        var myDataTable = new YAHOO.widget.ScrollingDataTable("userListTable",
	                myColumnDefs, myDataSource, {width:"1000px"});
	        
	        myDataTable.subscribe("rowClickEvent",myDataTable.onEventSelectRow);
	        myDataTable.subscribe("cellDblclickEvent",myDataTable.onEventShowCellEditor);
	        myDataTable.subscribe("editorBlurEvent", myDataTable.onEventSaveCellEditor);
	        myDataTable.subscribe("rowMouseoverEvent", myDataTable.onEventHighlightRow);
        	myDataTable.subscribe("rowMouseoutEvent", myDataTable.onEventUnhighlightRow);
        	myDataTable.subscribe("buttonClickEvent", editItemClick);
	        
        	var onCellEdit = function(oArgs) {
            	
        		//var brand = {id:0, name:"Spirent", image:"1.jpg", webSite:"http://www.spirent.com"};
				var elCell = oArgs.editor.getTdEl();
	            var oOldData = oArgs.oldData;
	            var oNewData = oArgs.newData;
	            
				var oUser = oArgs.editor._oRecord._oData;
	            var user = {
	            	id: oUser.id, 
	            	name: oUser.name, 
	            	email: oUser.email, 
	            	password: oUser.password,
	            	//securityQuestion: oUser.securityQuestion,
	            	//securityQuestionAnswer: oUser.securityQuestionAnswer,
	            	enabled: oUser.enabled,
	            	accountExpired: oUser.accountExpired,
	            	accountLocked: oUser.accountLocked,
	            	credentialsExpired: oUser.credentialsExpired,
	            	shippingAddress: oUser.shippingAddress,
	            	billingAddress: oUser.billingAddress,
	            	memberships: oUser.memberships,
	            	unlockToken: oUser.unlockToken,
	            	resetPswdToken: oUser.resetPswdToken,
	            	createdWhen: oUser.createdWhen,
	            	lastLogin:oUser.lastLogin
	            	};
		
				var fnSaveItemBack = function(item)
	        	{
	        		alert("Update Done!");
	        	};
	        	
	            UserManager.saveUser(user, true, fnSaveItemBack);
	        };
        	myDataTable.subscribe("editorSaveEvent", onCellEdit);

		};
		
        YAHOO.util.Event.onContentReady("userListTable", function () {
            
        	initUserListTable();
        });
    }

}
    

