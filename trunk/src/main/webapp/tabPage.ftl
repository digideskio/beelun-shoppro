<#--
	Url: /tab/{tabId}tab-url.html
	
	Controller:
		com.beelun.shoppro.web.TabController.handleRequest(HttpServletRequest, HttpServletResponse)
	
	Model:
		Type			name			description
		-----------------------------------------
		Tab 			currentTab		the tab for this url
		List<Category>	categoryList	a list of category for this tab

-->
<#if currentTab?exists>
<head>
    <title>${currentTab.pageTitle}</title>
    <#if currentTab.keywords?exists && currentTab.keywords?length != 0>
    	<meta name="keywords" content="${currentTab.keywords}" />
    </#if>
    <#if currentTab.description?exists && currentTab.description?length != 0>
    	<meta name="description" content="${currentTab.description}" />
    </#if>
    ${currentTab.metaTag}
</head>
<body>
	${currentTab.content}
</body>
<#else>
	empty page.
</#if>

