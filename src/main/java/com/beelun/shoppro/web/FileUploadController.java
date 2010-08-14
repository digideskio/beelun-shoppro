package com.beelun.shoppro.web;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.io.Writer;
import java.math.BigDecimal;
import java.nio.charset.Charset;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.apache.commons.lang.RandomStringUtils;
import org.apache.commons.lang.StringUtils;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.validation.BindException;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.servlet.ModelAndView;

import com.beelun.shoppro.Constants;
import com.beelun.shoppro.model.Brand;
import com.beelun.shoppro.model.Category;
import com.beelun.shoppro.model.Item;
import com.beelun.shoppro.model.Media;
import com.beelun.shoppro.model.Tab;
import com.beelun.shoppro.pojo.FileUpload;
import com.beelun.shoppro.service.BrandManager;
import com.beelun.shoppro.service.Category2ItemMapManager;
import com.beelun.shoppro.service.CategoryManager;
import com.beelun.shoppro.service.ItemManager;
import com.beelun.shoppro.service.MediaManager;
import com.beelun.shoppro.service.MyGlobManager;
import com.beelun.shoppro.service.Tab2CategoryMapManager;
import com.beelun.shoppro.service.TabManager;
import com.csvreader.CsvReader;

@Controller
@RequestMapping("/admin/import-items.html")
public class FileUploadController extends BaseFormController {
	private final Log log = LogFactory.getLog(FileUploadController.class);

	@Autowired
	ItemManager itemManager;

	@Autowired
	BrandManager brandManager;

	@Autowired
	MyGlobManager myGlobManager;

	@Autowired
	CategoryManager categoryManager;

	@Autowired
	Category2ItemMapManager category2ItemMapManager;

	@Autowired
	Tab2CategoryMapManager tab2CategoryMapManager;

	@Autowired
	TabManager tabManager;

	@Autowired
	MediaManager mediaManager;

	public FileUploadController() {
		setCommandName("fileUpload");
		setCommandClass(FileUpload.class);
		setFormView("/admin/upload");

		// TODO: show all items
		setSuccessView("redirect:/admin/import-items.html?success=true");
	}

	public ModelAndView onSubmit(HttpServletRequest request, HttpServletResponse response, Object command, BindException errors) throws Exception {
		log.debug("entering 'onSubmit' method...");

		boolean importItems = true;
		boolean fromSL = true;
		if (null == request.getParameter("fromSL")) {
			fromSL = false;
		}
		if (null == request.getParameter("importItems")) {
			importItems = false;
		}

		log.debug("fromSL: " + fromSL);
		FileUpload fileUpload = (FileUpload) command;

		// When size size exceeds limits
		if (fileUpload.getFile() == null) {
			Object[] args = new Object[] { getText("importItemsForm.file", request.getLocale()), "8MB" };
			if (fromSL) {
				this.sendResponseToSL(response, getText("errors.maxFileSize", args, request.getLocale()));
				return null;
			} else {
				errors.rejectValue("file", "errors.maxFileSize", args, "File");
				return showForm(request, response, errors);
			}
		}

		// validate a file was entered
		if (fileUpload.getFile().getSize() == 0) {
			Object[] args = new Object[] { getText("importItemsForm.file", request.getLocale()) };
			if (fromSL) {
				this.sendResponseToSL(response, getText("errors.required", args, request.getLocale()));
				return null;
			} else {
				errors.rejectValue("file", "errors.required", args, "File");
				return showForm(request, response, errors);
			}
		}

		// Get input stream
		InputStream inputStream = fileUpload.getFile().getInputStream();

		ModelAndView mv = null;
		if (importItems == true) {
			// Import items
			mv = handleItemImport(request, response, inputStream, errors, fromSL);
		} else {
			// Upload media and save the file to '/uploads' folder
			mv = handleMediaUpload(request, response, inputStream, fileUpload.getFile().getName(), errors);
		}

		inputStream.close();
		return mv;
	}

	/**
	 * (Obsolete)
	 * A media(image, video, or file) is uploaded to the site.
	 * 
	 * @param request
	 * @param response
	 * @param inputStream
	 * @param fileName
	 * @param errors
	 * @return
	 */
	private ModelAndView handleMediaUpload(HttpServletRequest request, HttpServletResponse response, InputStream inputStream, String fileName, BindException errors) {
		// Get parameters
		String title = request.getParameter("title");
		String caption = request.getParameter("caption");
		String description = request.getParameter("description");

		// Add a random string to file name to avoid naming conflicts
		String randomString = RandomStringUtils.random(16, true, true);
		String newFileName = randomString + fileName;

		// Do verifications.
		Media media = new Media(newFileName, title, caption, description);
		// if(!mediaManager.verifyConstaint(media)) {
		// this.sendResponseToSL(response, "Please meet below requirements and then retry: (1) fileName is unqiue, (2) title is not null");
		// return null;
		// }

		// Get upload directory
		String directoryRealPath = getServletContext().getRealPath("/WEB-INF/web.xml");

		try {
			// Save file now
			File f = new File(directoryRealPath + "/uploads/" + newFileName);
			OutputStream outputStream = new FileOutputStream(f);
			byte buf[] = new byte[1024]; // 1 KB buffer
			int len;
			while ((len = inputStream.read(buf)) > 0) {
				outputStream.write(buf, 0, len);
			}
			outputStream.close();
		} catch (Exception ex) {
			log.error("Error during upload media file. Msg: " + ex.getMessage());
			Object[] args = new Object[] { getText("importItemsForm.file", request.getLocale()) };
			this.sendResponseToSL(response, getText("errors.invalid", args, request.getLocale()));
			return null;
		}

		// Save the media now
		Media savedMedia = mediaManager.save(media);

		// Return success
		this.sendResponseToSL(response, "success:" + savedMedia.getId());
		return null;
	}

