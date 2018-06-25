using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000F08 RID: 3848
	public sealed class ThingDefCountRangeClass : IExposable
	{
		// Token: 0x04003CED RID: 15597
		public ThingDef thingDef;

		// Token: 0x04003CEE RID: 15598
		public IntRange countRange;

		// Token: 0x06005C5B RID: 23643 RVA: 0x002EEEC8 File Offset: 0x002ED2C8
		public ThingDefCountRangeClass()
		{
		}

		// Token: 0x06005C5C RID: 23644 RVA: 0x002EEED1 File Offset: 0x002ED2D1
		public ThingDefCountRangeClass(ThingDef thingDef, int min, int max) : this(thingDef, new IntRange(min, max))
		{
		}

		// Token: 0x06005C5D RID: 23645 RVA: 0x002EEEE2 File Offset: 0x002ED2E2
		public ThingDefCountRangeClass(ThingDef thingDef, IntRange countRange)
		{
			this.thingDef = thingDef;
			this.countRange = countRange;
		}

		// Token: 0x17000ED1 RID: 3793
		// (get) Token: 0x06005C5E RID: 23646 RVA: 0x002EEEFC File Offset: 0x002ED2FC
		public int Min
		{
			get
			{
				return this.countRange.min;
			}
		}

		// Token: 0x17000ED2 RID: 3794
		// (get) Token: 0x06005C5F RID: 23647 RVA: 0x002EEF1C File Offset: 0x002ED31C
		public int Max
		{
			get
			{
				return this.countRange.max;
			}
		}

		// Token: 0x17000ED3 RID: 3795
		// (get) Token: 0x06005C60 RID: 23648 RVA: 0x002EEF3C File Offset: 0x002ED33C
		public int TrueMin
		{
			get
			{
				return this.countRange.TrueMin;
			}
		}

		// Token: 0x17000ED4 RID: 3796
		// (get) Token: 0x06005C61 RID: 23649 RVA: 0x002EEF5C File Offset: 0x002ED35C
		public int TrueMax
		{
			get
			{
				return this.countRange.TrueMax;
			}
		}

		// Token: 0x06005C62 RID: 23650 RVA: 0x002EEF7C File Offset: 0x002ED37C
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<IntRange>(ref this.countRange, "countRange", default(IntRange), false);
		}

		// Token: 0x06005C63 RID: 23651 RVA: 0x002EEFB4 File Offset: 0x002ED3B4
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

		// Token: 0x06005C64 RID: 23652 RVA: 0x002EF028 File Offset: 0x002ED428
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

		// Token: 0x06005C65 RID: 23653 RVA: 0x002EF094 File Offset: 0x002ED494
		public static implicit operator ThingDefCountRangeClass(ThingDefCountRange t)
		{
			return new ThingDefCountRangeClass(t.ThingDef, t.CountRange);
		}

		// Token: 0x06005C66 RID: 23654 RVA: 0x002EF0BC File Offset: 0x002ED4BC
		public static explicit operator ThingDefCountRangeClass(ThingDefCount t)
		{
			return new ThingDefCountRangeClass(t.ThingDef, t.Count, t.Count);
		}

		// Token: 0x06005C67 RID: 23655 RVA: 0x002EF0EC File Offset: 0x002ED4EC
		public static explicit operator ThingDefCountRangeClass(ThingDefCountClass t)
		{
			return new ThingDefCountRangeClass(t.thingDef, t.count, t.count);
		}
	}
}
