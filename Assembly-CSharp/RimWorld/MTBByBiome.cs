using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020002A1 RID: 673
	public class MTBByBiome
	{
		// Token: 0x06000B48 RID: 2888 RVA: 0x00065DB8 File Offset: 0x000641B8
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured MTBByBiome: " + xmlRoot.OuterXml, false);
			}
			else
			{
				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "biome", xmlRoot.Name);
				this.mtbDays = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
			}
		}

		// Token: 0x04000616 RID: 1558
		public BiomeDef biome;

		// Token: 0x04000617 RID: 1559
		public float mtbDays;
	}
}
