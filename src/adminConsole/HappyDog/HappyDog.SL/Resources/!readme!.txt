 Make sure set class UIResources as public after changing the resource files like following:
 
 public class UIResources {
 
 public UIResources() 


Otherwise, you might encounter errors like following:
---
System.Windows.Markup.XamlParseException occurred
  Message="AG_E_PARSER_UNKNOWN_TYPE [Line: 10 Position: 33]"
  LineNumber=10
  LinePosition=33
  StackTrace:
       位于 System.Windows.Application.LoadComponent(Object component, Uri resourceLocator)
       位于 HappyDog.SL.Controls.Header.InitializeComponent()
       位于 HappyDog.SL.Controls.Header..ctor()
  InnerException: 
---