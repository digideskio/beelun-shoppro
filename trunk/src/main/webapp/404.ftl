<#--
	Shown in case of 404 error(the page is not found). 
	Sitemesh will not handle this, users need to define a full page. 
	
	Model:
		Type	Name	Description
		--------------------------------
		MyGlob	glob	global setting of the site. We will take glob.page404
-->

<#if glob?exists>
	${glob.page404}
<#else>
	<p>new error 404: page not found.</p>
</#if>
