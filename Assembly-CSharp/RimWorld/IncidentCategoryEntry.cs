using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000372 RID: 882
	public class IncidentCategoryEntry
	{
		// Token: 0x04000954 RID: 2388
		public IncidentCategoryDef category;

		// Token: 0x04000955 RID: 2389
		public float weight;

		// Token: 0x06000F44 RID: 3908 RVA: 0x00081578 File Offset: 0x0007F978
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "category", xmlRoot.Name);
			this.weight = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}
	}
}
