<#if item?exists>
<head>
  <title>${item.pageTitle}</title>
  <#if item.keywords?exists && item.keywords?length!=0>
  	<meta name="keywords" content="${item.keywords}" />
  </#if>
  <#if item.description?exists && item.description?length!=0>
  	<meta name="description" content="${item.description}" />
  </#if> ${item.metaTag}
</head>
<body>
	<p>THIS IS ITEM TEMPLATE 1 </p>
  <#-- show item here -->
  <div>
    <p class="ulTitle">
      ${rc.getMessage("ui.currentLocation")}: <a href="${rc.getContextPath()}/index.html">
        <span>${rc.getMessage("ui.home")}</span></a> <#if currentCategory?exists> >> <a href="${currentCategory.myUrl}">
          <span>${currentCategory.name}</span></a> >> <span>${item.name}</span> </#if>
    </p>
  </div>
  <div class="itemDiv grayBackground" style="max-height:10000px;">
      <img class="producttitle" alt="" title="" src="${rc.getContextPath()}/images/logo.gif" />
    <div class="productDetailContainer">
      <div class="divider">
      </div>
      <p class="para">
        <b>${item.name}</b></p>
        <br/>
      <div class="middlecontainer" id="middlecontainer">
      	  <div style="border:1px dashed #CCCCCC;padding:2px">
      		Serial Number: ${(item.serialNumber)!"N/A"}<br /><br />
          Inventory Number: ${(item.inventoryNumber)!"N/A"}
          </div>
          <br /><br />
          <div style="border:1px dashed #CCCCCC;padding:2px">
          Description:<br />
        
        ${item.detailedDescription}
        
        </div>
      </div>
     <br/><br/>
      <div class="addToCartDiv">
        <div class="button2 rounded">
          <span id="${item.id}" class="addToCart_button">Add to Cart</span>
        </div>
        <div class="detailPrice">
          <ins class="displaynone"></ins><del class="normalprice">${item.sellPrice?string.currency}</del>
          <div class="clearfloat">
          </div>
        </div>
      </div>
    </div>
    <div class="itemBigPicContainer">
    <#if item.image?contains("http")>
      <a href="${item.image}"><img class="itemBigPic" src="${item.image}" alt="${item.name}" style="width: 333px;
        height: 280px" /></a>
        
    <#elseif item.image == "">
    <img class="itemBigPic" src="${rc.getContextPath()}/images/NoPhotoAvailable.jpg" alt="${item.name}" style="width: 333px; height: 280px"/>
    <#else>
        <a href="${rc.getContextPath()}/images/${item.image}"><img class="itemBigPic" src="${rc.getContextPath()}/images/${item.image}" alt="${item.name}" style="width: 333px;
        height: 280px"/></a>
    </#if>
    </div>
    <div class="clearfloat">
    </div>
  </div>
</body>
<#else> empty page. </#if> 