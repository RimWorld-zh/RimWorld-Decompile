using System;
using System.Xml;

namespace Verse
{
	public class XmlContainer
	{
		public XmlNode node;

		public XmlContainer()
		{
		}

		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			this.node = xmlRoot;
		}
	}
}
