A Java open source shopping cart framework.

## Features ##
  * A fully featured shopping cart framework
  * SEO friendly
  * Browsing items
  * User management
  * Shopping cart management
  * Order management
  * Email notification
  * Integration with Paypal
  * Campatable with NetSuite item format
  * Admin console written by Sliverlight

## Features in commercial version ##
  * High traffic, multi-threading handling
  * Full text search
  * Single Sign On
  * Promotion management
  * Credit/points management
  * Split of static and dynamic contents
  * Payment gateway integration such as alipay
  * Integration with ERP/CRM required
  * Other customization(price negotiable)


## Recommend running environment ##
**Linux/Tomat/MySQL(Or any compatible stacks)**

## Dependent Frameworks ##
Shoppro depends on several popular standard frameworks.
  * Spring
  * Hibernate
  * Maven
  * Spring security
  * YUI(or JQuery)
  * Silverlight


## Get started ##
  1. Install JDK 6.0+
  1. Install maven 2.2+
  1. Install MySQL 5.0+, and make sure root password is 123456
  1. Check out the full source
  1. Install 3rd party jars. Go to /lib, run **setup.bat**(If you are using non-Windows, look at the file content, and run corresponding command)
  1. Go to root folder, run: **mvn clean jetty:run**
  1. And then you will see the site by visiting: http://localhost:8080
> To see admin console, visit: http://localhost:8080/admin-console.html, in pop up windows, log in with **admin@smc.com/beelun**.