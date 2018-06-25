using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000289 RID: 649
	public class WeatherCommonalityRecord
	{
		// Token: 0x04000560 RID: 1376
		public WeatherDef weather;

		// Token: 0x04000561 RID: 1377
		public float commonality = 0f;

		// Token: 0x06000AFB RID: 2811 RVA: 0x00063C38 File Offset: 0x00062038
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "weather", xmlRoot.Name);
			this.commonality = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}
	}
}
