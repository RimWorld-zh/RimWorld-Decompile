using System;

namespace Verse
{
	// Token: 0x02000F04 RID: 3844
	public struct ThingDefCountRange : IEquatable<ThingDefCountRange>, IExposable
	{
		// Token: 0x06005C17 RID: 23575 RVA: 0x002EC400 File Offset: 0x002EA800
		public ThingDefCountRange(ThingDef thingDef, int min, int max)
		{
			this = new ThingDefCountRange(thingDef, new IntRange(min, max));
		}

		// Token: 0x06005C18 RID: 23576 RVA: 0x002EC411 File Offset: 0x002EA811
		public ThingDefCountRange(ThingDef thingDef, IntRange countRange)
		{
			this.thingDef = thingDef;
			this.countRange = countRange;
		}

		// Token: 0x17000EC9 RID: 3785
		// (get) Token: 0x06005C19 RID: 23577 RVA: 0x002EC424 File Offset: 0x002EA824
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
		}

		// Token: 0x17000ECA RID: 3786
		// (get) Token: 0x06005C1A RID: 23578 RVA: 0x002EC440 File Offset: 0x002EA840
		public IntRange CountRange
		{
			get
			{
				return this.countRange;
			}
		}

		// Token: 0x17000ECB RID: 3787
		// (get) Token: 0x06005C1B RID: 23579 RVA: 0x002EC45C File Offset: 0x002EA85C
		public int Min
		{
			get
			{
				return this.countRange.min;
			}
		}

		// Token: 0x17000ECC RID: 3788
		// (get) Token: 0x06005C1C RID: 23580 RVA: 0x002EC47C File Offset: 0x002EA87C
		public int Max
		{
			get
			{
				return this.countRange.max;
			}
		}

		// Token: 0x17000ECD RID: 3789
		// (get) Token: 0x06005C1D RID: 23581 RVA: 0x002EC49C File Offset: 0x002EA89C
		public int TrueMin
		{
			get
			{
				return this.countRange.TrueMin;
			}
		}

		// Token: 0x17000ECE RID: 3790
		// (get) Token: 0x06005C1E RID: 23582 RVA: 0x002EC4BC File Offset: 0x002EA8BC
		public int TrueMax
		{
			get
			{
				return this.countRange.TrueMax;
			}
		}

		// Token: 0x06005C1F RID: 23583 RVA: 0x002EC4DC File Offset: 0x002EA8DC
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<IntRange>(ref this.countRange, "countRange", default(IntRange), false);
		}

		// Token: 0x06005C20 RID: 23584 RVA: 0x002EC514 File Offset: 0x002EA914
		public ThingDefCountRange WithCountRange(IntRange newCountRange)
		{
			return new ThingDefCountRange(this.thingDef, newCountRange);
		}

		// Token: 0x06005C21 RID: 23585 RVA: 0x002EC538 File Offset: 0x002EA938
		public ThingDefCountRange WithCountRange(int newMin, int newMax)
		{
			return new ThingDefCountRange(this.thingDef, newMin, newMax);
		}

		// Token: 0x06005C22 RID: 23586 RVA: 0x002EC55C File Offset: 0x002EA95C
		public override bool Equals(object obj)
		{
			return obj is ThingDefCountRange && this.Equals((ThingDefCountRange)obj);
		}

		// Token: 0x06005C23 RID: 23587 RVA: 0x002EC590 File Offset: 0x002EA990
		public bool Equals(ThingDefCountRange other)
		{
			return this == other;
		}

		// Token: 0x06005C24 RID: 23588 RVA: 0x002EC5B4 File Offset: 0x002EA9B4
		public static bool operator ==(ThingDefCountRange a, ThingDefCountRange b)
		{
			return a.thingDef == b.thingDef && a.countRange == b.countRange;
		}

		// Token: 0x06005C25 RID: 23589 RVA: 0x002EC5F4 File Offset: 0x002EA9F4
		public static bool operator !=(ThingDefCountRange a, ThingDefCountRange b)
		{
			return !(a == b);
		}

		// Token: 0x06005C26 RID: 23590 RVA: 0x002EC614 File Offset: 0x002EAA14
		public override int GetHashCode()
		{
			return Gen.HashCombine<ThingDef>(this.countRange.GetHashCode(), this.thingDef);
		}

		// Token: 0x06005C27 RID: 23591 RVA: 0x002EC648 File Offset: 0x002EAA48
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

		// Token: 0x06005C28 RID: 23592 RVA: 0x002EC6B4 File Offset: 0x002EAAB4
		public static implicit operator ThingDefCountRange(ThingDefCountRangeClass t)
		{
			return new ThingDefCountRange(t.thingDef, t.countRange);
		}

		// Token: 0x06005C29 RID: 23593 RVA: 0x002EC6DC File Offset: 0x002EAADC
		public static explicit operator ThingDefCountRange(ThingDefCount t)
		{
			return new ThingDefCountRange(t.ThingDef, t.Count, t.Count);
		}

		// Token: 0x06005C2A RID: 23594 RVA: 0x002EC70C File Offset: 0x002EAB0C
		public static explicit operator ThingDefCountRange(ThingDefCountClass t)
		{
			return new ThingDefCountRange(t.thingDef, t.count, t.count);
		}

		// Token: 0x04003CD6 RID: 15574
		private ThingDef thingDef;

		// Token: 0x04003CD7 RID: 15575
		private IntRange countRange;
	}
}
