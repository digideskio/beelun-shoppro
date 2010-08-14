<#--
	Introduction:
		Step 1/3 of Paypal checkout process. This is a form.
		
	Url:
		/customer/input-address.html
		
	Model:
		Type			Name		Description
		--------------------------------------
		List<String>	stateList	list of all states in US
-->

<#import "/spring.ftl" as spring/> 
<#assign xhtmlCompliant = true in spring> <#--
Step 1/3 -->
<head>
  <title>Address</title>
  <style>
    </style>
</head>
<div id="checkout_step1" class="checkout_steps">
</div>
<@spring.bind "addressWrapper.*" /> <#if spring.status.error>
<div class="error">
  <#list spring.status.errorMessages as error> ${error}<br />
  </#list>
</div>
</#if>
<form method="post" action="<@spring.url '/customer/input-address.html'/>" name="addressForm"
id="addressForm">
<div class="round graypanel2 narrow">
  <span class="gheader">Shipping address </span>
  <div class="round whitepanel2">
    <table class="singletr">
      <tr>
        <th>
          <span>First name<b class="redfont">*</b>:<span>
        </th>
        <td>
          <@spring.formInput "addressWrapper.shippingAddress.firstName"/> <@spring.showErrors
          "<br>
          ", "fieldError"/> <span style="margin-left: 55px;">Last name<b class="redfont">*</b>:<span>
            <@spring.formInput "addressWrapper.shippingAddress.lastName" /> <@spring.showErrors
            "<br>", "fieldError"/>
        </td>
      </tr>
      <tr>
        <th>
          <span>Address<b class="redfont">*</b>:<span>
        </th>
        <td>
          <@spring.formInput "addressWrapper.shippingAddress.address" /> <@spring.showErrors
          "<br>
          ", "fieldError"/>
        </td>
      </tr>
      <tr>
        <th>
          <span>Address2<b class="redfont">*</b>:<span>
        </th>
        <td>
          <@spring.formInput "addressWrapper.shippingAddress.address2" /> <@spring.showErrors
          "<br>
          ", "fieldError"/>
        </td>
      </tr>
      <tr>
        <th>
          <span>City<b class="redfont">*</b>:<span>
        </th>
        <td>
          <@spring.formInput "addressWrapper.shippingAddress.city" /> <@spring.showErrors
          "<br>
          ", "fieldError"/> <span>State<b class="redfont">*</b>:<span>
            <select name="shippingAddress.state" id="shippingAddress.state" size="1">
              <#list stateList as state> <#if state==addressWrapper.shippingAddress.state>
              <option value="${state}" selected="selected">${state}</option>
              <#else>
              <option value="${state}">${state}</option>
              </#if> </#list>
            </select>
            <span>Zip<b class="redfont">*</b>:<span> <@spring.formInput "addressWrapper.shippingAddress.zip"
              /> <@spring.showErrors "<br>", "fieldError"/>
        </td>
      </tr>
      <tr>
        <th>
          <span>Phone<b class="redfont">*</b>:<span>
        </th>
        <td>
          <@spring.formInput "addressWrapper.shippingAddress.phoneNumber" /> <@spring.showErrors
          "<br>
          ", "fieldError"/>
        </td>
      </tr>
    </table>
  </div>
</div>
<br />
<div class="round graypanel2 narrow">
  <span class="gheader">Billing address </span>
  <div class="round whitepanel2">
    <#if addressWrapper.sameAddress>
    <input type="checkbox" id="sameAddress" checked="checked" name="sameAddress" />
    <input type="hidden" id="_sameAddress" name="_sameAddress" />
    <#else>
    <input type="checkbox" id="sameAddress" name="sameAddress" />
    <input type="hidden" id="_sameAddress" name="_sameAddress" />
    </#if>
    <label for="sameAddress">
      My billing and shipping address are the same.</label>
    <br />
    <br />
    <#if addressWrapper.sameAddress>
    <div id="billingAddrDiv" class="hide">
      <#else>
      <div id="billingAddrDiv">
        </#if>
        <table class="singletr">
          <tr>
            <th>
              <span>First name<b class="redfont">*</b>:<span>
            </th>
            <td>
              <@spring.formInput "addressWrapper.billingAddress.firstName" /> <@spring.showErrors
              "<br>
              ", "fieldError"/> <span style="margin-left: 55px;">Last name<b class="redfont">*</b>:<span>
                <@spring.formInput "addressWrapper.billingAddress.lastName" /> <@spring.showErrors
                "<br>", "fieldError"/>
            </td>
          </tr>
          <tr>
            <th>
              <span>Address<b class="redfont">*</b>:<span>
            </th>
            <td>
              <@spring.formInput "addressWrapper.billingAddress.address" /> <@spring.showErrors
              "<br>
              ", "fieldError"/>
            </td>
          </tr>
          <tr>
            <th>
              <span>Address2<b class="redfont">*</b>:<span>
            </th>
            <td>
              <@spring.formInput "addressWrapper.billingAddress.address2" /> <@spring.showErrors
              "<br>
              ", "fieldError"/>
            </td>
          </tr>
          <tr>
            <th>
              <span>City<b class="redfont">*</b>:<span>
            </th>
            <td>
              <@spring.formInput "addressWrapper.billingAddress.city" /> <@spring.showErrors "<br>
              ", "fieldError"/> <span>State<b class="redfont">*</b>:<span>
                <select name="billingAddress.state" id="billingAddress.state" size="1">
                  <#list stateList as state> <#if state==addressWrapper.billingAddress.state>
                  <option value="${state}" selected="selected">${state}</option>
                  <#else>
                  <option value="${state}">${state}</option>
                  </#if> </#list>
                </select>
                <span>Zip<b class="redfont">*</b>:<span> <@spring.formInput "addressWrapper.billingAddress.zip"
                  /> <@spring.showErrors "<br>", "fieldError"/>
            </td>
          </tr>
          <tr>
            <th>
              <span>Phone<b class="redfont">*</b>:<span>
            </th>
            <td>
              <@spring.formInput "addressWrapper.billingAddress.phoneNumber" /> <@spring.showErrors
              "<br>
              ", "fieldError"/>
            </td>
          </tr>
        </table>
      </div>
    </div>
  </div>
  <br />
  <br />
  <div class="narrow ">
    <div class="right">
      <input type='image' name='submit' src='${rc.getContextPath()}/images/btn_xpressCheckout.gif'
        border='0' align='top' alt='PayPal' />
    </div>
  </div>
</form>
