package com.beelun.shoppro.web.fieldBinding;

import java.beans.PropertyEditorSupport;
import com.beelun.shoppro.model.type.ShipDateEnum;

/**
 * Conversion between Text and ShipDateEnum
 * 
 * @author bali
 *
 */
public class ShipDatePropertyEditor extends PropertyEditorSupport {
	
	public void setAsText(String incomming) {		
		setValue(ShipDateEnum.valueOf(incomming));
	}
	
	public String getAsText() {
		return ((ShipDateEnum)getValue()).toString();
	}
}