	/**
	 * Handle items import
	 * 
	 * @param request
	 * @param response
	 * @param inputStream
	 * @param errors
	 * @param fromSL
	 * @return
	 * @throws Exception
	 */
	private ModelAndView handleItemImport(HttpServletRequest request, HttpServletResponse response, InputStream inputStream, BindException errors, boolean fromSL) throws Exception {
		try {
			// Get InputStream and CsvReader
			CsvReader csvReader = new CsvReader(inputStream, ',', Charset.forName("UTF-8"));

			// Get product tab
			Tab productTab = tabManager.get(Constants.PRODUCT_TAB_ID);

			// Save the items one by one
			csvReader.readHeaders();
			while (csvReader.readRecord()) {
				Long nid = Long.parseLong(StringUtils.trim((csvReader.get("Internal ID")))); // required
				log.debug("nid: " + nid);
				Item item = itemManager.getItemByNID(nid);
				if (null == item) {
					item = new Item();
				}

				item.setNetSuiteId(nid);
				String isShowString = csvReader.get("Display in Web Site");
				if (!StringUtils.isBlank(isShowString) && (isShowString.equalsIgnoreCase("yes") || isShowString.equalsIgnoreCase("true"))) {
					item.setIsShown(true);
				} else {
					item.setIsShown(false);
				}
				item.setName(StringUtils.trim(csvReader.get("Store Display Name")));
				item.setShortDescription(csvReader.get("Store Description"));
				item.setDetailedDescription(csvReader.get("Detailed Description"));
				item.setImage(csvReader.get("Image URL"));
				item.setThumbNail(csvReader.get("Thumbnail URL"));
				if (!StringUtils.isBlank(csvReader.get("List Price"))) {
					item.setListPrice(new BigDecimal(csvReader.get("List Price")));
				}
				if (!StringUtils.isBlank(csvReader.get("Base Price"))) {
					item.setSellPrice(new BigDecimal(csvReader.get("Base Price")));
				} else {
					// If there is no sell price, this item won't show
					item.setIsShown(false);
				}

				if (!StringUtils.isBlank(csvReader.get("On Hand"))) {
					item.setInventoryNumber(Long.parseLong(StringUtils.trim(csvReader.get("On Hand"))));
				}
				item.setPageTitle(csvReader.get("Page Title"));
				item.setKeywords(csvReader.get("Search Keywords"));
				item.setMetaTag(csvReader.get("Meta Tag Html"));
				item.setUrl(csvReader.get("URL Component"));

				// Brand(or manufacturer in NetSuite) is a FK in item table, so
				// we need check it existence first.
				// If not exists, a new record will be created
				if (!StringUtils.isBlank(csvReader.get("Manufacturer"))) {
					Brand brand = brandManager.getOrCreateByName(StringUtils.trim(csvReader.get("Manufacturer")));
					item.setBrand(brand);
				} else {
					item.setBrand(brandManager.getNoBrand());
				}

				// Save this item
				Item newItem = itemManager.save(item);

				// Link this item to the default category by brand, if not
				Category c = categoryManager.getOrCreateCategoryFromBrand(item.getBrand());
				category2ItemMapManager.createMapIfNotExists(c, newItem);

				// This category will be listed to the product tab, if not
				tab2CategoryMapManager.createMapIfNotExists(productTab, c);
			}

			// Hide category which doesn't have shown items
			for (Category c1 : categoryManager.getShownAll()) {
				if (null == itemManager.getShown(c1) || itemManager.getShown(c1).size() == 0) {
					c1.setIsShown(false);
					categoryManager.save(c1);
				}
			}
		} catch (Exception ex) {
			log.error("Error during import items file. Msg: " + ex.getMessage());
			Object[] args = new Object[] { getText("importItemsForm.file", request.getLocale()) };
			if (fromSL) {
				this.sendResponseToSL(response, getText("errors.invalid", args, request.getLocale()));
				return null;
			} else {
				errors.rejectValue("file", "errors.invalid", args, "File");
				return showForm(request, response, errors);
			}
		}

		if (fromSL) {
			this.sendResponseToSL(response, "success");
			return null;
		} else {
			return new ModelAndView(getSuccessView());
		}
	}

	/**
	 * Send response to SL
	 * 
	 * @param response
	 * @param msg
	 */
	private void sendResponseToSL(HttpServletResponse response, String msg) {
		response.setContentType("text/xml");
		response.setCharacterEncoding("utf-8");
		Writer w;
		try {
			w = response.getWriter();
			w.write(msg);
			w.close();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		response.setStatus(200);
	}

	/**
	 * Add site type to the ftl
	 */
	protected ModelAndView showForm(HttpServletRequest request, HttpServletResponse response, BindException errors) throws Exception {
		log.debug("in showForm() to add site tyle, etc.");

		String siteType = myGlobManager.fetch().getSiteType().name();
		log.debug("siteType: " + siteType);

		ModelAndView mv = super.showForm(request, response, errors);
		mv.addObject("siteType", siteType);
		return mv;
	}
}
