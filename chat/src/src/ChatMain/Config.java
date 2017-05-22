package ChatMain;

import java.net.Inet4Address;
import java.net.ServerSocket;
import java.net.UnknownHostException;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerConfigurationException;
import javax.xml.transform.TransformerException;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;

import org.w3c.dom.*;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import org.xml.sax.SAXException;

import ChatMain.ChatMain.ReceiveMsg;

import javax.xml.xpath.*;




public class Config {

	public static String Addr = "null";
	public static String Port = "null";
	public static String Local = "null";
	
	public static ServerSocket sSocket = null;
	
	private static String filePath = "";
	
	
	static ReceiveMsg	receiveThread = new ReceiveMsg();
	static boolean		isRun		= true;
	static int			sleepTime	= 300;
	
	public Config() {
		filePath += "Config.xml";
		try {
			
			File f = new File(filePath);
			DocumentBuilderFactory factory = DocumentBuilderFactory
					.newInstance();
			DocumentBuilder builder = factory.newDocumentBuilder();
			Document doc = builder.parse(f);

			NodeList nl = doc.getElementsByTagName("Config");

			for (int i = 0; i < nl.getLength(); i++) {
				Port = doc.getElementsByTagName("Port").item(i)
						.getFirstChild().getNodeValue();
				
				Addr = doc.getElementsByTagName("IP").item(i)
								.getFirstChild().getNodeValue();
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
		
		
		try {
			Local = Inet4Address.getLocalHost().getHostAddress();
			
		} catch (UnknownHostException e) {
			e.printStackTrace();
		}
	}
	
	public static void ChangeIP(String a_IP)
	{
		try {
			
			File f = new File(filePath);
			DocumentBuilderFactory factory = DocumentBuilderFactory
					.newInstance();
			DocumentBuilder builder = factory.newDocumentBuilder();
			Document doc = builder.parse(f);

			NodeList nl = doc.getElementsByTagName("Config");

			for (int i = 0; i < nl.getLength(); i++) {
				doc.getElementsByTagName("IP").item(i).setTextContent(a_IP);
			}
			saveXml(filePath, doc);
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	
	
	public static void ChangeIP2(String a_IP)
	{
		DocumentBuilderFactory factory=DocumentBuilderFactory.newInstance();
        Element root=null;
        try {
            factory.setIgnoringElementContentWhitespace(true);
            
            DocumentBuilder db=factory.newDocumentBuilder();
            Document xmldoc=db.parse(new File(filePath));
            root=xmldoc.getDocumentElement();
            
            /*Element newNode;
            newNode=xmldoc.createElement("book");
            newNode.setTextContent("ok");
           	root.appendChild(newNode);
           	*/
           	//output(xmldoc);
           	
           	System.out.println("???");
           	Element node = (Element) selectSingleNode("/Config/IP", root);
           	node.setTextContent(a_IP);
           	output(node);
            /*
            theElem=xmldoc.createElement("price");
            theElem.setTextContent("20");
            theBook.appendChild(theElem);

            theElem=xmldoc.createElement("memo");
            theElem.setTextContent("新书的更好看。");
            theBook.appendChild(theElem);
            root.appendChild(theBook);
            System.out.println("--- 新建一本书开始 ----");
            output(xmldoc);
            //--- 新建一本书完成 ----

            //--- 下面对《哈里波特》做一些修改。 ----
            //--- 查询找《哈里波特》----
            theBook=(Element) selectSingleNode("/books/book[name='哈里波特']", root);
            System.out.println("--- 查询找《哈里波特》 ----");
            output(theBook);
            //--- 此时修改这本书的价格 -----
            theBook.getElementsByTagName("price").item(0).setTextContent("15");//getElementsByTagName返回的是NodeList，所以要跟上item(0)。另外，getElementsByTagName("price")相当于xpath的".//price"。
            System.out.println("--- 此时修改这本书的价格 ----");
            output(theBook);
            //--- 另外还想加一个属性id，值为B01 ----
            theBook.setAttribute("id", "B01");
            System.out.println("--- 另外还想加一个属性id，值为B01 ----");
            output(theBook);
            //--- 对《哈里波特》修改完成。 ----

            //--- 要用id属性删除《三国演义》这本书 ----
            theBook=(Element) selectSingleNode("/books/book[@id='B02']", root);
            System.out.println("--- 要用id属性删除《三国演义》这本书 ----");
            output(theBook);
            theBook.getParentNode().removeChild(theBook);
            System.out.println("--- 删除后的ＸＭＬ ----");
            output(xmldoc);

            //--- 再将所有价格低于10的书删除 ----
            NodeList someBooks=selectNodes("/books/book[price<10]", root);
            System.out.println("--- 再将所有价格低于10的书删除 ---");
            System.out.println("--- 符合条件的书有　"+someBooks.getLength()+"本。 ---");
            for(int i=0;i<someBooks.getLength();i++) {
                someBooks.item(i).getParentNode().removeChild(someBooks.item(i));
            }
            output(xmldoc);
*/
            saveXml(filePath, xmldoc);
        } catch (ParserConfigurationException e) {
            e.printStackTrace();
        } catch (SAXException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
	}
	
	
	public static void output(Node node) {//将node的XML字符串输出到控制台
        TransformerFactory transFactory=TransformerFactory.newInstance();
        try {
            Transformer transformer = transFactory.newTransformer();
            transformer.setOutputProperty("encoding", "UTF-8");
            transformer.setOutputProperty("indent", "yes");

            DOMSource source=new DOMSource();
            source.setNode(node);
            StreamResult result=new StreamResult();
            result.setOutputStream(System.out);
            
            transformer.transform(source, result);
        } catch (TransformerConfigurationException e) {
            e.printStackTrace();
        } catch (TransformerException e) {
            e.printStackTrace();
        }   
    }
    
    public static Node selectSingleNode(String express, Object source) {//查找节点，并返回第一个符合条件节点
        Node result=null;
        XPathFactory xpathFactory=XPathFactory.newInstance();
        XPath xpath=xpathFactory.newXPath();
        try {
            result=(Node) xpath.evaluate(express, source, XPathConstants.NODE);
        } catch (XPathExpressionException e) {
            e.printStackTrace();
        }
        
        return result;
    }
    
    public static NodeList selectNodes(String express, Object source) {//查找节点，返回符合条件的节点集。
        NodeList result=null;
        XPathFactory xpathFactory=XPathFactory.newInstance();
        XPath xpath=xpathFactory.newXPath();
        try {
            result=(NodeList) xpath.evaluate(express, source, XPathConstants.NODESET);
        } catch (XPathExpressionException e) {
            e.printStackTrace();
        }
        
        return result;
    }
    
    public static void saveXml(String fileName, Document doc) {//将Document输出到文件
        TransformerFactory transFactory=TransformerFactory.newInstance();
        try {
            Transformer transformer = transFactory.newTransformer();
            transformer.setOutputProperty("indent", "yes");

            DOMSource source=new DOMSource();
            source.setNode(doc);
            StreamResult result=new StreamResult();
            result.setOutputStream(new FileOutputStream(fileName));
            
            transformer.transform(source, result);
        } catch (TransformerConfigurationException e) {
            e.printStackTrace();
        } catch (TransformerException e) {
            e.printStackTrace();
        } catch (FileNotFoundException e) {
            e.printStackTrace();
        }   
    }
}


