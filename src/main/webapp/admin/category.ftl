<#ftl strip_whitespace=true>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<#-- This is a test file only, not for production purpose -->

<head>
    <title>Category Management</title>
	<script type="text/javascript" src="${rc.getContextPath()}/scripts/model/category.js"></script>
    <script type="text/javascript" src="/dwr/interface/CategoryManager.js"></script>
    
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
						<td class="menuItemActive">
							<span>Category</span>
						</td>
					</tr>
					<tr class="menuItem">
						<td>
							<a href="${rc.getContextPath()}/admin/tab2category.html" title="TabMap Mananger"><span>TabMap</span></a>
						</td>
					</tr>
					<tr class="menuItem">
						<td>
							<a href="${rc.getContextPath()}/admin/category2item.html" title="ItemMap Mananger"><span>ItemMap</span></a>
						</td>
					</tr>
					
				</table>
				</div>
			</td>
			<td width="89%" class="dataTable">
				<div class="dataTableTitle">
					<span>Category Management</span><br/>
				
				</div>
				<div class="dataTableToolbar">
					<div>
						<button class="button" id="addCategory" name="addTab" >Add New Category</button>
						<button class="button" id="deleteCategory" name="deleteTab" >Delete Selected Category</button>
					</div>
					
				</div>
				<!--<p>Messages:</p>-->
				<div id="showArea"></div>
				<div id="categoryTable"></div>
			</td>
			
		</tr>
	
	</table>


	<div id="editCategoryDialog" class="hide">
	<div class="hd">Modify Category</div>
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
													<td>Category id</td>
													<td><input id="categoryId" type="text" disabled="disabled" value=""></td>
												</tr>
												<tr>
													<td>name</td>
													<td><input id="name" type="text" value=""></td>
												</tr>
												<tr>
													<td>isShown</td>
													<td>
														<input type="radio" name="isShown" value="true"><label for="isShown">Yes</label>
														<input type="radio" name="isShown" value="false"><label for="isShown">No</label>
												
													</td>
												</tr>
												<!--<tr>
													<td>itemMap</td>
													<td><input id="itemMap" type="text" value=""></td>
												</tr>-->
												<tr>
													<td>pageTitle</td>
													<td><input id="pageTitle" type="text" value=""></td>
												</tr>
												<tr>
													<td>keywords</td>
													<td><input id="keywords" type="text" value=""></td>
												</tr>
												<tr>
													<td>description</td>
													<td><input id="description" type="text" value=""></td>
												</tr>
												<tr>
													<td>metaTag</td>
													<td><textarea id="metaTag" cols="50" rows="5" type="text" value=""></textarea></td>
												</tr>
												<tr>
													<td>url</td>
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