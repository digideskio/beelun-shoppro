<#ftl strip_whitespace=true>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<#-- This is a test file only, not for production purpose -->

<head>
    <title>Welcome to use Shoppro Admin System</title>
    <script type="text/javascript" src="/dwr/interface/OrderManager.js"></script>
</head>

<body>
	<p>Hello, ${currentUser}. Welcome to use admin tool of shoppro system!</p>
	<div class="panel narrow">
	
				<table cellspacing="0" width="100%">
					<tr> 
						<td class="PanelTitle">Order Dashboard</td>
					</tr>
					<tr>
						<td class="PanelBorder">
	
							<table cellspacing="1" class="PanelBox">
								<tr>
									<td class="PanelBox" valign="top">
										<p style="color:red; border-bottom:1px solid #2763A5">Click number to access the detail page!</p>
										<table>
											<tr>
												<th>New Order:&nbsp;</th>
												<td id="newOrderNum"></td>
											</tr>
											<tr>
												<th>Old Order: </th>
												<td id="oldOrderNum"></td>
											</tr>
										</table>
									
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
	
		</div>
		<br/>

	
</body>