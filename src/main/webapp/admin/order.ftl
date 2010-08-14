<#ftl strip_whitespace=true>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<#-- This is a test file only, not for production purpose -->

<head>
    <title>Order Management</title>
	<script type="text/javascript" src="${rc.getContextPath()}/scripts/model/order.js"></script>
    <script type="text/javascript" src="/dwr/interface/OrderManager.js"></script>
</head>

<body>

	<table width="100%" height="100%">
		<tr>
			<td width="10%" class="menuTd">
				<div>
				<table width="100%" align="top">
					<tr class="menuTitle">
						<td><span class="whiteFont">Function Menu</span><td>
					</tr>
					<tr class="menuItem">
						<td>
							<a href="${rc.getContextPath()}/admin/newOrder.html" title="NewOrder Mananger"><span>New Order</span></a>
						</td>
					</tr>
					<tr class="menuItem">
						<td class="menuItemActive">
							<span>Old Order</span>
						</td>
					</tr>
				</table>
				</div>
			</td>
			<td width="89%" class="dataTable">
				<div class="dataTableTitle">
					<span>Old Order Page</span><br/>
				
				</div>
				<div class="dataTableToolbar">
					<span>Old Order List Table</span>
					
				</div>
				<!--<p>Messages:</p>-->
				<div id="showArea"></div>
				
				<div id="orderTable"></div>
			</td>
			
		</tr>
	
	</table>

	
	
	<div id="editOrderDialog" class="hide">
	<div class="hd"></div>
	<div class="bd">
		<div>
			<table>
				<tr>
					<td>Order id</td>
					<td><input id="orderId" type="text" value=""></td>
				</tr>
				<tr>
					<td>orderDate</td>
					<td><input id="orderDate" type="text" value=""></td>
				</tr>
				<tr>
					<td>user</td>
					<td><input id="user" type="text" value=""></td>
				</tr>
				<tr>
					<td>address</td>
					<td><input id="address" type="text" value=""></td>
				</tr>
				<tr>
					<td>shipDate</td>
					<td><input id="shipDate" type="text" value=""></td>
				</tr>
				<tr>
					<td>shipTime</td>
					<td><input id="shipTime" type="text" value=""></td>
				</tr>
				<tr>
					<td>specifiedShipDate</td>
					<td><input id="specifiedShipDate" type="text" value=""></td>
				</tr>
				<tr>
					<td>notes</td>
					<td><input id="notes" type="text" value=""></td>
				</tr>
				<tr>
					<td>expressCorp</td>
					<td><input id="expressCorp" type="text" value=""></td>
				</tr>
				<tr>
					<td>paymentTool</td>
					<td><input id="paymentTool" type="text" value=""></td>
				</tr>
				<tr>
					<td>status</td>
					<td><input id="status" type="text" value=""></td>
				</tr>
				<tr>
					<td>orderItemSet</td>
					<td><input id="orderItemSet" type="text" value=""></td>
				</tr>
			</table>
		</div>

	</div>
	</div>
</body>