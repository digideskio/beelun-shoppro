package com.beelun.shoppro.model;

import java.io.Serializable;
import java.util.Date;
import java.util.HashSet;
import java.util.Set;

import javax.xml.bind.annotation.XmlTransient;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.security.GrantedAuthority;
import org.springframework.security.userdetails.UserDetails;


/**
 * Customers and admins
 * This class represents the basic "user" object in shoppro that allows for authentication
 * and user management.  It implements Acegi Security's UserDetails interface.
 * 
 * @author bali
 *
 */
public class User extends BaseObject implements Serializable, UserDetails {
	private transient final Log log = LogFactory.getLog(User.class);
	
    private static final long serialVersionUID = 3257568390917667126L;
    private Long id;
    
    // 'name' is a human readable name such as Bill Gates
    private String name;
    
    // 'email' is for username/password validation
    private String email;
    
    private String password;
    private boolean enabled = true;
    private boolean accountExpired = false;
    private boolean accountLocked = false;
    private boolean credentialsExpired = false;
    //private Address address;	// TODO: we may consider add multiple address support for single user
        
    //
    // shippingAddress & billingAddress will save the latest user address
    //
	private Address shippingAddress;
	private Address billingAddress; // us only
	private boolean sameAddress = true;  // us only. true means use shippingAddress as billingAddress		
	private transient Set<Membership> memberships = new HashSet<Membership>();
    private String unlockToken = null; // TODO: add expiration date?
    private String resetPswdToken = null; // DODO: expire?
    private Date createdWhen = new Date();
    private Date lastLogin = new Date();
    
    public String getResetPswdToken() {
		return resetPswdToken;
	}

	public void setResetPswdToken(String resetPswdToken) {
		this.resetPswdToken = resetPswdToken;
	}

	/**
	 * IMPORTANT: default constructor is needed to make CXF(web service thing) working
	 */
	public User() {}
    
    public User(Role role) {    	    
    	Membership m = new Membership();
    	m.setUser(this);
    	m.setRole(role);
    	memberships.add(m);
    }
    
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

	public String getEmail() {
		return email;
	}

	public void setEmail(String email) {
		this.email = email;
	}

	public String getPassword() {
		return password;
	}

	public void setPassword(String password) {
		this.password = password;
	}

	@Override
	public GrantedAuthority[] getAuthorities() {
		GrantedAuthority[] retAuth = getRoles().toArray(new GrantedAuthority[0]);
		if(log.isDebugEnabled()) {
			for(GrantedAuthority auth : retAuth) {
				log.debug(auth.getAuthority());
			}
		}
		return retAuth;
	}

	/**
	 * This is for spring security
	 */
	@Override
	public String getUsername() {
		return this.email;
	}

	@Override
	public boolean isAccountNonExpired() {
		return !this.isAccountExpired();
	}

	@Override
	public boolean isAccountNonLocked() {
		return !this.isAccountLocked();
	}

	@Override
	public boolean isCredentialsNonExpired() {
		return !this.isCredentialsExpired();
	}

	@Override
	public boolean isEnabled() {
		return this.enabled;
	}

	public void setAccountExpired(boolean accountExpired) {
		this.accountExpired = accountExpired;
	}

	public boolean isAccountExpired() {
		return accountExpired;
	}

	public void setAccountLocked(boolean accountLocked) {
		this.accountLocked = accountLocked;
	}

	public boolean isAccountLocked() {
		return accountLocked;
	}

	public void setCredentialsExpired(boolean credentialsExpired) {
		this.credentialsExpired = credentialsExpired;
	}

	public boolean isCredentialsExpired() {
		return credentialsExpired;
	}

	@XmlTransient
	public Set<Membership> getMemberships() {
		return memberships;
	}

	public void setMemberships(Set<Membership> memberships) {
		this.memberships = memberships;
	}

	public void setEnabled(boolean enabled) {
		this.enabled = enabled;
	}
	
	/**
	 * Get all roles this user have
	 * 
	 * @return
	 */
	public Set<Role> getRoles() {
		Set<Role> roleSet = new HashSet<Role>();
		for(Membership membership : this.getMemberships()) {
			roleSet.add(membership.getRole());
		}
		return roleSet;
	}
	
    public boolean equals(Object o) {
        if (this == o) {
            return true;
        }
        if (!(o instanceof User)) {
            return false;
        }

        final User user = (User) o;

        return this.hashCode() == user.hashCode();
    }

    public int hashCode() {
        return (email != null ? email.hashCode() : 0);
    }

    public String toString() {
    	return this.email;
    }

	public void setUnlockToken(String unlockToken) {
		this.unlockToken = unlockToken;
	}

	public String getUnlockToken() {
		return unlockToken;
	}

	public Date getCreatedWhen() {
		return createdWhen;
	}

	public void setCreatedWhen(Date createdWhen) {
		this.createdWhen = createdWhen;
	}

	public Date getLastLogin() {
		return lastLogin;
	}

	public void setLastLogin(Date lastLogin) {
		this.lastLogin = lastLogin;
	}

	public Address getShippingAddress() {
		return shippingAddress;
	}

	public void setShippingAddress(Address shippingAddress) {
		this.shippingAddress = shippingAddress;
	}

	public Address getBillingAddress() {
		return billingAddress;
	}

	public void setBillingAddress(Address billingAddress) {
		this.billingAddress = billingAddress;
	}
	
    public boolean isSameAddress() {
		return sameAddress;
	}

	public void setSameAddress(boolean sameAddress) {
		this.sameAddress = sameAddress;
	}

}
