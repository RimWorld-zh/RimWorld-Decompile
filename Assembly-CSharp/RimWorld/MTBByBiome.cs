using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020002A3 RID: 675
	public class MTBByBiome
	{
		// Token: 0x04000617 RID: 1559
		public BiomeDef biome;

		// Token: 0x04000618 RID: 1560
		public float mtbDays;

		// Token: 0x06000B49 RID: 2889 RVA: 0x00065F6C File Offset: 0x0006436C
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
