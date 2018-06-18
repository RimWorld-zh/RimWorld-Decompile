using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006DE RID: 1758
	public class WaterSplash : Projectile
	{
		// Token: 0x06002638 RID: 9784 RVA: 0x00147928 File Offset: 0x00145D28
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
