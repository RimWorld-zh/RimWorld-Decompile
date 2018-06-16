using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x0200028B RID: 651
	public class AnimalBiomeRecord
	{
		// Token: 0x06000B01 RID: 2817 RVA: 0x00063BBC File Offset: 0x00061FBC
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "biome", xmlRoot.Name);
			this.commonality = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}

		// Token: 0x0400056A RID: 1386
		public BiomeDef biome;

		// Token: 0x0400056B RID: 1387
		public float commonality = 0f;
	}
}
