using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000370 RID: 880
	public class IncidentCategoryEntry
	{
		// Token: 0x06000F40 RID: 3904 RVA: 0x00081428 File Offset: 0x0007F828
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "category", xmlRoot.Name);
			this.weight = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}

		// Token: 0x04000954 RID: 2388
		public IncidentCategoryDef category;

		// Token: 0x04000955 RID: 2389
		public float weight;
	}
}
