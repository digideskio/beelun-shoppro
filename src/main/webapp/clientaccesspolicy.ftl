<#--
	Url:
		/clientaccesspolicy.xml
		
	Controller:
		com.beelun.shoppro.web.AdminController.handleClientaccesspolicy(HttpServletRequest, HttpServletResponse)
		
	Model:
		N/A
-->

<?xml version="1.0" encoding="utf-8" ?>
<access-policy>
  <cross-domain-access>
    <policy>
      <allow-from http-request-headers="*" >
        <domain uri="*"/>
      </allow-from>
      <grant-to>
        <resource path="/" include-subpaths="true"/>
      </grant-to>
    </policy>
  </cross-domain-access>
</access-policy>