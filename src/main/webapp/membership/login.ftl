<#--
	Introduction:
		The normal customer login page.
	
	Url:
		/membership/login.*
		
	Controller:
		com.beelun.shoppro.web.MembershipController.handleLoginRequest(HttpServletRequest, HttpServletResponse)
		
	Model:
		N/A
-->

<head>
<title>Login</title>
</head>

<div class="round graypanel narrow">
  <span class="gheader">
    <img src="${rc.getContextPath()}/images/signin.png" class="headimg">
    Login </span>
  <div class="round whitepanel">
  	<#assign error = RequestParameters.error!"none">
  	<#if error == "c">
  	<div class="error">Verification word is incorrect.</div>
  	<#elseif error == "true">
  	<div class="error">The e-mail address or password is incorrect. Please retype the e-mail address and password, or <a href="membership/create-user.html">sign up</a> if you haven't already done so.</div>
  	</#if>
    <form name='f' id='f' action='/j_spring_security_check' method='POST'>
    <input type="hidden" id="urlOnCaptchaFailure" name="urlOnCaptchaFailure" value="/membership/login.html?error=c" />
    <table class="singletr">
      <tr>
        <th>
          Email address:</td><td>
            <input type='text' name='j_username' id='j_username'>
        </th>
      </tr>
      <tr>
        <th>
          Password:</td><td>
            <input type='password' name='j_password' />
        </th>
      </tr>
      <tr>
        <th>
        </th>
        <td>
          <a href="/membership/forget-password.html"><span id="forgetPassword">Forget your password?<span></a>
        </td>
      </tr>
      <tr>
        <td colspan="2">
          <div>
            <hr />
          </div>
        </td>
      </tr>
      <tr>
        <th>
          Word verification:
        </th>
        <td>
          <table>
            <tbody>
              <tr>
                <td valign="top">
                  <span>Type the characters you see in the picture below.</span>
                  <div>
                    <img src="/jcaptcha.jpg" /></div>
                </td>
              </tr>
              <tr>
                <td>
                  <div>
                    <input class="captcha" type="text" name="jcaptcha" value="" /></div>
                  <span>Letters are not case-sensitive</span>
                </td>
              </tr>
            </tbody>
          </table>
        </td>
      </tr>
      <tr>
        <th>
        </th>
        <td>
          <div class="buttondiv">
            <div class="button2 rounded">
              <span onclick="document.f.submit()">${rc.getMessage("ui.loginSubmit")}</span>
            </div>
          </div>
        </td>
      </tr>
    </table>
    </form>
  </div>
</div>
