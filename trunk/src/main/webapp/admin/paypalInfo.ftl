<#ftl strip_whitespace=true>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<#-- This is a test file only, not for production purpose -->

<head>
    <title>Global Mangerment</title>
    <script type="text/javascript" src="${rc.getContextPath()}/scripts/lib/yui/build/tabview/tabview-min.js"></script>
	<script type="text/javascript" src="${rc.getContextPath()}/scripts/model/paypalInfo.js"></script>
    <script type="text/javascript" src="/dwr/interface/PaypalAccessInfoManager.js"></script>
    <link rel="stylesheet" type="text/css" href="${rc.getContextPath()}/scripts/lib/yui/build/tabview/assets/skins/sam/tabview.css" />
	
</head>

<body>

	<table width="100%" height="100%" style="border-collapse:separate;">
		<tr>
			<td width="10%" class="menuTd">
				<div>
				<table width="100%" align="top">
					<tr class="menuTitle">
						<td><span class="whiteFont">Function Menu</span><td>
					</tr>
					<tr class="menuItem">
						<td>
							<a href="${rc.getContextPath()}/admin/global.html" title="Global Mananger"><span>Global Setting</span></a>
						</td>
					</tr>
					<tr class="menuItem">
						<td class="menuItemActive">
							<span>PaypalAccessInfo</span>
						</td>
					</tr>
					
				</table>
				</div>
			</td>
			<td width="89%" class="dataTable">
				<div class="dataTableTitle">
					<span>Global Mangerment</span><br/>
				
				</div>
				<!--<p>Messages:</p>-->
				<div id="showArea"></div>
				
				<div id="paypalInfoTable" class="adminTable">
					<div id="paypalInfoTabView" class="yui-navset">
					    <ul class="yui-nav">
					        <li class="selected"><><a href="#tab1"><em>Common Setting</em></a></li>
					    </ul>            
					    <div class="yui-content">
					        <div id="tab1">
								<table>
									<tr style="display:none">
										<td>id</td>
										<td><input id="paypalInfoId" type="text" value="" disabled="disabled"></td>
									</tr>
									<tr>
										<td>apiUserName</td>
										<td><input id="apiUserName" type="text" value=""></td>
									</tr>
									<tr>
										<td>apiPassword</td>
										<td><input id="apiPassword" type="text" value=""></td>
									</tr>
									<tr>
										<td>apiSignature</td>
										<td><input id="apiSignature" type="text" value=""></td>
									</tr>
									<tr>
										<td>useSandbox</td>
										<td><input id="useSandbox" type="text" value=""></td>
									</tr>
								</table>
							</div>
							
					    </div>
					</div>
				</div>
				<br/>
				<div><button id="save" class="button">Save</button></div>
			</td>
		</tr>
	</table>
	

</body>