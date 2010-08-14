<#ftl strip_whitespace=true>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<#-- This is a test file only, not for production purpose -->

<head>
    <title>Item Managerment</title>
    <script type="text/javascript" src="${rc.getContextPath()}/scripts/model/item.js"></script> 
    <script type="text/javascript" src="/dwr/interface/ItemManager.js"></script>
    <script type="text/javascript" src="/dwr/interface/BrandManager.js"></script>
</head>

<body>

	<table width="100%" height="100%">
		<tr>
			<td width="10%" style="vertical-align:top;border-right:5px solid #6F777A">
				<div>
				<table width="100%" align="top">
					<tr style="height:30px;background-color:#2763A5;border-bottom:1px solid #EEEEEE;">
						<td><span style="color:white;">Function Menu</span><td>
					</tr>
					<tr style="border-bottom:1px solid #EEEEEE;">
						<td style="background-color:#6F777A">
							<span style="color:#EEEEEE">Item</span>
						</td>
					</tr>
					<tr style="border-bottom:1px solid #EEEEEE;">
						<td>
							<a href="${rc.getContextPath()}/admin/tab.html" title="Tab Mananger"><span>Tab</span></a>
						</td>
					</tr>
					<tr style="border-bottom:1px solid #EEEEEE;">
						<td>
							<a href="${rc.getContextPath()}/admin/category.html" title="Category Mananger"><span>Category</span></a>
						</td>
					</tr>
					<tr style="border-bottom:1px solid #EEEEEE;">
						<td>
							<a href="${rc.getContextPath()}/admin/tab2category.html" title="TabMap Mananger"><span>TabMap</span></a>
						</td>
					</tr>
					<tr style="border-bottom:1px solid #EEEEEE;">
						<td>
							<a href="${rc.getContextPath()}/admin/category2item.html" title="ItemMap Mananger"><span>ItemMap</span></a>
						</td>
					</tr>
					
				</table>
				</div>
			</td>
			<td width="89%" style="vertical-align:top;border-bottom:5px solid #6F777A;border-right:5px solid #6F777A">
				<div style="background-color:#6F777A;">
					<span style="color:#CCCCCC">Item Managerment</span><br/>
				
				</div>
				<div style="background:#F0F0F0 none repeat scroll 0 0;border-bottom:3px solid #6F777A;border-top:1px solid #FFFFFF;line-height:30px;padding:0 8px;">
					<div>
						<button class="button" id="addItem" name="addItem" >Add New Item</button>
						<button class="button" id="deleteItem" name="deleteItem" >Delete Selected Item</button>
					</div>
					
				</div>
				<!--<p>Messages:</p>-->
				<div id="showArea"></div>
				
				
				<p></p>
				<div id="itemListTable"></div>
			</td>
			
		</tr>
	
	</table>
	
	<div id="editItemDialog" class="hide">
	<div class="hd">Modify Item</div>
	<div class="bd">
		<div class="panel full">
	
				<table cellspacing="0" width="100%">
					<tr> 
						<td class="PanelTitle">Item Detail</td>
					</tr>
					<tr>
						<td class="PanelBorder">
	
							<table cellspacing="1" class="PanelBox">
								<tr>
									<td class="PanelBox" valign="top">
										<p style="color:red; border-bottom:1px solid #2763A5">Modify the item from below textbox.</p>
										<br/>
											<table>
												<tr>
													<td style="color:#2763A5">Item id</td>
													<td><input id="itemId" type="text" disabled="disabled" value=""></td>
												</tr>
												<tr>
													<td style="color:#2763A5">Brand</td>
													<td>
														<select id="brand"></select>
														<input type="hidden" id="brandId"/>
														<input type="hidden" id="brandImage"/>
														<input type="hidden" id="brandWebsite"/>
													</td>
												</tr>
												<tr>
													<td style="color:#2763A5">netSuite Id</td>
													<td><input id="netSuiteId" type="text" width="350px" disabled="disabled" value=""></td>
												</tr>
												<tr>
													<td style="color:#2763A5">name</td>
													<td><input id="name" type="text" value=""></td>
												</tr>
												<tr>
													<td style="color:#2763A5">serialNumber</td>
													<td><input id="serialNumber" type="text" value=""></td>
												</tr>
												<tr>
													<td style="color:#2763A5">shortDescription</td>
													<td><input id="shortDescription" type="text" value=""></td>
												</tr>
												<tr>
													<td style="color:#2763A5">detailedDescription</td>
													<td><textarea id="detailedDescription" cols="50" rows="5" type="text" value=""></textarea></td>
												</tr>
												<tr>
													<td style="color:#2763A5">image</td>
													<td><input id="image" type="text" value=""></td>
												</tr>
												<tr>
													<td style="color:#2763A5">thumbNail</td>
													<td><input id="thumbNail" type="text" value=""></td>
												</tr>
												<tr>
													<td style="color:#2763A5">listPrice</td>
													<td><input id="listPrice" type="text" value=""></td>
												</tr>
												<tr>
													<td style="color:#2763A5">sellPrice</td>
													<td><input id="sellPrice" type="text" value=""></td>
												</tr>
												<tr>
													<td style="color:#2763A5">inventoryNumber</td>
													<td><input id="inventoryNumber" type="text" value=""></td>
												</tr>
												<tr>
													<td style="color:#2763A5">isShown</td>
													<td><!--<input id="isShown" type="text" value="">-->
														<input type="radio" name="isShown" value="true"><label for="isShown">Yes</label>
														<input type="radio" name="isShown" value="false"><label for="isShown">No</label>
												
													
													</td>
												</tr>
												<tr>
													<td style="color:#2763A5">pageTitle</td>
													<td><input id="pageTitle" type="text" value=""></td>
												</tr>
												<tr>
													<td style="color:#2763A5">keywords</td>
													<td><input id="keywords" type="text" value=""></td>
												</tr>
												<tr>
													<td style="color:#2763A5">description</td>
													<td><input id="description" type="text" value=""></td>
												</tr>
												<tr>
													<td style="color:#2763A5">metaTag</td>
													<td><input id="metaTag" type="text" value=""></td>
												</tr>
												<tr>
													<td style="color:#2763A5">url</td>
													<td><input id="url" type="text" value=""></td>
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
	</div>
</body>