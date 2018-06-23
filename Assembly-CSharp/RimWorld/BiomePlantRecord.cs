using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000288 RID: 648
	public class BiomePlantRecord
	{
		// Token: 0x04000562 RID: 1378
		public ThingDef plant;

		// Token: 0x04000563 RID: 1379
		public float commonality = 0f;

		// Token: 0x06000AF9 RID: 2809 RVA: 0x00063B34 File Offset: 0x00061F34
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "plant", xmlRoot.Name);
			this.commonality = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}
	}
}
