using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000F04 RID: 3844
	public sealed class ThingDefCountRangeClass : IExposable
	{
		// Token: 0x04003CEA RID: 15594
		public ThingDef thingDef;

		// Token: 0x04003CEB RID: 15595
		public IntRange countRange;

		// Token: 0x06005C51 RID: 23633 RVA: 0x002EE848 File Offset: 0x002ECC48
		public ThingDefCountRangeClass()
		{
		}

		// Token: 0x06005C52 RID: 23634 RVA: 0x002EE851 File Offset: 0x002ECC51
		public ThingDefCountRangeClass(ThingDef thingDef, int min, int max) : this(thingDef, new IntRange(min, max))
		{
		}

		// Token: 0x06005C53 RID: 23635 RVA: 0x002EE862 File Offset: 0x002ECC62
		public ThingDefCountRangeClass(ThingDef thingDef, IntRange countRange)
		{
			this.thingDef = thingDef;
			this.countRange = countRange;
		}

		// Token: 0x17000ED2 RID: 3794
		// (get) Token: 0x06005C54 RID: 23636 RVA: 0x002EE87C File Offset: 0x002ECC7C
		public int Min
		{
			get
			{
				return this.countRange.min;
			}
		}

		// Token: 0x17000ED3 RID: 3795
		// (get) Token: 0x06005C55 RID: 23637 RVA: 0x002EE89C File Offset: 0x002ECC9C
		public int Max
		{
			get
			{
				return this.countRange.max;
			}
		}

		// Token: 0x17000ED4 RID: 3796
		// (get) Token: 0x06005C56 RID: 23638 RVA: 0x002EE8BC File Offset: 0x002ECCBC
		public int TrueMin
		{
			get
			{
				return this.countRange.TrueMin;
			}
		}

		// Token: 0x17000ED5 RID: 3797
		// (get) Token: 0x06005C57 RID: 23639 RVA: 0x002EE8DC File Offset: 0x002ECCDC
		public int TrueMax
		{
			get
			{
				return this.countRange.TrueMax;
			}
		}

		// Token: 0x06005C58 RID: 23640 RVA: 0x002EE8FC File Offset: 0x002ECCFC
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<IntRange>(ref this.countRange, "countRange", default(IntRange), false);
		}

		// Token: 0x06005C59 RID: 23641 RVA: 0x002EE934 File Offset: 0x002ECD34
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

		// Token: 0x06005C5A RID: 23642 RVA: 0x002EE9A8 File Offset: 0x002ECDA8
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

		// Token: 0x06005C5B RID: 23643 RVA: 0x002EEA14 File Offset: 0x002ECE14
		public static implicit operator ThingDefCountRangeClass(ThingDefCountRange t)
		{
			return new ThingDefCountRangeClass(t.ThingDef, t.CountRange);
		}

		// Token: 0x06005C5C RID: 23644 RVA: 0x002EEA3C File Offset: 0x002ECE3C
		public static explicit operator ThingDefCountRangeClass(ThingDefCount t)
		{
			return new ThingDefCountRangeClass(t.ThingDef, t.Count, t.Count);
		}

		// Token: 0x06005C5D RID: 23645 RVA: 0x002EEA6C File Offset: 0x002ECE6C
		public static explicit operator ThingDefCountRangeClass(ThingDefCountClass t)
		{
			return new ThingDefCountRangeClass(t.thingDef, t.count, t.count);
		}
	}
}
