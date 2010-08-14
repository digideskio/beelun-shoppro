<#ftl strip_whitespace=true>

<#--
	Introduction:
		The template with tab, category
-->

<?xml version="1.0" encoding="utf-8" ?>
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="${rc.locale.language}_${rc.locale.country}"
lang="${rc.locale.language}_${rc.locale.country}">
<head>
  <title><#if title != ''>${title}<#else>Welcome</#if></title>
  <meta http-equiv="Cache-Control" content="no-store" />
  <meta http-equiv="Pragma" content="no-cache" />
  <meta http-equiv="Expires" content="0" />
  <meta http-equiv="content-type" content="text/html; charset=utf-8" />
  <script type="text/javascript" src="${base}/scripts/core.js"></script>

  <script type="text/javascript" src="${base}/scripts/App.js"></script>

  <script type="text/javascript" src="${base}/scripts/Util.js"></script>

  <!-- YUI CSS for Menu     -->
  <link rel="stylesheet" type="text/css" href="${base}/scripts/lib/yui/build/reset-fonts-grids/reset-fonts-grids.css">
  <link rel="stylesheet" type="text/css" href="${base}/scripts/lib/yui/build/menu/assets/skins/sam/menu.css">
  <!-- YUI Menu source file -->

  <script type="text/javascript" src="${base}/scripts/lib/yui/build/yahoo-dom-event/yahoo-dom-event.js"></script>

  <script type="text/javascript" src="${base}/scripts/lib/yui/build/container/container_core.js"></script>

  <script type="text/javascript" src="${base}/scripts/lib/yui/build/yahoo/yahoo-min.js"></script>

  <script type="text/javascript" src="${base}/scripts/lib/yui/build/event/event-min.js"></script>

  <script type="text/javascript" src="${base}/scripts/lib/yui/build/connection/connection-min.js"></script>

  <link rel="stylesheet" type="text/css" href="${base}/scripts/lib/yui/build/fonts/fonts-min.css" />
  <link rel="stylesheet" type="text/css" href="${base}/scripts/lib/yui/build/button/assets/skins/sam/button.css" />
  <link rel="stylesheet" type="text/css" href="${base}/scripts/lib/yui/build/container/assets/skins/sam/container.css" />

  <script type="text/javascript" src="${base}/scripts/lib/yui/build/element/element-min.js"></script>

  <script type="text/javascript" src="${base}/scripts/lib/yui/build/button/button-min.js"></script>

  <script type="text/javascript" src="${base}/scripts/lib/yui/build/dragdrop/dragdrop-min.js"></script>

  <script type="text/javascript" src="${base}/scripts/lib/yui/build/container/container-min.js"></script>

  <script type="text/javascript" src="${base}/scripts/lib/yui/build/event-mouseenter/event-mouseenter-min.js"></script>

  <script type="text/javascript" src="${base}/scripts/gen_validatorv31.js"></script>
  <link rel="shortcut icon" href="${base}/images/favicon.ico" type="image/x-icon" />
  <link rel="stylesheet" type="text/css" href="${base}/styles/deliciouslyblue/theme.css"
    title="default" />
  <link rel="stylesheet" type="text/css" href="${base}/styles/shoppro.css" />
  <link rel="stylesheet" type="text/css" href="${base}/styles/shoppro-header-footer.css" />
  <link rel="stylesheet" type="text/css" href="${base}/styles/shoppro-tab-category.css" />
  <link rel="stylesheet" type="text/css" href="${base}/styles/home-page.css" />
  ${head}
</head>
<#if categoryList?exists && categoryList?size != 0>
<#assign hasLeftContent = true>
<#else>
<#assign hasLeftContent = false>
</#if>
<body class="yui-skin-sam" onload="Beelun.shoppro.Start();" style="background-color: #BBBBBB;">
  <div id="page" style="">
    <div id="stage">
      <div id="ms-header-container">
        <#include "header.ftl">
      </div>
      <div id="content">
        <#if hasLeftContent == true>
        <div id="leftcontent">
          <div id="leftnav">
            <div class="tborder">
        			<span title="${rc.getMessage("ui.navTitle")}" class="navheader rounded" id="navHeader">${rc.getMessage("ui.navTitle")}</span>
            </div>
            <div id="ct100_SubNavigation" class="navcontent">
              <ul>
                <#if categoryList?exists> <#list categoryList as category>
                <li><a href="${category.myUrl}"
                  title="${category.name}"><span>${category.name}</span></a> </li>
                </#list> </#if>
              </ul>
            </div>
          </div>
        </div>
        </#if>
       
        <#if hasLeftContent == true>
      	<div id="main">
      	<#else>
      	<!--
      	<div id="main" style="float: none; width: 100%">
      	-->
      	</#if>
          <#include "/messages.ftl"/> ${body}
          <div id="underground">
            <#if page.getProperty("page.underground")?exists> ${page.getProperty("page.underground")}
            </#if>
          </div>
         <!--
        </div>
        -->
        <div id="nav">
          <div id="tab_left">
          </div>
          <div class="wrapper">
            <ul class="clearfix">
              <#list glob.shownTabs as tab>
              <li><a href="${tab.myUrl}" title="${tab.pageTitle}"><span>${tab.name}</span></a></li>
              </#list>
              <li><a id="tab_right" href=""></a></li>
            </ul>
          </div>
        </div>
        <!-- end nav -->
      </div>
      <!-- end content -->
      <div id="ms-footer-container">
        <#include "footer.ftl">
      </div>
      <div id="addToCartDialog" class="yui-pe-content">
        <div class="hd"><img style="vertical-align:middle" src="${base}/images/${rc.getMessage("ui.doneImg")}"> ${rc.getMessage("ui.addedInCart")}</div>
        <div class="bd">
          <div>
            <span>Total <span id="dialogTotalArticle"></span> items</span> <span>Total price: 
              $<span id="dialogTotalPrice"></span></span>
          </div>
        </div>
      </div>
    </div>
  </div>
</body>
</html>
