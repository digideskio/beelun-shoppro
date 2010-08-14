<#--
	Shown in case of 500 error(Internal Server Error). 
	Sitemesh will not handle this, users need to define a full page. 
	
	Model:
		Type	Name	Description
		--------------------------------
		MyGlob	glob	global setting of the site. We will take glob.page500
-->

<#if glob?exists>
	${glob.page500}
<#else>
	<p>error 500: Internal server error.</p>
</#if>
