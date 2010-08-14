<#ftl strip_whitespace=true>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<#-- This is a test file only, not for production purpose -->

<head>
    <title>User Mangerment</title>
	<script type="text/javascript" src="${rc.getContextPath()}/scripts/model/user.js"></script>
    <script type="text/javascript" src="/dwr/interface/UserManager.js"></script>
</head>

<body>
	<table width="100%" height="100%" style="border-collapse:separate;">
		<tr>
			<td style="vertical-align:top;border:5px solid #6F777A">
				<div style="background-color:#6F777A;">
					<span style="color:#CCCCCC">User Management</span><br/>
				
				</div>
				<div style="background:#F0F0F0 none repeat scroll 0 0;border-bottom:3px solid #6F777A;border-top:1px solid #FFFFFF;line-height:30px;padding:0 8px;">
					<span>User List Table</span><br/>
					<span style="color:red">Double click Enabled column to modify the value. </span>
					
				</div>
				<!--<p>Messages:</p>-->
				<div id="showArea"></div>
				
				<div id="userListTable"></div>
			</td>
			
		</tr>
	
	</table>


	
	<div id="editUserDialog" class="hide">
	<div class="hd">Modify User</div>
	<div class="bd">
	<div class="panel full">
	
				<table cellspacing="0" width="100%">
					<tr> 
						<td class="PanelTitle">User Detail</td>
					</tr>
					<tr>
						<td class="PanelBorder">
	
							<table cellspacing="1" class="PanelBox">
								<tr>
									<td class="PanelBox" valign="top">
										<p style="color:red; border-bottom:1px solid #2763A5">Modify the user from below textbox.</p>
										<br/>
											<table>
												<tr>
													<td>User id</td>
													<td><input id="userId" type="text" value=""></td>
												</tr>
												<tr>
													<td>name</td>
													<td><input id="name" type="text" value=""></td>
												</tr>
												<tr>
													<td>email</td>
													<td><input id="email" type="text" value=""></td>
												</tr>
												<tr>
													<td>password</td>
													<td><input id="password" type="text" value=""></td>
												</tr>
												<tr>
													<td>securityQuestion</td>
													<td><input id="securityQuestion" type="text" value=""></td>
												</tr>
												<tr>
													<td>securityQuestionAnswer</td>
													<td><input id="securityQuestionAnswer" type="text" value=""></td>
												</tr>
												<tr>
													<td>enabled</td>
													<td><input id="enabled" type="text" value=""></td>
												</tr>
												<tr>
													<td>accountExpired</td>
													<td><input id="accountExpired" type="text" value=""></td>
												</tr>
												<tr>
													<td>accountLocked</td>
													<td><input id="accountLocked" type="text" value=""></td>
												</tr>
												<tr>
													<td>credentialsExpired</td>
													<td><input id="credentialsExpired" type="text" value=""></td>
												</tr>
												<tr>
													<td>shippingAddress</td>
													<td><input id="shippingAddress" type="text" value=""></td>
												</tr>
												<tr>
													<td>billingAddress</td>
													<td><input id="billingAddress" type="text" value=""></td>
												</tr>
											</table>
	
									
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>

	</div>
	</div>
</body>