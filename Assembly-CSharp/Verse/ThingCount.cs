using System;

namespace Verse
{
	// Token: 0x02000EFE RID: 3838
	public struct ThingCount : IEquatable<ThingCount>, IExposable
	{
		// Token: 0x06005BEC RID: 23532 RVA: 0x002EBB40 File Offset: 0x002E9F40
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

		// Token: 0x17000EC2 RID: 3778
		// (get) Token: 0x06005BED RID: 23533 RVA: 0x002EBBF0 File Offset: 0x002E9FF0
		public Thing Thing
		{
			get
			{
				return this.thing;
			}
		}

		// Token: 0x17000EC3 RID: 3779
		// (get) Token: 0x06005BEE RID: 23534 RVA: 0x002EBC0C File Offset: 0x002EA00C
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x06005BEF RID: 23535 RVA: 0x002EBC27 File Offset: 0x002EA027
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x06005BF0 RID: 23536 RVA: 0x002EBC50 File Offset: 0x002EA050
		public ThingCount WithCount(int newCount)
		{
			return new ThingCount(this.thing, newCount);
		}

		// Token: 0x06005BF1 RID: 23537 RVA: 0x002EBC74 File Offset: 0x002EA074
		public override bool Equals(object obj)
		{
			return obj is ThingCount && this.Equals((ThingCount)obj);
		}

		// Token: 0x06005BF2 RID: 23538 RVA: 0x002EBCA8 File Offset: 0x002EA0A8
		public bool Equals(ThingCount other)
		{
			return this == other;
		}

		// Token: 0x06005BF3 RID: 23539 RVA: 0x002EBCCC File Offset: 0x002EA0CC
		public static bool operator ==(ThingCount a, ThingCount b)
		{
			return a.thing == b.thing && a.count == b.count;
		}

		// Token: 0x06005BF4 RID: 23540 RVA: 0x002EBD08 File Offset: 0x002EA108
		public static bool operator !=(ThingCount a, ThingCount b)
		{
			return !(a == b);
		}

		// Token: 0x06005BF5 RID: 23541 RVA: 0x002EBD28 File Offset: 0x002EA128
		public override int GetHashCode()
		{
			return Gen.HashCombine<Thing>(this.count, this.thing);
		}

		// Token: 0x06005BF6 RID: 23542 RVA: 0x002EBD50 File Offset: 0x002EA150
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

		// Token: 0x06005BF7 RID: 23543 RVA: 0x002EBDBC File Offset: 0x002EA1BC
		public static implicit operator ThingCount(ThingCountClass t)
		{
			return new ThingCount(t.thing, t.Count);
		}

		// Token: 0x04003CCD RID: 15565
		private Thing thing;

		// Token: 0x04003CCE RID: 15566
		private int count;
	}
}
