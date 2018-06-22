using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CCA RID: 3274
	public class XmlContainer
	{
		// Token: 0x06004878 RID: 18552 RVA: 0x0026140C File Offset: 0x0025F80C
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			this.node = xmlRoot;
		}

		// Token: 0x040030FC RID: 12540
		public XmlNode node;
	}
}
