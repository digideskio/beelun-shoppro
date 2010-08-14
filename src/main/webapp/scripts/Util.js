//
//  Copyright (c) 2008-2009 by Bilen, Inc.
//
// The information contained herein is confidential, proprietary to Bilen,
// Inc., and considered a trade secret as defined in section 499C of the
// penal code of the Shanghai. Use of this information by anyone
// other than authorized employees of Bilen, Inc. is granted only under a
// written non-disclosure agreement, expressly prescribing the scope and
// manner of such use.
//
if (!window['Beelun']) {
    window['Beelun'] = {};
}
if (!window['Beelun']['shoppro']) {
    window['Beelun']['shoppro'] = {};
}

Beelun.shoppro.Util = {
		
	isNumber: function(String) { 
        if(String.replace(/(^\s*)|(\s*$)/g, "")=="")
        {
           return false;
        }
        var Letters = "1234567890"; 
        var i; 
        var c; 
        for( i = 0; i < String.length; i ++ ) 
        { 
            c = String.charAt( i ); 
            if (Letters.indexOf( c ) ==-1) 
            { 
               return false; 
            } 
        } 
        return true; 
    },
    
    trimTxt: function(txt) {
    	return txt.replace(/(^\s*)|(\s*$)/g, "");
    },
    
    isForbid: function(temp_str) {
        temp_str=trimTxt(temp_str);
    	temp_str = temp_str.replace('*',"@");
    	temp_str = temp_str.replace('--',"@");
    	temp_str = temp_str.replace('/',"@");
    	temp_str = temp_str.replace('+',"@");
    	temp_str = temp_str.replace('\'',"@");
    	temp_str = temp_str.replace('\\',"@");
    	temp_str = temp_str.replace('$',"@");
    	temp_str = temp_str.replace('^',"@");
    	temp_str = temp_str.replace('.',"@");
    	//temp_str = temp_str.replace('(',"@");
    	//temp_str = temp_str.replace(')',"@");
    	//temp_str = temp_str.replace(',',"@");
    	temp_str = temp_str.replace(';',"@");
    	temp_str = temp_str.replace('<',"@");
    	temp_str = temp_str.replace('>',"@");
    	//temp_str = temp_str.replace('?',"@");
    	temp_str = temp_str.replace('"',"@");
    	temp_str = temp_str.replace('{',"@");
    	temp_str = temp_str.replace('}',"@");
    	//temp_str = temp_str.replace('[',"@");
    	//temp_str = temp_str.replace(']',"@");
    	var forbid_str=new String('@,%,~,&');
    	var forbid_array=new Array();
    	forbid_array=forbid_str.split(',');
    	for(i=0;i<forbid_array.length;i++)
    	{
    		if(temp_str.search(new RegExp(forbid_array[i])) != -1)
    		return false;
    	}
    	return true;
    },
    
    isEmpty: function(inputId) {
    	if(this.trimTxt(YAHOO.util.Dom.get(inputId).value)=='')
    	{
    		return true
		}
    	return false;
    },
    
    userFormValidator: function() {
    	
    	var frmvalidator  = new Validator("userForm");
    	frmvalidator.EnableOnPageErrorDisplaySingleBox();
    	frmvalidator.EnableMsgsTogether();
    	frmvalidator.EnableFocusOnError(true);
    	frmvalidator.addValidation("name","req","Please enter your Name");
    	frmvalidator.addValidation("name","maxlen=20",	"Max length for FirstName is 20");
    	frmvalidator.addValidation("name","alpha_s","Name can contain alphabetic chars only");
    	 
    	frmvalidator.addValidation("email","maxlen=50");
    	frmvalidator.addValidation("email","req");
    	frmvalidator.addValidation("email","email");

    	//frmvalidator.addValidation("securityQuestion","maxlen=30");
    	//frmvalidator.addValidation("securityQuestion","req","Please enter your security Question");

    	//frmvalidator.addValidation("securityQuestionAnswer","maxlen=30");
    	//frmvalidator.addValidation("securityQuestionAnswer","req","Please enter your security Question");

    	frmvalidator.setAddnlValidationFunction("Beelun.shoppro.Util.DoCustomValidation");
    },
    
    DoCustomValidation: function(){
		  var frm = document.forms["userForm"];
		  if(frm.password.value != frm.repassword.value)
		  {
		    sfm_show_error_msg('The Password and verified password don not match!',frm.password);
		    return false;
		  }
		  else
		  {
		    return true;
		  }
    	
    }
    
    
	
}