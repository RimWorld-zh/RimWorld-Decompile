using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CCD RID: 3277
	public class XmlContainer
	{
		// Token: 0x06004867 RID: 18535 RVA: 0x0025FFF4 File Offset: 0x0025E3F4
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			this.node = xmlRoot;
		}

		// Token: 0x040030F1 RID: 12529
		public XmlNode node;
	}
}
