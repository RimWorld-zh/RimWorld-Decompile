using System;

namespace Verse
{
	// Token: 0x02000F01 RID: 3841
	public sealed class ThingCountClass : IExposable
	{
		// Token: 0x06005BFC RID: 23548 RVA: 0x002EBDDA File Offset: 0x002EA1DA
		public ThingCountClass()
		{
		}

		// Token: 0x06005BFD RID: 23549 RVA: 0x002EBDE3 File Offset: 0x002EA1E3
		public ThingCountClass(Thing thing, int count)
		{
			this.thing = thing;
			this.Count = count;
		}

		// Token: 0x17000EC5 RID: 3781
		// (get) Token: 0x06005BFE RID: 23550 RVA: 0x002EBDFC File Offset: 0x002EA1FC
		// (set) Token: 0x06005BFF RID: 23551 RVA: 0x002EBE18 File Offset: 0x002EA218
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

		// Token: 0x06005C00 RID: 23552 RVA: 0x002EBEF5 File Offset: 0x002EA2F5
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			Scribe_Values.Look<int>(ref this.countInt, "count", 1, false);
		}

		// Token: 0x06005C01 RID: 23553 RVA: 0x002EBF1C File Offset: 0x002EA31C
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

		// Token: 0x06005C02 RID: 23554 RVA: 0x002EBF88 File Offset: 0x002EA388
		public static implicit operator ThingCountClass(ThingCount t)
		{
			return new ThingCountClass(t.Thing, t.Count);
		}

		// Token: 0x04003CD0 RID: 15568
		public Thing thing;

		// Token: 0x04003CD1 RID: 15569
		private int countInt;
	}
}
