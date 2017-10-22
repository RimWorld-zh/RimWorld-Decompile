using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class WaterSplash : Projectile
	{
		protected override void Impact(Thing hitThing)
		{
			base.Impact(hitThing);
			List<Thing> list = new List<Thing>();
			foreach (Thing item in base.Map.thingGrid.ThingsAt(base.Position))
			{
				if (item.def == ThingDefOf.Fire)
				{
					list.Add(item);
				}
			}
			List<Thing>.Enumerator enumerator2 = list.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					Thing current2 = enumerator2.Current;
					current2.Destroy(DestroyMode.Vanish);
				}
			}
			finally
			{
				((IDisposable)(object)enumerator2).Dispose();
			}
		}
	}
}
