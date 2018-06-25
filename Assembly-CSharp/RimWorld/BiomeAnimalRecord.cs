using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x0200028C RID: 652
	public class BiomeAnimalRecord
	{
		// Token: 0x04000568 RID: 1384
		public PawnKindDef animal;

		// Token: 0x04000569 RID: 1385
		public float commonality = 0f;

		// Token: 0x06000B00 RID: 2816 RVA: 0x00063D18 File Offset: 0x00062118
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "animal", xmlRoot.Name);
			this.commonality = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}
	}
}
