<#--
	Url:
		/customer/order-list.html
		
	Controller:
		com.beelun.shoppro.web.OrderController.handleOrderListRequest(HttpServletRequest, HttpServletResponse)
		
	Model:
		Type			Name		Description
		-----------------------------------------------------------------
		List<Order>		orderList	list of order for current customer
-->

<head>
  <title>${rc.getMessage("orderList.title")}</title>
</head>
<#if orderList?exists && orderList.size != 0>
<div class="panel wide">
  <div class="round graypanel">
    <span class="gheader">
      <img src="${rc.getContextPath()}/images/setting.png" class="headimg">
      ${rc.getMessage("orderList.title")} </span>
    <div class="round whitepanel">
      <div class="orderMain">
        <h3>
          ${rc.getMessage("orderList.title")}</h3>
        <div class="orderDiv">
          <table width="100%" class="orderTable" cellpadding="0" cellspacing="0">
            <thead>
              <tr class="wthead">
                <th>
                  ${rc.getMessage("order.id")}
                </th>
                <th>
                  ${rc.getMessage("order.orderDate")}
                </th>
                <th>
                  ${rc.getMessage("ui.totalValue")}
                </th>
                <th>
                  ${rc.getMessage("order.status")}
                </th>
              </tr>
            </thead>
            <tbody>
              <#list orderList as order> <#assign total=0> <#list order.orderItemSet as item>
              <#assign total=total + (item.itemCount * item.item.sellPrice)> </#list>
              <tr>
                <td>
                  <a href="${rc.getContextPath()}/customer/order.html?id=${order.id}">${order.serialNumber}</a>
                </td>
                <td>
                  ${order.orderDate}
                </td>
                <td>
                  ${total?string.currency}
                </td>
                <td>
                  ${order.status}
                </td>
              </tr>
              </#list>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</div>
<#else>
<div class="panel wide">
  <div class="round graypanel">
    <span class="gheader">
      <img src="${rc.getContextPath()}/images/setting.png" class="headimg">
      ${rc.getMessage("orderList.title")} </span>
    <p>
      There is no order yet. Start shopping now.</p>
  </div>
</div>
</#if>