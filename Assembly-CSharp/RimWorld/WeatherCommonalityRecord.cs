using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000287 RID: 647
	public class WeatherCommonalityRecord
	{
		// Token: 0x06000AF9 RID: 2809 RVA: 0x00063A8C File Offset: 0x00061E8C
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "weather", xmlRoot.Name);
			this.commonality = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}

		// Token: 0x04000562 RID: 1378
		public WeatherDef weather;

		// Token: 0x04000563 RID: 1379
		public float commonality = 0f;
	}
}
