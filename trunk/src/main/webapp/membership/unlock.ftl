<#--
	Introduction:
		Unlock a locked user(Often this is happending when user just filled signup form)
		
	Url:
		/membership/unlock.*
		
	Controller:
		com.beelun.shoppro.web.MembershipController.handleUnlockRequest(HttpServletRequest, HttpServletResponse)
		
	Model:
		Type	Name			Description
		------------------------------------
		boolean	isSuccessful	To indicate whether this unlock is successful or not.
-->


<title>Unlock your account</title>
<#if isSuccessful == true>
<div class="round graypanel narrow" >
	<span class="gheader">
	<img src="${rc.getContextPath()}/images/ok.png" class="headimg"> Done
	</span>
	<div class="round whitepanel">
	<p>Your account is unlocked. You can go ahead to <a href="/membership/login.html">log in</a> now.</p>
	</div>
</div>
<#else>
<div class="round graypanel narrow" >
	<span class="gheader">
	<img src="${rc.getContextPath()}/images/error.png" class="headimg"> error
	</span>
	<div class="round whitepanel">
		Make sure you have clicked the correct link from your mail box.
		<br/> <a href="/index.html">Home</a>
	</div>
</div>
</#if>