using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000289 RID: 649
	public class PlantBiomeRecord
	{
		// Token: 0x04000564 RID: 1380
		public BiomeDef biome;

		// Token: 0x04000565 RID: 1381
		public float commonality = 0f;

		// Token: 0x06000AFB RID: 2811 RVA: 0x00063B80 File Offset: 0x00061F80
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "biome", xmlRoot.Name);
			this.commonality = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}
	}
}
