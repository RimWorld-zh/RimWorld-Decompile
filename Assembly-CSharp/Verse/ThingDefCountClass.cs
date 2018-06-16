using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000F03 RID: 3843
	public sealed class ThingDefCountClass : IExposable
	{
		// Token: 0x06005C0F RID: 23567 RVA: 0x002EC1F6 File Offset: 0x002EA5F6
		public ThingDefCountClass()
		{
		}

		// Token: 0x06005C10 RID: 23568 RVA: 0x002EC200 File Offset: 0x002EA600
		public ThingDefCountClass(ThingDef thingDef, int count)
		{
			if (count < 0)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to set ThingDefCountClass count to ",
					count,
					". thingDef=",
					thingDef
				}), false);
				count = 0;
			}
			this.thingDef = thingDef;
			this.count = count;
		}

		// Token: 0x17000EC8 RID: 3784
		// (get) Token: 0x06005C11 RID: 23569 RVA: 0x002EC25C File Offset: 0x002EA65C
		public string Summary
		{
			get
			{
				return this.count + "x " + ((this.thingDef == null) ? "null" : this.thingDef.label);
			}
		}

		// Token: 0x06005C12 RID: 23570 RVA: 0x002EC2A6 File Offset: 0x002EA6A6
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x06005C13 RID: 23571 RVA: 0x002EC2CC File Offset: 0x002EA6CC
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured ThingDefCountClass: " + xmlRoot.OuterXml, false);
			}
			else
			{
				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "thingDef", xmlRoot.Name);
				this.count = (int)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(int));
			}
		}

		// Token: 0x06005C14 RID: 23572 RVA: 0x002EC340 File Offset: 0x002EA740
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.count,
				"x ",
				(this.thingDef == null) ? "null" : this.thingDef.defName,
				")"
			});
		}

		// Token: 0x06005C15 RID: 23573 RVA: 0x002EC3AC File Offset: 0x002EA7AC
		public override int GetHashCode()
		{
			return (int)this.thingDef.shortHash + this.count << 16;
		}

		// Token: 0x06005C16 RID: 23574 RVA: 0x002EC3D8 File Offset: 0x002EA7D8
		public static implicit operator ThingDefCountClass(ThingDefCount t)
		{
			return new ThingDefCountClass(t.ThingDef, t.Count);
		}

		// Token: 0x04003CD4 RID: 15572
		public ThingDef thingDef;

		// Token: 0x04003CD5 RID: 15573
		public int count;
	}
}
