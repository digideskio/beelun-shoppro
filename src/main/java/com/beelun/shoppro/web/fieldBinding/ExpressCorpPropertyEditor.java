package com.beelun.shoppro.web.fieldBinding;

import java.beans.PropertyEditorSupport;

import org.hibernate.SessionFactory;

import com.beelun.shoppro.dao.hibernate.GenericDaoHibernate;
import com.beelun.shoppro.model.ExpressCorp;

/**
 * For class 'ExpressCorp'
 * Refer to:
 * http://stackoverflow.com/questions/516670/spring-binding-to-an-object-rather-than-a-string-or-primitive
 *
 * @author <a href="mailto:bill@beelun.com">Bill Li</a>
 *
 */
public class ExpressCorpPropertyEditor extends PropertyEditorSupport {
	private GenericDaoHibernate<ExpressCorp, Long> gdh = null;
	
	public ExpressCorpPropertyEditor(SessionFactory sessionFactory) {
		gdh = new GenericDaoHibernate<ExpressCorp, Long>(ExpressCorp.class, sessionFactory);
	}
	
	public void setAsText(String incomming) {
		ExpressCorp expressCorp = (ExpressCorp) gdh.get(Long.parseLong(incomming));
		setValue(expressCorp);
	}
	public String getAsText() {
		return ((ExpressCorp)getValue()).getId().toString();
	}
}
