<#import "/spring.ftl" as spring/> <#assign xhtmlCompliant = true in spring> 

<#--
	Url:
		/membership/forget-password.*

	Controller:
		com.beelun.shoppro.web.MembershipController.handleForgetPasswordRequest(HttpServletRequest, HttpServletResponse)
		
	Model data:
		string "status": "resetPasswordMailIsOut" or "noSuchEmail" 
 -->
 
<head>
  <title><@spring.message "forgetpassword.title"/></title>
</head>

<#assign status = status!"none">

<#if status == "resetPasswordMailIsOut">
<div class="round graypanel narrow">
  <span class="gheader">
    <img src="${rc.getContextPath()}/images/ok.png" class="headimg">
    Done </span>
  <div class="round whitepanel">
    Please follow instructions in your email to reset your password.
  </div>
</div>
<#else>
<div class="round graypanel narrow">
  <span class="gheader">
    <img src="${rc.getContextPath()}/images/warn.png" class="headimg">
    <@spring.message "forgetpassword.title"/> </span>
  <div class="round whitepanel">
    <#if status == "noSuchEmail">
    	<div class="error">No such email. Please retry.</div> 
    </#if> 
    Please input your email address below:
    <form id="sendMePasswordForm" name="sendMePasswordForm" method="post" action="<@spring.url '/membership/forget-password.html'/>">
    <input name="email" type="input" id="email" />
    <input type="submit" class="button" name="send" value="Send password to me" />
    </form>
  </div>
</div>
</#if>