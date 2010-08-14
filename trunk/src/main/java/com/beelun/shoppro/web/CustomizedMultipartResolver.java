package com.beelun.shoppro.web;

import java.util.Collections;
import java.util.List;

import javax.servlet.http.HttpServletRequest;

import org.apache.commons.fileupload.FileUpload;
import org.apache.commons.fileupload.FileUploadBase;
import org.apache.commons.fileupload.FileUploadException;
import org.apache.commons.fileupload.servlet.ServletFileUpload;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.springframework.web.multipart.MultipartException;
import org.springframework.web.multipart.commons.CommonsMultipartResolver;

/**
 * Check upload file size here. Catch the exception as necessary, so that UI will get a nice error message instead of a 500 page.
 * @author bali
 *
 */
public class CustomizedMultipartResolver extends CommonsMultipartResolver {
	private final Log log = LogFactory.getLog(UserFormController.class);
	
    /**
     * Parse the given servlet request, resolving its multipart elements.
     *
     * @param request the request to parse
     * @return the parsing result
     * @throws MultipartException 
     */
    @SuppressWarnings("unchecked")
	@Override
    protected MultipartParsingResult parseRequest(final HttpServletRequest request) throws MultipartException {
        String encoding = determineEncoding(request);
        FileUpload fileUpload = prepareFileUpload(encoding);

        List fileItems = Collections.EMPTY_LIST;

        try {
            fileItems = ((ServletFileUpload) fileUpload).parseRequest(request);
        } catch (FileUploadBase.SizeLimitExceededException ex) {
        	log.warn("file size exceeds limit.");
            fileItems = Collections.EMPTY_LIST;
        } catch (FileUploadException ex) {
            throw new MultipartException("Could not parse multipart servlet request", ex);
        }

        return parseFileItems(fileItems, encoding);
    }

}
