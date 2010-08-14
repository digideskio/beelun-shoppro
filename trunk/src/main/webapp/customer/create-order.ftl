<#--
	Introduction:
		For zhs_cn page. This is for creating order. Might be broken...
-->

<#import "/spring.ftl" as spring/> 
<#assign xhtmlCompliant = true in spring> 
<#--checkout page.-->
<head>
  <title><@spring.message "orderForm.title"/></title>
  <link rel="stylesheet" type="text/css" href="${rc.getContextPath()}/scripts/lib/yui/build/calendar/assets/skins/sam/calendar.css" />
  <script type="text/javascript" src="${rc.getContextPath()}/scripts/lib/yui/build/calendar/calendar-min.js"></script>
</head>
<@spring.bind "order.*"/> <#if spring.status.error>
<div class="error">
  <#list spring.status.errorMessages as error> ${error}<br />
  </#list>
</div>
</#if> <#if !itemList?exists || itemList?size==0> ${rc.getMessage("ui.noItemPro")}
<#else>
<div class="round graypanel wide">
  <span class="gheader">
    <img src="${rc.getContextPath()}/images/order.png" class="headimg">
    <@spring.message "orderForm.title"/> </span>
  <div class="round whitepanel">
    <form method="post" action="<@spring.url '/customer/create-order.html'/>" name="orderForm"
    id="orderForm">
    <@spring.formHiddenInput "order.id"/>
    <div class="orderMain">
      <h3>
        ${rc.getMessage("orderForm.inputRecipientAddress")}</h3>
      <div id="receiver" style="line-height: 150%;">
        <div id="division" class="orderDiv">
          <table width="100%" cellpadding="0" cellspacing="0" class="orderTable">
            <tbody>
              <tr>
                <th style="background: #EFEFEF none repeat scroll 0 0;">
                  ${rc.getMessage("orderForm.confirmRecipientAddress")}:
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
                    <input id="recAddress" type="text" value="<#if shippingAddress?exists>${shippingAddress.address}<#else></#if>">
                  </td>
                </tr>
                <tr>
                  <th style="width: 190px;">
                    <em style="color: red;">*</em> ${rc.getMessage("ui.zip")}：
                  </th>
                  <td>
                    <input id="recZip" type="text" value="<#if shippingAddress?exists>${shippingAddress.zip}<#else></#if>">
                  </td>
                </tr>
                <tr>
                  <th style="width: 190px;">
                    <em style="color: red;">*</em> ${rc.getMessage("ui.recipientName")}：
                  </th>
                  <td>
                    <input id="recRecipientName" type="text" value="<#if shippingAddress?exists>${shippingAddress.recipientName}<#else></#if>">
                  </td>
                </tr>
                <tr>
                  <th style="width: 190px;">
                    <em style="color: red;">*</em> ${rc.getMessage("ui.mobileNumber")}：
                  </th>
                  <td>
                    <input id="recMobile" type="text" value="<#if shippingAddress?exists>${shippingAddress.mobileNumber}<#else></#if>">
                  </td>
                </tr>
                <tr>
                  <th style="width: 190px;">
                    ${rc.getMessage("ui.phoneNumber")}：
                  </th>
                  <td>
                    <input id="recPhone" type="text" value="<#if shippingAddress?exists>${shippingAddress.phoneNumber}<#else></#if>">
                  </td>
                </tr>
                <tr>
                  <td colspan="2">
                    <button type="button" class="button" id="saveRecInfo">
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
                    <@spring.message "orderForm.shipDate"/>:
                  </th>
                  <td>
                    <select name="shipDate" id="shipDate" size="1">
                      <option value="ANY_DATE"><@spring.message 'select.shipDate.0'/></option>
                      <option value="BUSINESS_DAY"><@spring.message 'select.shipDate.1'/></option>
                      <option value="NON_BUSINESS_DAY"><@spring.message 'select.shipDate.2'/></option>
                      <option value="SPECIFIED_DATE"><@spring.message 'select.shipDate.3'/></option>
                    </select>
                    <input class="hide" type="text" id="specifiedDate" name="specifiedDate">
                    <div id="shipDateCalContainer" style="float: right; position: absolute; margin-left: 120px;
                      margin-top: -20px">
                    </div>
                  </td>
                </tr>
                <tr>
                  <th style="background: #EFEFEF none repeat scroll 0 0;">
                    <@spring.message "orderForm.shipTime"/>:
                  </th>
                  <td>
                    <select name="shipTime" size="1">
                      <option value="ANY_TIME"><@spring.message 'select.shipTime.0'/></option>
                      <option value="MORNING"><@spring.message 'select.shipTime.1'/></option>
                      <option value="AFTERNOON"><@spring.message 'select.shipTime.2'/></option>
                      <option value="EVENING"><@spring.message 'select.shipTime.3'/></option>
                    </select>
                  </td>
                </tr>
                <tr>
                  <th style="background: #EFEFEF none repeat scroll 0 0;">
                    <@spring.message "orderForm.notes"/>:
                  </th>
                  <td>
                    <@spring.formTextarea "order.notes", 'id="notes"'/> <@spring.showErrors "<br>
                    ", "fieldError"/>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
        <!-- end of receiver-->
        <h3>
          ${rc.getMessage("orderForm.expressCorp")}</h3>
        <div style="line-height: 150%">
          <div class="orderDiv">
            <table width="100%" cellpadding="0" cellspacing="0" border="0" class="orderTable">
              <tbody>
                <#assign expressCorpChecked=1> <#list expressCorpList as express>
                <tr>
                  <th style="background: #EFEFEF none repeat scroll 0 0;">
										<label style=""><input type="radio" name="expressCorp" value="${express.id}" <#if expressCorpChecked == 1>checked="checked"</#if> > ${express.shortName}：</label>
                  </th>
                  <td>
                    ${express.webSite}
                  </td>
                </tr>
                <#assign expressCorpChecked=0> </#list>
              </tbody>
            </table>
          </div>
        </div>
        <h3>
          ${rc.getMessage("orderForm.paymentTool")}</h3>
        <div style="line-height: 150%">
          <div class="orderDiv">
            <table width="100%" cellpadding="0" cellspacing="0" border="0" class="orderTable">
              <tbody>
                <#assign paymentChecked=1> <#list paymentList as payment>
                <tr>
                  <th style="background: #EFEFEF none repeat scroll 0 0;">
										<label><input type="radio" name="paymentTool" value="${payment.id}" <#if paymentChecked == 1>checked="checked"</#if>/> ${payment.name}</label>
                  </th>
                  <td>
                    ${payment.description}
                  </td>
                </tr>
                <#assign paymentChecked=0> </#list>
              </tbody>
            </table>
          </div>
        </div>
        <div style="background: #F5F4EC none repeat scroll 0% 0%; border: 1px solid #E5DDC7;">
          <h3>
            ${rc.getMessage("orderForm.orderItemSet")}</h3>
          <div class="orderDiv">
            <table width="100%" class="orderTable" cellpadding="0" cellspacing="0">
              <thead>
                <tr style="font-weigh:bold;background-color:#E2E7E7;color:#777777;font-family:Verdana,Arial,Helvetica,"Sans Serif"; line-height:30px;">
								  <th>${rc.getMessage("ui.itemName")}</th>
									<th>${rc.getMessage("ui.itemPrice")}</th>
									<th>${rc.getMessage("ui.itemCount")}</th>
									<th>${rc.getMessage("ui.totalValue")}</th>
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
        <br />
        </br>
        <div>
          <table width="100%">
            <tr>
              <td align="right">
                <div style="float: right; margin-right: 20px; margin-bottom: 10px">
                  <div class="button2 rounded">
                    <span onclick="document.orderForm.submit()">Create Order</span>
                  </div>
                </div>
              </td>
            </tr>
          </table>
        </div>
      </div>
    </form>
  </div>
</div>
</#if>