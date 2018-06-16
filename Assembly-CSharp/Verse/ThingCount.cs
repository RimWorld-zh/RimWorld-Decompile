using System;

namespace Verse
{
	// Token: 0x02000EFF RID: 3839
	public struct ThingCount : IEquatable<ThingCount>, IExposable
	{
		// Token: 0x06005BEE RID: 23534 RVA: 0x002EBA64 File Offset: 0x002E9E64
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

		// Token: 0x17000EC3 RID: 3779
		// (get) Token: 0x06005BEF RID: 23535 RVA: 0x002EBB14 File Offset: 0x002E9F14
		public Thing Thing
		{
			get
			{
				return this.thing;
			}
		}

		// Token: 0x17000EC4 RID: 3780
		// (get) Token: 0x06005BF0 RID: 23536 RVA: 0x002EBB30 File Offset: 0x002E9F30
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x06005BF1 RID: 23537 RVA: 0x002EBB4B File Offset: 0x002E9F4B
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x06005BF2 RID: 23538 RVA: 0x002EBB74 File Offset: 0x002E9F74
		public ThingCount WithCount(int newCount)
		{
			return new ThingCount(this.thing, newCount);
		}

		// Token: 0x06005BF3 RID: 23539 RVA: 0x002EBB98 File Offset: 0x002E9F98
		public override bool Equals(object obj)
		{
			return obj is ThingCount && this.Equals((ThingCount)obj);
		}

		// Token: 0x06005BF4 RID: 23540 RVA: 0x002EBBCC File Offset: 0x002E9FCC
		public bool Equals(ThingCount other)
		{
			return this == other;
		}

		// Token: 0x06005BF5 RID: 23541 RVA: 0x002EBBF0 File Offset: 0x002E9FF0
		public static bool operator ==(ThingCount a, ThingCount b)
		{
			return a.thing == b.thing && a.count == b.count;
		}

		// Token: 0x06005BF6 RID: 23542 RVA: 0x002EBC2C File Offset: 0x002EA02C
		public static bool operator !=(ThingCount a, ThingCount b)
		{
			return !(a == b);
		}

		// Token: 0x06005BF7 RID: 23543 RVA: 0x002EBC4C File Offset: 0x002EA04C
		public override int GetHashCode()
		{
			return Gen.HashCombine<Thing>(this.count, this.thing);
		}

		// Token: 0x06005BF8 RID: 23544 RVA: 0x002EBC74 File Offset: 0x002EA074
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

		// Token: 0x06005BF9 RID: 23545 RVA: 0x002EBCE0 File Offset: 0x002EA0E0
		public static implicit operator ThingCount(ThingCountClass t)
		{
			return new ThingCount(t.thing, t.Count);
		}

		// Token: 0x04003CCE RID: 15566
		private Thing thing;

		// Token: 0x04003CCF RID: 15567
		private int count;
	}
}
