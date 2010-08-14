package com.beelun.shoppro.pojo;

import java.io.Serializable;
import java.math.BigDecimal;
import java.util.Date;

/**
 * Chart data point model. Used for transferring data from backend to front end
 * 
 * @author bill
 *
 */
public class ChartDataPointModel implements Serializable {
	// This class will be mapped to front end. So it must implement Serializable class
	private static final long serialVersionUID = 7648576509109134884L;
	
	/**
	 * X
	 */
	private int xInt;
	private Date xDate;
	private String xString;
	
	/**
	 * Y
	 */
	private BigDecimal y;
	
	/**
	 * Default constructor
	 */
	public ChartDataPointModel() {}
	
	public ChartDataPointModel(int x, BigDecimal y) {
		this.xInt = x;
		this.y = y;
	}
	
	public ChartDataPointModel(Date x, BigDecimal y) {
		this.xDate = x;
		this.y = y;
	}

	public ChartDataPointModel(String x, BigDecimal y) {
		this.xString = x;
		this.y = y;
	}
	
	public BigDecimal getY() {
		return y;
	}
	public void setY(BigDecimal y) {
		this.y = y;
	}


	public int getxInt() {
		return xInt;
	}


	public void setxInt(int xInt) {
		this.xInt = xInt;
	}


	public Date getxDate() {
		return xDate;
	}


	public void setxDate(Date xDate) {
		this.xDate = xDate;
	}


	public String getxString() {
		return xString;
	}

	public void setxString(String xString) {
		this.xString = xString;
	}
	
}
