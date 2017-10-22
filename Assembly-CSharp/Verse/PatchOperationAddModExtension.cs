using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	public class PatchOperationAddModExtension : PatchOperationPathed
	{
		private XmlContainer value;

		protected override bool ApplyWorker(XmlDocument xml)
		{
			XmlNode node = this.value.node;
			bool result = false;
			IEnumerator enumerator = xml.SelectNodes(base.xpath).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
					XmlNode xmlNode = current as XmlNode;
					XmlNode xmlNode2 = xmlNode["modExtensions"];
					if (xmlNode2 == null)
					{
						xmlNode2 = xmlNode.OwnerDocument.CreateElement("modExtensions");
						xmlNode.AppendChild(xmlNode2);
					}
					for (int i = 0; i < node.ChildNodes.Count; i++)
					{
						xmlNode2.AppendChild(xmlNode.OwnerDocument.ImportNode(node.ChildNodes[i], true));
					}
					result = true;
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			return result;
		}
	}
}
