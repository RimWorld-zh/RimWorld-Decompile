using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x0200028C RID: 652
	public class BiomeAnimalRecord
	{
		// Token: 0x04000566 RID: 1382
		public PawnKindDef animal;

		// Token: 0x04000567 RID: 1383
		public float commonality = 0f;

		// Token: 0x06000B01 RID: 2817 RVA: 0x00063D1C File Offset: 0x0006211C
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "animal", xmlRoot.Name);
			this.commonality = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}
	}
}
