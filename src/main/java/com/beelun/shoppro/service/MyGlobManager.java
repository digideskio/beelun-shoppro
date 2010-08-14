package com.beelun.shoppro.service;

import com.beelun.shoppro.model.MyGlob;

public interface MyGlobManager {
	public MyGlob fetch();
	public MyGlob save(MyGlob myGlob);
}
