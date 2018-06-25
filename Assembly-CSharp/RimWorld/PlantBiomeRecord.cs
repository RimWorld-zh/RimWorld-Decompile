using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x0200028B RID: 651
	public class PlantBiomeRecord
	{
		// Token: 0x04000564 RID: 1380
		public BiomeDef biome;

		// Token: 0x04000565 RID: 1381
		public float commonality = 0f;

		// Token: 0x06000AFF RID: 2815 RVA: 0x00063CD0 File Offset: 0x000620D0
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "biome", xmlRoot.Name);
			this.commonality = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}
	}
}
