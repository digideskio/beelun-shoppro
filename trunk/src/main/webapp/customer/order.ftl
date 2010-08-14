<#--
	Url:
		/customer/order.html
		
	Controller:
		com.beelun.shoppro.web.OrderController.handleOrdertRequest(HttpServletRequest, HttpServletResponse)
		
	Model:
		Type	Name	Description
		-------------------------------------
		Order	order	the order to be shown
-->

<#import "/spring.ftl" as spring/> 
<#assign xhtmlCompliant = true in spring>
<head>
  <title>${rc.getMessage("order.title")}</title>
</head>
<div class="panel wide">
  <div class="round graypanel">
    <span class="gheader">
      <img src="${rc.getContextPath()}/images/coins.png" class="headimg">
      ${rc.getMessage("order.title")}(${order.serialNumber})</span>
    <div class="round whitepanel">
      <div class="orderMain">
        <#if order.status == "NOTPAID">
        <div id="receiver" style="line-height: 150%;">
          <div id="division" class="orderDiv">
            <table width="100%" cellpadding="0" cellspacing="0" border="0" class="orderTable">
              <tbody>
                <tr>
                  <td>
                    <form action='/customer/paypal-express-checkout.html' method='POST'>
                    <input type="hidden" name="orderId" value="${order.id}">
                    ${rc.getMessage("order.notComplete")}( ${rc.getMessage("order.clickToPayOrder")}):
                    <input type='image' name='submit' src='${rc.getContextPath()}/images/btn_xpressCheckout.gif'
                      border='0' align='top' alt='PayPal' />
                    </form>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
        <!-- end of receiver-->
        </#if>
        <div style="line-height: 150%">
          <div class="orderDiv">
            <table width="100%" cellpadding="0" cellspacing="0" border="0" class="orderTable">
              <tbody>
                <tr>
                  <th style="background: #EFEFEF none repeat scroll 0 0;">
                    ${rc.getMessage("order.id")}: ${order.serialNumber}
                  </th>
                  <td>
                    ${rc.getMessage("order.orderDate")}: ${order.orderDate}
                  </td>
                  <td>
                    ${rc.getMessage("order.status")}: ${order.status}
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
          <div style="background: #F5F4EC none repeat scroll 0% 0%; border: 1px solid #E5DDC7;">
            <h3>
              ${rc.getMessage("orderForm.orderItemSet")}</h3>
            <div class="orderDiv">
              <table width="100%" class="orderTable" cellpadding="0" cellspacing="0">
                <thead>
                  <tr style="font-weigh: bold; background-color: #E2E7E7; color: #777777; font-family: Verdana,Arial,Helvetica,'Sans Serif';
                    line-height: 30px;">
                    <th>
                      ${rc.getMessage("ui.itemName")}
                    </th>
                    <th>
                      ${rc.getMessage("ui.itemPrice")}
                    </th>
                    <th>
                      ${rc.getMessage("ui.itemCount")}
                    </th>
                    <th>
                      ${rc.getMessage("ui.totalValue")}
                    </th>
                  </tr>
                </thead>
                <tbody>
                  <#list order.orderItemSet as orderItem> <#assign subtotal=orderItem.itemCount *
                  orderItem.sellPrice>
                  <tr>
                    <td>
                      <a href="${rc.getContextPath()}/${orderItem.item.myUrl}">${orderItem.item.name}</a>
                    </td>
                    <td>
                      ${orderItem.sellPrice?string.currency}
                    </td>
                    <td>
                      ${orderItem.itemCount}
                    </td>
                    <td>
                      ${subtotal?string.currency}
                    </td>
                  </tr>
                  </#list>
                </tbody>
              </table>
            </div>
          </div>
          <h3>
            Shipping address</h3>
          <div id="recipientInfo" class="orderDiv">
            <table cellpadding="0" cellspacing="0" width="100%" class="orderTable">
              <tbody>
                <tr>
                  <th style="width: 190px;">
                    ${rc.getMessage("ui.recipientName")}：
                  </th>
                  <td>
                    ${order.shippingAddress.firstName} ${order.shippingAddress.lastName}
                  </td>
                </tr>
                <tr>
                  <th style="width: 190px;">
                    ${rc.getMessage("ui.address")}：
                  </th>
                  <td>
                    ${order.shippingAddress.address}
                  </td>
                </tr>
                <tr>
                  <th style="width: 190px;">
                    Address2：
                  </th>
                  <td>
                    ${order.shippingAddress.address2}
                  </td>
                </tr>
                <tr>
                  <th style="width: 190px;">
                    City：
                  </th>
                  <td>
                    ${order.shippingAddress.city}
                  </td>
                </tr>
                <tr>
                  <th style="width: 190px;">
                    State：
                  </th>
                  <td>
                    ${order.shippingAddress.state}
                  </td>
                </tr>
                <tr>
                  <th style="width: 190px;">
                    ${rc.getMessage("ui.zip")}：
                  </th>
                  <td>
                    ${order.shippingAddress.zip}
                  </td>
                </tr>
                <tr>
                  <th style="width: 190px;">
                    ${rc.getMessage("ui.phoneNumber")}：
                  </th>
                  <td>
                    ${order.shippingAddress.phoneNumber}
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
          <#if order.sameAddress> <span>(Billing and shipping address are the same.)</span>
          <#else>
          <h3>
            Billing address</h3>
          <div class="orderDiv">
            <table cellpadding="0" cellspacing="0" width="100%" class="orderTable">
              <tbody>
                <tr>
                  <th style="width: 190px;">
                    ${rc.getMessage("ui.recipientName")}：
                  </th>
                  <td>
                    ${order.billingAddress.firstName} ${order.billingAddress.lastName}
                  </td>
                </tr>
                <tr>
                  <th style="width: 190px;">
                    ${rc.getMessage("ui.address")}：
                  </th>
                  <td>
                    ${order.billingAddress.address}
                  </td>
                </tr>
                <tr>
                  <th style="width: 190px;">
                    Address2：
                  </th>
                  <td>
                    ${order.billingAddress.address2}
                  </td>
                </tr>
                <tr>
                  <th style="width: 190px;">
                    City：
                  </th>
                  <td>
                    ${order.billingAddress.city}
                  </td>
                </tr>
                <tr>
                  <th style="width: 190px;">
                    State：
                  </th>
                  <td>
                    ${order.billingAddress.state}
                  </td>
                </tr>
                <tr>
                  <th style="width: 190px;">
                    ${rc.getMessage("ui.zip")}：
                  </th>
                  <td>
                    ${order.billingAddress.zip}
                  </td>
                </tr>
                <tr>
                  <th style="width: 190px;">
                    ${rc.getMessage("ui.phoneNumber")}：
                  </th>
                  <td>
                    ${order.billingAddress.phoneNumber}
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
          </#if>
          <div style="background: #F5F4EC none repeat scroll 0 0; border: 1px solid #E5DDC7;">
            <table class="orderTable">
              <tbody>
                <tr>
                  <th style="text-align: left">
                    Total:
                  </th>
                  <td style="background: #FBF7EE none repeat scroll 0 0; color: #FF6600; width: 160px;">
                    ${order.amount?string.currency}
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>
