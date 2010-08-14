<#--
	Url:
		/customer/checkout-paypal-error.html
		
	Controller:
		com.beelun.shoppro.web.CustomerController.handleCheckoutErrorRequest(HttpServletRequest, HttpServletResponse)
		
	Model:
		N/A

-->

<title>Checkout with Paypal error</title>
<div class="round graypanel narrow">
  <span class="gheader">
    <img src="${rc.getContextPath()}/images/error.png" class="headimg">
    Error </span>
  <div class="round whitepanel">
    <span>
      <p>
        Error occured when you checkout with Paypal. Please retry later. If this problem
        persists, please contact our support. Thanks you.</p>
      <p>
        <a href="/customer/order-list.html">my orders</a></p>
    </span>
  </div>
</div>
