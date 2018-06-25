using System;

namespace Verse
{
	// Token: 0x02000F03 RID: 3843
	public struct ThingCount : IEquatable<ThingCount>, IExposable
	{
		// Token: 0x04003CEB RID: 15595
		private Thing thing;

		// Token: 0x04003CEC RID: 15596
		private int count;

		// Token: 0x06005C1E RID: 23582 RVA: 0x002EE414 File Offset: 0x002EC814
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
		// (get) Token: 0x06005C1F RID: 23583 RVA: 0x002EE4C4 File Offset: 0x002EC8C4
		public Thing Thing
		{
			get
			{
				return this.thing;
			}
		}

		// Token: 0x17000EC6 RID: 3782
		// (get) Token: 0x06005C20 RID: 23584 RVA: 0x002EE4E0 File Offset: 0x002EC8E0
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x06005C21 RID: 23585 RVA: 0x002EE4FB File Offset: 0x002EC8FB
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x06005C22 RID: 23586 RVA: 0x002EE524 File Offset: 0x002EC924
		public ThingCount WithCount(int newCount)
		{
			return new ThingCount(this.thing, newCount);
		}

		// Token: 0x06005C23 RID: 23587 RVA: 0x002EE548 File Offset: 0x002EC948
		public override bool Equals(object obj)
		{
			return obj is ThingCount && this.Equals((ThingCount)obj);
		}

		// Token: 0x06005C24 RID: 23588 RVA: 0x002EE57C File Offset: 0x002EC97C
		public bool Equals(ThingCount other)
		{
			return this == other;
		}

		// Token: 0x06005C25 RID: 23589 RVA: 0x002EE5A0 File Offset: 0x002EC9A0
		public static bool operator ==(ThingCount a, ThingCount b)
		{
			return a.thing == b.thing && a.count == b.count;
		}

		// Token: 0x06005C26 RID: 23590 RVA: 0x002EE5DC File Offset: 0x002EC9DC
		public static bool operator !=(ThingCount a, ThingCount b)
		{
			return !(a == b);
		}

		// Token: 0x06005C27 RID: 23591 RVA: 0x002EE5FC File Offset: 0x002EC9FC
		public override int GetHashCode()
		{
			return Gen.HashCombine<Thing>(this.count, this.thing);
		}

		// Token: 0x06005C28 RID: 23592 RVA: 0x002EE624 File Offset: 0x002ECA24
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

		// Token: 0x06005C29 RID: 23593 RVA: 0x002EE690 File Offset: 0x002ECA90
		public static implicit operator ThingCount(ThingCountClass t)
		{
			return new ThingCount(t.thing, t.Count);
		}
	}
}
