using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02000289 RID: 649
	public class WeatherCommonalityRecord
	{
		// Token: 0x04000562 RID: 1378
		public WeatherDef weather;

		// Token: 0x04000563 RID: 1379
		public float commonality = 0f;

		// Token: 0x06000AFA RID: 2810 RVA: 0x00063C34 File Offset: 0x00062034
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "weather", xmlRoot.Name);
			this.commonality = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}
	}
}
