package com.beelun.shoppro.web.fieldBinding;

import java.beans.PropertyEditorSupport;

import com.beelun.shoppro.model.type.ShipTimeEnum;

/**
 * Conversion between Text and ShipTimeEnum
 * 
 * @author bali
 *
 */
public class ShipTimePropertyEditor extends PropertyEditorSupport {

	public void setAsText(String incomming) {		
		setValue(ShipTimeEnum.valueOf(incomming));
	}
	
	public String getAsText() {
		return ((ShipTimeEnum)getValue()).toString();
	}
	
}
