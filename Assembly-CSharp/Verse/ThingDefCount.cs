using System;

namespace Verse
{
	// Token: 0x02000F02 RID: 3842
	public struct ThingDefCount : IEquatable<ThingDefCount>, IExposable
	{
		// Token: 0x06005C03 RID: 23555 RVA: 0x002EBFB0 File Offset: 0x002EA3B0
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

		// Token: 0x17000EC6 RID: 3782
		// (get) Token: 0x06005C04 RID: 23556 RVA: 0x002EC008 File Offset: 0x002EA408
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
		}

		// Token: 0x17000EC7 RID: 3783
		// (get) Token: 0x06005C05 RID: 23557 RVA: 0x002EC024 File Offset: 0x002EA424
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x06005C06 RID: 23558 RVA: 0x002EC03F File Offset: 0x002EA43F
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x06005C07 RID: 23559 RVA: 0x002EC064 File Offset: 0x002EA464
		public ThingDefCount WithCount(int newCount)
		{
			return new ThingDefCount(this.thingDef, newCount);
		}

		// Token: 0x06005C08 RID: 23560 RVA: 0x002EC088 File Offset: 0x002EA488
		public override bool Equals(object obj)
		{
			return obj is ThingDefCount && this.Equals((ThingDefCount)obj);
		}

		// Token: 0x06005C09 RID: 23561 RVA: 0x002EC0BC File Offset: 0x002EA4BC
		public bool Equals(ThingDefCount other)
		{
			return this == other;
		}

		// Token: 0x06005C0A RID: 23562 RVA: 0x002EC0E0 File Offset: 0x002EA4E0
		public static bool operator ==(ThingDefCount a, ThingDefCount b)
		{
			return a.thingDef == b.thingDef && a.count == b.count;
		}

		// Token: 0x06005C0B RID: 23563 RVA: 0x002EC11C File Offset: 0x002EA51C
		public static bool operator !=(ThingDefCount a, ThingDefCount b)
		{
			return !(a == b);
		}

		// Token: 0x06005C0C RID: 23564 RVA: 0x002EC13C File Offset: 0x002EA53C
		public override int GetHashCode()
		{
			return Gen.HashCombine<ThingDef>(this.count, this.thingDef);
		}

		// Token: 0x06005C0D RID: 23565 RVA: 0x002EC164 File Offset: 0x002EA564
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

		// Token: 0x06005C0E RID: 23566 RVA: 0x002EC1D0 File Offset: 0x002EA5D0
		public static implicit operator ThingDefCount(ThingDefCountClass t)
		{
			return new ThingDefCount(t.thingDef, t.count);
		}

		// Token: 0x04003CD2 RID: 15570
		private ThingDef thingDef;

		// Token: 0x04003CD3 RID: 15571
		private int count;
	}
}
