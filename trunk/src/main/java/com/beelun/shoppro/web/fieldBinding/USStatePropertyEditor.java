package com.beelun.shoppro.web.fieldBinding;

import java.beans.PropertyEditorSupport;
import com.beelun.shoppro.model.type.USStateEnum;

/**
 * UI <--> Enum converter
 * 
 * @author bali
 *
 */
public class USStatePropertyEditor extends PropertyEditorSupport {
	public void setAsText(String incomming) {		
		setValue(USStateEnum.valueOf(incomming));
	}
	
	public String getAsText() {
		return ((USStateEnum)getValue()).toString();
	}

}
