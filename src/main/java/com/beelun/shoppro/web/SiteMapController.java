package com.beelun.shoppro.web;

import java.io.Writer;
import java.text.SimpleDateFormat;
import java.util.List;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.dom4j.Document;
import org.dom4j.DocumentFactory;
import org.dom4j.DocumentHelper;
import org.dom4j.Element;
import org.dom4j.QName;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.servlet.ModelAndView;

import com.beelun.shoppro.SuperContainer;
import com.beelun.shoppro.model.Article;
import com.beelun.shoppro.model.Category;
import com.beelun.shoppro.model.Item;
import com.beelun.shoppro.model.Tab;
import com.beelun.shoppro.service.ArticleManager;
import com.beelun.shoppro.service.CategoryManager;
import com.beelun.shoppro.service.ItemManager;
import com.beelun.shoppro.service.TabManager;
import com.beelun.shoppro.utils.GeneralUtils;
import com.beelun.shoppro.utils.ServletUtils;

/**
 * Controller for /sitemap.xml
 * Refer to:
 * http://www.sitemaps.org/protocol.php
 * http://en.wikipedia.org/wiki/Robots_exclusion_standard
 * 
 * @author bali
 * 
 */
@Controller
public class SiteMapController {
	private transient final Log log = LogFactory.getLog(SiteMapController.class);
	private transient final String dtFormat = "yyyy-MM-dd";

	@Autowired
	TabManager tabManager;

	@Autowired
	CategoryManager categoryManager;

	@Autowired
	ItemManager itemManager;

	@Autowired
	ArticleManager articleManager;

	@RequestMapping("/sitemap.html")
	public ModelAndView handleSiteMapHtmlRequest(HttpServletRequest request, HttpServletResponse response) throws Exception {
		// Fill in the visible content
		ModelAndView mv = new ModelAndView("sitemapHtml");
		List<Tab> tabList = tabManager.getShownTabs();
		List<Category> categoryList = categoryManager.getShownAll();
		List<Item> itemList = itemManager.getShownAll();
		List<Article> articleList = articleManager.getShownAll();
		mv.addObject("tabListAll", tabList);
		mv.addObject("categoryListAll", categoryList);
		mv.addObject("itemListAll", itemList);
		mv.addObject("articleListAll", articleList);

		// Return
		return mv;
	}

	@RequestMapping("/sitemap.xml")
	public ModelAndView handleSiteMapXmlRequest(HttpServletRequest request, HttpServletResponse response) throws Exception {
		log.debug("in handleSiteMapRequest()...");

		// This controller will return xml format, so we set it here.
		response.setContentType("text/xml");
		response.setCharacterEncoding("utf-8");
		QName rootName = DocumentFactory.getInstance().createQName("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
		final Document document = DocumentHelper.createDocument();
		final Element rootElement = document.addElement(rootName);

		final String baseUrl = SuperContainer.BaseUrl;
		// Tab
		for (Tab tab : tabManager.getShownTabs()) {
			Element urlElement = rootElement.addElement("url");
			urlElement.addElement("lastmod").addText(new SimpleDateFormat(dtFormat).format(tab.getUpdated()));
			urlElement.addElement("loc").addText(tab.getMyUrl());
			urlElement.addElement("changefreq").addText("weekly"); // tab is weekly
		}

		// Category
		for (Category category : categoryManager.getShownAll()) {
			Element urlElement = rootElement.addElement("url");
			urlElement.addElement("loc").addText(category.getMyUrl());
			urlElement.addElement("lastmod").addText(new SimpleDateFormat(dtFormat).format(category.getUpdated()));
			urlElement.addElement("changefreq").addText("weekly"); // category is weekly
		}

		// Item
		for (Item item : itemManager.getShownAll()) {
			Element urlElement = rootElement.addElement("url");
			urlElement.addElement("loc").addText(item.getMyUrl());
			urlElement.addElement("lastmod").addText(new SimpleDateFormat(dtFormat).format(item.getUpdated()));
			urlElement.addElement("changefreq").addText("daily"); // item is weekly
			urlElement.addElement("priority").addText("0.8"); // higher priority
		}

		// Article
		for (Article article : articleManager.getShownAll()) {
			Element urlElement = rootElement.addElement("url");
			urlElement.addElement("loc").addText(article.getMyUrl());
			urlElement.addElement("lastmod").addText(new SimpleDateFormat(dtFormat).format(article.getUpdated()));
			urlElement.addElement("changefreq").addText("daily"); // item is weekly
			urlElement.addElement("priority").addText("0.8"); // higher priority
		}

		// Membership
		Element urlElement = rootElement.addElement("url");
		urlElement.addElement("loc").addText(String.format("%s/membership/%s", baseUrl, "forget-password.html"));
		urlElement.addElement("changefreq").addText("monthly");

		urlElement = rootElement.addElement("url");
		urlElement.addElement("loc").addText(String.format("%s/membership/%s", baseUrl, "login.html"));
		urlElement.addElement("changefreq").addText("monthly");

		urlElement = rootElement.addElement("url");
		urlElement.addElement("loc").addText(String.format("%s/membership/%s", baseUrl, "create-user.html"));
		urlElement.addElement("changefreq").addText("monthly");

		// Write to response now
		String result = GeneralUtils.toString(document, false);
		final Writer w = response.getWriter();
		w.write(result);
		w.close();

		// Result
		return null;
	}

	@RequestMapping("/robots.txt")
	public ModelAndView handleRobotsRequest(HttpServletRequest request, HttpServletResponse response) throws Exception {
		log.debug("in handleRobotsRequest()...");

		response.setContentType("text/plain");
		response.setCharacterEncoding("utf-8");

		final String eol = System.getProperty("line.separator");
		String baseUrl = ServletUtils.getBaseUrl(request);

		StringBuilder sb = new StringBuilder();
		sb.append("User-agent: *" + eol);
		sb.append("Disallow: /images" + eol);
		sb.append("Disallow: /admin" + eol);
		sb.append("Disallow: /customer" + eol);
		sb.append("Allow: /tab" + eol);
		sb.append("Allow: /category" + eol);
		sb.append("Allow: /item" + eol);
		sb.append("Sitemap: " + baseUrl + "/sitemap.xml" + eol);

		// Write to response
		final Writer w = response.getWriter();
		w.write(sb.toString());
		w.close();

		return null;
	}
}
