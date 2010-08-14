package com.beelun.shoppro.service.impl;

import java.util.Date;
import java.util.List;

import javax.jws.WebService;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.beelun.shoppro.model.Article;
import com.beelun.shoppro.model.Brand;
import com.beelun.shoppro.model.Category;
import com.beelun.shoppro.model.Item;
import com.beelun.shoppro.model.Media;
import com.beelun.shoppro.model.MyGlob;
import com.beelun.shoppro.model.Order;
import com.beelun.shoppro.model.PaypalAccessInfo;
import com.beelun.shoppro.model.Tab;
import com.beelun.shoppro.model.Template;
import com.beelun.shoppro.model.User;
import com.beelun.shoppro.model.type.OrderStatusEnum;
import com.beelun.shoppro.model.type.TemplateTypeEnum;
import com.beelun.shoppro.pojo.ChartDataPointModel;
import com.beelun.shoppro.pojo.MappingStatus;
import com.beelun.shoppro.pojo.MappingStatusEnum;
import com.beelun.shoppro.pojo.TimeAccuracyEnum;
import com.beelun.shoppro.service.ArticleManager;
import com.beelun.shoppro.service.BrandManager;
import com.beelun.shoppro.service.CategoryManager;
import com.beelun.shoppro.service.ItemManager;
import com.beelun.shoppro.service.MediaManager;
import com.beelun.shoppro.service.MyGlobManager;
import com.beelun.shoppro.service.OrderManager;
import com.beelun.shoppro.service.PaypalAccessInfoManager;
import com.beelun.shoppro.service.RoleManager;
import com.beelun.shoppro.service.TabManager;
import com.beelun.shoppro.service.TemplateManager;
import com.beelun.shoppro.service.UserManager;
import com.beelun.shoppro.service.WSEntryManager;

@Service(value = "wsEntryManager")
@WebService(name="WSEntryManager")
public class WSEntryManagerImpl implements WSEntryManager {
	@Autowired
	ItemManager itemManager;

	@Autowired
	TabManager tabManager;

	@Autowired
	CategoryManager categoryManager;
	
	@Autowired
	MediaManager mediaManager;

	@Autowired
	MyGlobManager myGlobManager;

	@Autowired
	OrderManager orderManager;

	@Autowired
	UserManager userManager;

	@Autowired
	ArticleManager articleManager;

	@Autowired
	BrandManager brandManager;

	@Autowired
	RoleManager roleManager;

	@Autowired
	PaypalAccessInfoManager paypalInfoManager;

	@Autowired
	TemplateManager templateManager;

	@Override
	public Media createNew_Media(Media media, byte[] content) {
		return mediaManager.createNew(media, content);
	}

	@Override
	public MyGlob fetch_MyGlob() {
		return myGlobManager.fetch();
	}

	@Override
	public PaypalAccessInfo fetch_Paypal() {
		return paypalInfoManager.fetch();
	}

	@Override
	public int getAllCount_Order() {
		return orderManager.getAllCount();
	}

	@Override
	public int getAllCount_Item() {
		return itemManager.getAllCount();
	}

	@Override
	public int getAllCount_Media() {
		return mediaManager.getAllCount();
	}

	@Override
	public int getAllCount_User() {
		return userManager.getAllCount();
	}

	@Override
	public List<Article> getAll_Article() {
		return articleManager.getAll();
	}

	@Override
	public List<Brand> getAll_Brand() {
		return brandManager.getAll();
	}

	@Override
	public List<Category> getAll_Category() {
		return categoryManager.getAll();
	}

	@Override
	public List<Tab> getAll_Tab() {
		return tabManager.getAll();
	}

	@Override
	public List<Order> getByCondition_Order(String orderBy, boolean ascending, int firstResult, int maxResult) {
		return orderManager.getByCondition(orderBy, ascending, firstResult, maxResult);
	}

	@Override
	public List<Category> getByCondition_Category(String orderBy,
			boolean ascending, int firstResult, int maxResult) {
		return categoryManager.getByCondition(orderBy, ascending, firstResult, maxResult);
	}

	@Override
	public List<Item> getByCondition_Item(String orderBy, boolean ascending,
			int firstResult, int maxResult, Long categoryId) {
		return itemManager.getByCondition(orderBy, ascending, firstResult, maxResult, categoryId);
	}

	@Override
	public List<Media> getByCondition_Media(String orderBy, boolean ascending,
			int firstResult, int maxResult) {
		return mediaManager.getByCondition(orderBy, ascending, firstResult, maxResult);
	}

	@Override
	public List<User> getByCondition_User(String orderBy, boolean ascending,
			int firstResult, int maxResult) {
		return userManager.getByCondition(orderBy, ascending, firstResult, maxResult);
	}

	@Override
	public int getCountByCondition_Item(Long categoryId) {
		return itemManager.getCountByCondition(categoryId);
	}

	@Override
	public List<MappingStatus> getMappingStatus_Category(List<Long> idList) {
		return categoryManager.getMappingStatus(idList);
	}

