<#ftl strip_whitespace=true>

<#--
	For obsolete admin console. Safe to remove this later on.

-->

<?xml version="1.0" encoding="utf-8" ?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="${rc.locale.language}_${rc.locale.country}"
lang="${rc.locale.language}_${rc.locale.country}">
<#-- TBD: admin decorators page goes here -->
<head>
  <title><#if title != ''>${title}<#else>Admin</#if></title>
  <meta http-equiv="Cache-Control" content="no-store" />
  <meta http-equiv="Pragma" content="no-cache" />
  <meta http-equiv="Expires" content="0" />
  <meta http-equiv="content-type" content="text/html; charset=utf-8" />
  <link rel="shortcut icon" href="${base}/images/favicon.ico" type="image/x-icon" />
  <link rel="stylesheet" type="text/css" href="${base}/styles/deliciouslyblue/theme.css"
    title="default" />
  <link rel="alternate stylesheet" type="text/css" href="${base}/styles/deliciouslygreen/theme.css"
    title="green" />
  <link rel="stylesheet" type="text/css" href="${base}/styles/deliciouslyblue/shopproAdmin.css"
    title="default" />

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

  <script type="text/javascript" src="/dwr/engine.js"></script>

  <script type="text/javascript" src="/dwr/util.js"></script>

  <script type="text/javascript" src="${base}/scripts/AdminCore.js"></script>

  <script type="text/javascript" src="${base}/scripts/AdminApp.js"></script>
  <link rel="stylesheet" type="text/css" href="${base}/styles/shoppro.css" />
  <link rel="stylesheet" type="text/css" href="${base}/styles/shoppro-header-footer.css" />
  ${head}
</head>
<body class="yui-skin-sam" onload="Beelun.shoppro.admin.Start();" style="background-color: #BBBBBB;">
  <a name="top"></a>
  <div id="page">
    <div id="stage">
      <div id="ms-header-container">
        <#include "admin-header.ftl">
      </div>
      <div id="content">
        <div id="main" style="float: none; width: 100%; padding: 0;">
          ${body}
          <div id="underground">
            <#if page.getProperty("page.underground")?exists> ${page.getProperty("page.underground")}
            </#if>
          </div>
        </div>
        <div id="nav1">
          <div class="navbar">
            <h2 class="accessibility">
              Navigation</h2>
            <ul class="clearfix">
              <li><a href="${base}/admin/global.html" title="User Mananger"><span>Global Setting</span></a></li>
              |
              <li><a href="${base}/admin/import-items.html" title="Item Mananger"><span>Content</span></a></li>
              |
              <li><a href="${base}/admin/customer.html" title="User Mananger"><span>User</span></a></li>
              |
              <li><a href="${base}/admin/order.html" title="User Mananger"><span>Order</span></a></li>
            </ul>
          </div>
        </div>
        <!-- end nav -->
      </div>
      <!-- end content -->
      <div id="ms-footer-container">
        <#include "admin-footer.ftl">
      </div>
    </div>
  </div>
</body>
</html>
