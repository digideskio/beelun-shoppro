package com.beelun.shoppro.web;

import java.util.List;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.beans.factory.annotation.Autowired; //import org.springframework.stereotype.Controller;
//import org.springframework.ui.ModelMap;
import org.springframework.web.bind.annotation.RequestMapping;

import com.beelun.shoppro.model.Category;
import com.beelun.shoppro.model.Tab;
import com.beelun.shoppro.service.CategoryManager;
import com.beelun.shoppro.service.TabManager;
import com.beelun.shoppro.utils.ServletUtils;

import org.springframework.web.servlet.ModelAndView;
import org.springframework.web.servlet.mvc.Controller;

@org.springframework.stereotype.Controller
public class TabController implements Controller {
	private transient final Log log = LogFactory.getLog(TabController.class);

	@Autowired
	TabManager tabManager;

	@Autowired
	CategoryManager categoryManager;

	@RequestMapping("/tab/*")
	public ModelAndView handleRequest(HttpServletRequest request,
			HttpServletResponse response) throws Exception {
		Long id = ServletUtils.getId(request);
		log.debug("tab id: " + id);
		Tab tab = tabManager.get(id);
						
		if(tab != null && tab.getIsShown() == true) {
			List<Category> categoryList = categoryManager.getShown(tab);
	
			// Put categoryList/currentTab to session, so that it can be used across
			// the conversation
			request.getSession().setAttribute("categoryList", categoryList);
			request.getSession().setAttribute("currentTab", tab);
	
			return new ModelAndView("tabPage");
		} else {
			response.sendError(404);
			return null;
		}
	}
}
