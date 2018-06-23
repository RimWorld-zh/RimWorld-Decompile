using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000F02 RID: 3842
	public sealed class ThingDefCountClass : IExposable
	{
		// Token: 0x04003CE6 RID: 15590
		public ThingDef thingDef;

		// Token: 0x04003CE7 RID: 15591
		public int count;

		// Token: 0x06005C35 RID: 23605 RVA: 0x002EE306 File Offset: 0x002EC706
		public ThingDefCountClass()
		{
		}

		// Token: 0x06005C36 RID: 23606 RVA: 0x002EE310 File Offset: 0x002EC710
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

		// Token: 0x17000ECB RID: 3787
		// (get) Token: 0x06005C37 RID: 23607 RVA: 0x002EE36C File Offset: 0x002EC76C
		public string Summary
		{
			get
			{
				return this.count + "x " + ((this.thingDef == null) ? "null" : this.thingDef.label);
			}
		}

		// Token: 0x06005C38 RID: 23608 RVA: 0x002EE3B6 File Offset: 0x002EC7B6
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x06005C39 RID: 23609 RVA: 0x002EE3DC File Offset: 0x002EC7DC
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

		// Token: 0x06005C3A RID: 23610 RVA: 0x002EE450 File Offset: 0x002EC850
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

		// Token: 0x06005C3B RID: 23611 RVA: 0x002EE4BC File Offset: 0x002EC8BC
		public override int GetHashCode()
		{
			return (int)this.thingDef.shortHash + this.count << 16;
		}

		// Token: 0x06005C3C RID: 23612 RVA: 0x002EE4E8 File Offset: 0x002EC8E8
		public static implicit operator ThingDefCountClass(ThingDefCount t)
		{
			return new ThingDefCountClass(t.ThingDef, t.Count);
		}
	}
}
