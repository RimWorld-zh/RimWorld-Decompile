using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CCC RID: 3276
	public class XmlContainer
	{
		// Token: 0x040030FC RID: 12540
		public XmlNode node;

		// Token: 0x0600487B RID: 18555 RVA: 0x002614E8 File Offset: 0x0025F8E8
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			this.node = xmlRoot;
		}
	}
}
