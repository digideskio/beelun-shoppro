<#--
	Introduction:
		This is the zhs_CN version of checkout page for inputting details of order
		
	Url:
		/customer/checkout.html
		
	Controller:
		com.beelun.shoppro.web.CustomerController.handleCheckoutRequest(HttpServletRequest, HttpServletResponse)
		
	Model:
		Type				Name			Description
		----------------------------------------------
		List<CartItem>		itemList		list of cart items in shopping cart for current customer
		Address				shippingAddress	(optional)the shipping address of current customer(if exist)
		List<PaymentTool>	paymentList		list of available payment tools
		List<ExpressCorp>	expressCorpList	list of available express corp
-->

<#import "/spring.ftl" as spring/> 
<#assign xhtmlCompliant = true in spring> 
<head>
  <title><@spring.message "orderForm.title"/></title>
</head>
<#if !itemList?exists> ${rc.getMessage("ui.noItemPro")} <#else>
<div class="orderMain">
  <h3>
    填写收货人信息</h3>
  <div id="receiver" style="line-height: 150%;">
    <div id="division" class="orderDiv">
      <table width="100%" cellpadding="0" cellspacing="0" class="orderTable">
        <tbody>
          <tr>
            <th style="background: #EFEFEF none repeat scroll 0 0;">
              确认收获地址：
            </th>
            <td>
              <#if shippingAddress?exists>
              <input id="recAddressId" type="hidden" value="${shippingAddress.id}">
              <label>
                <input type="radio" value="0" name="receiveInfo" class="recInfoRadio" checked="checked" />
                <span>${shippingAddress.address} (${rc.getMessage("ui.recipientName")}: ${shippingAddress.recipientName}
                  PhoneNumber: ${shippingAddress.phoneNumber} ZipCode:${shippingAddress.zip})</span>
              </label>
              <#else>
              <input id="recAddressId" type="hidden" value="-1">
              <label>
                <input type="radio" value="0" name="receiveInfo" class="recInfoRadio" />
                <span style="color: red">You don't have a address. Please input recipient Address
                  in below form.</span>
              </label>
              </#if> <a id="editRecInfo" href="#">edit </a>
              <br />
              <label>
                <#if shippingAddress?exists>
                <input type="radio" value="1" name="receiveInfo" class="recInfoRadio" />
                <#else>
                <input type="radio" value="1" name="receiveInfo" class="recInfoRadio" checked="checked" />
                </#if> other address
              </label>
              <br />
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    <#if shippingAddress?exists>
    <div id="recipientInfo" class="orderDiv hide">
      <#else>
      <div id="recipientInfo" class="orderDiv">
        </#if>
        <table cellpadding="0" cellspacing="0" width="100%" class="orderTable">
          <tbody>
            <tr>
              <th style="width: 190px;">
                <em style="color: red;">*</em> ${rc.getMessage("ui.address")}：
              </th>
              <td>
                <input id="recAddress" type="text" value="${shippingAddress.address}">
              </td>
            </tr>
            <tr>
              <th style="width: 190px;">
                <em style="color: red;">*</em> ${rc.getMessage("ui.zip")}：
              </th>
              <td>
                <input id="recZip" type="text" value="${shippingAddress.zip}">
              </td>
            </tr>
            <tr>
              <th style="width: 190px;">
                <em style="color: red;">*</em> ${rc.getMessage("ui.recipientName")}：
              </th>
              <td>
                <input id="recRecipientName" type="text" value="${shippingAddress.recipientName}">
              </td>
            </tr>
            <tr>
              <th style="width: 190px;">
                <em style="color: red;">*</em> ${rc.getMessage("ui.mobileNumber")}：
              </th>
              <td>
                <input id="recMobile" type="text" value="${shippingAddress.mobileNumber}">
              </td>
            </tr>
            <tr>
              <th style="width: 190px;">
                ${rc.getMessage("ui.phoneNumber")}：
              </th>
              <td>
                <input id="recPhone" type="text" value="${shippingAddress.phoneNumber}">
              </td>
            </tr>
            <tr>
              <td colspan="2">
                <button type="button" id="saveRecInfo">
                  ${rc.getMessage("ui.saveThisInfo")}</button>
                <span id="saveRecInfoMessage" style="color: red"></span>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <div class="orderDiv">
        <table width="100%" cellpadding="0" cellspacing="0" border="0" class="orderTable">
          <tbody>
            <tr>
              <th style="background: #EFEFEF none repeat scroll 0 0;">
                送货日期：
              </th>
              <td>
                日期
              </td>
            </tr>
            <tr>
              <th style="background: #EFEFEF none repeat scroll 0 0;">
                ${rc.getMessage("ui.orderMessage")}:
              </th>
              <td>
                <textarea vtype="textarea" rows="2" cols="40" type="textarea"></textarea>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <!-- end of receiver-->
    <h3>
      配送方式</h3>
    <div style="line-height: 150%">
      <div class="orderDiv">
        <table width="100%" cellpadding="0" cellspacing="0" border="0" class="orderTable">
          <tbody>
            <#list expressCorpList as express>
            <tr>
              <th style="background: #EFEFEF none repeat scroll 0 0;">
                <label>
                  <input type="radio" name="expressCorp" value="${express.id}" />${express.shortName}：</label>
              </th>
              <td>
                ${express.webSite}
              </td>
            </tr>
            </#list>
          </tbody>
        </table>
      </div>
    </div>
    <h3>
      支付方式</h3>
    <div style="line-height: 150%">
      <div class="orderDiv">
        <table width="100%" cellpadding="0" cellspacing="0" border="0" class="orderTable">
          <tbody>
            <#list paymentList as payment>
            <tr>
              <th style="background: #EFEFEF none repeat scroll 0 0;">
                <input type="radio" name="paymentTool" value="${payment.id}" />${payment.name}</label>
              </th>
              <td>
                ${payment.description}
              </td>
            </tr>
            </#list>
          </tbody>
        </table>
      </div>
    </div>
    <div style="background: #F5F4EC none repeat scroll 0% 0%; border: 1px solid #E5DDC7;">
      <h3>
        购买的商品</h3>
      <div class="orderDiv">
        <table width="100%" class="orderTable" cellpadding="0" cellspacing="0">
          <thead>
            <tr style="background-color: #0080FF;">
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
            <#assign total=0> <#list itemList as item> <#assign subtotal=item.count * item.item.sellPrice>
            <tr>
              <td>
                ${item.item.name}
              </td>
              <td>
                ${item.item.sellPrice}
              </td>
              <td>
                ${item.count}
              </td>
              <td>
                ${subtotal?string.currency}
              </td>
            </tr>
            <#assign total=total + (item.count * item.item.sellPrice)> </#list>
          </tbody>
        </table>
      </div>
    </div>
    <div style="background: #F5F4EC none repeat scroll 0 0; border: 1px solid #E5DDC7;">
      <table class="orderTable">
        <tbody>
          <tr>
            <th style="text-align: right">
              ${rc.getMessage("ui.totalValue")}:
            </th>
            <td style="background: #FBF7EE none repeat scroll 0 0; color: #FF6600; width: 160px;">
              ${total?string.currency}
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
  <div>
    <input type="submit" class="button" name="save" value="Save" />
    <input type="submit" class="button" name="cancel" value="Cancel" />
  </div>
</#if> 