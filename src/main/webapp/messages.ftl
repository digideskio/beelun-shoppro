<#-- 
	Success Messages
	
	In controller, it is set as follows:   
          request.getSession().setAttribute("message", 
                    getText("user.deleted", user.getName()));
-->
<#if message?exists>
    <div class="message">${message?html}</div>
</#if>
