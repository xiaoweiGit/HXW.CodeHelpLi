using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace  HXW.CodeHelpLi
{
    public class XmlUtility
    {
        /// <summary>
        /// 通过节点属性值查找第一个匹配的节点
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static XElement GetFirstXElement(string xmlPath, string attribute, string value)
        {

            XElement xe = XElement.Load(xmlPath);
            IEnumerable<XElement> elements = xe.Elements();

            foreach (XElement e in elements)
            {
                if (e.Attribute(attribute).Value != null &&
                    string.Compare(e.Attribute(attribute).Value, value) == 0) return e;
            }

            return null;
        }
    }
}
