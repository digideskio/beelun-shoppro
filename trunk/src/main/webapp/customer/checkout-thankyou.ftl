<#--
	Introduction:
		Step 3/3 of checkout with Paypal 
		 
	Url:
		/customer/checkout-thankyou.html
		
	Controller:
		com.beelun.shoppro.web.CustomerController.handleCheckoutThankyouRequest(HttpServletRequest, HttpServletResponse)
		
	Model:
		Type	Name						Description
		--------------------------------------------------------------------
		String	RequestParameters.orderId	id of the processed order

--> 

<#if RequestParameters.orderId?exists> 
<div id="checkout_step3" class="checkout_steps"></div>
<div class="round graypanel narrow">
  <span class="gheader">
    <img src="${rc.getContextPath()}/images/ok.png" class="headimg">
    Thank you </span>
  <div class="round whitepanel">
    <p>
      Thank you! You've compeleted check out process with <a href="/customer/order.html?id=${RequestParameters.orderId}">
        your order</a>.</p>
  </div>
</div>
<#else>
<div class="round graypanel narrow">
  <span class="gheader">
    <img src="${rc.getContextPath()}/images/ok.png" class="headimg">
    Error </span>
  <div class="round whitepanel">
    <p>
      hmmm...You should not visit this url directly.</p>
  </div>
</div>
</#if> 