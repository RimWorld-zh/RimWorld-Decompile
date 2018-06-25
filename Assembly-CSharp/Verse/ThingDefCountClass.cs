using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000F07 RID: 3847
	public sealed class ThingDefCountClass : IExposable
	{
		// Token: 0x04003CF1 RID: 15601
		public ThingDef thingDef;

		// Token: 0x04003CF2 RID: 15602
		public int count;

		// Token: 0x06005C3F RID: 23615 RVA: 0x002EEBA6 File Offset: 0x002ECFA6
		public ThingDefCountClass()
		{
		}

		// Token: 0x06005C40 RID: 23616 RVA: 0x002EEBB0 File Offset: 0x002ECFB0
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

		// Token: 0x17000ECA RID: 3786
		// (get) Token: 0x06005C41 RID: 23617 RVA: 0x002EEC0C File Offset: 0x002ED00C
		public string Summary
		{
			get
			{
				return this.count + "x " + ((this.thingDef == null) ? "null" : this.thingDef.label);
			}
		}

		// Token: 0x06005C42 RID: 23618 RVA: 0x002EEC56 File Offset: 0x002ED056
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x06005C43 RID: 23619 RVA: 0x002EEC7C File Offset: 0x002ED07C
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

		// Token: 0x06005C44 RID: 23620 RVA: 0x002EECF0 File Offset: 0x002ED0F0
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

		// Token: 0x06005C45 RID: 23621 RVA: 0x002EED5C File Offset: 0x002ED15C
		public override int GetHashCode()
		{
			return (int)this.thingDef.shortHash + this.count << 16;
		}

		// Token: 0x06005C46 RID: 23622 RVA: 0x002EED88 File Offset: 0x002ED188
		public static implicit operator ThingDefCountClass(ThingDefCount t)
		{
			return new ThingDefCountClass(t.ThingDef, t.Count);
		}
	}
}
