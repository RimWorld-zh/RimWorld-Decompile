using System;

namespace Verse
{
	// Token: 0x02000F03 RID: 3843
	public struct ThingDefCountRange : IEquatable<ThingDefCountRange>, IExposable
	{
		// Token: 0x04003CE8 RID: 15592
		private ThingDef thingDef;

		// Token: 0x04003CE9 RID: 15593
		private IntRange countRange;

		// Token: 0x06005C3D RID: 23613 RVA: 0x002EE510 File Offset: 0x002EC910
		public ThingDefCountRange(ThingDef thingDef, int min, int max)
		{
			this = new ThingDefCountRange(thingDef, new IntRange(min, max));
		}

		// Token: 0x06005C3E RID: 23614 RVA: 0x002EE521 File Offset: 0x002EC921
		public ThingDefCountRange(ThingDef thingDef, IntRange countRange)
		{
			this.thingDef = thingDef;
			this.countRange = countRange;
		}

		// Token: 0x17000ECC RID: 3788
		// (get) Token: 0x06005C3F RID: 23615 RVA: 0x002EE534 File Offset: 0x002EC934
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
		}

		// Token: 0x17000ECD RID: 3789
		// (get) Token: 0x06005C40 RID: 23616 RVA: 0x002EE550 File Offset: 0x002EC950
		public IntRange CountRange
		{
			get
			{
				return this.countRange;
			}
		}

		// Token: 0x17000ECE RID: 3790
		// (get) Token: 0x06005C41 RID: 23617 RVA: 0x002EE56C File Offset: 0x002EC96C
		public int Min
		{
			get
			{
				return this.countRange.min;
			}
		}

		// Token: 0x17000ECF RID: 3791
		// (get) Token: 0x06005C42 RID: 23618 RVA: 0x002EE58C File Offset: 0x002EC98C
		public int Max
		{
			get
			{
				return this.countRange.max;
			}
		}

		// Token: 0x17000ED0 RID: 3792
		// (get) Token: 0x06005C43 RID: 23619 RVA: 0x002EE5AC File Offset: 0x002EC9AC
		public int TrueMin
		{
			get
			{
				return this.countRange.TrueMin;
			}
		}

		// Token: 0x17000ED1 RID: 3793
		// (get) Token: 0x06005C44 RID: 23620 RVA: 0x002EE5CC File Offset: 0x002EC9CC
		public int TrueMax
		{
			get
			{
				return this.countRange.TrueMax;
			}
		}

		// Token: 0x06005C45 RID: 23621 RVA: 0x002EE5EC File Offset: 0x002EC9EC
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<IntRange>(ref this.countRange, "countRange", default(IntRange), false);
		}

		// Token: 0x06005C46 RID: 23622 RVA: 0x002EE624 File Offset: 0x002ECA24
		public ThingDefCountRange WithCountRange(IntRange newCountRange)
		{
			return new ThingDefCountRange(this.thingDef, newCountRange);
		}

		// Token: 0x06005C47 RID: 23623 RVA: 0x002EE648 File Offset: 0x002ECA48
		public ThingDefCountRange WithCountRange(int newMin, int newMax)
		{
			return new ThingDefCountRange(this.thingDef, newMin, newMax);
		}

		// Token: 0x06005C48 RID: 23624 RVA: 0x002EE66C File Offset: 0x002ECA6C
		public override bool Equals(object obj)
		{
			return obj is ThingDefCountRange && this.Equals((ThingDefCountRange)obj);
		}

		// Token: 0x06005C49 RID: 23625 RVA: 0x002EE6A0 File Offset: 0x002ECAA0
		public bool Equals(ThingDefCountRange other)
		{
			return this == other;
		}

		// Token: 0x06005C4A RID: 23626 RVA: 0x002EE6C4 File Offset: 0x002ECAC4
		public static bool operator ==(ThingDefCountRange a, ThingDefCountRange b)
		{
			return a.thingDef == b.thingDef && a.countRange == b.countRange;
		}

		// Token: 0x06005C4B RID: 23627 RVA: 0x002EE704 File Offset: 0x002ECB04
		public static bool operator !=(ThingDefCountRange a, ThingDefCountRange b)
		{
			return !(a == b);
		}

		// Token: 0x06005C4C RID: 23628 RVA: 0x002EE724 File Offset: 0x002ECB24
		public override int GetHashCode()
		{
			return Gen.HashCombine<ThingDef>(this.countRange.GetHashCode(), this.thingDef);
		}

		// Token: 0x06005C4D RID: 23629 RVA: 0x002EE758 File Offset: 0x002ECB58
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

		// Token: 0x06005C4E RID: 23630 RVA: 0x002EE7C4 File Offset: 0x002ECBC4
		public static implicit operator ThingDefCountRange(ThingDefCountRangeClass t)
		{
			return new ThingDefCountRange(t.thingDef, t.countRange);
		}

		// Token: 0x06005C4F RID: 23631 RVA: 0x002EE7EC File Offset: 0x002ECBEC
		public static explicit operator ThingDefCountRange(ThingDefCount t)
		{
			return new ThingDefCountRange(t.ThingDef, t.Count, t.Count);
		}

		// Token: 0x06005C50 RID: 23632 RVA: 0x002EE81C File Offset: 0x002ECC1C
		public static explicit operator ThingDefCountRange(ThingDefCountClass t)
		{
			return new ThingDefCountRange(t.thingDef, t.count, t.count);
		}
	}
}
