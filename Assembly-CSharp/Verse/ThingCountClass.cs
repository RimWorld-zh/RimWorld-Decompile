using System;

namespace Verse
{
	// Token: 0x02000F00 RID: 3840
	public sealed class ThingCountClass : IExposable
	{
		// Token: 0x06005BFA RID: 23546 RVA: 0x002EBEB6 File Offset: 0x002EA2B6
		public ThingCountClass()
		{
		}

		// Token: 0x06005BFB RID: 23547 RVA: 0x002EBEBF File Offset: 0x002EA2BF
		public ThingCountClass(Thing thing, int count)
		{
			this.thing = thing;
			this.Count = count;
		}

		// Token: 0x17000EC4 RID: 3780
		// (get) Token: 0x06005BFC RID: 23548 RVA: 0x002EBED8 File Offset: 0x002EA2D8
		// (set) Token: 0x06005BFD RID: 23549 RVA: 0x002EBEF4 File Offset: 0x002EA2F4
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

		// Token: 0x06005BFE RID: 23550 RVA: 0x002EBFD1 File Offset: 0x002EA3D1
		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			Scribe_Values.Look<int>(ref this.countInt, "count", 1, false);
		}

		// Token: 0x06005BFF RID: 23551 RVA: 0x002EBFF8 File Offset: 0x002EA3F8
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

		// Token: 0x06005C00 RID: 23552 RVA: 0x002EC064 File Offset: 0x002EA464
		public static implicit operator ThingCountClass(ThingCount t)
		{
			return new ThingCountClass(t.Thing, t.Count);
		}

		// Token: 0x04003CCF RID: 15567
		public Thing thing;

		// Token: 0x04003CD0 RID: 15568
		private int countInt;
	}
}
