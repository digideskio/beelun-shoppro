package com.beelun.shoppro.service;

import com.beelun.shoppro.model.PaypalAccessInfo;

/**
 * Manager to access paypal API access info
 * @author bali
 *
 */
public interface PaypalAccessInfoManager {
	public PaypalAccessInfo fetch();
	public PaypalAccessInfo save(PaypalAccessInfo paypalAccessInfo);
}
