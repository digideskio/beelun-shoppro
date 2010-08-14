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
import com.beelun.shoppro.model.Tab;
import com.beelun.shoppro.service.CategoryManager;
import com.beelun.shoppro.service.TabManager;

@org.springframework.stereotype.Controller
public class IndexController implements Controller {
	private transient final Log log = LogFactory.getLog(IndexController.class);

	@Autowired
	TabManager tabManager;

	@Autowired
	CategoryManager categoryManager;

	@Override
	@RequestMapping("/index.html")
	public ModelAndView handleRequest(HttpServletRequest request,
			HttpServletResponse response) throws Exception {				
		log.debug("show index.html page");
		
		Tab tab = tabManager.getTabByUrl("index.html");
		List<Category> categoryList = categoryManager.getShown(tab);

		// Put categoryList to session, so that it can be used across the
		// conversation
		request.getSession().setAttribute("categoryList", categoryList);
		request.getSession().setAttribute("currentTab", tab);

		return new ModelAndView("tabPage");
	}
}
