using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// CommonReadXml 的摘要说明
/// </summary>
public class CommonReadXml
{

    public const string xmlPath = "measure";

    private static string xmlName;

    public static string XmlName
    {
        get { return xmlName; }
        set { xmlName = value; }
    }


    public static string xmlFullName
    {
        get
        {
            return xmlPath + "\\" + XmlName;
        }
    }

    //网站根目录
    /// <summary>
    /// 网站根目录
    /// </summary>
    public static string BaseDir
    {
        get
        {
            return System.AppDomain.CurrentDomain.BaseDirectory;
        }
    }

    //从根配置文件,用全名读取xml节点
    /// <summary>
    /// 从根配置文件,用全名读取xml节点
    /// </summary>
    /// <param name="path"></param>
    /// <param name="xmlFileName"></param>
    /// <returns></returns>
    public static XmlNode GetXmlNodeFromRootConfig(string path)
    {
        return GetXmlNode(path, xmlFullName);
    }


    //用全名读取xml节点
    /// <summary>
    /// 用全名读取xml节点
    /// </summary>
    /// <param name="path"></param>
    /// <param name="xmlFileName"></param>
    /// <returns></returns>
    public static XmlNode GetXmlNode(string path, string xmlFileName)
    {
        string baseDir = BaseDir;
        XmlDocument docxml = new XmlDocument();

        if (File.Exists(baseDir + xmlFileName))
        {
            docxml.Load(baseDir + xmlFileName);

            XmlNode node = docxml.SelectSingleNode(path);
            return node;
        }
        return null;
    }

    //按路径读取Node的节点
    /// <summary>
    /// 按路径读取Node的节点
    /// </summary>
    /// <param name="node"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static XmlNode GetXmlNode(XmlNode node, string path)
    {
        if (node != null)
        {
            XmlNode resultNode = node.SelectSingleNode(path);
            return resultNode;
        }
        return null;
    }

    //按路径读取Node的节点
    /// <summary>
    /// 按路径读取Node的节点
    /// </summary>
    /// <param name="path"></param>
    /// <param name="node"></param>
    /// <returns></returns>
    public static XmlNode GetXmlNode(string path, XmlNode node)
    {
        if (node != null)
        {
            XmlNode resultNode = node.SelectSingleNode(path);
            return resultNode;
        }
        return null;
    }


