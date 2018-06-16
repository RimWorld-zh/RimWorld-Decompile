using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000289 RID: 649
	public class PlantBiomeRecord
	{
		// Token: 0x06000AFD RID: 2813 RVA: 0x00063B24 File Offset: 0x00061F24
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "biome", xmlRoot.Name);
			this.commonality = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}

		// Token: 0x04000566 RID: 1382
		public BiomeDef biome;

		// Token: 0x04000567 RID: 1383
		public float commonality = 0f;
	}
}
