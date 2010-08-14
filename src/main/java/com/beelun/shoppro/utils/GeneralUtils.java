package com.beelun.shoppro.utils;

import java.io.BufferedReader;
import java.io.ByteArrayOutputStream;
import java.io.FileReader;
import java.io.IOException;
import java.io.StringReader;
import java.io.StringWriter;

import javax.xml.bind.JAXBContext;
import javax.xml.bind.Marshaller;
import javax.xml.bind.Unmarshaller;
import javax.xml.transform.stream.StreamSource;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.dom4j.Document;
import org.dom4j.io.OutputFormat;
import org.dom4j.io.XMLWriter;

import com.beelun.shoppro.model.Item;

public class GeneralUtils {
	private static final Log log = LogFactory.getLog(GeneralUtils.class);
	/**
	 * org.dom4j.Document -> String
	 * 
	 * @param document
	 * @return
	 * @throws Exception
	 */
	public static String toString(Document document, boolean suppressDeclaration)
			throws Exception {
		try {
			final OutputFormat format = OutputFormat.createPrettyPrint();
			format.setEncoding("UTF-8");
			format.setTrimText(false);
			format.setSuppressDeclaration(suppressDeclaration);
			final StringWriter out = new StringWriter();
			final XMLWriter writer = new XMLWriter(out, format);
			writer.write(document);
			writer.close();
			return out.toString();
		} catch (final Exception e) {
			throw e;
		}
	}

	/**
	 * Compare two float numbers
	 * 
	 * @param f1
	 * @param f2
	 * @return
	 */
	public static boolean floatEqual(float f1, float f2) {
		return Math.abs(f1 - f2) < 0.00001;
	}
	
	/**
	 * Generate descriptive url string
	 * 
	 * Do following:
	 * (0) trim
	 * (1) replace whitespace with "-"
	 * (2) remove characters matching pattern [^0-9a-zA-Z/-] 
	 * (3) remove multiple contiguous letters [/-] 
	 * @param urlString
	 * @return
	 */
	public static String toDescriptiveUrl(String urlString) {
		if(null == urlString) {
			return null;
		}
		
		// (0) Trim
		String s1 = urlString.trim();
		
		// (1)
		String s2 = s1.replace(' ', '-');
		
		// (2)
		String s3 = s2.replaceAll("[^0-9a-zA-Z/-]", "-");
		
		// (3)
		String s4 = s3.replaceAll("[/-]", "-");
		
		return s4;
	}
	
	/**
	 * Item -> Xml
	 * 
	 * @return
	 */
	public static String ConvertItemToXmlString(Item item) {
		try {
			ByteArrayOutputStream stream = new ByteArrayOutputStream();
			JAXBContext jc = JAXBContext.newInstance( com.beelun.shoppro.model.Item.class );
			Marshaller m = jc.createMarshaller();
			m.setProperty( Marshaller.JAXB_FORMATTED_OUTPUT, Boolean.TRUE );
			m.marshal(item, stream );
			return stream.toString("UTF-8");
		} catch (Exception ex) {
			log.error(ex.getMessage() + ex.toString());
		} 
		
		return null;
	}
	
	/**
	 * Xml -> Item
	 * @param xmlString
	 * @return
	 */
	public static Item ConvertXmlStringToItem(String xmlString) {
		try {
			JAXBContext jc = JAXBContext.newInstance( com.beelun.shoppro.model.Item.class );
	        Unmarshaller u = jc.createUnmarshaller();
	        StringBuffer xmlStr = new StringBuffer( xmlString);
	        return (Item)u.unmarshal( new StreamSource( new StringReader( xmlStr.toString() ) ) );
		}catch(Exception ex) {
			log.error(ex.getMessage() + ex.toString());
        }
        return null;
	}
	
	/**
	 * Read file content out
	 * 
	 * @param filename
	 * @return
	 * @throws IOException
	 */
	public static String readFile(String filename) throws IOException {
		String lineSep = System.getProperty("line.separator");
		BufferedReader br = new BufferedReader(new FileReader(filename));
		String nextLine = "";
		StringBuffer sb = new StringBuffer();
		while ((nextLine = br.readLine()) != null) {
			sb.append(nextLine);
			//
			// note:
			// BufferedReader strips the EOL character
			// so we add a new one!
			//
			sb.append(lineSep);
		}
		br.close();
		return sb.toString();
	}	
}
