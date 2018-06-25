using System;

namespace Verse
{
	// Token: 0x02000F05 RID: 3845
	public struct ThingDefCount : IEquatable<ThingDefCount>, IExposable
	{
		// Token: 0x04003CE7 RID: 15591
		private ThingDef thingDef;

		// Token: 0x04003CE8 RID: 15592
		private int count;

		// Token: 0x06005C33 RID: 23603 RVA: 0x002EE740 File Offset: 0x002ECB40
		public ThingDefCount(ThingDef thingDef, int count)
		{
			if (count < 0)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to set ThingDefCount count to ",
					count,
					". thingDef=",
					thingDef
				}), false);
				count = 0;
			}
			this.thingDef = thingDef;
			this.count = count;
		}

		// Token: 0x17000EC8 RID: 3784
		// (get) Token: 0x06005C34 RID: 23604 RVA: 0x002EE798 File Offset: 0x002ECB98
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
		}

		// Token: 0x17000EC9 RID: 3785
		// (get) Token: 0x06005C35 RID: 23605 RVA: 0x002EE7B4 File Offset: 0x002ECBB4
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x06005C36 RID: 23606 RVA: 0x002EE7CF File Offset: 0x002ECBCF
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x06005C37 RID: 23607 RVA: 0x002EE7F4 File Offset: 0x002ECBF4
		public ThingDefCount WithCount(int newCount)
		{
			return new ThingDefCount(this.thingDef, newCount);
		}

		// Token: 0x06005C38 RID: 23608 RVA: 0x002EE818 File Offset: 0x002ECC18
		public override bool Equals(object obj)
		{
			return obj is ThingDefCount && this.Equals((ThingDefCount)obj);
		}

		// Token: 0x06005C39 RID: 23609 RVA: 0x002EE84C File Offset: 0x002ECC4C
		public bool Equals(ThingDefCount other)
		{
			return this == other;
		}

		// Token: 0x06005C3A RID: 23610 RVA: 0x002EE870 File Offset: 0x002ECC70
		public static bool operator ==(ThingDefCount a, ThingDefCount b)
		{
			return a.thingDef == b.thingDef && a.count == b.count;
		}

		// Token: 0x06005C3B RID: 23611 RVA: 0x002EE8AC File Offset: 0x002ECCAC
		public static bool operator !=(ThingDefCount a, ThingDefCount b)
		{
			return !(a == b);
		}

		// Token: 0x06005C3C RID: 23612 RVA: 0x002EE8CC File Offset: 0x002ECCCC
		public override int GetHashCode()
		{
			return Gen.HashCombine<ThingDef>(this.count, this.thingDef);
		}

		// Token: 0x06005C3D RID: 23613 RVA: 0x002EE8F4 File Offset: 0x002ECCF4
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

		// Token: 0x06005C3E RID: 23614 RVA: 0x002EE960 File Offset: 0x002ECD60
		public static implicit operator ThingDefCount(ThingDefCountClass t)
		{
			return new ThingDefCount(t.thingDef, t.count);
		}
	}
}
