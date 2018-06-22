using System;

namespace Verse
{
	// Token: 0x02000EFE RID: 3838
	public struct ThingCount : IEquatable<ThingCount>, IExposable
	{
		// Token: 0x06005C14 RID: 23572 RVA: 0x002EDB74 File Offset: 0x002EBF74
		public ThingCount(Thing thing, int count)
		{
			if (count < 0)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to set ThingCount stack count to ",
					count,
					". thing=",
					thing
				}), false);
				count = 0;
			}
			if (count > thing.stackCount)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to set ThingCount stack count to ",
					count,
					", but thing's stack count is only ",
					thing.stackCount,
					". thing=",
					thing
				}), false);
				count = thing.stackCount;
			}
			this.thing = thing;
			this.count = count;
		}

		// Token: 0x17000EC6 RID: 3782
		// (get) Token: 0x06005C15 RID: 23573 RVA: 0x002EDC24 File Offset: 0x002EC024
		public Thing Thing
		{
			get
			{
				return this.thing;
			}
		}

		// Token: 0x17000EC7 RID: 3783
		// (get) Token: 0x06005C16 RID: 23574 RVA: 0x002EDC40 File Offset: 0x002EC040
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x06005C17 RID: 23575 RVA: 0x002EDC5B File Offset: 0x002EC05B
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x06005C18 RID: 23576 RVA: 0x002EDC84 File Offset: 0x002EC084
		public ThingCount WithCount(int newCount)
		{
			return new ThingCount(this.thing, newCount);
		}

		// Token: 0x06005C19 RID: 23577 RVA: 0x002EDCA8 File Offset: 0x002EC0A8
		public override bool Equals(object obj)
		{
			return obj is ThingCount && this.Equals((ThingCount)obj);
		}

		// Token: 0x06005C1A RID: 23578 RVA: 0x002EDCDC File Offset: 0x002EC0DC
		public bool Equals(ThingCount other)
		{
			return this == other;
		}

		// Token: 0x06005C1B RID: 23579 RVA: 0x002EDD00 File Offset: 0x002EC100
		public static bool operator ==(ThingCount a, ThingCount b)
		{
			return a.thing == b.thing && a.count == b.count;
		}

		// Token: 0x06005C1C RID: 23580 RVA: 0x002EDD3C File Offset: 0x002EC13C
		public static bool operator !=(ThingCount a, ThingCount b)
		{
			return !(a == b);
		}

		// Token: 0x06005C1D RID: 23581 RVA: 0x002EDD5C File Offset: 0x002EC15C
		public override int GetHashCode()
		{
			return Gen.HashCombine<Thing>(this.count, this.thing);
		}

		// Token: 0x06005C1E RID: 23582 RVA: 0x002EDD84 File Offset: 0x002EC184
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.count,
				"x ",
				(this.thing == null) ? "null" : this.thing.LabelShort,
				")"
			});
		}

		// Token: 0x06005C1F RID: 23583 RVA: 0x002EDDF0 File Offset: 0x002EC1F0
		public static implicit operator ThingCount(ThingCountClass t)
		{
			return new ThingCount(t.thing, t.Count);
		}

		// Token: 0x04003CE0 RID: 15584
		private Thing thing;

		// Token: 0x04003CE1 RID: 15585
		private int count;
	}
}
