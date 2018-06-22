using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006DA RID: 1754
	public class WaterSplash : Projectile
	{
		// Token: 0x06002630 RID: 9776 RVA: 0x00147ACC File Offset: 0x00145ECC
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
