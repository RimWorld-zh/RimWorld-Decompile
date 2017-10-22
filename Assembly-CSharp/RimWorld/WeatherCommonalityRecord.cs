using System.Xml;
using Verse;

namespace RimWorld
{
	public class WeatherCommonalityRecord
	{
		public WeatherDef weather;

		public float commonality = 0f;

		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "weather", xmlRoot.Name);
			this.commonality = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}
	}
}
