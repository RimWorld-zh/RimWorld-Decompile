using System;

namespace Verse
{
	// Token: 0x02000F08 RID: 3848
	public struct ThingDefCountRange : IEquatable<ThingDefCountRange>, IExposable
	{
		// Token: 0x04003CF3 RID: 15603
		private ThingDef thingDef;

		// Token: 0x04003CF4 RID: 15604
		private IntRange countRange;

		// Token: 0x06005C47 RID: 23623 RVA: 0x002EEDB0 File Offset: 0x002ED1B0
		public ThingDefCountRange(ThingDef thingDef, int min, int max)
		{
			this = new ThingDefCountRange(thingDef, new IntRange(min, max));
		}

		// Token: 0x06005C48 RID: 23624 RVA: 0x002EEDC1 File Offset: 0x002ED1C1
		public ThingDefCountRange(ThingDef thingDef, IntRange countRange)
		{
			this.thingDef = thingDef;
			this.countRange = countRange;
		}

		// Token: 0x17000ECB RID: 3787
		// (get) Token: 0x06005C49 RID: 23625 RVA: 0x002EEDD4 File Offset: 0x002ED1D4
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
		}

		// Token: 0x17000ECC RID: 3788
		// (get) Token: 0x06005C4A RID: 23626 RVA: 0x002EEDF0 File Offset: 0x002ED1F0
		public IntRange CountRange
		{
			get
			{
				return this.countRange;
			}
		}

		// Token: 0x17000ECD RID: 3789
		// (get) Token: 0x06005C4B RID: 23627 RVA: 0x002EEE0C File Offset: 0x002ED20C
		public int Min
		{
			get
			{
				return this.countRange.min;
			}
		}

		// Token: 0x17000ECE RID: 3790
		// (get) Token: 0x06005C4C RID: 23628 RVA: 0x002EEE2C File Offset: 0x002ED22C
		public int Max
		{
			get
			{
				return this.countRange.max;
			}
		}

		// Token: 0x17000ECF RID: 3791
		// (get) Token: 0x06005C4D RID: 23629 RVA: 0x002EEE4C File Offset: 0x002ED24C
		public int TrueMin
		{
			get
			{
				return this.countRange.TrueMin;
			}
		}

		// Token: 0x17000ED0 RID: 3792
		// (get) Token: 0x06005C4E RID: 23630 RVA: 0x002EEE6C File Offset: 0x002ED26C
		public int TrueMax
		{
			get
			{
				return this.countRange.TrueMax;
			}
		}

		// Token: 0x06005C4F RID: 23631 RVA: 0x002EEE8C File Offset: 0x002ED28C
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<IntRange>(ref this.countRange, "countRange", default(IntRange), false);
		}

		// Token: 0x06005C50 RID: 23632 RVA: 0x002EEEC4 File Offset: 0x002ED2C4
		public ThingDefCountRange WithCountRange(IntRange newCountRange)
		{
			return new ThingDefCountRange(this.thingDef, newCountRange);
		}

		// Token: 0x06005C51 RID: 23633 RVA: 0x002EEEE8 File Offset: 0x002ED2E8
		public ThingDefCountRange WithCountRange(int newMin, int newMax)
		{
			return new ThingDefCountRange(this.thingDef, newMin, newMax);
		}

		// Token: 0x06005C52 RID: 23634 RVA: 0x002EEF0C File Offset: 0x002ED30C
		public override bool Equals(object obj)
		{
			return obj is ThingDefCountRange && this.Equals((ThingDefCountRange)obj);
		}

		// Token: 0x06005C53 RID: 23635 RVA: 0x002EEF40 File Offset: 0x002ED340
		public bool Equals(ThingDefCountRange other)
		{
			return this == other;
		}

		// Token: 0x06005C54 RID: 23636 RVA: 0x002EEF64 File Offset: 0x002ED364
		public static bool operator ==(ThingDefCountRange a, ThingDefCountRange b)
		{
			return a.thingDef == b.thingDef && a.countRange == b.countRange;
		}

		// Token: 0x06005C55 RID: 23637 RVA: 0x002EEFA4 File Offset: 0x002ED3A4
		public static bool operator !=(ThingDefCountRange a, ThingDefCountRange b)
		{
			return !(a == b);
		}

		// Token: 0x06005C56 RID: 23638 RVA: 0x002EEFC4 File Offset: 0x002ED3C4
		public override int GetHashCode()
		{
			return Gen.HashCombine<ThingDef>(this.countRange.GetHashCode(), this.thingDef);
		}

		// Token: 0x06005C57 RID: 23639 RVA: 0x002EEFF8 File Offset: 0x002ED3F8
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

		// Token: 0x06005C58 RID: 23640 RVA: 0x002EF064 File Offset: 0x002ED464
		public static implicit operator ThingDefCountRange(ThingDefCountRangeClass t)
		{
			return new ThingDefCountRange(t.thingDef, t.countRange);
		}

		// Token: 0x06005C59 RID: 23641 RVA: 0x002EF08C File Offset: 0x002ED48C
		public static explicit operator ThingDefCountRange(ThingDefCount t)
		{
			return new ThingDefCountRange(t.ThingDef, t.Count, t.Count);
		}

		// Token: 0x06005C5A RID: 23642 RVA: 0x002EF0BC File Offset: 0x002ED4BC
		public static explicit operator ThingDefCountRange(ThingDefCountClass t)
		{
			return new ThingDefCountRange(t.thingDef, t.count, t.count);
		}
	}
}
