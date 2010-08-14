<#ftl strip_whitespace=true>

<#--
	For obsolete admin console. Safe to remove this later on.
-->

<?xml version="1.0" encoding="utf-8" ?>

    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="${rc.locale.language}_${rc.locale.country}" lang="${rc.locale.language}_${rc.locale.country}" >

<#-- TBD: admin decorators page goes here -->

<head>
    <title><#if title != ''>${title}<#else>Admin</#if></title>
    
    <meta http-equiv="Cache-Control" content="no-store"/>
    <meta http-equiv="Pragma" content="no-cache"/>
    <meta http-equiv="Expires" content="0"/>
    <meta http-equiv="content-type" content="text/html; charset=utf-8"/>
    <link rel="shortcut icon" href="${base}/images/favicon.ico" type="image/x-icon"/>
    
    <script type="text/javascript" src="${base}/scripts/prototype.js"></script>
    <script type="text/javascript" src="${base}/scripts/scriptaculous.js"></script>
    <script type="text/javascript" src="${base}/scripts/stylesheetswitcher.js"></script>
    <script type="text/javascript" src="${base}/scripts/global.js"></script>

    <!-- YUI CSS for Menu     -->
    <link rel="stylesheet" type="text/css" href="${base}/scripts/lib/yui/build/reset-fonts-grids/reset-fonts-grids.css">
 	<link rel="stylesheet" type="text/css" href="${base}/scripts/lib/yui/build/menu/assets/skins/sam/menu.css">
 	<!-- YUI Menu source file -->
    
    <link rel="stylesheet" type="text/css" href="${base}/scripts/lib/yui/build/fonts/fonts-min.css" />
	<link rel="stylesheet" type="text/css" href="${base}/scripts/lib/yui/build/container/assets/skins/sam/container.css" />

    <link rel="stylesheet" type="text/css" href="${base}/scripts/lib/yui/build/fonts/fonts-min.css" />
	<link rel="stylesheet" type="text/css" href="${base}/scripts/lib/yui/build/datatable/assets/skins/sam/datatable.css" />
	<link rel="stylesheet" type="text/css" href="${base}/scripts/lib/yui/build/paginator/assets/skins/sam/paginator.css" />

	<script type="text/javascript" src="${rc.getContextPath()}/scripts/lib/yui/build/yahoo-dom-event/yahoo-dom-event.js"></script>
	<script type="text/javascript" src="${rc.getContextPath()}/scripts/lib/yui/build/connection/connection-min.js"></script>
	<script type="text/javascript" src="${rc.getContextPath()}/scripts/lib/yui/build/element/element-min.js"></script>
	<script type="text/javascript" src="${rc.getContextPath()}/scripts/lib/yui/build/button/button-min.js"></script>
	<script type="text/javascript" src="${rc.getContextPath()}/scripts/lib/yui/build/dragdrop/dragdrop-min.js"></script>
	<script type="text/javascript" src="${rc.getContextPath()}/scripts/lib/yui/build/container/container-min.js"></script>
	
	
	<script type="text/javascript" src="${rc.getContextPath()}/scripts/lib/yui/build/dragdrop/dragdrop-min.js"></script>
	<script type="text/javascript" src="${rc.getContextPath()}/scripts/lib/yui/build/element/element-min.js"></script>
	<script type="text/javascript" src="${rc.getContextPath()}/scripts/lib/yui/build/paginator/paginator-min.js"></script>
	<script type="text/javascript" src="${rc.getContextPath()}/scripts/lib/yui/build/datasource/datasource-min.js"></script>
	<script type="text/javascript" src="${rc.getContextPath()}/scripts/lib/yui/build/datatable/datatable-min.js"></script>
	
	<link rel="stylesheet" type="text/css" href="${rc.getContextPath()}/scripts/lib/yui/build/fonts/fonts-min.css" />
	<link rel="stylesheet" type="text/css" href="${rc.getContextPath()}/scripts/lib/yui/build/button/assets/skins/sam/button.css" />
	<link rel="stylesheet" type="text/css" href="${rc.getContextPath()}/scripts/lib/yui/build/container/assets/skins/sam/container.css" />

	<link rel="stylesheet" type="text/css" href="${base}/styles/deliciouslyblue/theme-admin.css" title="default" />
    <link rel="stylesheet" type="text/css" href="${base}/styles/deliciouslyblue/shopproAdmin.css" title="default" />

    <script type="text/javascript" src="/dwr/engine.js"></script>
    <script type="text/javascript" src="/dwr/util.js"></script>    
     <#if siteType == "US">
    <script type="text/javascript" src="${base}/scripts/AdminCore.js"></script>
    <script type="text/javascript" src="${base}/scripts/AdminApp.js"></script>
    <#elseif siteType == "CN">
    </#if> 

    ${head}
</head>

<body class="yui-skin-sam"  onLoad="Beelun.shoppro.admin.Start();" style="margin:0px;background-color:#BBBBBB;">

<a name="top"></a>

<div id="page">
	<div id="header">
		<div id="header" class="clearfix">
			<table width="100%">
				<tr>
				    <td width="45%">
		    			<h1 class="shopName" onclick="location.href='${base}/'">${glob.shopName}</h1>
					    
					    <div id="branding">
					    </div>
				    </td>
				    <td width="55%" class="headerRight">
				    	<div>
							<div>
									<span>${rc.getMessage("ui.welcome")}, ${currentUser} !</span>&nbsp;	
									<span>[<a href="${base}/logout.html">${rc.getMessage("ui.logout")}</a>]</span>								
							</div>
				    			
				    	</div>
		    		</td>
				</tr>
			</table>
		</div>
	</div>

    <div id="content">
		
        <div id="main">

            ${body}

            <div id="underground">
                <#if page.getProperty("page.underground")?exists>
                ${page.getProperty("page.underground")}
                </#if>
            </div>
        </div>

        <div id="nav1">
            <div class="navbar">
                <h2 class="accessibility">Navigation</h2>
                <ul class="clearfix">
                	<li style="line-height:25px"><a style="line-height:25px" href="${base}/admin/global.html" title="User Mananger"><span>Global Setting</span></a></li> | 
                	<li><a href="${base}/admin/import-items.html" title="Item Mananger"><span>Content</span></a></li> | 
                	<li><a href="${base}/admin/customer.html" title="User Mananger"><span>User</span></a></li> | 
                	<li><a href="${base}/admin/order.html" title="User Mananger"><span>Order</span></a></li> |
                	<li><a href="${base}/admin/article.html" title="Item Mananger"><span>Article</span></a></li>
                </ul>
            </div>
        </div><!-- end nav -->

    </div><!-- end content -->

       
</div>
</body>
</html>
