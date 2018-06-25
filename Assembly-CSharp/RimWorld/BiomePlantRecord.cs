using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x0200028A RID: 650
	public class BiomePlantRecord
	{
		// Token: 0x04000562 RID: 1378
		public ThingDef plant;

		// Token: 0x04000563 RID: 1379
		public float commonality = 0f;

		// Token: 0x06000AFD RID: 2813 RVA: 0x00063C84 File Offset: 0x00062084
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "plant", xmlRoot.Name);
			this.commonality = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}
	}
}
