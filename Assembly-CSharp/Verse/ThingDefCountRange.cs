using System;

namespace Verse
{
	// Token: 0x02000F03 RID: 3843
	public struct ThingDefCountRange : IEquatable<ThingDefCountRange>, IExposable
	{
		// Token: 0x06005C15 RID: 23573 RVA: 0x002EC4DC File Offset: 0x002EA8DC
		public ThingDefCountRange(ThingDef thingDef, int min, int max)
		{
			this = new ThingDefCountRange(thingDef, new IntRange(min, max));
		}

		// Token: 0x06005C16 RID: 23574 RVA: 0x002EC4ED File Offset: 0x002EA8ED
		public ThingDefCountRange(ThingDef thingDef, IntRange countRange)
		{
			this.thingDef = thingDef;
			this.countRange = countRange;
		}

		// Token: 0x17000EC8 RID: 3784
		// (get) Token: 0x06005C17 RID: 23575 RVA: 0x002EC500 File Offset: 0x002EA900
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
		}

		// Token: 0x17000EC9 RID: 3785
		// (get) Token: 0x06005C18 RID: 23576 RVA: 0x002EC51C File Offset: 0x002EA91C
		public IntRange CountRange
		{
			get
			{
				return this.countRange;
			}
		}

		// Token: 0x17000ECA RID: 3786
		// (get) Token: 0x06005C19 RID: 23577 RVA: 0x002EC538 File Offset: 0x002EA938
		public int Min
		{
			get
			{
				return this.countRange.min;
			}
		}

		// Token: 0x17000ECB RID: 3787
		// (get) Token: 0x06005C1A RID: 23578 RVA: 0x002EC558 File Offset: 0x002EA958
		public int Max
		{
			get
			{
				return this.countRange.max;
			}
		}

		// Token: 0x17000ECC RID: 3788
		// (get) Token: 0x06005C1B RID: 23579 RVA: 0x002EC578 File Offset: 0x002EA978
		public int TrueMin
		{
			get
			{
				return this.countRange.TrueMin;
			}
		}

		// Token: 0x17000ECD RID: 3789
		// (get) Token: 0x06005C1C RID: 23580 RVA: 0x002EC598 File Offset: 0x002EA998
		public int TrueMax
		{
			get
			{
				return this.countRange.TrueMax;
			}
		}

		// Token: 0x06005C1D RID: 23581 RVA: 0x002EC5B8 File Offset: 0x002EA9B8
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<IntRange>(ref this.countRange, "countRange", default(IntRange), false);
		}

		// Token: 0x06005C1E RID: 23582 RVA: 0x002EC5F0 File Offset: 0x002EA9F0
		public ThingDefCountRange WithCountRange(IntRange newCountRange)
		{
			return new ThingDefCountRange(this.thingDef, newCountRange);
		}

		// Token: 0x06005C1F RID: 23583 RVA: 0x002EC614 File Offset: 0x002EAA14
		public ThingDefCountRange WithCountRange(int newMin, int newMax)
		{
			return new ThingDefCountRange(this.thingDef, newMin, newMax);
		}

		// Token: 0x06005C20 RID: 23584 RVA: 0x002EC638 File Offset: 0x002EAA38
		public override bool Equals(object obj)
		{
			return obj is ThingDefCountRange && this.Equals((ThingDefCountRange)obj);
		}

		// Token: 0x06005C21 RID: 23585 RVA: 0x002EC66C File Offset: 0x002EAA6C
		public bool Equals(ThingDefCountRange other)
		{
			return this == other;
		}

		// Token: 0x06005C22 RID: 23586 RVA: 0x002EC690 File Offset: 0x002EAA90
		public static bool operator ==(ThingDefCountRange a, ThingDefCountRange b)
		{
			return a.thingDef == b.thingDef && a.countRange == b.countRange;
		}

		// Token: 0x06005C23 RID: 23587 RVA: 0x002EC6D0 File Offset: 0x002EAAD0
		public static bool operator !=(ThingDefCountRange a, ThingDefCountRange b)
		{
			return !(a == b);
		}

		// Token: 0x06005C24 RID: 23588 RVA: 0x002EC6F0 File Offset: 0x002EAAF0
		public override int GetHashCode()
		{
			return Gen.HashCombine<ThingDef>(this.countRange.GetHashCode(), this.thingDef);
		}

		// Token: 0x06005C25 RID: 23589 RVA: 0x002EC724 File Offset: 0x002EAB24
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

		// Token: 0x06005C26 RID: 23590 RVA: 0x002EC790 File Offset: 0x002EAB90
		public static implicit operator ThingDefCountRange(ThingDefCountRangeClass t)
		{
			return new ThingDefCountRange(t.thingDef, t.countRange);
		}

		// Token: 0x06005C27 RID: 23591 RVA: 0x002EC7B8 File Offset: 0x002EABB8
		public static explicit operator ThingDefCountRange(ThingDefCount t)
		{
			return new ThingDefCountRange(t.ThingDef, t.Count, t.Count);
		}

		// Token: 0x06005C28 RID: 23592 RVA: 0x002EC7E8 File Offset: 0x002EABE8
		public static explicit operator ThingDefCountRange(ThingDefCountClass t)
		{
			return new ThingDefCountRange(t.thingDef, t.count, t.count);
		}

		// Token: 0x04003CD5 RID: 15573
		private ThingDef thingDef;

		// Token: 0x04003CD6 RID: 15574
		private IntRange countRange;
	}
}
