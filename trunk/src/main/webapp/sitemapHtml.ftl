<#-- 	
	/sitemap.html:
	
	Controller:
		com.beelun.shoppro.web.SiteMapController.handleSiteMapHtmlRequest()
	
	Model:
		List<Tab> tabListAll
		List<Category> categoryListAll
		List<Item> itemListAll
		List<Article> articleListAll	
 	
 	Refer to:
 		http://www.smartechconsulting.com/site/files/sitemap-new.html
 		http://www.google.com/sitemap.html
 -->
 <title>Site Map</title>
 <body>
 <ul class="font16" style="margin-top:-50px">
 <#list tabListAll as tab>
 	<#if tab.isShown == true>
	<li><a href="${tab.myUrl}" alt="${tab.name}" title="${tab.pageTitle}"><img src="${rc.getContextPath()}/images/tab.gif"> ${tab.name}</a>
		<ul>
		<#list tab.categoryMap as tabMap>
			<#if tabMap.category.isShown == true>
			<li><img src="${rc.getContextPath()}/images/category.gif"><a href="${tabMap.category.myUrl}" alt="${tabMap.category.name}" title="${tabMap.category.pageTitle}"> ${tabMap.category.name}</a>
				<ul>
				<#list tabMap.category.itemMap as itemMap>
					<#if itemMap.item.isShown == true && itemMap.item.myUrl?length != 0 && itemMap.item.name?length != 0>
						<li><img src="${rc.getContextPath()}/images/item.gif"><a href="${itemMap.item.myUrl}" alt="${itemMap.item.name}" title="${itemMap.item.pageTitle}"> ${itemMap.item.name}</a></li>
					</#if>					
				</#list>
				</ul>
			</li>
			</#if>
		</#list>
		</ul>
	</li>
	</#if>
 </#list>
 <#list articleListAll as article>
	<li><a href="${article.myUrl}"><img src="${rc.getContextPath()}/images/tab.gif"> ${article.title}
	</li>
 </#list>
 </ul><#--End of Tab list--> 
 
 </body>