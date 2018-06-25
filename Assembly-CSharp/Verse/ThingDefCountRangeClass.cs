using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000F09 RID: 3849
	public sealed class ThingDefCountRangeClass : IExposable
	{
		// Token: 0x04003CF5 RID: 15605
		public ThingDef thingDef;

		// Token: 0x04003CF6 RID: 15606
		public IntRange countRange;

		// Token: 0x06005C5B RID: 23643 RVA: 0x002EF0E8 File Offset: 0x002ED4E8
		public ThingDefCountRangeClass()
		{
		}

		// Token: 0x06005C5C RID: 23644 RVA: 0x002EF0F1 File Offset: 0x002ED4F1
		public ThingDefCountRangeClass(ThingDef thingDef, int min, int max) : this(thingDef, new IntRange(min, max))
		{
		}

		// Token: 0x06005C5D RID: 23645 RVA: 0x002EF102 File Offset: 0x002ED502
		public ThingDefCountRangeClass(ThingDef thingDef, IntRange countRange)
		{
			this.thingDef = thingDef;
			this.countRange = countRange;
		}

		// Token: 0x17000ED1 RID: 3793
		// (get) Token: 0x06005C5E RID: 23646 RVA: 0x002EF11C File Offset: 0x002ED51C
		public int Min
		{
			get
			{
				return this.countRange.min;
			}
		}

		// Token: 0x17000ED2 RID: 3794
		// (get) Token: 0x06005C5F RID: 23647 RVA: 0x002EF13C File Offset: 0x002ED53C
		public int Max
		{
			get
			{
				return this.countRange.max;
			}
		}

		// Token: 0x17000ED3 RID: 3795
		// (get) Token: 0x06005C60 RID: 23648 RVA: 0x002EF15C File Offset: 0x002ED55C
		public int TrueMin
		{
			get
			{
				return this.countRange.TrueMin;
			}
		}

		// Token: 0x17000ED4 RID: 3796
		// (get) Token: 0x06005C61 RID: 23649 RVA: 0x002EF17C File Offset: 0x002ED57C
		public int TrueMax
		{
			get
			{
				return this.countRange.TrueMax;
			}
		}

		// Token: 0x06005C62 RID: 23650 RVA: 0x002EF19C File Offset: 0x002ED59C
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<IntRange>(ref this.countRange, "countRange", default(IntRange), false);
		}

		// Token: 0x06005C63 RID: 23651 RVA: 0x002EF1D4 File Offset: 0x002ED5D4
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured ThingDefCountRangeClass: " + xmlRoot.OuterXml, false);
			}
			else
			{
				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "thingDef", xmlRoot.Name);
				this.countRange = (IntRange)ParseHelper.FromString(xmlRoot.FirstChild.Value, typeof(IntRange));
			}
		}

		// Token: 0x06005C64 RID: 23652 RVA: 0x002EF248 File Offset: 0x002ED648
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.countRange,
				"x ",
				(this.thingDef == null) ? "null" : this.thingDef.defName,
				")"
			});
		}

		// Token: 0x06005C65 RID: 23653 RVA: 0x002EF2B4 File Offset: 0x002ED6B4
		public static implicit operator ThingDefCountRangeClass(ThingDefCountRange t)
		{
			return new ThingDefCountRangeClass(t.ThingDef, t.CountRange);
		}

		// Token: 0x06005C66 RID: 23654 RVA: 0x002EF2DC File Offset: 0x002ED6DC
		public static explicit operator ThingDefCountRangeClass(ThingDefCount t)
		{
			return new ThingDefCountRangeClass(t.ThingDef, t.Count, t.Count);
		}

		// Token: 0x06005C67 RID: 23655 RVA: 0x002EF30C File Offset: 0x002ED70C
		public static explicit operator ThingDefCountRangeClass(ThingDefCountClass t)
		{
			return new ThingDefCountRangeClass(t.thingDef, t.count, t.count);
		}
	}
}
