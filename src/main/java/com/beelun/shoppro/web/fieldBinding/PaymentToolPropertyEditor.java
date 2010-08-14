package com.beelun.shoppro.web.fieldBinding;

import java.beans.PropertyEditorSupport;

import org.hibernate.SessionFactory;

import com.beelun.shoppro.dao.hibernate.GenericDaoHibernate;
import com.beelun.shoppro.model.PaymentTool;

/**
 * For class 'PaymentTool'
 *
 * @author <a href="mailto:bill@beelun.com">Bill Li</a>
 *
 */
public class PaymentToolPropertyEditor extends PropertyEditorSupport {
	private GenericDaoHibernate<PaymentTool, Long> gdh = null;
	
	public PaymentToolPropertyEditor(SessionFactory sessionFactory) {
		gdh = new GenericDaoHibernate<PaymentTool, Long>(PaymentTool.class, sessionFactory);
	}
	
	public void setAsText(String incomming) {
		PaymentTool paymentTool = (PaymentTool) gdh.get(Long.parseLong(incomming));
		setValue(paymentTool);
	}
	public String getAsText() {
		return ((PaymentTool)getValue()).getId().toString();
	}

}
