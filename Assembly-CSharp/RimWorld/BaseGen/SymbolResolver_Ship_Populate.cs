using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003DF RID: 991
	public class SymbolResolver_Ship_Populate : SymbolResolver
	{
		// Token: 0x060010FB RID: 4347 RVA: 0x00090A78 File Offset: 0x0008EE78
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

		// Token: 0x060010FC RID: 4348 RVA: 0x00090B60 File Offset: 0x0008EF60
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
	}
}
