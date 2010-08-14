package com.beelun.shoppro.web;

import java.nio.charset.Charset;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import javax.servlet.ServletOutputStream;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.servlet.ModelAndView;

import com.beelun.shoppro.Constants;
import com.beelun.shoppro.model.Item;
import com.beelun.shoppro.service.ItemManager;
import com.beelun.shoppro.service.MyGlobManager;
import com.csvreader.CsvWriter;

/**
 * Controllers for resources requiring 'ROLE_ADMIN' role to access
 * 
 * @author Bill Li(bill@beelun.com)
 * 
 */
@org.springframework.stereotype.Controller
public class AdminController {
	private transient final Log log = LogFactory.getLog(AdminController.class);

	@Autowired
	MyGlobManager myGlobManager;

	@Autowired
	ItemManager itemManager;

	// This method is for old admin console. Comment it out and remove later on
//	@RequestMapping("/admin/*.html")
//	public ModelAndView handleOrders(HttpServletRequest request,
//			HttpServletResponse response) throws Exception {
//
//		log.debug("url: " + request.getRequestURI());
//		String viewName = request.getRequestURI().substring(0, request.getRequestURI().length()-5);
//		log.debug("viewName: " + viewName);
//		
//		String siteType = myGlobManager.fetch().getSiteType().name();
//		log.debug("siteType: " + siteType);
//
//		return new ModelAndView(viewName, "siteType", siteType);
//	}
//	
	//
	// Web service should be access by admin console only. So put these two methods here 
	//
	
	
	@RequestMapping("/clientaccesspolicy.xml")
	public ModelAndView handleClientaccesspolicy(HttpServletRequest request,
			HttpServletResponse response) throws Exception {		
		response.setContentType("text/xml");
		response.setCharacterEncoding("utf-8");		
		return new ModelAndView("clientaccesspolicy");
	}
	
	@RequestMapping("/crossdomain.xml")
	public ModelAndView handleCrossdomain(HttpServletRequest request,
			HttpServletResponse response) throws Exception {		
		response.setContentType("text/xml");
		response.setCharacterEncoding("utf-8");		
		return new ModelAndView("crossdomain");
	}

	@RequestMapping("/admin-console.html")
	public ModelAndView handleAdminConsole(HttpServletRequest request,
			HttpServletResponse response) throws Exception {				
		return new ModelAndView("admin-console");
	}

	@RequestMapping("/sac.html")
	public ModelAndView handleBac(HttpServletRequest request,
			HttpServletResponse response) throws Exception {	
		log.debug("request SL admin console");
		return new ModelAndView("sac");
	}

	@RequestMapping("/admin/export-items.html")
	public ModelAndView handleItemDownload(HttpServletRequest request,
			HttpServletResponse response) throws Exception {		
		// Sent response as file
		SimpleDateFormat formatPattern = new SimpleDateFormat("yyyy-MM-dd_HH-mm-ss");
		String nowFormatted = formatPattern.format(new Date());
		response.setContentType("application/octet-stream");
		response.setHeader("Content-Disposition","attachment;filename=ItemsFromShoppro_" + nowFormatted + ".csv");
		
		// Get parameter
		String orderBy = "id"; // Default is 'id'
		if(request.getParameter("orderBy") != null)
		{
			orderBy = request.getParameter("orderBy");
		}
		Long categoryId = Constants.ALL_CATEGORIES;  // Default is 'all'
		try{
			categoryId = Long.parseLong(request.getParameter("categoryId"));
		}catch(Exception ex){};
		boolean ascending = Boolean.getBoolean(request.getParameter("ascending")); // This won't throw exception
		String searchText = request.getParameter("searchText");
		
		// Get response stream & csv writer
		ServletOutputStream out = response.getOutputStream();
		CsvWriter csvWriter = new CsvWriter(out, ',', Charset.forName("UTF-8"));
		
		// Write header
		csvWriter.writeRecord(new String[] {"Internal ID","Store Display Name","Store Description","Detailed Description","Image URL",
				"Thumbnail URL","List Price","Base Price","On Hand","Display in Web Site","Page Title","Search Keywords","Meta Tag Html","URL Component","Manufacturer"});
		
		// Get the item list
		List<Item> itemList = null;
		if(null == searchText) {
			itemList = itemManager.getByCondition(orderBy, ascending, 0, 0, categoryId);
		} else {
			itemList = itemManager.searchByText(searchText, 0, 0);
		}
		for(Item item : itemList) {
			List<String> columns = new ArrayList<String>();
			columns.add(item.getNetSuiteId().toString());
			columns.add(item.getName());
			columns.add(item.getShortDescription());
			columns.add(item.getDetailedDescription());
			columns.add(item.getImage());
			columns.add(item.getThumbNail());
			if(item.getListPrice() != null){
				columns.add(item.getListPrice().toPlainString());
			} else {
				columns.add("");
			}
			if(item.getSellPrice() != null) {
				columns.add(item.getSellPrice().toPlainString());
			} else {
				columns.add("");
			}
			if(item.getInventoryNumber() != null) {
				columns.add(item.getInventoryNumber().toString());
			} else {
				columns.add("");
			}
			columns.add(item.getIsShown()==true?"Yes": "No");
			columns.add(item.getPageTitle());
			columns.add(item.getKeywords());
			columns.add(item.getMetaTag());
			columns.add(item.getUrl());
			if(item.getBrand() != null) {
				columns.add(item.getBrand().getId().equals(Constants.NO_BRAND_ID)?(""):(item.getBrand().getName()));
			} else {
				columns.add("");
			}
			csvWriter.writeRecord(columns.toArray(new String[]{""}));
		}
		
		// Close streams and return
		csvWriter.flush();
		csvWriter.close();
		out.flush();
		out.close();
		return null;
	}

}
