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


Beelun.shoppro.admin.model.item = {

    getItem: function(brandList) {
    	var item = {};
    	
    	var itemIdEle = YAHOO.util.Dom.get('itemId');
		if(itemIdEle.value)
		{
			item.id = itemIdEle.value;
		}

		var brandEle = YAHOO.util.Dom.get('brand');
		//item.brand = brandEle.value;
		var brandName = brandEle.options[brandEle.selectedIndex].text;
		var brandId =  YAHOO.util.Dom.get('brandId').value;
		var brandImage = YAHOO.util.Dom.get('brandImage').value;
		var brandWebsite = YAHOO.util.Dom.get('brandWebsite').value;
		if(brandName != "NULL")
		{
			item.brand = {id:brandId, name:brandName, image:brandImage, webSite:brandWebsite};
		}
		
		//item.brand = {id:0, name:"Spirent", image:"1.jpg", webSite:"http://www.spirent.com"};

		var netSuiteIdEle = YAHOO.util.Dom.get('netSuiteId');
		item.netSuiteId = netSuiteIdEle.value;
		
		var nameEle = YAHOO.util.Dom.get('name');
		item.name = nameEle.value;
		
		var serialNumberEle = YAHOO.util.Dom.get('serialNumber');
		item.serialNumber = serialNumberEle.value;
		
		var shortDescriptionEle = YAHOO.util.Dom.get('shortDescription');
		item.shortDescription = shortDescriptionEle.value;
		
		var detailedDescriptionEle = YAHOO.util.Dom.get('detailedDescription');
		item.detailedDescription = detailedDescriptionEle.value;
		
		var imageEle = YAHOO.util.Dom.get('image');
		item.image = imageEle.value;
		
		var thumbNailEle = YAHOO.util.Dom.get('thumbNail');
		item.thumbNail = thumbNailEle.value;
		
		var listPriceEle = YAHOO.util.Dom.get('listPrice');
		item.listPrice = listPriceEle.value;
		
		var sellPriceEle = YAHOO.util.Dom.get('sellPrice');
		item.sellPrice = sellPriceEle.value;
		
		var inventoryNumberEle = YAHOO.util.Dom.get('inventoryNumber');
		item.inventoryNumber = inventoryNumberEle.value;
		
		//var isShownEle = YAHOO.util.Dom.get('isShown');
		//item.isShown = isShownEle.value;
		
		var isShownEle = document.getElementsByName("isShown");
		if (isShownEle[0].checked)
		{
			item.isShown = isShownEle[0].value;
		}
		else
		{
			item.isShown = isShownEle[1].value;
		}
		
		var pageTitleEle = YAHOO.util.Dom.get('pageTitle');
		item.pageTitle = pageTitleEle.value;
		
		var keywordsEle = YAHOO.util.Dom.get('keywords');
		item.keywords = keywordsEle.value;
		
		var descriptionEle = YAHOO.util.Dom.get('description');
		item.description = descriptionEle.value;
		
		var metaTagEle = YAHOO.util.Dom.get('metaTag');
		item.metaTag = metaTagEle.value;
		
		var urlEle = YAHOO.util.Dom.get('url');
		item.url = urlEle.value;
		
		return item;
                
    },
    
    setItem: function(item) {
    	
    	var itemId = item.id;
    	var brand;
    	var brandId;
		var brandImage;
		var brandWebsite;
    	if(item.brand != null)
    	{
    		brand = item.brand.name;
    		brandId = item.brand.id;
    		brandImage = item.brand.image;
    		brandWebsite = item.brand.webSite;
    	}
    	else
    	{
    		brand = "NULL";
    		brandId = "NULL";
    		brandImage = "NULL";
    		brandWebsite = "NULL";
    	}
		
		
		var netSuiteId = item.netSuiteId;
		var name = item.name;
		var serialNumber = item.serialNumber;
		var shortDescription = item.shortDescription;
		var detailedDescription = item.detailedDescription;
		var image = item.image;
		var thumbNail = item.thumbNail;
		var listPrice = item.listPrice;
		var sellPrice = item.sellPrice;
		var inventoryNumber = item.inventoryNumber;
		var isShown = item.isShown;
		var pageTitle = item.pageTitle;
		var keywords = item.keywords;
		var description = item.description;
		var metaTag = item.metaTag;
		var url = item.url;
		
		var itemIdEle = YAHOO.util.Dom.get('itemId');
		itemIdEle.value = itemId;
		
		var brandEle = YAHOO.util.Dom.get('brand');
		var op = document.createElement("option");
	    op.appendChild(document.createTextNode(brand));
	    brandEle.appendChild(op);
	    var brandIdEle = YAHOO.util.Dom.get('brandId');
	    brandIdEle.value = brandId;
	    var brandImageEle = YAHOO.util.Dom.get('brandImage');
	    brandImageEle.value = brandImage;
	    var brandWebsiteEle = YAHOO.util.Dom.get('brandWebsite');
	    brandWebsiteEle.value = brandWebsite;
	    
	    
	    var callback = function(brandList) {
	    	var brandEle = YAHOO.util.Dom.get('brand');
	    	for(var i=0; i<brandEle.options.length; i)
	    	{
	    		brandEle.remove(i);
    		}
	    	
	    	for(var i=0; i<brandList.length; i++)
	    	{
	    		var op = document.createElement("option");
	    		op.text = brandList[i].name;
	    	    if(op.text == brand)
	    	    {
	    	    	op.selected = "selected";
	    	    	var brandIdEle = YAHOO.util.Dom.get('brandId');
    			    brandIdEle.value = brandList[i].id;
    			    var brandImageEle = YAHOO.util.Dom.get('brandImage');
    			    brandImageEle.value = brandList[i].image;
    			    var brandWebsiteEle = YAHOO.util.Dom.get('brandWebsite');
    			    brandWebsiteEle.value = brandList[i].webSite;
	    	    }
	    	    try
	    	    {
	    	    	brandEle.add(op,null); // standards compliant
	    	    }
	    	    catch(ex)
	    	    {
	    	    	brandEle.add(op); // IE only
	    	    }
	    	}
	    	
	    	var op = document.createElement("option");
    		op.text = "NULL";
	    	if(brand == "NULL")
	    	{
	    		op.selected = "selected";
	    		var brandIdEle = YAHOO.util.Dom.get('brandId');
			    brandIdEle.value = "NULL";
			    var brandImageEle = YAHOO.util.Dom.get('brandImage');
			    brandImageEle.value = "NULL";
			    var brandWebsiteEle = YAHOO.util.Dom.get('brandWebsite');
			    brandWebsiteEle.value = "NULL";
	    	}
	    	try
    	    {
    	    	brandEle.add(op,null); // standards compliant
    	    }
    	    catch(ex)
    	    {
    	    	brandEle.add(op); // IE only
    	    }
		   
	    };
	    
		BrandManager.getAll(callback);

		//brandEle.value = brand;
		var netSuiteIdEle = YAHOO.util.Dom.get('netSuiteId');
		netSuiteIdEle.value = netSuiteId;
		netSuiteIdEle.disabled = true;
		var nameEle = YAHOO.util.Dom.get('name');
		nameEle.value = name;
		var serialNumberEle = YAHOO.util.Dom.get('serialNumber');
		serialNumberEle.value = serialNumber;
		var shortDescriptionEle = YAHOO.util.Dom.get('shortDescription');
		shortDescriptionEle.value = shortDescription;
		var detailedDescriptionEle = YAHOO.util.Dom.get('detailedDescription');
		detailedDescriptionEle.value = detailedDescription;
		var imageEle = YAHOO.util.Dom.get('image');
		imageEle.value = image;
		var thumbNailEle = YAHOO.util.Dom.get('thumbNail');
		thumbNailEle.value = thumbNail;
		var listPriceEle = YAHOO.util.Dom.get('listPrice');
		listPriceEle.value = listPrice;
		var sellPriceEle = YAHOO.util.Dom.get('sellPrice');
		sellPriceEle.value = sellPrice;
		var inventoryNumberEle = YAHOO.util.Dom.get('inventoryNumber');
		inventoryNumberEle.value = inventoryNumber;
		
		var isShownEle = document.getElementsByName("isShown");
		if (isShown == true || isShown == "true")
		{
			isShownEle[0].checked = "true";
		}
		else
		{
			isShownEle[1].checked = "true";
		}
		
		
		//var isShownEle = YAHOO.util.Dom.get('isShown');
		//isShownEle.value = isShown;
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
    
    clearItem: function() {
		
		var itemIdEle = YAHOO.util.Dom.get('itemId');
		itemIdEle.value = "";
		
	    var brandIdEle = YAHOO.util.Dom.get('brandId');
	    brandIdEle.value = "";
	    var brandImageEle = YAHOO.util.Dom.get('brandImage');
	    brandImageEle.value = "";
	    var brandWebsiteEle = YAHOO.util.Dom.get('brandWebsite');
	    brandWebsiteEle.value = "";
	    
	    
	    var callback = function(brandList) {
	    	var brandEle = YAHOO.util.Dom.get('brand');
	    	for(var i=0; i<brandEle.options.length; i)
	    	{
	    		brandEle.remove(i);
    		}
	    	for(var i=0; i<brandList.length; i++)
	    	{
	    		var op = document.createElement("option");
	    		op.text = brandList[i].name;
	    	    if(i == 0)
	    	    {
	    	    	op.selected = "selected";
	    	    	var brandIdEle = YAHOO.util.Dom.get('brandId');
    			    brandIdEle.value = brandList[i].id;
    			    var brandImageEle = YAHOO.util.Dom.get('brandImage');
    			    brandImageEle.value = brandList[i].image;
    			    var brandWebsiteEle = YAHOO.util.Dom.get('brandWebsite');
    			    brandWebsiteEle.value = brandList[i].webSite;
	    	    }
	    	    try
	    	    {
	    	    	brandEle.add(op,null); // standards compliant
	    	    }
	    	    catch(ex)
	    	    {
	    	    	brandEle.add(op); // IE only
	    	    }
	    	}
		   
	    };
	    
		BrandManager.getAll(callback);

		//brandEle.value = brand;
		var netSuiteIdEle = YAHOO.util.Dom.get('netSuiteId');
		netSuiteIdEle.value = "";
		netSuiteIdEle.disabled = false;
		
		var nameEle = YAHOO.util.Dom.get('name');
		nameEle.value = "";
		var serialNumberEle = YAHOO.util.Dom.get('serialNumber');
		serialNumberEle.value = "";
		var shortDescriptionEle = YAHOO.util.Dom.get('shortDescription');
		shortDescriptionEle.value = "";
		var detailedDescriptionEle = YAHOO.util.Dom.get('detailedDescription');
		detailedDescriptionEle.value = "";
		var imageEle = YAHOO.util.Dom.get('image');
		imageEle.value = "";
		var thumbNailEle = YAHOO.util.Dom.get('thumbNail');
		thumbNailEle.value = "";
		var listPriceEle = YAHOO.util.Dom.get('listPrice');
		listPriceEle.value = "";
		var sellPriceEle = YAHOO.util.Dom.get('sellPrice');
		sellPriceEle.value = "";
		var inventoryNumberEle = YAHOO.util.Dom.get('inventoryNumber');
		inventoryNumberEle.value = "";
		var isShownEle = document.getElementsByName("isShown");
		isShownEle[0].checked = "true";
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
    }

}
    

