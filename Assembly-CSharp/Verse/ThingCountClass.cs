using System;

namespace Verse
{
	// Token: 0x02000F04 RID: 3844
	public sealed class ThingCountClass : IExposable
	{
		// Token: 0x04003CE5 RID: 15589
		public Thing thing;

		// Token: 0x04003CE6 RID: 15590
		private int countInt;

		// Token: 0x06005C2C RID: 23596 RVA: 0x002EE56A File Offset: 0x002EC96A
		public ThingCountClass()
		{
		}

		// Token: 0x06005C2D RID: 23597 RVA: 0x002EE573 File Offset: 0x002EC973
		public ThingCountClass(Thing thing, int count)
		{
			this.thing = thing;
			this.Count = count;
		}

		// Token: 0x17000EC7 RID: 3783
		// (get) Token: 0x06005C2E RID: 23598 RVA: 0x002EE58C File Offset: 0x002EC98C
		// (set) Token: 0x06005C2F RID: 23599 RVA: 0x002EE5A8 File Offset: 0x002EC9A8
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

		// Token: 0x06005C30 RID: 23600 RVA: 0x002EE685 File Offset: 0x002ECA85
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			Scribe_Values.Look<int>(ref this.countInt, "count", 1, false);
		}

		// Token: 0x06005C31 RID: 23601 RVA: 0x002EE6AC File Offset: 0x002ECAAC
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

		// Token: 0x06005C32 RID: 23602 RVA: 0x002EE718 File Offset: 0x002ECB18
		public static implicit operator ThingCountClass(ThingCount t)
		{
			return new ThingCountClass(t.Thing, t.Count);
		}
	}
}
