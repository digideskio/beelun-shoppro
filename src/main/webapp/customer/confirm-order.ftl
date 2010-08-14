<#-- 
	Introduction:
		Step 2/3 of Paypal checkout process
		
	Url:
		/customer/confirm-order.html
		
	Controller:
		 com.beelun.shoppro.web.CustomerController.handleConfirmOrderRequest(HttpServletRequest, HttpServletResponse)
		 
	Model:
		Type	Name	Description
		----------------------------------
		Order	order	the order being processed

--> 
<title>Confirm order</title>
<#if order?exists && RequestParameters.token?exists>
<div id="checkout_step2" class="checkout_steps">
</div>
<div class="round graypanel narrow">
  <span class="gheader">
    <img src="${rc.getContextPath()}/images/ok.png" class="headimg">
    Confirm order </span>
  <div class="round whitepanel">
    <#assign token=RequestParameters.token > <#if order.status == "NOTPAID">
    <p>
      Please confirm your order below:</p>
    Order: <a href="/customer/order.html?id=${order.id}">my order</a>
    <p>
      Amount: ${order.paymentAmount?string.currency}</p>
    <#if order.paymentAmount ==0 >
    <p>
      You don't need pay for this order.</p>
    <#else>
    <br />
    <form method="post" action="/customer/paypal-checkout-get-details.html">
    <input type="hidden" name="TOKEN" value="${token}">
    <input type="hidden" name="orderId" value="${order.id}">
    <input type="hidden" name="AMT" value="${order.amount}">
    <button class="button" name="submit" type="submit">
      ${rc.getMessage("ui.checkout.payNow")}</button>
    </form>
    </#if> <#else>
    <p>
      No need to pay for <a href="/customer/order.html?id=${order.id}">this order</a>.</p>
    </#if>
  </div>
</div>
<#else>
<div class="round graypanel narrow">
  <span class="gheader">
    <img src="${rc.getContextPath()}/images/error.png" class="headimg">
    Thank you </span>
  <div class="round whitepanel">
    <p>
      This order doesn't exits.</p>
  </div>
</div>
</#if>