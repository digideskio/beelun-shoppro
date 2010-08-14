<#-- 

	Introduction:
		Simple header containing logo, user name and logout 
	
	Model:
		Type	Name			Description
		------------------------------------------------------
		User	currentUser		currently logged in user

-->

<div id="ms-header">
  <h1>
    <a href="${base}/" target="_parent" title="${glob.shopName}"><span></span>${glob.shopName}</a></h1>
  <ul>
    <li>
      <ul>
        <#if currentUser?exists>
        <li class="header_li">${currentUser}</li>
        <li class="header_li"><a href="${base}/logout.html">${rc.getMessage("ui.logout")}</a></li>
        </#if>
      </ul>
    </li>
    <li>
      <form action="${base}/google-cse-results.html" id="cse-search-box">
      <div>
        <input type="hidden" name="cx" value="009705515215130087514:a93r1ilig7u" />
        <input type="hidden" name="cof" value="FORID:10" />
        <input type="hidden" name="ie" value="UTF-8" />
        <input type="text" name="q" size="31" />
        <input type="submit" name="sa" value="Search" />
      </div>
      </form>

      <script type="text/javascript" src="http://www.google.com/cse/brand?form=cse-search-box&lang=en"></script>

    </li>
  </ul>
</div>
