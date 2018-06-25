using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000372 RID: 882
	public class IncidentCategoryEntry
	{
		// Token: 0x04000957 RID: 2391
		public IncidentCategoryDef category;

		// Token: 0x04000958 RID: 2392
		public float weight;

		// Token: 0x06000F43 RID: 3907 RVA: 0x00081588 File Offset: 0x0007F988
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "category", xmlRoot.Name);
			this.weight = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}
	}
}
