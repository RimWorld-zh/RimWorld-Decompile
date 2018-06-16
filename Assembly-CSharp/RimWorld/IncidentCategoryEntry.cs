using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000370 RID: 880
	public class IncidentCategoryEntry
	{
		// Token: 0x06000F40 RID: 3904 RVA: 0x0008123C File Offset: 0x0007F63C
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "category", xmlRoot.Name);
			this.weight = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}

		// Token: 0x04000952 RID: 2386
		public IncidentCategoryDef category;

		// Token: 0x04000953 RID: 2387
		public float weight;
	}
}
