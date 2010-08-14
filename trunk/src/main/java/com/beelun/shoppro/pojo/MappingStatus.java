package com.beelun.shoppro.pojo;

/**
 * A class representing mapping status of list of items(to category) or categories(to tab)
 * 
 * @author bill
 *
 */
public class MappingStatus {
	private MappingStatusEnum mappingStatus;
	
	/**
	 * Id of tab(in case of tab2category mapping) or category(in case of category2item mapping)
	 */
	private Long id;
	
	/**
	 * Name of tab(in case of tab2category mapping) or category(in case of category2item mapping)
	 */
	private String name;

	public Long getId() {
		return id;
	}
	public void setId(Long id) {
		this.id = id;
	}
	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
	}
	public MappingStatusEnum getMappingStatus() {
		return mappingStatus;
	}
	public void setMappingStatus(MappingStatusEnum mappingStatus) {
		this.mappingStatus = mappingStatus;
	}
}
