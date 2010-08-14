package com.beelun.shoppro.web;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.servlet.ModelAndView;

import com.beelun.shoppro.model.Article;
import com.beelun.shoppro.service.ArticleManager;
import com.beelun.shoppro.utils.ServletUtils;

@Controller
public class ArticleController {
	private transient final Log log = LogFactory.getLog(ArticleController.class);

	@Autowired
	ArticleManager articleManager;
	
	@RequestMapping("/article/*")
	public ModelAndView handleRequest(HttpServletRequest request,
			HttpServletResponse response) throws Exception {
		Long id = ServletUtils.getId(request);
		log.debug("article id: " + id);
		Article article = articleManager.get(id);
		if(article != null && article.getIsShown() == true) {		
			return new ModelAndView("articlePage", "article", article);
		} else {
			response.sendError(404);
			return null;
		}
	}

}
