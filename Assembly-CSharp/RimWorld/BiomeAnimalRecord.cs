using System.Xml;
using Verse;

namespace RimWorld
{
	public class BiomeAnimalRecord
	{
		public PawnKindDef animal;

		public float commonality = 0f;

		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "animal", xmlRoot.Name);
			this.commonality = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}
	}
}
