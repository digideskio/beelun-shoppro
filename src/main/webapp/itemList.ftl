<#--
	Url: 
		/cart/itemList.html
		
	Controller:
		com.beelun.shoppro.web.ShoppingCartController.handleListItem(HttpServletRequest, HttpServletResponse)
		
	Model:
		Type			Name			Description
		---------------------------------------------
		List<CartItem>	cartItemList	item list in current cart
-->

<head>
  <title>Shopping Cart</title>
</head>
<div class="round graypanel wide">
  <span class="gheader">
    <img src="${rc.getContextPath()}/images/carticon.png" class="headimg">
    My shopping cart </span>
  <div class="round whitepanel">
    <#if cartItemList?exists && cartItemList?size!=0>
    <table width="100%" border="1px" bordercolor="#CCCCCC" align="center" class="whitetable">
      <tr class="wthead">
        <td width="10%">
          ${rc.getMessage("ui.itemId")}
        </td>
        <td>
          ${rc.getMessage("ui.itemName")}
        </td>
        <td width="15%">
          ${rc.getMessage("ui.itemPrice")}
        </td>
        <td width="15%">
          ${rc.getMessage("ui.itemCount")}
        </td>
        <td width="10%">
          ${rc.getMessage("ui.deleteItem")}
        </td>
      </tr>
      <#assign totalValue = 0> <#list cartItemList as cartItem> <#assign totalValue=totalValue
      + (cartItem.count * cartItem.item.sellPrice)> </#list> <#list cartItemList as cartItem>
      <tr class="singletr">
        <td>
          ${cartItem.item.id}
        </td>
        <td>
          <a href="${cartItem.item.myUrl}">${cartItem.item.name}</a>
        </td>
        <td>
          ${cartItem.item.sellPrice?string.currency}
        </td>
        <td align="center">
          <!--<a><img src="${rc.getContextPath()}/images/${rc.getMessage("ui.minusImg")}"></a>-->
          <input type="text" class="itemCountInput" maxlength="4" style="width: 30px" value="${cartItem.count}" />
          <input type="hidden" value="${cartItem.count}" />
          <!--<a><img src="${rc.getContextPath()}/images/${rc.getMessage("ui.addImg")}"></a>-->
        </td>
        <td>
          <a href="#" class="deleteItem">Delete</a>
        </td>
      </tr>
      </#list>
      <tr class="doubletr">
        <td colspan="5" align="right">
          ${rc.getMessage("ui.totalValue")}: <span class="redfont" id="totalValue">${totalValue?string.currency}</span>
        </td>
      </tr>
    </table>
    <div class="buttondiv">
      <div class="floatright">
        <div class="button2 rounded">
          <#-- cn version <span onclick="document.location='${rc.getContextPath()}/customer/create-order.html'">
            Create order</span> --> <span onclick="document.location='${rc.getContextPath()}/customer/input-address.html'">
              Checkout Now</span>
        </div>
      </div>
    </div>
    <#else> ${rc.getMessage("ui.noItemPro")} </#if>
  </div>
</div>
