using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class SiegeBlueprintPlacer
	{
		private const int MaxArtyCount = 2;

		public const float ArtyCost = 60f;

		private const int MinSandbagDistSquared = 36;

		private static IntVec3 center;

		private static Faction faction;

		private static List<IntVec3> placedSandbagLocs = new List<IntVec3>();

		private static readonly IntRange NumSandbagRange = new IntRange(2, 4);

		private static readonly IntRange SandbagLengthRange = new IntRange(2, 7);

		public static IEnumerable<Blueprint_Build> PlaceBlueprints(IntVec3 placeCenter, Map map, Faction placeFaction, float points)
		{
			SiegeBlueprintPlacer.center = placeCenter;
			SiegeBlueprintPlacer.faction = placeFaction;
			foreach (Blueprint_Build item in SiegeBlueprintPlacer.PlaceSandbagBlueprints(map))
			{
				yield return item;
			}
			foreach (Blueprint_Build item2 in SiegeBlueprintPlacer.PlaceArtilleryBlueprints(points, map))
			{
				yield return item2;
			}
		}

		private static bool CanPlaceBlueprintAt(IntVec3 root, Rot4 rot, ThingDef buildingDef, Map map)
		{
			return GenConstruct.CanPlaceBlueprintAt(buildingDef, root, rot, map, false, null).Accepted;
		}

		private static IEnumerable<Blueprint_Build> PlaceSandbagBlueprints(Map map)
		{
			SiegeBlueprintPlacer.placedSandbagLocs.Clear();
			int numSandbags = SiegeBlueprintPlacer.NumSandbagRange.RandomInRange;
			int i = 0;
			while (i < numSandbags)
			{
				IntVec3 bagRoot = SiegeBlueprintPlacer.FindSandbagRoot(map);
				if (bagRoot.IsValid)
				{
					Rot4 growDirA = (bagRoot.x <= SiegeBlueprintPlacer.center.x) ? Rot4.East : Rot4.West;
					Rot4 growDirB = (bagRoot.z <= SiegeBlueprintPlacer.center.z) ? Rot4.North : Rot4.South;
					foreach (Blueprint_Build item in SiegeBlueprintPlacer.MakeSandbagLine(bagRoot, map, growDirA, SiegeBlueprintPlacer.SandbagLengthRange.RandomInRange))
					{
						yield return item;
					}
					bagRoot += growDirB.FacingCell;
					foreach (Blueprint_Build item2 in SiegeBlueprintPlacer.MakeSandbagLine(bagRoot, map, growDirB, SiegeBlueprintPlacer.SandbagLengthRange.RandomInRange))
					{
						yield return item2;
					}
					i++;
					continue;
				}
				break;
			}
		}

		private static IEnumerable<Blueprint_Build> MakeSandbagLine(IntVec3 root, Map map, Rot4 growDir, int maxLength)
		{
			IntVec3 cur = root;
			int i = 0;
			while (i < maxLength && SiegeBlueprintPlacer.CanPlaceBlueprintAt(cur, Rot4.North, ThingDefOf.Sandbags, map))
			{
				yield return GenConstruct.PlaceBlueprintForBuild(ThingDefOf.Sandbags, cur, map, Rot4.North, SiegeBlueprintPlacer.faction, null);
				SiegeBlueprintPlacer.placedSandbagLocs.Add(cur);
				cur += growDir.FacingCell;
				i++;
			}
		}

		private static IEnumerable<Blueprint_Build> PlaceArtilleryBlueprints(float points, Map map)
		{
			IEnumerable<ThingDef> artyDefs = from def in DefDatabase<ThingDef>.AllDefs
			where def.building != null && def.building.buildingTags.Contains("Artillery_BaseDestroyer")
			select def;
			int numArtillery2 = Mathf.RoundToInt((float)(points / 60.0));
			numArtillery2 = Mathf.Clamp(numArtillery2, 1, 2);
			int i = 0;
			while (i < numArtillery2)
			{
				Rot4 rot = Rot4.Random;
				ThingDef artyDef = artyDefs.RandomElement();
				IntVec3 artySpot = SiegeBlueprintPlacer.FindArtySpot(artyDef, rot, map);
				if (artySpot.IsValid)
				{
					yield return GenConstruct.PlaceBlueprintForBuild(artyDef, artySpot, map, rot, SiegeBlueprintPlacer.faction, ThingDefOf.Steel);
					points = (float)(points - 60.0);
					i++;
					continue;
				}
				break;
			}
		}

		private static IntVec3 FindSandbagRoot(Map map)
		{
			CellRect cellRect = CellRect.CenteredOn(SiegeBlueprintPlacer.center, 13);
			cellRect.ClipInsideMap(map);
			CellRect cellRect2 = CellRect.CenteredOn(SiegeBlueprintPlacer.center, 8);
			cellRect2.ClipInsideMap(map);
			int num = 0;
			goto IL_002d;
			IL_002d:
			IntVec3 randomCell;
			while (true)
			{
				num++;
				if (num > 200)
				{
					return IntVec3.Invalid;
				}
				randomCell = cellRect.RandomCell;
				if (!cellRect2.Contains(randomCell) && map.reachability.CanReach(randomCell, SiegeBlueprintPlacer.center, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Deadly) && SiegeBlueprintPlacer.CanPlaceBlueprintAt(randomCell, Rot4.North, ThingDefOf.Sandbags, map))
				{
					bool flag = false;
					for (int i = 0; i < SiegeBlueprintPlacer.placedSandbagLocs.Count; i++)
					{
						float num2 = (float)(SiegeBlueprintPlacer.placedSandbagLocs[i] - randomCell).LengthHorizontalSquared;
						if (num2 < 36.0)
						{
							flag = true;
						}
					}
					if (!flag)
						break;
				}
			}
			return randomCell;
			IL_00f7:
			goto IL_002d;
		}

		private static IntVec3 FindArtySpot(ThingDef artyDef, Rot4 rot, Map map)
		{
			CellRect cellRect = CellRect.CenteredOn(SiegeBlueprintPlacer.center, 8);
			cellRect.ClipInsideMap(map);
			int num = 0;
			goto IL_0017;
			IL_0017:
			IntVec3 randomCell;
			while (true)
			{
				num++;
				if (num > 200)
				{
					return IntVec3.Invalid;
				}
				randomCell = cellRect.RandomCell;
				if (map.reachability.CanReach(randomCell, SiegeBlueprintPlacer.center, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Deadly) && !randomCell.Roofed(map) && SiegeBlueprintPlacer.CanPlaceBlueprintAt(randomCell, rot, artyDef, map))
					break;
			}
			return randomCell;
			IL_007d:
			goto IL_0017;
		}
	}
}
