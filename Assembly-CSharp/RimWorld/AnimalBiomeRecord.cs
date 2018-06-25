using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x0200028D RID: 653
	public class AnimalBiomeRecord
	{
		// Token: 0x04000568 RID: 1384
		public BiomeDef biome;

		// Token: 0x04000569 RID: 1385
		public float commonality = 0f;

		// Token: 0x06000B03 RID: 2819 RVA: 0x00063D68 File Offset: 0x00062168
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "biome", xmlRoot.Name);
			this.commonality = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}
	}
}