    //用全名读取xml节点，读取错误不返回异常
    /// <summary>
    /// 用全名读取xml节点，读取错误不返回异常
    /// </summary>
    /// <param name="path"></param>
    /// <param name="xmlFileName"></param>
    /// <returns></returns>
    public static XmlNode GetXmlNodeNoException(string path, string xmlFileName, out Exception exception)
    {
        XmlNode node = null;
        exception = null;

        try
        {
            node = GetXmlNode(path, xmlFileName);
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        return node;
    }

    //从根配置文件,用全名读取xml节点内容
    /// <summary>
    /// 从根配置文件,用全名读取xml节点内容
    /// </summary>
    /// <param name="path"></param>
    /// <param name="xmlFileName"></param>
    /// <returns></returns>
    public static string GetXmlInnerTextFromRootConfig(string path)
    {
        return GetXmlInnerText(path, xmlFullName);
    }

    //用全名读取xml节点内容
    /// <summary>
    /// 用全名读取xml节点内容
    /// </summary>
    /// <param name="path"></param>
    /// <param name="xmlFileName"></param>
    /// <returns></returns>
    public static string GetXmlInnerText(string path, string xmlFileName)
    {
        XmlNode node = GetXmlNode(path, xmlFileName);

        if (node != null && node.InnerText != null
            && node.InnerText.Trim() != string.Empty)
        {
            return node.InnerText.Trim();
        }

        return string.Empty;
    }

    //用全名读取xml节点内容
    /// <summary>
    /// 用全名读取xml节点内容
    /// </summary>
    /// <param name="path"></param>
    /// <param name="xmlFileName"></param>
    /// <returns></returns>
    public static string GetXmlInnerText(string path, XmlNode branchNode)
    {
        XmlNode leafNode = GetXmlNode(path, branchNode);

        if (leafNode != null && leafNode.InnerText != null
            && leafNode.InnerText.Trim() != string.Empty)
        {
            return leafNode.InnerText.Trim();
        }

        return string.Empty;
    }

    //用全名读取xml节点内容，读取错误不返回异常
    /// <summary>
    /// 用全名读取xml节点内容，读取错误不返回异常
    /// </summary>
    /// <param name="path"></param>
    /// <param name="xmlFileName"></param>
    /// <returns></returns>
    public static string GetXmlInnerTextNoException(string path, string xmlFileName, out Exception exception)
    {
        XmlNode node = GetXmlNodeNoException(path, xmlFileName, out exception);

        if (node != null && node.InnerText != null
            && node.InnerText.Trim() != string.Empty)
        {
            return node.InnerText.Trim();
        }

        return string.Empty;
    }



    //读取所有叶子节点
    /// <summary>
    /// 读取所有叶子节点
    /// </summary>
    /// <param name="path"></param>
    /// <param name="xmlName"></param>
    /// <returns></returns>
    public static List<XmlNode> GetAllLeafNodes(string path, string xmlName)
    {
        List<XmlNode> listnode = new List<XmlNode>();

        XmlNode parentNode = GetXmlNode(path, xmlName);

        if (parentNode != null)
        {
            return GetAllLeafNodes(parentNode);
        }

        return null;
    }

    //读取所有叶子节点
    /// <summary>
    /// 读取所有叶子节点
    /// </summary>
    /// <param name="node"></param>
    /// <param name="xmlName"></param>
    /// <returns></returns>
    public static List<XmlNode> GetAllLeafNodes(XmlNode node)
    {
        if (node == null) return null;

        List<XmlNode> dicNode = new List<XmlNode>();

        //只添加元素节点(XmlNodeType.Element),不添加注视或者值节点.     ???值节点
        if (IsEffectiveNode(node))
        {
            if (node.ChildNodes != null && node.ChildNodes.Count > 0)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    dicNode.AddRange(GetAllLeafNodes(childNode));
                }
            }

            if (dicNode.Count <= 0)
            {
                dicNode.Add(node);
            }
        }

