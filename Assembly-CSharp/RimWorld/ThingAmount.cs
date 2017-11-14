using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public struct ThingAmount
	{
		public Thing thing;

		public int count;

		public ThingAmount(Thing thing, int count)
		{
			this.thing = thing;
			this.count = count;
		}

		public override string ToString()
		{
			return "(" + this.count + "x " + ((this.thing == null) ? "null" : this.thing.ThingID) + ")";
		}

		public static int CountUsed(List<ThingAmount> list, Thing thing)
		{
			for (int i = 0; i < list.Count; i++)
			{
				ThingAmount thingAmount = list[i];
				if (thingAmount.thing == thing)
				{
					ThingAmount thingAmount2 = list[i];
					return thingAmount2.count;
				}
			}
			return 0;
		}

		public static void AddToList(List<ThingAmount> list, Thing thing, int countToAdd)
		{
			for (int i = 0; i < list.Count; i++)
			{
				ThingAmount thingAmount = list[i];
				if (thingAmount.thing == thing)
				{
					int index = i;
					ThingAmount thingAmount2 = list[i];
					Thing obj = thingAmount2.thing;
					ThingAmount thingAmount3 = list[i];
					list[index] = new ThingAmount(obj, thingAmount3.count + countToAdd);
					return;
				}
			}
			list.Add(new ThingAmount(thing, countToAdd));
		}
	}
}
