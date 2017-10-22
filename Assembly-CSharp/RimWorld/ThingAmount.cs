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
			int num = 0;
			int result;
			while (true)
			{
				if (num < list.Count)
				{
					ThingAmount thingAmount = list[num];
					if (thingAmount.thing == thing)
					{
						ThingAmount thingAmount2 = list[num];
						result = thingAmount2.count;
						break;
					}
					num++;
					continue;
				}
				result = 0;
				break;
			}
			return result;
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
