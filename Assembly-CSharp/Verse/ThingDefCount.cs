using System;

namespace Verse
{
	// Token: 0x02000F06 RID: 3846
	public struct ThingDefCount : IEquatable<ThingDefCount>, IExposable
	{
		// Token: 0x04003CEF RID: 15599
		private ThingDef thingDef;

		// Token: 0x04003CF0 RID: 15600
		private int count;

		// Token: 0x06005C33 RID: 23603 RVA: 0x002EE960 File Offset: 0x002ECD60
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
		// (get) Token: 0x06005C34 RID: 23604 RVA: 0x002EE9B8 File Offset: 0x002ECDB8
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
		}

		// Token: 0x17000EC9 RID: 3785
		// (get) Token: 0x06005C35 RID: 23605 RVA: 0x002EE9D4 File Offset: 0x002ECDD4
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x06005C36 RID: 23606 RVA: 0x002EE9EF File Offset: 0x002ECDEF
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x06005C37 RID: 23607 RVA: 0x002EEA14 File Offset: 0x002ECE14
		public ThingDefCount WithCount(int newCount)
		{
			return new ThingDefCount(this.thingDef, newCount);
		}

		// Token: 0x06005C38 RID: 23608 RVA: 0x002EEA38 File Offset: 0x002ECE38
		public override bool Equals(object obj)
		{
			return obj is ThingDefCount && this.Equals((ThingDefCount)obj);
		}

		// Token: 0x06005C39 RID: 23609 RVA: 0x002EEA6C File Offset: 0x002ECE6C
		public bool Equals(ThingDefCount other)
		{
			return this == other;
		}

		// Token: 0x06005C3A RID: 23610 RVA: 0x002EEA90 File Offset: 0x002ECE90
		public static bool operator ==(ThingDefCount a, ThingDefCount b)
		{
			return a.thingDef == b.thingDef && a.count == b.count;
		}

		// Token: 0x06005C3B RID: 23611 RVA: 0x002EEACC File Offset: 0x002ECECC
		public static bool operator !=(ThingDefCount a, ThingDefCount b)
		{
			return !(a == b);
		}

		// Token: 0x06005C3C RID: 23612 RVA: 0x002EEAEC File Offset: 0x002ECEEC
		public override int GetHashCode()
		{
			return Gen.HashCombine<ThingDef>(this.count, this.thingDef);
		}

		// Token: 0x06005C3D RID: 23613 RVA: 0x002EEB14 File Offset: 0x002ECF14
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

		// Token: 0x06005C3E RID: 23614 RVA: 0x002EEB80 File Offset: 0x002ECF80
		public static implicit operator ThingDefCount(ThingDefCountClass t)
		{
			return new ThingDefCount(t.thingDef, t.count);
		}
	}
}
