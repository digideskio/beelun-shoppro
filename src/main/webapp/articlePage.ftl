<#--
	Url:
		/article/*
		
	Controller:
		com.beelun.shoppro.web.ArticleController.handleRequest(HttpServletRequest, HttpServletResponse)
		
	Model:
		Type		Name	Description
		--------------------------------------------
		Article		article	object for the article
-->

<#if article?exists>
<head>
  <title>${article.pageTitle}</title>
  <meta name="keywords" content="${article.keywords}" />
  <#if article.description?exists>
  <meta name="description" content="${article.description}" />
  </#if> ${article.metaTag}
</head>
<body>
<h1>${article.title}<h1>
${article.content}
</body>
<#else>
No article.
</#if>