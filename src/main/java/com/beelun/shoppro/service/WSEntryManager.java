package com.beelun.shoppro.service;

import java.util.Date;
import java.util.List;

import javax.jws.WebParam;
import javax.jws.WebService;

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

@WebService(name="WSEntryManager")
public interface WSEntryManager {
	//
	// Item manager
	//
	public Item save_Item(@WebParam(name="item") Item item);
	public boolean removeMany_Item(@WebParam(name="idList") Long[] idList);
	public List<Item> getByCondition_Item(@WebParam(name="orderBy") String orderBy, @WebParam(name="ascending") boolean ascending, @WebParam(name="firstResult") int firstResult, @WebParam(name="maxResult") int maxResult, @WebParam(name="categoryId") Long categoryId);
	public int getCountByCondition_Item(@WebParam(name="categoryId") Long categoryId);
	public List<Item> searchByText_Item(@WebParam(name="text") String text, @WebParam(name="firstResult") int firstResult, @WebParam(name="maxResult") int maxResult);
	public int searchByTextCount_Item(@WebParam(name="text") String text);
	public int getAllCount_Item();
	public List<MappingStatus> getMappingStatus_Item(@WebParam(name="idList") List<Long> idList);
	public boolean setMappingStatus_Item(@WebParam(name="itemIdList") List<Long> itemIdList, @WebParam(name="categoryId") Long categoryId, @WebParam(name="mappingStatus") MappingStatusEnum mappingStatus);

	//
	// Category manager
	//
	public Category save_Category(@WebParam(name="category") Category category);
	public boolean removeMany_Category(@WebParam(name="idList") Long[] idList);
	public List<Category> getByCondition_Category(@WebParam(name="orderBy") String orderBy, @WebParam(name="ascending") boolean ascending, @WebParam(name="firstResult") int firstResult, @WebParam(name="maxResult") int maxResult);
	public List<MappingStatus> getMappingStatus_Category(@WebParam(name="idList") List<Long> idList);
	public boolean setMappingStatus_Category(@WebParam(name="categoryIdList") List<Long> categoryIdList, @WebParam(name="tabId") Long tabId, @WebParam(name="mappingStatus") MappingStatusEnum mappingStatus);	
	public List<Category> getAll_Category();

	//
	// Tab manager
	//
	public List<Tab> getAll_Tab(); 			// Get all tabs in the db
	public Tab save_Tab(@WebParam(name="tab") Tab tab);
	public boolean removeMany_Tab(@WebParam(name="idList") Long[] idList);
	
	//
	// Article
	//
	public Article save_Article(@WebParam(name="article") Article article);
	public boolean removeMany_Article(@WebParam(name="idList") Long[] idList); 
	public List<Article> getAll_Article();
	
	//
	// Brand
	//
	public Brand save_Brand(@WebParam(name="brand") Brand brand);
	public boolean removeMany_Brand(@WebParam(name="idList") Long[] idList);
	public List<Brand> getAll_Brand();
	
	//
	// Media
	//
	public Media save_Media(@WebParam(name="media") Media media);
	public Media createNew_Media(@WebParam(name="media") Media media, @WebParam(name="content") byte[] content);
	public boolean removeMany_Media(@WebParam(name="mediaIdList") Long[] mediaIdList);
	public List<Media> getByCondition_Media(@WebParam(name="orderBy") String orderBy, @WebParam(name="ascending") boolean ascending, @WebParam(name="firstResult") int firstResult, @WebParam(name="maxResult") int maxResult);
	public int getAllCount_Media();
	public List<Media> searchByText_Media(@WebParam(name="text") String text, @WebParam(name="firstResult") int firstResult, @WebParam(name="maxResult") int maxResult);
	public int searchByTextCount_Media(@WebParam(name="text") String text);
	
	//
	// User
	//
	public List<User> getByCondition_User(@WebParam(name="orderBy") String orderBy, @WebParam(name="ascending") boolean ascending, @WebParam(name="firstResult") int firstResult, @WebParam(name="maxResult") int maxResult);
	public int getAllCount_User();
	public User getByEmail_User(@WebParam(name="userEmail") String userEmail);
	public boolean resetPassword_User(@WebParam(name="userId") Long userId, @WebParam(name="newPasswordInPlainText") String newPasswordInPlainText);
	public boolean enableDisableUserAccess_User(@WebParam(name="userId") Long userId, @WebParam(name="isEnabled") boolean isEnabled);
	public User createNewAdmin_User(@WebParam(name="userName") String userName, @WebParam(name="email") String email, @WebParam(name="password") String password);
	//
	// Order
	//
	public Order save_Order(@WebParam(name="order") Order order);
	public List<Order> getOrderListByStatus_Order(@WebParam(name="orderStatus") OrderStatusEnum orderStatus);
	public List<Order> getByCondition_Order(@WebParam(name="orderBy") String orderBy, @WebParam(name="ascending") boolean ascending, @WebParam(name="firstResult") int firstResult, @WebParam(name="maxResult") int maxResult);
	public int getAllCount_Order();
	public Order getByOrderNo_Order(@WebParam(name="orderNo") String orderNo);
	public Order changeOrderStatus_Order(@WebParam(name="orderId") Long orderId, @WebParam(name="newOrderStatus") OrderStatusEnum newStatus);

	//
	// Template
	//
	public Template save_Template(@WebParam(name="template") Template template);
	public List<Template> getByType_Template(@WebParam(name="templateType") TemplateTypeEnum templateType);
	public List<Template> getAll_Template();
	
	//
	// MyGlob
	//
	public MyGlob fetch_MyGlob();
	public MyGlob save_MyGlob(@WebParam(name="myGlob") MyGlob myGlob);
	
	//
	// Paypal
	//
	public PaypalAccessInfo fetch_Paypal();
	public PaypalAccessInfo save_Paypal(@WebParam(name="paypalAccessInfo") PaypalAccessInfo paypalAccessInfo);
		
	//
	// Stat
	//
	public List<ChartDataPointModel> getSalesByBrand(@WebParam(name="specifiedTime") Date specifiedTime, @WebParam(name="accuracy") TimeAccuracyEnum accuracy);
	public List<ChartDataPointModel> getNewUserTrend(@WebParam(name="specifiedStartingTime") Date specifiedStartingTime, @WebParam(name="timeAccuracy") TimeAccuracyEnum timeAccuracy, @WebParam(name="maxCount") int maxCount);
	
}
