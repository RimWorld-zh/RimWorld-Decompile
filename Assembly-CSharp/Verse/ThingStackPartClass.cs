namespace Verse
{
	public sealed class ThingStackPartClass : IExposable
	{
		public Thing thing;

		private int countInt;

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
					Log.Warning("Tried to set ThingStackPartClass stack count to " + value + ". thing=" + this.thing);
					this.countInt = 0;
				}
				else if (value > this.thing.stackCount)
				{
					Log.Warning("Tried to set ThingStackPartClass stack count to " + value + ", but thing's stack count is only " + this.thing.stackCount + ". thing=" + this.thing);
					this.countInt = this.thing.stackCount;
				}
				else
				{
					this.countInt = value;
				}
			}
		}

		public ThingStackPartClass()
		{
		}

		public ThingStackPartClass(Thing thing, int count)
		{
			this.thing = thing;
			this.Count = count;
		}

		public void ExposeData()
		{
			Scribe_References.Look<Thing>(ref this.thing, "thing", false);
			Scribe_Values.Look<int>(ref this.countInt, "count", 1, false);
		}
	}
}
