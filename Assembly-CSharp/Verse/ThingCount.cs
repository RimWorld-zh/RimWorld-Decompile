using System;

namespace Verse
{
	// Token: 0x02000F02 RID: 3842
	public struct ThingCount : IEquatable<ThingCount>, IExposable
	{
		// Token: 0x04003CE3 RID: 15587
		private Thing thing;

		// Token: 0x04003CE4 RID: 15588
		private int count;

		// Token: 0x06005C1E RID: 23582 RVA: 0x002EE1F4 File Offset: 0x002EC5F4
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

		// Token: 0x17000EC5 RID: 3781
		// (get) Token: 0x06005C1F RID: 23583 RVA: 0x002EE2A4 File Offset: 0x002EC6A4
		public Thing Thing
		{
			get
			{
				return this.thing;
			}
		}

		// Token: 0x17000EC6 RID: 3782
		// (get) Token: 0x06005C20 RID: 23584 RVA: 0x002EE2C0 File Offset: 0x002EC6C0
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x06005C21 RID: 23585 RVA: 0x002EE2DB File Offset: 0x002EC6DB
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x06005C22 RID: 23586 RVA: 0x002EE304 File Offset: 0x002EC704
		public ThingCount WithCount(int newCount)
		{
			return new ThingCount(this.thing, newCount);
		}

		// Token: 0x06005C23 RID: 23587 RVA: 0x002EE328 File Offset: 0x002EC728
		public override bool Equals(object obj)
		{
			return obj is ThingCount && this.Equals((ThingCount)obj);
		}

		// Token: 0x06005C24 RID: 23588 RVA: 0x002EE35C File Offset: 0x002EC75C
		public bool Equals(ThingCount other)
		{
			return this == other;
		}

		// Token: 0x06005C25 RID: 23589 RVA: 0x002EE380 File Offset: 0x002EC780
		public static bool operator ==(ThingCount a, ThingCount b)
		{
			return a.thing == b.thing && a.count == b.count;
		}

		// Token: 0x06005C26 RID: 23590 RVA: 0x002EE3BC File Offset: 0x002EC7BC
		public static bool operator !=(ThingCount a, ThingCount b)
		{
			return !(a == b);
		}

		// Token: 0x06005C27 RID: 23591 RVA: 0x002EE3DC File Offset: 0x002EC7DC
		public override int GetHashCode()
		{
			return Gen.HashCombine<Thing>(this.count, this.thing);
		}

		// Token: 0x06005C28 RID: 23592 RVA: 0x002EE404 File Offset: 0x002EC804
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

		// Token: 0x06005C29 RID: 23593 RVA: 0x002EE470 File Offset: 0x002EC870
		public static implicit operator ThingCount(ThingCountClass t)
		{
			return new ThingCount(t.thing, t.Count);
		}
	}
}
