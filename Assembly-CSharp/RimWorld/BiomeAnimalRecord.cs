using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x0200028A RID: 650
	public class BiomeAnimalRecord
	{
		// Token: 0x06000AFF RID: 2815 RVA: 0x00063B70 File Offset: 0x00061F70
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "animal", xmlRoot.Name);
			this.commonality = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
		}

		// Token: 0x04000568 RID: 1384
		public PawnKindDef animal;

		// Token: 0x04000569 RID: 1385
		public float commonality = 0f;
	}
}
