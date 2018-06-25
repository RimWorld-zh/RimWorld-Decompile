using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000F09 RID: 3849
	public sealed class ThingOption
	{
		// Token: 0x04003CEF RID: 15599
		public ThingDef thingDef;

		// Token: 0x04003CF0 RID: 15600
		public float weight = 1f;

		// Token: 0x06005C68 RID: 23656 RVA: 0x002EF118 File Offset: 0x002ED518
		public ThingOption()
		{
		}

		// Token: 0x06005C69 RID: 23657 RVA: 0x002EF12C File Offset: 0x002ED52C
		public ThingOption(ThingDef thingDef, float weight)
		{
			this.thingDef = thingDef;
			this.weight = weight;
		}

		// Token: 0x06005C6A RID: 23658 RVA: 0x002EF150 File Offset: 0x002ED550
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

		// Token: 0x06005C6B RID: 23659 RVA: 0x002EF1C4 File Offset: 0x002ED5C4
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
