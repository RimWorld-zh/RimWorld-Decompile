using System;

namespace Verse
{
	// Token: 0x02000F07 RID: 3847
	public struct ThingDefCountRange : IEquatable<ThingDefCountRange>, IExposable
	{
		// Token: 0x04003CEB RID: 15595
		private ThingDef thingDef;

		// Token: 0x04003CEC RID: 15596
		private IntRange countRange;

		// Token: 0x06005C47 RID: 23623 RVA: 0x002EEB90 File Offset: 0x002ECF90
		public ThingDefCountRange(ThingDef thingDef, int min, int max)
		{
			this = new ThingDefCountRange(thingDef, new IntRange(min, max));
		}

		// Token: 0x06005C48 RID: 23624 RVA: 0x002EEBA1 File Offset: 0x002ECFA1
		public ThingDefCountRange(ThingDef thingDef, IntRange countRange)
		{
			this.thingDef = thingDef;
			this.countRange = countRange;
		}

		// Token: 0x17000ECB RID: 3787
		// (get) Token: 0x06005C49 RID: 23625 RVA: 0x002EEBB4 File Offset: 0x002ECFB4
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
		}

		// Token: 0x17000ECC RID: 3788
		// (get) Token: 0x06005C4A RID: 23626 RVA: 0x002EEBD0 File Offset: 0x002ECFD0
		public IntRange CountRange
		{
			get
			{
				return this.countRange;
			}
		}

		// Token: 0x17000ECD RID: 3789
		// (get) Token: 0x06005C4B RID: 23627 RVA: 0x002EEBEC File Offset: 0x002ECFEC
		public int Min
		{
			get
			{
				return this.countRange.min;
			}
		}

		// Token: 0x17000ECE RID: 3790
		// (get) Token: 0x06005C4C RID: 23628 RVA: 0x002EEC0C File Offset: 0x002ED00C
		public int Max
		{
			get
			{
				return this.countRange.max;
			}
		}

		// Token: 0x17000ECF RID: 3791
		// (get) Token: 0x06005C4D RID: 23629 RVA: 0x002EEC2C File Offset: 0x002ED02C
		public int TrueMin
		{
			get
			{
				return this.countRange.TrueMin;
			}
		}

		// Token: 0x17000ED0 RID: 3792
		// (get) Token: 0x06005C4E RID: 23630 RVA: 0x002EEC4C File Offset: 0x002ED04C
		public int TrueMax
		{
			get
			{
				return this.countRange.TrueMax;
			}
		}

		// Token: 0x06005C4F RID: 23631 RVA: 0x002EEC6C File Offset: 0x002ED06C
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<IntRange>(ref this.countRange, "countRange", default(IntRange), false);
		}

		// Token: 0x06005C50 RID: 23632 RVA: 0x002EECA4 File Offset: 0x002ED0A4
		public ThingDefCountRange WithCountRange(IntRange newCountRange)
		{
			return new ThingDefCountRange(this.thingDef, newCountRange);
		}

		// Token: 0x06005C51 RID: 23633 RVA: 0x002EECC8 File Offset: 0x002ED0C8
		public ThingDefCountRange WithCountRange(int newMin, int newMax)
		{
			return new ThingDefCountRange(this.thingDef, newMin, newMax);
		}

		// Token: 0x06005C52 RID: 23634 RVA: 0x002EECEC File Offset: 0x002ED0EC
		public override bool Equals(object obj)
		{
			return obj is ThingDefCountRange && this.Equals((ThingDefCountRange)obj);
		}

		// Token: 0x06005C53 RID: 23635 RVA: 0x002EED20 File Offset: 0x002ED120
		public bool Equals(ThingDefCountRange other)
		{
			return this == other;
		}

		// Token: 0x06005C54 RID: 23636 RVA: 0x002EED44 File Offset: 0x002ED144
		public static bool operator ==(ThingDefCountRange a, ThingDefCountRange b)
		{
			return a.thingDef == b.thingDef && a.countRange == b.countRange;
		}

		// Token: 0x06005C55 RID: 23637 RVA: 0x002EED84 File Offset: 0x002ED184
		public static bool operator !=(ThingDefCountRange a, ThingDefCountRange b)
		{
			return !(a == b);
		}

		// Token: 0x06005C56 RID: 23638 RVA: 0x002EEDA4 File Offset: 0x002ED1A4
		public override int GetHashCode()
		{
			return Gen.HashCombine<ThingDef>(this.countRange.GetHashCode(), this.thingDef);
		}

		// Token: 0x06005C57 RID: 23639 RVA: 0x002EEDD8 File Offset: 0x002ED1D8
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

		// Token: 0x06005C58 RID: 23640 RVA: 0x002EEE44 File Offset: 0x002ED244
		public static implicit operator ThingDefCountRange(ThingDefCountRangeClass t)
		{
			return new ThingDefCountRange(t.thingDef, t.countRange);
		}

		// Token: 0x06005C59 RID: 23641 RVA: 0x002EEE6C File Offset: 0x002ED26C
		public static explicit operator ThingDefCountRange(ThingDefCount t)
		{
			return new ThingDefCountRange(t.ThingDef, t.Count, t.Count);
		}

		// Token: 0x06005C5A RID: 23642 RVA: 0x002EEE9C File Offset: 0x002ED29C
		public static explicit operator ThingDefCountRange(ThingDefCountClass t)
		{
			return new ThingDefCountRange(t.thingDef, t.count, t.count);
		}
	}
}
