using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003CD RID: 973
	public class SymbolResolver_OutdoorLighting : SymbolResolver
	{
		// Token: 0x060010C7 RID: 4295 RVA: 0x0008EFBC File Offset: 0x0008D3BC
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			ThingDef def;
			if (rp.faction == null || rp.faction.def.techLevel >= TechLevel.Industrial)
			{
				def = ThingDefOf.StandingLamp;
			}
			else
			{
				def = ThingDefOf.TorchLamp;
			}
			this.FindNearbyGlowers(rp.rect);
			for (int i = 0; i < rp.rect.Area / 4; i++)
			{
				IntVec3 randomCell = rp.rect.RandomCell;
				if (randomCell.Standable(map) && randomCell.GetFirstItem(map) == null && randomCell.GetFirstPawn(map) == null && randomCell.GetFirstBuilding(map) == null)
				{
					Region region = randomCell.GetRegion(map, RegionType.Set_Passable);
					if (region != null && region.Room.PsychologicallyOutdoors && region.Room.UsesOutdoorTemperature)
					{
						if (!this.AnyGlowerNearby(randomCell))
						{
							if (!BaseGenUtility.AnyDoorAdjacentCardinalTo(randomCell, map))
							{
								Thing thing = GenSpawn.Spawn(def, randomCell, map, WipeMode.Vanish);
								if (thing.def.CanHaveFaction && thing.Faction != rp.faction)
								{
									thing.SetFaction(rp.faction, null);
								}
								SymbolResolver_OutdoorLighting.nearbyGlowers.Add(thing.TryGetComp<CompGlower>());
							}
						}
					}
				}
			}
			SymbolResolver_OutdoorLighting.nearbyGlowers.Clear();
		}

		// Token: 0x060010C8 RID: 4296 RVA: 0x0008F130 File Offset: 0x0008D530
		private void FindNearbyGlowers(CellRect rect)
		{
			Map map = BaseGen.globalSettings.map;
			SymbolResolver_OutdoorLighting.nearbyGlowers.Clear();
			rect = rect.ExpandedBy(4);
			rect = rect.ClipInsideMap(map);
			CellRect.CellRectIterator iterator = rect.GetIterator();
			while (!iterator.Done())
			{
				Region region = iterator.Current.GetRegion(map, RegionType.Set_Passable);
				if (region != null && region.Room.PsychologicallyOutdoors)
				{
					List<Thing> thingList = iterator.Current.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						CompGlower compGlower = thingList[i].TryGetComp<CompGlower>();
						if (compGlower != null)
						{
							SymbolResolver_OutdoorLighting.nearbyGlowers.Add(compGlower);
						}
					}
				}
				iterator.MoveNext();
			}
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x0008F200 File Offset: 0x0008D600
		private bool AnyGlowerNearby(IntVec3 c)
		{
			for (int i = 0; i < SymbolResolver_OutdoorLighting.nearbyGlowers.Count; i++)
			{
				if (c.InHorDistOf(SymbolResolver_OutdoorLighting.nearbyGlowers[i].parent.Position, SymbolResolver_OutdoorLighting.nearbyGlowers[i].Props.glowRadius + 2f))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000A3A RID: 2618
		private static List<CompGlower> nearbyGlowers = new List<CompGlower>();

		// Token: 0x04000A3B RID: 2619
		private const float Margin = 2f;
	}
}
