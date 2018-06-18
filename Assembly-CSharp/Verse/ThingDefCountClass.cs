using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000F02 RID: 3842
	public sealed class ThingDefCountClass : IExposable
	{
		// Token: 0x06005C0D RID: 23565 RVA: 0x002EC2D2 File Offset: 0x002EA6D2
		public ThingDefCountClass()
		{
		}

		// Token: 0x06005C0E RID: 23566 RVA: 0x002EC2DC File Offset: 0x002EA6DC
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

		// Token: 0x17000EC7 RID: 3783
		// (get) Token: 0x06005C0F RID: 23567 RVA: 0x002EC338 File Offset: 0x002EA738
		public string Summary
		{
			get
			{
				return this.count + "x " + ((this.thingDef == null) ? "null" : this.thingDef.label);
			}
		}

		// Token: 0x06005C10 RID: 23568 RVA: 0x002EC382 File Offset: 0x002EA782
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x06005C11 RID: 23569 RVA: 0x002EC3A8 File Offset: 0x002EA7A8
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

		// Token: 0x06005C12 RID: 23570 RVA: 0x002EC41C File Offset: 0x002EA81C
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

		// Token: 0x06005C13 RID: 23571 RVA: 0x002EC488 File Offset: 0x002EA888
		public override int GetHashCode()
		{
			return (int)this.thingDef.shortHash + this.count << 16;
		}

		// Token: 0x06005C14 RID: 23572 RVA: 0x002EC4B4 File Offset: 0x002EA8B4
		public static implicit operator ThingDefCountClass(ThingDefCount t)
		{
			return new ThingDefCountClass(t.ThingDef, t.Count);
		}

		// Token: 0x04003CD3 RID: 15571
		public ThingDef thingDef;

		// Token: 0x04003CD4 RID: 15572
		public int count;
	}
}
