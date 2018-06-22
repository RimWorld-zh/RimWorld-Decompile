using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000F05 RID: 3845
	public sealed class ThingOption
	{
		// Token: 0x06005C5E RID: 23646 RVA: 0x002EEA98 File Offset: 0x002ECE98
		public ThingOption()
		{
		}

		// Token: 0x06005C5F RID: 23647 RVA: 0x002EEAAC File Offset: 0x002ECEAC
		public ThingOption(ThingDef thingDef, float weight)
		{
			this.thingDef = thingDef;
			this.weight = weight;
		}

		// Token: 0x06005C60 RID: 23648 RVA: 0x002EEAD0 File Offset: 0x002ECED0
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

		// Token: 0x06005C61 RID: 23649 RVA: 0x002EEB44 File Offset: 0x002ECF44
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

		// Token: 0x04003CEC RID: 15596
		public ThingDef thingDef;

		// Token: 0x04003CED RID: 15597
		public float weight = 1f;
	}
}
