using System;

namespace Verse
{
	// Token: 0x02000F00 RID: 3840
	public sealed class ThingCountClass : IExposable
	{
		// Token: 0x06005C22 RID: 23586 RVA: 0x002EDEEA File Offset: 0x002EC2EA
		public ThingCountClass()
		{
		}

		// Token: 0x06005C23 RID: 23587 RVA: 0x002EDEF3 File Offset: 0x002EC2F3
		public ThingCountClass(Thing thing, int count)
		{
			this.thing = thing;
			this.Count = count;
		}

		// Token: 0x17000EC8 RID: 3784
		// (get) Token: 0x06005C24 RID: 23588 RVA: 0x002EDF0C File Offset: 0x002EC30C
		// (set) Token: 0x06005C25 RID: 23589 RVA: 0x002EDF28 File Offset: 0x002EC328
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

		// Token: 0x06005C26 RID: 23590 RVA: 0x002EE005 File Offset: 0x002EC405
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			Scribe_Values.Look<int>(ref this.countInt, "count", 1, false);
		}

		// Token: 0x06005C27 RID: 23591 RVA: 0x002EE02C File Offset: 0x002EC42C
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

		// Token: 0x06005C28 RID: 23592 RVA: 0x002EE098 File Offset: 0x002EC498
		public static implicit operator ThingCountClass(ThingCount t)
		{
			return new ThingCountClass(t.Thing, t.Count);
		}

		// Token: 0x04003CE2 RID: 15586
		public Thing thing;

		// Token: 0x04003CE3 RID: 15587
		private int countInt;
	}
}
