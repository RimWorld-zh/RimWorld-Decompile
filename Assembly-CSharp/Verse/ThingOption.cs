using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000F0A RID: 3850
	public sealed class ThingOption
	{
		// Token: 0x04003CF7 RID: 15607
		public ThingDef thingDef;

		// Token: 0x04003CF8 RID: 15608
		public float weight = 1f;

		// Token: 0x06005C68 RID: 23656 RVA: 0x002EF338 File Offset: 0x002ED738
		public ThingOption()
		{
		}

		// Token: 0x06005C69 RID: 23657 RVA: 0x002EF34C File Offset: 0x002ED74C
		public ThingOption(ThingDef thingDef, float weight)
		{
			this.thingDef = thingDef;
			this.weight = weight;
		}

		// Token: 0x06005C6A RID: 23658 RVA: 0x002EF370 File Offset: 0x002ED770
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured ThingOption: " + xmlRoot.OuterXml, false);
			}
			else
			{
				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "thingDef", xmlRoot.Name);
				this.weight = (float)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(float));
			}
		}

		// Token: 0x06005C6B RID: 23659 RVA: 0x002EF3E4 File Offset: 0x002ED7E4
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				(this.thingDef == null) ? "null" : this.thingDef.defName,
				", weight=",
				this.weight.ToString("0.##"),
				")"
			});
		}
	}
}
