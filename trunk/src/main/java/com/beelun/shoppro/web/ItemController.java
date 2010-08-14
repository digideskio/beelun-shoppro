package com.beelun.shoppro.web;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.servlet.ModelAndView;

import com.beelun.shoppro.model.Item;
import com.beelun.shoppro.service.ItemManager;
import com.beelun.shoppro.utils.ServletUtils;

@Controller
public class ItemController {
	private transient final Log log = LogFactory.getLog(ItemController.class);
	private final String DEFAULT_ITEM_DTEMPLATE = "uploads/template/defaultItemTemplate";
	
	@Autowired
	ItemManager itemManager;
	
	@RequestMapping("/item/*")
	public ModelAndView handleRequest(HttpServletRequest request,
			HttpServletResponse response) throws Exception {
		Long id = ServletUtils.getId(request);
		log.debug("item id: " + id);
		Item item = itemManager.get(id);
		
		// Add template support
		String defaultTemplate = DEFAULT_ITEM_DTEMPLATE;
		if(item.getTemplate() != null) {
			defaultTemplate = item.getTemplate().getFMPath();
		}
		
		if(item != null && item.getIsShown() == true) {		
			return new ModelAndView(defaultTemplate, "item", item);
		} else {
			response.sendError(404);
			return null;
		}
	}
}
