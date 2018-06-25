using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x0200028B RID: 651
	public class PlantBiomeRecord
	{
		// Token: 0x04000566 RID: 1382
		public BiomeDef biome;

		// Token: 0x04000567 RID: 1383
		public float commonality = 0f;

		// Token: 0x06000AFE RID: 2814 RVA: 0x00063CCC File Offset: 0x000620CC
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "biome", xmlRoot.Name);
			this.commonality = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}
	}
}
