using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020002A1 RID: 673
	public class MTBByBiome
	{
		// Token: 0x04000615 RID: 1557
		public BiomeDef biome;

		// Token: 0x04000616 RID: 1558
		public float mtbDays;

		// Token: 0x06000B46 RID: 2886 RVA: 0x00065E20 File Offset: 0x00064220
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
	}
}
