using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000F05 RID: 3845
	public sealed class ThingOption
	{
		// Token: 0x06005C36 RID: 23606 RVA: 0x002ECA64 File Offset: 0x002EAE64
		public ThingOption()
		{
		}

		// Token: 0x06005C37 RID: 23607 RVA: 0x002ECA78 File Offset: 0x002EAE78
		public ThingOption(ThingDef thingDef, float weight)
		{
			this.thingDef = thingDef;
			this.weight = weight;
		}

		// Token: 0x06005C38 RID: 23608 RVA: 0x002ECA9C File Offset: 0x002EAE9C
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

		// Token: 0x06005C39 RID: 23609 RVA: 0x002ECB10 File Offset: 0x002EAF10
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

		// Token: 0x04003CD9 RID: 15577
		public ThingDef thingDef;

		// Token: 0x04003CDA RID: 15578
		public float weight = 1f;
	}
}