        return dicNode;
    }

    //读取所有叶子节点的InnerText
    /// <summary>
    /// 读取所有叶子节点的InnerText
    /// </summary>
    /// <param name="path"></param>
    /// <param name="xmlName"></param>
    /// <returns></returns>
    public static List<string> GetAllLeafNodesInnerText(string path, string xmlName)
    {
        XmlNode parentNode = GetXmlNode(path, xmlName);

        if (parentNode != null)
        {
            return GetAllLeafNodesInnerText(parentNode);
        }

        return null;
    }

    //读取所有叶子节点的InnerText
    /// <summary>
    /// 读取所有叶子节点的InnerText
    /// </summary>
    /// <param name="node"></param>
    /// <param name="xmlName"></param>
    /// <returns></returns>
    public static List<string> GetAllLeafNodesInnerText(XmlNode node)
    {
        if (node == null) return null;

        List<string> lstInnerText = new List<string>();

        //只添加元素节点(XmlNodeType.Element),不添加注视或者值节点.     ???值节点
        if (node.NodeType == XmlNodeType.Element)
        {
            if (node.ChildNodes != null && node.ChildNodes.Count > 0)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    lstInnerText.AddRange(GetAllLeafNodesInnerText(childNode));
                }
            }

            if (lstInnerText.Count <= 0)
            {
                if (node.InnerText != null && node.InnerText.Trim() != string.Empty)
                {
                    lstInnerText.Add(node.InnerText.Trim());
                }
            }
        }

        return lstInnerText;
    }


    //读取所有叶子节点的OuterText
    /// <summary>
    /// 读取所有叶子节点的OuterText
    /// </summary>
    /// <param name="path"></param>
    /// <param name="xmlName"></param>
    /// <returns></returns>
    public static List<string> GetAllLeafNodesOuterText(XmlNode node)
    {
        //List<XmlNode> lstLeafNodes = GetAllLeafNodes(node);
        List<XmlNode> lstLeafNodes = GetAllLeafElementNodes(node);

        List<string> lstOuterText = new List<string>();
        if (lstLeafNodes != null && lstLeafNodes.Count > 0)
        {
            foreach (XmlNode leaf in lstLeafNodes)
            {
                if (leaf != null)
                {
                    //XmlNode endNode = GetParentFirstEndElement(leaf);

                    //if (endNode != null)
                    //{
                    //    lstOuterText.Add(endNode.OuterXml);
                    //}
                    lstOuterText.Add(leaf.OuterXml);
                }
            }
        }

        return lstOuterText;
    }

    //读取所有叶子节点
    /// <summary>
    /// 读取所有叶子节点
    /// </summary>
    /// <param name="xmlFileName"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static List<XmlNode> GetAllLeafElementNodes(string xmlFileName, string path)
    {
        XmlNode node = GetXmlNode(path, xmlFileName);

        return GetAllLeafElementNodes(node);
    }

    //读取所有叶子节点
    /// <summary>
    /// 读取所有叶子节点
    /// </summary>
    /// <param name="node"></param>
    /// <param name="xmlName"></param>
    /// <returns></returns>
    public static List<XmlNode> GetAllLeafElementNodes(XmlNode node)
    {
        List<XmlNode> dicNode = new List<XmlNode>();

        //只添加元素节点(XmlNodeType.Element),不添加注视或者值节点.     ???值节点
        if (IsEffectiveNode(node))
        {
            if (IsLastElementNode(node))
            {
                dicNode.Add(node);
            }
            else if (node.ChildNodes != null && node.ChildNodes.Count > 0)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    dicNode.AddRange(GetAllLeafElementNodes(childNode));
                }
            }
        }

        return dicNode;
    }

    //判断是否是最后一个Element节点
    /// <summary>
    /// 判断是否是最后一个Element节点
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public static bool IsLastElementNode(XmlNode node)
    {
        if (node == null || node.NodeType != XmlNodeType.Element)
        {
            return false;
        }

        if (node.ChildNodes != null && node.ChildNodes.Count > 0)
        {
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (childNode != null)
                {
                    if (!IsNoElement(childNode))
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    //判断是否含有Element节点(包含本身以及所有子节点)
    /// <summary>
    /// 判断是否含有Element节点(包含本身以及所有子节点)
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public static bool IsNoElement(XmlNode node)
    {
        if (node != null)
        {
            if (node.NodeType == XmlNodeType.Element) return false;

            if (node.ChildNodes != null && node.ChildNodes.Count > 0)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (!IsNoElement(child))
                    {
                        return false;
                    }
                }

            }
        }
        return true;
    }

    //获得本节点以上的第一个Element节点(包含本身)
    /// <summary>
    /// 获得本节点以上的第一个Element节点(包含本身)
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public static XmlNode GetFirstParentElement(XmlNode node)
    {
        if (node == null) return null;

        if (node.NodeType == XmlNodeType.Element)
        {
            return node;
        }
        else
        {
            if (node.ParentNode != null)
            {
                return GetFirstParentElement(node.ParentNode);
            }
        }

        return node;
    }


    //是否有效地Node(不是注释Node)
    /// <summary>
    /// 是否有效地Node(不是注释Node)
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public static bool IsEffectiveNode(XmlNode node)
    {
        if (node != null)
        {
            //注释不是有效地XmlNode
            if (node.NodeType != XmlNodeType.Comment)
            {
                return true;
            }
        }

        return false;
    }

    //是否是元素Node
    /// <summary>
    /// 是否是元素Node
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public static bool IsElementNode(XmlNode node)
    {
        if (IsEffectiveNode(node))
        {
            //return node.NodeType == XmlNodeType.Element;

            if (node.NodeType == XmlNodeType.Element)
            {
                return true;
            }
        }

        return false;
    }
}

