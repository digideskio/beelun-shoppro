<#ftl strip_whitespace=true>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<#-- This is a test file only, not for production purpose -->

<head>
    <title>Global Mangerment</title>
    <script type="text/javascript" src="${rc.getContextPath()}/scripts/lib/yui/build/tabview/tabview-min.js"></script>
	<script type="text/javascript" src="${rc.getContextPath()}/scripts/model/myGlob.js"></script>
    <script type="text/javascript" src="/dwr/interface/MyGlobManager.js"></script>
    <link rel="stylesheet" type="text/css" href="${rc.getContextPath()}/scripts/lib/yui/build/tabview/assets/skins/sam/tabview.css" />
	
</head>

<body>

	<table width="100%" height="100%" style="border-collapse:separate;">
		<tr>
			<td width="10%" style="vertical-align:top;border-right:5px solid #6F777A">
				<div>
				<table width="100%" align="top">
					<tr style="height:30px;background-color:#2763A5;border-bottom:1px solid #EEEEEE;">
						<td><span style="color:white;">Function Menu</span><td>
					</tr>
					<tr style="border-bottom:1px solid #EEEEEE;">
						<td style="background-color:#6F777A">
							<span style="color:#EEEEEE">Global Setting</span>
						</td>
					</tr>
					<tr style="border-bottom:1px solid #EEEEEE;">
						<td>
							<a href="${rc.getContextPath()}/admin/paypalInfo.html" title="PaypalAccessInfo Mananger"><span>PaypalAccessInfo</span></a>
						</td>
					</tr>
					
				</table>
				</div>
			</td>
			<td width="89%" style="vertical-align:top;border-bottom:5px solid #6F777A;border-right:5px solid #6F777A">
				<div style="background-color:#6F777A;">
					<span style="color:#CCCCCC">PaypalAccessInfo Management</span><br/>
				
				</div>
				<!--<p>Messages:</p>-->
				<div id="showArea"></div>
				
				<div id="myGlobTable"></div>
				
				<div id="globalTable" class="adminTable">
					<div id="globalTabView" class="yui-navset">
					    <ul class="yui-nav">
					        <li class="selected"><><a href="#tab1"><em>Common Setting</em></a></li>
					        <li><a href="#tab2"><em>Footer Setting</em></a></li>
					        <li><a href="#tab3"><em>Page 404 Setting</em></a></li>
					        <li><a href="#tab4"><em>Page 500 Setting</em></a></li>
					    </ul>            
					    <div class="yui-content">
					        <div id="tab1">
								<table>
									<tr style="display:none">
										<td>id</td>
										<td><input id="myGlobId" type="text" value="" disabled="disabled"></td>
									</tr>
									<tr>
										<td>shopName</td>
										<td><input id="shopName" type="text" value=""></td>
									</tr>
									<tr>
										<td>slogan</td>
										<td><input id="slogan" type="text" value=""></td>
									</tr>
									<tr>
										<td>phoneNumber</td>
										<td><input id="phoneNumber" type="text" value=""></td>
									</tr>
									<tr>
										<td>logoAbsoluteUrl</td>
										<td><input id="logoAbsoluteUrl" type="text" value=""></td>
									</tr>
									<tr>
										<td>address</td>
										<td><input id="address" type="text" value=""></td>
									</tr>
									<tr>
										<td>pageNoSearchResult</td>
										<td><input id="pageNoSearchResult" type="text" value=""></td>
									</tr>
									<tr>
										<td>googleCustSearchCode</td>
										<td><input id="googleCustSearchCode" type="text" value=""></td>
									</tr>
									<tr>
										<td>signupAgreement</td>
										<td><textarea id="signupAgreement" cols="70" rows="5" value=""></textarea></td>
									</tr>
									<tr>
										<td>unlockEmailTemplate</td>
										<td><textarea id="unlockEmailTemplate" cols="70" rows="5" value=""></textarea></td>
									</tr>
									<tr>
										<td>resetPasswordMailTemplate</td>
										<td><textarea id="resetPasswordMailTemplate" cols="70" rows="5" value=""></textarea></td>
									</tr>
									<tr>
										<td>version</td>
										<td><input id="version" type="text" value=""></td>
									</tr>
									<tr>
										<td>siteType</td>
										<td><input id="siteType" type="text" value="" disabled="disabled"></td>
									</tr>
									<!--<tr>
										<td>maxUploadFileSize</td>
										<td><input id="maxUploadFileSize" type="text" value=""></td>
									</tr>-->
								</table>
							</div>
							<div id="tab2">
					        	<table>
									<tr>
										<td>footer</td>
										<td><textarea id="footer" cols="100" rows="20" value=""></textarea></td>
									</tr>
								</table>
	        
					        </div>
					        <div id="tab3">
					        	<table>
									<tr>
										<td>page404</td>
										<td><textarea id="page404" cols="100" rows="20"  type="text" value=""></textarea></td>
									</tr>
								</table>
	        
					        </div>
					        <div id="tab4">
					        	<table>
									<tr>
										<td>page500</td>
										<td><textarea id="page500" cols="100" rows="20"  type="text" value=""></textarea></td>
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