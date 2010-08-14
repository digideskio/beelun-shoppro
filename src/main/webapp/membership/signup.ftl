<#--
	Introduction:
		The user signup form
		
	Url:
		/membership/create-user.html
		
	Controller:
		com.beelun.shoppro.web.UserFormController
	
	Model:
		A newly created user
			OR 
		user a previous user(when user click back in browser. Is this working now?)
-->

<#import "/spring.ftl" as spring/> 
<#assign xhtmlCompliant = true in spring>
<head>
  <title><@spring.message "signUp.title"/></title>
  <link href="styles/calendar.css" type="text/css" rel="stylesheet" />

  <script type="text/javascript" src="scripts/calendar.js"></script>

  <script type="text/javascript" src="scripts/calendar-setup.js"></script>

  <script type="text/javascript" src="scripts/lang/calendar-en.js"></script>

</head>
<@spring.bind "user.*"/>
<div class="loginLeft">
  <div class="round graypanel">
    <span class="gheader">
      <img src="${rc.getContextPath()}/images/signin.png" class="headimg">
      Existing Account </span>
    <div class="round whitepanel">
     <table class="singletr">
        <tr>
          <th>
          </th>
          <td>
            <div class="buttondiv">
              <div class="button2 rounded">
                <span onclick="document.location='${rc.getContextPath()}/membership/login.html'"><#--${rc.getMessage("ui.loginSubmit")}-->Click here to login</span>
              </div>
            </div>
          </td>
        </tr>
      </table>
    </div>
  </div>
</div>
<div class="SignupRight">
  <div class="round graypanel">
    <span class="gheader">
      <img src="${rc.getContextPath()}/images/signup.png" class="headimg">
      New Account </span>
    <div class="round whitepanel">
      <form method="post" action="<@spring.url '/membership/create-user.html'/>" id="userForm"
      name="userForm">
      <#if (RequestParameters.error!"none") == "c">
      	<div class="error">Verification word is incorrect.</div>
      </#if>
      <#if spring.status.error>
      <div class="error">
        <#list spring.status.errorMessages as error> ${error}<br />
        </#list>
      </div>
      </#if> <@spring.formHiddenInput "user.id"/>
      
      <input type="hidden" id="urlOnCaptchaFailure" name="urlOnCaptchaFailure" value="/membership/create-user.html?error=c" />
      <table class="singletr">
        <tr>
          <th>
            <label for="name">
              <@spring.message "user.name"/>:</label>
          </th>
          <td>
            <@spring.formInput "user.name", 'id="name"'/> 
            <@spring.showErrors "<br>", "fieldError"/>
          </td>
        </tr>
        <tr>
          <th>
            <label for="email">
              <@spring.message "user.email"/>:</label>
          </th>
          <td>
            <@spring.formInput "user.email", 'id="email"'/> 
            <@spring.showErrors "<br>", "fieldError"/>
          </td>
        </tr>
        <tr>
          <th>
            <label for="password">
              <@spring.message "user.password"/>:</label>
          </th>
          <td>
            <@spring.formPasswordInput "user.password", 'id="password"'/> 
            <@spring.showErrors "<br>", "fieldError"/>
          </td>
        </tr>
        <tr>
          <th>
            <label for="repassword">
              <@spring.message "user.repassword"/>:</label>
          </th>
          <td>
            <input name="repassword_name" type="password" id="repassword" />
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
                      <input type="text" name="jcaptcha" value="" /></div>
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
            <div id='userForm_errorloc' class='error_strings' style="color: red">
            </div>
          </td>
        </tr>
        <tr>
          <th>
          </th>
          <td>
            <div class="agreement" style="margin: 5px;">
              <a target="_blank" href="/article/2-privacy-statement.html">${glob.signupAgreement}</a>
            </div>
          </td>
        </tr>
        <tr>
          <th>
          </th>
          <td>
            <div class="buttondiv">
              <div class="button2 rounded">
                <span onclick="document.userForm.submit()">Agree & Sign up</span>
              </div>
            </div>
          </td>
        </tr>
      </table>
      </form>
    </div>
  </div>
</div>
<#--
<script type="text/javascript">
    Form.focusFirstElement($('userForm'));
    Calendar.setup(
    {
        inputField  : "birthday",      // id of the input field
        ifFormat    : "%m/%d/%Y",      // the date format
        button      : "birthdayCal"    // id of the button
    }
    );
</script>
-->