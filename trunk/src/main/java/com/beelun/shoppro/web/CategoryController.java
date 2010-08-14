package com.beelun.shoppro.web;

import java.util.List;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.servlet.ModelAndView;
import org.springframework.web.servlet.mvc.Controller;

import com.beelun.shoppro.model.Category;
import com.beelun.shoppro.model.Item;
import com.beelun.shoppro.service.CategoryManager;
import com.beelun.shoppro.service.ItemManager;
import com.beelun.shoppro.utils.ServletUtils;

/**
 * Controller of URL /category/*.html
 * 
 * @author Bill Li(bill@beelun.com)
 * 
 */
@org.springframework.stereotype.Controller
public class CategoryController implements Controller {
	private transient final Log log = LogFactory
			.getLog(CategoryController.class);

	final String DEFAULT_CATOGERY_DTEMPLATE = "uploads/template/defaultCategoryTemplate";
	
	@Autowired
	CategoryManager categoryManager;

	@Autowired
	ItemManager itemManager;

	@Override
	@RequestMapping("/category/*")
	public ModelAndView handleRequest(HttpServletRequest request,
			HttpServletResponse response) throws Exception {
		Long id = ServletUtils.getId(request);
		log.debug("category id: " + id);		
		Category category = categoryManager.get(id);
		
		// Add template support
		String defaultTemplate = DEFAULT_CATOGERY_DTEMPLATE;
		if(category.getTemplate() != null) {
			defaultTemplate = category.getTemplate().getFMPath();
		}

		if(category != null && category.getIsShown() == true) {
			List<Item> itemList = itemManager.getShown(category);
			request.getSession().setAttribute("currentCategory", category);
			return new ModelAndView(defaultTemplate, "itemList", itemList);
		} else {
			// If notShown, return a 404(page not found).
			response.sendError(404);
			return null;
		}
	}
}
