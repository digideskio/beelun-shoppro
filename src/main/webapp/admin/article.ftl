<#ftl strip_whitespace=true>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<#-- This is a test file only, not for production purpose -->

<head>
    <title>Article Management</title>
	<script type="text/javascript" src="${rc.getContextPath()}/scripts/model/article.js"></script>
    <script type="text/javascript" src="/dwr/interface/ArticleManager.js"></script>
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
						<td class="menuItemActive">
							<span>Article</span>
						</td>
					</tr>
					
				</table>
				</div>
			</td>
			<td width="89%" class="dataTable">
				<div class="dataTableTitle">
					<span>Article Management</span><br/>
				
				</div>
				<div class="dataTableToolbar">
					<div>
						<button class="button" id="addArticle" name="addTab" >Add New Article</button>
						<!--<button class="button" id="deleteArticle" name="deleteTab" >Delete Selected Article</button>-->
					</div>
					
				</div>
				<!--<p>Messages:</p>-->
				<div id="showArea"></div>
				
				<div id="articleTable"></div>
			</td>
			
		</tr>
	
	</table>


	<div id="editArticleDialog" class="hide">
	<div class="hd">Modify Article</div>
	<div class="bd">
			<div class="panel full">
	
				<table cellspacing="0" width="100%">
					<tr> 
						<td class="PanelTitle">Article Detail</td>
					</tr>
					<tr>
						<td class="PanelBorder">
	
							<table cellspacing="1" class="PanelBox">
								<tr>
									<td class="PanelBox" valign="top">
										<p style="color:red; border-bottom:1px solid #2763A5">Modify the article from below textbox.</p>
										<br/>
											<table>
												<tr>
													<td>Article id</td>
													<td><input id="articleId" type="text" disabled="disabled" value=""></td>
												</tr>
												<tr>
													<td>title</td>
													<td><input id="title" type="text" value=""></td>
												</tr>
												<tr>
													<td>isShown</td>
													<td><!--<input id="isShown" type="text" value="">-->
														<input type="radio" name="isShown" value="true"><label for="isShown">Yes</label>
														<input type="radio" name="isShown" value="false"><label for="isShown">No</label>
												
													
													</td>
												</tr>
												<tr>
													<td>content</td>
													<td><textarea id="articleContent" cols="50" rows="5" type="text" value=""></textarea></td>
												</tr>
												<tr>
													<td>updated</td>
													<td><input id="updated" type="text" disabled="disabled" value=""></td>
												</tr>
												<!--<tr>
													<td>categoryMap</td>
													<td><input id="categoryMap" type="text" value=""></td>
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
													<td><input id="metaTag" type="text" value=""></td>
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