// ===============================================================================
// 版权    ：枢木
// 创建时间：2011-7 修改：
// 作者    ：枢木 ideal35500@qq.com
// 文件    ：
// 功能    ：
// 说明    ：
// ===============================================================================

using System;
using System.Xml;

namespace TSF.ENTLIB.Common.Util
{
    /// <summary>
    /// Xml工具
    /// </summary>
    public sealed class XmlUtil
    {
        private XmlUtil() { }

        public static XmlNode AppendElement(XmlNode node, string newElementName)
        {
            return AppendElement(node, newElementName, null);
        }

        public static XmlNode AppendElement(XmlNode node, string newElementName, string innerValue)
        {
            XmlNode node2;

            if (node is XmlDocument)
            {
                node2 = node.AppendChild(((XmlDocument)node).CreateElement(newElementName));
            }
            else
            {
                node2 = node.AppendChild(node.OwnerDocument.CreateElement(newElementName));
            }

            if (innerValue != null)
            {
                node2.AppendChild(node.OwnerDocument.CreateTextNode(innerValue));
            }

            return node2;
        }

        public static XmlAttribute CreateAttribute(XmlDocument xmlDocument, string name, string value)
        {
            XmlAttribute attribute = xmlDocument.CreateAttribute(name);
            attribute.Value = value;

            return attribute;
        }

        /// <summary>
        /// 获取属性的值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetAttribute(XmlNode node, string attributeName, string defaultValue)
        {
            XmlAttribute attribute = node.Attributes[attributeName];

            if (attribute != null)
            {
                return attribute.Value;
            }

            return defaultValue;
        }

        /// <summary>
        /// 获取节点的值
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="nodeXPath"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetNodeValue(XmlNode parentNode, string nodeXPath, string defaultValue)
        {
            XmlNode node = parentNode.SelectSingleNode(nodeXPath);

            if (node.FirstChild != null)
            {
                return node.FirstChild.Value;
            }
            if (node != null)
            {
                return node.Value;
            }

            return defaultValue;
        }

        public static void SetAttribute(XmlNode node, string attributeName, string attributeValue)
        {
            if (node.Attributes[attributeName] != null)
            {
                node.Attributes[attributeName].Value = attributeValue;
            }
            else
            {
                node.Attributes.Append(CreateAttribute(node.OwnerDocument, attributeName, attributeValue));
            }
        }

        /// <summary>
        /// xml非法字符过滤
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string XmlIllegalCharacters(string content)
        {
            return System.Text.RegularExpressions.Regex.Replace(content, "[\x00-\x08|\x0b-\x0c|\x0e-\x1f]", "");
        }
    }
}