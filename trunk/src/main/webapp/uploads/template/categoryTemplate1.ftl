<#if currentCategory?exists>
<head>
    <title>${currentCategory.pageTitle}</title>
    <#if currentCategory.keywords?exists && currentCategory.keywords?length != 0>
    	<meta name="keywords" content="${currentCategory.keywords}" />
    </#if>    
    <#if currentCategory.description?exists && currentCategory.description?length != 0>
    	<meta name="description" content="${currentCategory.description}" />
    </#if>     
    ${currentCategory.metaTag!""}    
</head>
<body>
	<p>THIS IS CATETOGRY TEMPLATE 1 </p>
<#--
	show itemList here in some way
-->
	<div>
		<p class="ulTitle">
			${rc.getMessage("ui.currentLocation")}: 
			<a href="${rc.getContextPath()}/index.html"><span>${rc.getMessage("ui.home")}</span></a> >>
			<a href="${currentCategory.myUrl}"><span>${currentCategory.name}</span></a>
		</p>
	</div>
	
	<!-- Title -->
	<div class="results-heading" style="">
	<h3 class="transparent">${currentCategory.name}</h3>
	</div>
	<!-- End of Title -->
	
	<!-- List -->
	<ul class="product_list">
	
		<#if !itemList?exists || itemList?size==0>
		<!--TBD-->
		<#else>
			<#list itemList as item>
			<!-- Item -->
			<!--<li class="product-first" onclick="document.location='http://store.microsoft.com/microsoft/Office-Language-Pack-2007-Language-Spanish/product/2D3A57CF'">-->
			<li class="product_item"  style="width:99%;height:100px">
				
				<#if item.image?contains("http")>
				<img class="product" style="margin:0;width:102px;height:73px;float:left;margin:10px;" onclick="document.location='${item.myUrl}'" src="${item.image}" title="${item.name}" alt=""/>
				<#elseif item.image == "">
				<img class="product" style="margin:0;width:102px;height:73px;float:left;margin:10px;" onclick="document.location='${item.myUrl}'" src="${rc.getContextPath()}/images/NoPhotoAvailable.jpg" title="${item.name}" alt=""/>
				<#else>
				<img class="product" style="margin:0;width:102px;height:73px;float:left;margin:10px;" onclick="document.location='${item.myUrl}'" src="${rc.getContextPath()}/images/${item.image}" title="${item.name}" alt=""/>
				</#if>
				<h4 style="float:none; margin-left:150px;"> <a href="${item.myUrl}">${item.name}</a> </h4>
				<ul style="float:none; margin-left:150px;">
					<li>${(item.shortDescription)!"No description."}</li>
				</ul>
				<div class="price" style="top:30px;left:460px">
					<span>${item.sellPrice?string.currency}</span>
				</div>
				<div class="button3 rounded" style="width:95px;">
					<span id="${item.id}" class="addToCart_button">Add to Cart</span>
					<!--<a href="#"><span id="${item.id}" class="addToCart">Buy</span></a>-->
				</div>
			</li>
			<!--End of one item-->
    		</#list>
		</#if>
	</ul>
	<!-- End of List -->

</body>
<#else>
	empty page.
</#if>

