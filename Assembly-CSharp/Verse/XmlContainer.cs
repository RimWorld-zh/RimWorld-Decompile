using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000CCE RID: 3278
	public class XmlContainer
	{
		// Token: 0x06004869 RID: 18537 RVA: 0x0026001C File Offset: 0x0025E41C
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			this.node = xmlRoot;
		}

		// Token: 0x040030F3 RID: 12531
		public XmlNode node;
	}
}
