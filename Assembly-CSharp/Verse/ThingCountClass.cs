using System;

namespace Verse
{
	// Token: 0x02000F05 RID: 3845
	public sealed class ThingCountClass : IExposable
	{
		// Token: 0x04003CED RID: 15597
		public Thing thing;

		// Token: 0x04003CEE RID: 15598
		private int countInt;

		// Token: 0x06005C2C RID: 23596 RVA: 0x002EE78A File Offset: 0x002ECB8A
		public ThingCountClass()
		{
		}

		// Token: 0x06005C2D RID: 23597 RVA: 0x002EE793 File Offset: 0x002ECB93
		public ThingCountClass(Thing thing, int count)
		{
			this.thing = thing;
			this.Count = count;
		}

		// Token: 0x17000EC7 RID: 3783
		// (get) Token: 0x06005C2E RID: 23598 RVA: 0x002EE7AC File Offset: 0x002ECBAC
		// (set) Token: 0x06005C2F RID: 23599 RVA: 0x002EE7C8 File Offset: 0x002ECBC8
		public int Count
		{
			get
			{
				return this.countInt;
			}
			set
			{
				if (value < 0)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Tried to set ThingCountClass stack count to ",
						value,
						". thing=",
						this.thing
					}), false);
					this.countInt = 0;
				}
				else if (this.thing != null && value > this.thing.stackCount)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Tried to set ThingCountClass stack count to ",
						value,
						", but thing's stack count is only ",
						this.thing.stackCount,
						". thing=",
						this.thing
					}), false);
					this.countInt = this.thing.stackCount;
				}
				else
				{
					this.countInt = value;
				}
			}
		}

		// Token: 0x06005C30 RID: 23600 RVA: 0x002EE8A5 File Offset: 0x002ECCA5
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			Scribe_Values.Look<int>(ref this.countInt, "count", 1, false);
		}

		// Token: 0x06005C31 RID: 23601 RVA: 0x002EE8CC File Offset: 0x002ECCCC
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.Count,
				"x ",
				(this.thing == null) ? "null" : this.thing.LabelShort,
				")"
			});
		}

		// Token: 0x06005C32 RID: 23602 RVA: 0x002EE938 File Offset: 0x002ECD38
		public static implicit operator ThingCountClass(ThingCount t)
		{
			return new ThingCountClass(t.Thing, t.Count);
		}
	}
}
