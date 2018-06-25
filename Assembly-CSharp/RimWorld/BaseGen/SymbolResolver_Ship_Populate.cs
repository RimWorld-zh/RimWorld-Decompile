using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.BaseGen
{
	public class SymbolResolver_Ship_Populate : SymbolResolver
	{
		public SymbolResolver_Ship_Populate()
		{
		}

		public override void Resolve(ResolveParams rp)
		{
			if (rp.thrustAxis == null)
			{
				Log.ErrorOnce("No thrust axis when generating ship parts", 50627817, false);
			}
			foreach (KeyValuePair<ThingDef, int> keyValuePair in ShipUtility.RequiredParts())
			{
				for (int i = 0; i < keyValuePair.Value; i++)
				{
					Rot4 rotation = Rot4.Random;
					if (keyValuePair.Key == ThingDefOf.Ship_Engine && rp.thrustAxis != null)
					{
						rotation = rp.thrustAxis.Value;
					}
					this.AttemptToPlace(keyValuePair.Key, rp.rect, rotation, rp.faction);
				}
			}
		}

		public void AttemptToPlace(ThingDef thingDef, CellRect rect, Rot4 rotation, Faction faction)
		{
			Map map = BaseGen.globalSettings.map;
			Thing thing;
			IntVec3 loc = (from cell in rect.Cells.InRandomOrder(null)
			where GenConstruct.CanPlaceBlueprintAt(thingDef, cell, rotation, map, false, null).Accepted && GenAdj.OccupiedRect(cell, rotation, thingDef.Size).AdjacentCellsCardinal.Any(delegate(IntVec3 edgeCell)
			{
				bool result;
				if (edgeCell.InBounds(map))
				{
					result = edgeCell.GetThingList(map).Any((Thing thing) => thing.def == ThingDefOf.Ship_Beam);
				}
				else
				{
					result = false;
				}
				return result;
			})
			select cell).FirstOrFallback(IntVec3.Invalid);
			if (loc.IsValid)
			{
				thing = ThingMaker.MakeThing(thingDef, null);
				thing.SetFaction(faction, null);
				CompHibernatable compHibernatable = thing.TryGetComp<CompHibernatable>();
				if (compHibernatable != null)
				{
					compHibernatable.State = HibernatableStateDefOf.Hibernating;
				}
				GenSpawn.Spawn(thing, loc, BaseGen.globalSettings.map, rotation, WipeMode.Vanish, false);
			}
		}

		[CompilerGenerated]
		private sealed class <AttemptToPlace>c__AnonStorey0
		{
			internal ThingDef thingDef;

			internal Rot4 rotation;

			internal Map map;

			private static Predicate<Thing> <>f__am$cache0;

			public <AttemptToPlace>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 cell)
			{
				return GenConstruct.CanPlaceBlueprintAt(this.thingDef, cell, this.rotation, this.map, false, null).Accepted && GenAdj.OccupiedRect(cell, this.rotation, this.thingDef.Size).AdjacentCellsCardinal.Any(delegate(IntVec3 edgeCell)
				{
					bool result;
					if (edgeCell.InBounds(this.map))
					{
						result = edgeCell.GetThingList(this.map).Any((Thing thing) => thing.def == ThingDefOf.Ship_Beam);
					}
					else
					{
						result = false;
					}
					return result;
				});
			}

			internal bool <>m__1(IntVec3 edgeCell)
			{
				bool result;
				if (edgeCell.InBounds(this.map))
				{
					result = edgeCell.GetThingList(this.map).Any((Thing thing) => thing.def == ThingDefOf.Ship_Beam);
				}
				else
				{
					result = false;
				}
				return result;
			}

			private static bool <>m__2(Thing thing)
			{
				return thing.def == ThingDefOf.Ship_Beam;
			}
		}
	}
}
