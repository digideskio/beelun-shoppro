<#-- 
	Introduction:
		A notification page sent after user send the registration info
		
	Url:
		/membership/signup-done.*
		
	Controller:
		com.beelun.shoppro.web.MembershipController.handleSignupDoneRequest(HttpServletRequest, HttpServletResponse)
		
	Model:
		N/A
-->
<title>Signup info received</title>

<div class="round graypanel narrow" >
	<span class="gheader">
	<img src="${rc.getContextPath()}/images/ok.png" class="headimg"> Done
	</span>
	<div class="round whitepanel">
		Your info is received successfully. 

To complete the sign up process, please go to your registered email box, and click the link per instructions.
Back to <a href="/index.html">home</a>

	</div>
</div>