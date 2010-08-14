<?xml version="1.0" encoding="UTF-8"?>
<#--
	Two urls would use this:
	
	(1)
	Url:
		/customer/create-update-address.html
		
	Controller:
		com.beelun.shoppro.web.AddressAjaxController.handleAjaxRequest(HttpServletRequest, HttpServletResponse)
		
	Model:
		Type	Name		Description
		---------------------------------------
		String	code		Resulting code of this request
		String	message		message of this request
		String	result		result of the request
		
	(2)
	Url:
		/cart/cartAjax.html
	
	Controller:
		com.beelun.shoppro.web.ShoppingCartController.handleAjaxRequest(HttpServletRequest, HttpServletResponse)
		
	Model:
		Type	Name		Description
		---------------------------------------
		String	code		Resulting code of this request
		String	message		message of this request
		String	result		result of the request
		
-->
<root>
    <Status>
		<Code>${code!200}</Code>
		<Message>${message!}</Message>
    </Status>
    <Result>${result!}</Result>	
</root>
