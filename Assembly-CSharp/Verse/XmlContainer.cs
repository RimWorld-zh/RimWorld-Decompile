using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CCD RID: 3277
	public class XmlContainer
	{
		// Token: 0x04003103 RID: 12547
		public XmlNode node;

		// Token: 0x0600487B RID: 18555 RVA: 0x002617C8 File Offset: 0x0025FBC8
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			this.node = xmlRoot;
		}
	}
}
