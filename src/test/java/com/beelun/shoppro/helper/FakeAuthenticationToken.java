package com.beelun.shoppro.helper;

import java.io.Serializable;
import java.security.Principal;

import org.springframework.security.Authentication;
import org.springframework.security.GrantedAuthority;
import org.springframework.security.userdetails.UserDetails;

/**
 * A helper class to simulate TestingAuthenticationToken, which will be available in 3.0.0.M1 and above.
 *
 * @author <a href="mailto:bill@beelun.com">Bill Li</a>
 *
 */
public class FakeAuthenticationToken implements Authentication, Principal,
		Serializable {
	private static final long serialVersionUID = 354768603444620998L;

	UserDetails userDetails;
	GrantedAuthority[] grantedAuthorities;
	
	public FakeAuthenticationToken(UserDetails userDetails, String password, GrantedAuthority[] grantedAuthorities)
	{
		this.userDetails = userDetails;
		this.grantedAuthorities = grantedAuthorities;
	}
	
	@Override
	public GrantedAuthority[] getAuthorities() {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public Object getCredentials() {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public Object getDetails() {
		// TODO Auto-generated method stub
		return null;
	}

	@Override
	public Object getPrincipal() {
		return this.userDetails;
	}

	@Override
	public boolean isAuthenticated() {
		// TODO Auto-generated method stub
		return false;
	}

	@Override
	public void setAuthenticated(boolean arg0) throws IllegalArgumentException {
		// TODO Auto-generated method stub

	}

	@Override
	public String getName() {
		// TODO Auto-generated method stub
		return null;
	}

}
