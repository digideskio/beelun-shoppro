<#--
	The admin login form(obsolete, safe to remove later on)
-->

<head>
  <title>Admin login</title>
  <!-- YUI CSS for Menu     -->
  <link rel="stylesheet" type="text/css" href="${rc.getContextPath()}/scripts/lib/yui/build/reset-fonts-grids/reset-fonts-grids.css">
  <link rel="stylesheet" type="text/css" href="${rc.getContextPath()}/scripts/lib/yui/build/menu/assets/skins/sam/menu.css">
  <link rel="stylesheet" type="text/css" href="${rc.getContextPath()}/scripts/lib/yui/build/menu/assets/skins/sam/menu.css">
  <link rel="stylesheet" type="text/css" href="${rc.getContextPath()}/scripts/lib/yui/build/fonts/fonts-min.css" />
  <link rel="stylesheet" type="text/css" href="${rc.getContextPath()}/scripts/lib/yui/build/button/assets/skins/sam/button.css" />
  <link rel="stylesheet" type="text/css" href="${rc.getContextPath()}/scripts/lib/yui/build/container/assets/skins/sam/container.css" />
  <link rel="stylesheet" type="text/css" href="${rc.getContextPath()}/styles/deliciouslyblue/theme.css"
    title="default" />
  <link rel="alternate stylesheet" type="text/css" href="${rc.getContextPath()}/styles/deliciouslygreen/theme.css"
    title="green" />
  <link rel="stylesheet" type="text/css" href="${rc.getContextPath()}/styles/shoppro.css" />
</head>
<div class="panel narrow" style="width: 600px; margin: 0 200px;">
  <div class="round" style="background-color: #F7F7F7; padding: 2px 3px; border: 1px solid #CCCCCC;
    margin: 10px 30px">
    <span style='font-size-adjust: none; height: 40px; line-height: 40px; padding: 0 10px;
      margin-top: 5px; color: #333333; font-weight: bolder; font-size: 16px; font-family: Verdana,Arial,Helvetica,"Sans Serif";
      font-variant: normal; font-style: normal'>
      <img src="${rc.getContextPath()}/images/signin.png" style="width: 20px; height: 20px;
        vertical-align: middle;">
      Login to admin console </span>
    <div class="round" style="background-color: #FFFFFF; padding: 20px 10px; border: 1px solid #CCCCCC;">
      <#assign error = RequestParameters.error!"none"> <#if error == "c">
      <div class="error">
        Verification word is incorrect.</div>
      <#elseif error == "true">
      <div class="error">
        The e-mail address or password is incorrect. Please retype the e-mail address and
        password.</div>
      </#if>
      <form name='f' id='f' action='/j_spring_security_check' method='POST'>
      <input type="hidden" id="urlOnCaptchaFailure" name="urlOnCaptchaFailure" value="/membership/admin-login.html?error=c" />
      <table>
        <tr>
          <th>
            Admin email address:
          </th>
          <td>
            <input type='text' name='j_username' id='j_username'>
          </td>
        </tr>
        <tr>
          <th>
            Password:
          </th>
          <td>
            <input type='password' name='j_password' />
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
</div>
