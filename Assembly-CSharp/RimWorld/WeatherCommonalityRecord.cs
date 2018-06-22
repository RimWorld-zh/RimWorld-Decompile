using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000287 RID: 647
	public class WeatherCommonalityRecord
	{
		// Token: 0x06000AF7 RID: 2807 RVA: 0x00063AE8 File Offset: 0x00061EE8
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "weather", xmlRoot.Name);
			this.commonality = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}

		// Token: 0x04000560 RID: 1376
		public WeatherDef weather;

		// Token: 0x04000561 RID: 1377
		public float commonality = 0f;
	}
}
