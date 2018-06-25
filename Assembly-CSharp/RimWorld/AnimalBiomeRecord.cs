using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x0200028D RID: 653
	public class AnimalBiomeRecord
	{
		// Token: 0x0400056A RID: 1386
		public BiomeDef biome;

		// Token: 0x0400056B RID: 1387
		public float commonality = 0f;

		// Token: 0x06000B02 RID: 2818 RVA: 0x00063D64 File Offset: 0x00062164
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "biome", xmlRoot.Name);
			this.commonality = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}
	}
}
