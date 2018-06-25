using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x0200028A RID: 650
	public class BiomePlantRecord
	{
		// Token: 0x04000564 RID: 1380
		public ThingDef plant;

		// Token: 0x04000565 RID: 1381
		public float commonality = 0f;

		// Token: 0x06000AFC RID: 2812 RVA: 0x00063C80 File Offset: 0x00062080
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "plant", xmlRoot.Name);
			this.commonality = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}
	}
}
