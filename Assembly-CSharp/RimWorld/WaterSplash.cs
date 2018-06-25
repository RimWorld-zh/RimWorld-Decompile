using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class WaterSplash : Projectile
	{
		public WaterSplash()
		{
		}

		protected override void Impact(Thing hitThing)
		{
			base.Impact(hitThing);
			List<Thing> list = new List<Thing>();
			foreach (Thing thing in base.Map.thingGrid.ThingsAt(base.Position))
			{
				if (thing.def == ThingDefOf.Fire)
				{
					list.Add(thing);
				}
			}
			foreach (Thing thing2 in list)
			{
				thing2.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