	@Override
	public List<MappingStatus> getMappingStatus_Item(List<Long> idList) {
		return itemManager.getMappingStatus(idList);
	}

	@Override
	public List<ChartDataPointModel> getNewUserTrend(
			Date specifiedStartingTime, TimeAccuracyEnum timeAccuracy,
			int maxCount) {
		return userManager.getNewUserTrend(specifiedStartingTime, timeAccuracy, maxCount);
	}

	@Override
	public List<Order> getOrderListByStatus_Order(OrderStatusEnum orderStatus) {
		return orderManager.getOrderListByStatus(orderStatus);
	}

	@Override
	public List<ChartDataPointModel> getSalesByBrand(Date specifiedTime,
			TimeAccuracyEnum accuracy) {
		return orderManager.getSalesByBrand(specifiedTime, accuracy);
	}

	@Override
	public boolean removeMany_Article(Long[] idList) {
		return articleManager.removeMany(idList);
	}

	@Override
	public boolean removeMany_Brand(Long[] idList) {
		return brandManager.removeMany(idList);
	}

	@Override
	public boolean removeMany_Category(Long[] idList) {
		return categoryManager.removeMany(idList);
	}

	@Override
	public boolean removeMany_Item(Long[] idList) {
		return itemManager.removeItems(idList);
	}

	@Override
	public boolean removeMany_Media(Long[] mediaIdList) {
		return mediaManager.removeMany(mediaIdList);
	}

	@Override
	public boolean removeMany_Tab(Long[] idList) {
		return tabManager.removeMany(idList);
	}

	@Override
	public Order save_Order(Order order) {
		return orderManager.save(order);
	}

	@Override
	public Article save_Article(Article article) {
		return articleManager.save(article);
	}

	@Override
	public Brand save_Brand(Brand brand) {
		return brandManager.save(brand);
	}

	@Override
	public Category save_Category(Category category) {
		return categoryManager.save(category);
	}

	@Override
	public Item save_Item(Item item) {
		return itemManager.save(item);
	}

	@Override
	public Media save_Media(Media media) {
		return mediaManager.save(media);
	}

	@Override
	public MyGlob save_MyGlob(MyGlob myGlob) {
		return myGlobManager.save(myGlob);
	}

	@Override
	public PaypalAccessInfo save_Paypal(PaypalAccessInfo paypalAccessInfo) {
		return paypalInfoManager.save(paypalAccessInfo);
	}

	@Override
	public Tab save_Tab(Tab tab) {
		return tabManager.save(tab);
	}

	@Override
	public int searchByTextCount_Item(String text) {
		return itemManager.searchByTextCount(text);
	}

	@Override
	public int searchByTextCount_Media(String text) {
		return mediaManager.searchByTextCount(text);
	}

	@Override
	public List<Item> searchByText_Item(String text, int firstResult,
			int maxResult) {
		return itemManager.searchByText(text, firstResult, maxResult);		
	}

	@Override
	public List<Media> searchByText_Media(String text, int firstResult,
			int maxResult) {
		return mediaManager.searchByText(text, firstResult, maxResult);
	}

	@Override
	public boolean setMappingStatus_Category(List<Long> categoryIdList,
			Long tabId, MappingStatusEnum mappingStatus) {
		return categoryManager.setMappingStatus(categoryIdList, tabId, mappingStatus);
	}

	@Override
	public boolean setMappingStatus_Item(List<Long> itemIdList,
			Long categoryId, MappingStatusEnum mappingStatus) {
		return itemManager.setMappingStatus(itemIdList, categoryId, mappingStatus);
	}

	@Override
	public User getByEmail_User(String userEmail) {
		return userManager.getUserByEmail(userEmail);
	}

	@Override
	public Order getByOrderNo_Order(String orderNo) {
		return orderManager.getByOrderNo(orderNo);
	}

	@Override
	public Order changeOrderStatus_Order(Long orderId, OrderStatusEnum newStatus) {
		return orderManager.changeOrderStatus(orderId, newStatus);
	}

	@Override
	public boolean enableDisableUserAccess_User(Long userId, boolean isEnabled) {
		return userManager.enableDisableUserAccess(userId, isEnabled);
	}

	@Override
	public boolean resetPassword_User(Long userId, String newPasswordInPlainText) {
		return userManager.resetPassword(userId, newPasswordInPlainText);
	}

	@Override
	public User createNewAdmin_User(String userName, String email, String password) {
		User u = new User(roleManager.getAdminRole());
		u.setName(userName);
		u.setEmail(email);
		u.setPassword(password);
		u.setEnabled(true);
		return userManager.saveUser(u, true);
	}

	@Override
	public List<Template> getByType_Template(TemplateTypeEnum templateType) {
		return templateManager.GetByType(templateType);
	}

	@Override
	public Template save_Template(Template template) {
		return templateManager.save(template);
	}
	
	@Override
	public List<Template> getAll_Template() {
		return templateManager.GetAll();
	}

}
