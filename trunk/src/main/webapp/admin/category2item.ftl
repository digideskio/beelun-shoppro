<#ftl strip_whitespace=true>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<#-- This is a test file only, not for production purpose -->

<head>
    <title>ItemMap Management</title>
	<script type="text/javascript" src="${rc.getContextPath()}/scripts/model/categoryMap.js"></script>
    <script type="text/javascript" src="/dwr/interface/CategoryManager.js"></script>
    <script type="text/javascript" src="/dwr/interface/ItemManager.js"></script>
    <script type="text/javascript" src="/dwr/interface/Category2ItemMapManager.js"></script>

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
							<a href="${rc.getContextPath()}/admin/import-items.html" title="Item Mananger"><span>Item</span></a>
						</td>
					</tr>
					<tr class="menuItem">
						<td>
							<a href="${rc.getContextPath()}/admin/tab.html" title="Tab Mananger"><span>Tab</span></a>
						</td>
					</tr>
					<tr class="menuItem">
						<td>
							<a href="${rc.getContextPath()}/admin/category.html" title="CategoryMap Mananger"><span>Category</span></a>
						</td>
					</tr>
					<tr class="menuItem">
						<td>
							<a href="${rc.getContextPath()}/admin/tab2category.html" title="TabMap Mananger"><span>TabMap</span></a>
						</td>
					</tr>
					<tr class="menuItem">
						<td class="menuItemActive">
							<span>ItemMap</span>
						</td>
					</tr>
					
				</table>
				</div>
			</td>
			<td width="89%" class="dataTable">
				<div class="dataTableTitle">
					<span>ItemMap Page</span><br/>
				
				</div>
				<div class="dataTableToolbar">
					<div>
						<button class="button" id="addCategoryMap" name="addTabMap" >Add New CategoryMap</button>
						<button class="button" id="deleteCategoryMap" name="deleteTabMap" >Delete Selected CategoryMap</button>
					</div>
					
				</div>
				<!--<p>Messages:</p>-->
				<div id="showArea"></div>

				<div id="categoryMapTable"></div>
			</td>
			
		</tr>
	
	</table>
	
	<div id="editCategoryMapDialog" class="hide">
	<div class="hd">Modify Category-Item Mapping</div>
	<div class="bd">
		<div class="panel full">
	
				<table cellspacing="0" width="100%">
					<tr> 
						<td class="PanelTitle">CategoryMap Detail</td>
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
													<td>CategoryMap id</td>
													<td><input id="categoryMapId" type="text" disabled="disabled" value=""></td>
												</tr>
												<tr>
													<td>category</td>
													<td>
														<select id="category"></select>
														<!--<input id="category" type="text" value="">-->
													</td>
												</tr>
												<tr>
													<td>item</td>
													<td>
														<select id="item"></select>
														<!--<input id="item" type="text" value="">-->
													</td>
												</tr>
												<tr>
													<td>itemOrder</td>
													<td><input id="itemOrder" type="text" value=""></td>
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