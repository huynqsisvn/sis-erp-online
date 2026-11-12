using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace SisPo.Pages.Input.POImexInv
{
    public class Ult
    {
        private static string[] nodes = new string[4]
        {
      "HHDVu",
      "TTChung",
      "NBan",
      "TToan"
        };
        private static string[] UnusedNames = "TTKhac".Split(';');
        private static string[] KeepNames = "MCCQT".Split(';');


        public static DataSet ReadXml(string filename)
        {
            try
            {
                if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
                    return (DataSet)null;
                filename = Ult.DeleteNodesAndGetNewPath(filename);
                DataSet dataSet = new DataSet();
                int num = (int)dataSet.ReadXml(filename);
                dataSet.DataSetName = "Inv";
                try
                {
                    File.Delete(filename);
                }
                catch (Exception ex)
                {
                }
                return dataSet;
            }
            catch (Exception ex)
            {
                //int num = (int)Ult.Msg.Show(20, "Tệp XML không hợp lệ!");
            }
            return (DataSet)null;
        }

        internal static string DeleteNodesAndGetNewPath(string filename)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(filename);
            List<XmlNode> xmlNodeList = new List<XmlNode>();
            foreach (XmlNode childNode in xmlDocument.ChildNodes)
            {
                if (!Ult.CheckAndDeleteNode(childNode))
                    xmlNodeList.Add(childNode);
            }
            for (int index = 0; index < xmlNodeList.Count; ++index)
                xmlDocument.RemoveChild(xmlNodeList[index]);
            foreach (XmlNode childNode in xmlDocument.ChildNodes)
                Ult.DelUnusedNode(childNode);
            string filename1 = Path.GetTempPath() + Path.GetFileName(filename);
            xmlDocument.Save(filename1);
            return filename1;
        }

        internal static bool CheckAndDeleteNode(XmlNode node)
        {
            if (((IEnumerable<string>)Ult.KeepNames).Any<string>((Func<string, bool>)(x => x.Equals(node.Name, StringComparison.OrdinalIgnoreCase))))
                return true;
            if (node.NodeType != XmlNodeType.Element)
                return false;
            if (((IEnumerable<string>)Ult.nodes).Any<string>((Func<string, bool>)(x => x == node.Name)))
                return true;
            List<XmlNode> xmlNodeList = new List<XmlNode>();
            bool flag1 = false;
            foreach (XmlNode childNode in node.ChildNodes)
            {
                bool flag2 = Ult.CheckAndDeleteNode(childNode);
                if (!flag2)
                    xmlNodeList.Add(childNode);
                if (!flag1 & flag2)
                    flag1 = true;
            }
            for (int index = 0; index < xmlNodeList.Count; ++index)
                node.RemoveChild(xmlNodeList[index]);
            return flag1;
        }

        internal static void DelUnusedNode(XmlNode node)
        {
            if (node.NodeType != XmlNodeType.Element)
                return;
            List<XmlNode> xmlNodeList = new List<XmlNode>();
            if (node.Name == "HHDVu")
            {
                Ult.CheckTienNode(node);
                Ult.CheckThueNode(node);
            }
            foreach (XmlNode childNode in node.ChildNodes)
            {
                XmlNode childnode = childNode;
                if (((IEnumerable<string>)Ult.UnusedNames).Any<string>((Func<string, bool>)(x => x.Equals(childnode.Name, StringComparison.OrdinalIgnoreCase))))
                    xmlNodeList.Add(childnode);
                else
                    Ult.DelUnusedNode(childnode);
            }
            for (int index = 0; index < xmlNodeList.Count; ++index)
                node.RemoveChild(xmlNodeList[index]);
        }

        private static void CheckThueNode(XmlNode node)
        {
            try
            {
                if (node.ChildNodes.Cast<XmlNode>().Any<XmlNode>((Func<XmlNode, bool>)(x => x.Name.Equals("TThue", StringComparison.OrdinalIgnoreCase))))
                    return;
                XmlNode xmlNode1 = node.ChildNodes.Cast<XmlNode>().FirstOrDefault<XmlNode>((Func<XmlNode, bool>)(x => x.Name.Equals("TTKhac", StringComparison.OrdinalIgnoreCase)));
                if (xmlNode1 == null)
                    return;
                foreach (XmlNode childNode in xmlNode1.ChildNodes)
                {
                    if (childNode.ChildNodes.Cast<XmlNode>().Any<XmlNode>((Func<XmlNode, bool>)(x =>
                    {
                        if (!x.Name.Equals("TTruong", StringComparison.OrdinalIgnoreCase))
                            return false;
                        string innerText = x.InnerText;
                        return innerText.Equals("VATAmount", StringComparison.OrdinalIgnoreCase) || innerText.Equals("Tiền thuế", StringComparison.OrdinalIgnoreCase);
                    })))
                    {
                        XmlNode xmlNode2 = childNode.ChildNodes.Cast<XmlNode>().FirstOrDefault<XmlNode>((Func<XmlNode, bool>)(x => x.Name.Equals("DLieu", StringComparison.OrdinalIgnoreCase)));
                        if (xmlNode2 != null)
                        {
                            XmlElement element = node.OwnerDocument.CreateElement("TThue");
                            element.InnerText = xmlNode2.InnerText;
                            node.AppendChild((XmlNode)element);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private static void CheckTienNode(XmlNode node)
        {
            try
            {
                if (node.ChildNodes.Cast<XmlNode>().Any<XmlNode>((Func<XmlNode, bool>)(x => x.Name.Equals("TienTruocCK", StringComparison.OrdinalIgnoreCase))))
                    return;
                XmlNode xmlNode1 = node.ChildNodes.Cast<XmlNode>().FirstOrDefault<XmlNode>((Func<XmlNode, bool>)(x => x.Name.Equals("TTKhac", StringComparison.OrdinalIgnoreCase)));
                if (xmlNode1 == null)
                    return;
                foreach (XmlNode childNode in xmlNode1.ChildNodes)
                {
                    if (childNode.ChildNodes.Cast<XmlNode>().Any<XmlNode>((Func<XmlNode, bool>)(x => x.Name.Equals("TTruong", StringComparison.OrdinalIgnoreCase) && x.InnerText.Equals("Amount", StringComparison.OrdinalIgnoreCase))))
                    {
                        XmlNode xmlNode2 = childNode.ChildNodes.Cast<XmlNode>().FirstOrDefault<XmlNode>((Func<XmlNode, bool>)(x => x.Name.Equals("DLieu", StringComparison.OrdinalIgnoreCase)));
                        if (xmlNode2 != null)
                        {
                            XmlElement element = node.OwnerDocument.CreateElement("TienTruocCK");
                            element.InnerText = xmlNode2.InnerText;
                            node.AppendChild((XmlNode)element);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        public static string GetInvHash(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return "";
            if (!File.Exists(fileName))
                return GetMD5HashFromFile(fileName);
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            byte[] hash = new MD5CryptoServiceProvider().ComputeHash((Stream)fileStream);
            fileStream.Close();
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < hash.Length; ++index)
                stringBuilder.Append(hash[index].ToString("x2"));
            return stringBuilder.ToString();
        }
        public static string GetMD5HashFromFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return "";
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            byte[] hash = new MD5CryptoServiceProvider().ComputeHash((Stream)fileStream);
            fileStream.Close();
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < hash.Length; ++index)
                stringBuilder.Append(hash[index].ToString("x2"));
            return stringBuilder.ToString();
        }
        /////---------------------------------------
        internal static string GetValueFromJsonString(string data, string name)
        {
            try
            {
                JObject jObject = JObject.Parse(data);
                return jObject.SelectToken(name).ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        internal static string FormatAuthorizationToken(string authorizationToken)
        {
            try
            {
                string result = "";
                if (!string.IsNullOrEmpty(authorizationToken))
                {
                    result = "Bearer " + authorizationToken;
                }
                return result;
            }
            catch (Exception)
            {
                return "";
            }
        }

        internal static string ConvertObjectToJSon(object o)
        {
            return JsonConvert.SerializeObject(RuntimeHelpers.GetObjectValue(o));
        }
	}
}
