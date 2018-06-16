using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000F06 RID: 3846
	public sealed class ThingOption
	{
		// Token: 0x06005C38 RID: 23608 RVA: 0x002EC988 File Offset: 0x002EAD88
		public ThingOption()
		{
		}

		// Token: 0x06005C39 RID: 23609 RVA: 0x002EC99C File Offset: 0x002EAD9C
		public ThingOption(ThingDef thingDef, float weight)
		{
			this.thingDef = thingDef;
			this.weight = weight;
		}

		// Token: 0x06005C3A RID: 23610 RVA: 0x002EC9C0 File Offset: 0x002EADC0
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

		// Token: 0x06005C3B RID: 23611 RVA: 0x002ECA34 File Offset: 0x002EAE34
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

		// Token: 0x04003CDA RID: 15578
		public ThingDef thingDef;

		// Token: 0x04003CDB RID: 15579
		public float weight = 1f;
	}
}
