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


if (!window['Beelun']) {
    window['Beelun'] = {};
}
if (!window['Beelun']['shoppro']) {
    window['Beelun']['shoppro'] = {};
}
if (!window['Beelun']['shoppro']['admin']) {
    window['Beelun']['shoppro']['admin'] = {};
}
if (!window['Beelun']['shoppro']['admin']['model']) {
    window['Beelun']['shoppro']['admin']['model'] = {};
}


(function() {
	Beelun.shoppro.admin.Start = function() {

        Beelun.shoppro.admin.AdminApp = new Beelun.shoppro.admin.AdminApp();

    }

})();