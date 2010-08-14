<#--
	The common header
	
	Model:
		Type	Name			Description
		-------------------------------------
		User	currentUser		(optional)currently logged in user
		com.beelun.shoppro.service.ShoppingCart
				shoppingCart	shopping cart for current logged in user
-->
<div id="ms-header">
  <h1>
    <a href="${base}/" target="_parent" title="${glob.shopName}"><span></span>${glob.shopName}</a></h1>
  <ul>
    <li>
      <ul>
        <li title="${glob.phoneNumber}">${glob.phoneNumber}</li>
        <#if currentUser?exists>
        <li class="header_li"><a href="${base}/customer/order-list.html" title="View order history" style="font-weight:bold">${currentUser}</a></li>
        <li class="header_li"><a href="${base}/logout.html">${rc.getMessage("ui.logout")}</a></li>
        <#else>
        <li class="header_li"><a href="${base}/membership/login.html">${rc.getMessage("ui.login")}</a></li>
        <li class="header_li"><a href="${base}/membership/create-user.html">${rc.getMessage("ui.signUp")}</a></li>
        </#if>
        <li class="header_li">
          <div id="myCartButton">
          	<#if shoppingCart?exists>
          	  <#assign itemNumber=shoppingCart.itemNumber>
          	<#else>
          	  <#assign itemNumber=0>
          	</#if>
            <a id="cartLink" href="${base}/cart/itemList.html" title="Shopping Cart" target="_parent">
              Cart(<span id="itemNumberInCart">${itemNumber}</span>)</a></div>
          <!-- TODO: add item# here -->
        </li>
      </ul>
    </li>
    <li>
		<form action="${base}/google-cse-results.html" id="cse-search-box">
		  <div>
		    <input type="hidden" name="cx" value="009705515215130087514:a93r1ilig7u" />
		    <input type="hidden" name="cof" value="FORID:10" />
		    <input type="hidden" name="ie" value="UTF-8" />
		    <input type="text" name="q" size="31" />
		    <input type="submit" name="sa" value="Search" />
		  </div>
		</form>
		<script type="text/javascript" src="http://www.google.com/cse/brand?form=cse-search-box&lang=en"></script>
    </li>
  </ul>
</div>
