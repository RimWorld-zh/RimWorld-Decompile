using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020006DC RID: 1756
	public class WaterSplash : Projectile
	{
		// Token: 0x06002633 RID: 9779 RVA: 0x00147E7C File Offset: 0x0014627C
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
