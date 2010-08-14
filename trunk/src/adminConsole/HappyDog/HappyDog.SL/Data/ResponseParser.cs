using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Linq;

using HappyDog.SL.Resources;
using HappyDog.SL.Common;

namespace HappyDog.SL.Data
{
    /// <summary>
    /// All response from backend should use this class for parsing response
    /// </summary>
    public class ResponseParser
    {
        public XElement GetResultXElement(string resXmlString)
        {
            try
            {
                XElement xe = XElement.Parse(resXmlString);
                ResponseStatusCode code = (ResponseStatusCode)Convert.ToInt32(xe.Element("Status").Element("Code").Value);
                if(ResponseStatusCode.OK == code)
                {
                    return xe.Element("Result");
                }
                else
                {
                    switch (code)
                    { 
                        case ResponseStatusCode.BadRequest:
                            throw new ErrorResponseExp(UIResources.ERROR_400_BadRequest);
                            
                        case ResponseStatusCode.Conflict:
                            throw new ErrorResponseExp(UIResources.ERROR_409_Conflict);
                            
                        case ResponseStatusCode.IntertalServerError:
                            throw new ErrorResponseExp(UIResources.ERROR_500_IntertalServerError);
                            
                        case ResponseStatusCode.NotFound:
                            throw new ErrorResponseExp(UIResources.ERROR_404_NotFound);
                            
                        case ResponseStatusCode.Unauthorized:
                            throw new ErrorResponseExp(UIResources.ERROR_401_Unauthorized);
                            
                        default:
                            // Should never go here
                            throw new Exception();
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }

    /// <summary>
    /// Error response exception
    /// </summary>
    public class ErrorResponseExp : HDException
    {
        public ErrorResponseExp() { }
        public ErrorResponseExp(string message) : base(message) { }
        public ErrorResponseExp(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Refer to: org.openbravo.service.web.BaseWebServiceServlet.doService
    /// </summary>
    public enum ResponseStatusCode
    {
        OK = 200,                       // for successfull requests 
        BadRequest = 400,               // in case the uri could not be parsed or in case of invalid xml 
        Unauthorized = 401,             // is returned when a security exception occurs 
        NotFound = 404,                 // if the entityname does not exist or the if an individual business object is addressed, if the business object does not exist.
        Conflict = 409,                 // is returned for a POST or PUT action, specifies that the data which was posted is out-of-date or invalid. 
        IntertalServerError = 500       // if an unrecoverable application error occured. 
    }
}
