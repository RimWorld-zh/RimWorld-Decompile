using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000288 RID: 648
	public class BiomePlantRecord
	{
		// Token: 0x06000AFB RID: 2811 RVA: 0x00063AD8 File Offset: 0x00061ED8
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "plant", xmlRoot.Name);
			this.commonality = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}

		// Token: 0x04000564 RID: 1380
		public ThingDef plant;

		// Token: 0x04000565 RID: 1381
		public float commonality = 0f;
	}
}
