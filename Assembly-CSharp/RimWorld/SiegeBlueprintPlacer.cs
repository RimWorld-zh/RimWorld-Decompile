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
		private static IntVec3 center;

		private static Faction faction;

		private static List<IntVec3> placedSandbagLocs = new List<IntVec3>();

		private const int MaxArtyCount = 2;

		public const float ArtyCost = 60f;

		private const int MinSandbagDistSquared = 36;

		private static readonly IntRange NumSandbagRange = new IntRange(2, 4);

		private static readonly IntRange SandbagLengthRange = new IntRange(2, 7);

		public static IEnumerable<Blueprint_Build> PlaceBlueprints(IntVec3 placeCenter, Map map, Faction placeFaction, float points)
		{
			SiegeBlueprintPlacer.center = placeCenter;
			SiegeBlueprintPlacer.faction = placeFaction;
			using (IEnumerator<Blueprint_Build> enumerator = SiegeBlueprintPlacer.PlaceSandbagBlueprints(map).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Blueprint_Build blue2 = enumerator.Current;
					yield return blue2;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			using (IEnumerator<Blueprint_Build> enumerator2 = SiegeBlueprintPlacer.PlaceArtilleryBlueprints(points, map).GetEnumerator())
			{
				if (enumerator2.MoveNext())
				{
					Blueprint_Build blue = enumerator2.Current;
					yield return blue;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_016d:
			/*Error near IL_016e: Unexpected return in MoveNext()*/;
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
					using (IEnumerator<Blueprint_Build> enumerator = SiegeBlueprintPlacer.MakeSandbagLine(bagRoot, map, growDirA, SiegeBlueprintPlacer.SandbagLengthRange.RandomInRange).GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							Blueprint_Build bag2 = enumerator.Current;
							yield return bag2;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
					bagRoot += growDirB.FacingCell;
					using (IEnumerator<Blueprint_Build> enumerator2 = SiegeBlueprintPlacer.MakeSandbagLine(bagRoot, map, growDirB, SiegeBlueprintPlacer.SandbagLengthRange.RandomInRange).GetEnumerator())
					{
						if (enumerator2.MoveNext())
						{
							Blueprint_Build bag = enumerator2.Current;
							yield return bag;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
					i++;
					continue;
				}
				break;
			}
			yield break;
			IL_027a:
			/*Error near IL_027b: Unexpected return in MoveNext()*/;
		}

		private static IEnumerable<Blueprint_Build> MakeSandbagLine(IntVec3 root, Map map, Rot4 growDir, int maxLength)
		{
			int i = 0;
			if (i < maxLength && SiegeBlueprintPlacer.CanPlaceBlueprintAt(root, Rot4.North, ThingDefOf.Sandbags, map))
			{
				yield return GenConstruct.PlaceBlueprintForBuild(ThingDefOf.Sandbags, root, map, Rot4.North, SiegeBlueprintPlacer.faction, null);
				/*Error: Unable to find new state assignment for yield return*/;
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
			if (i < numArtillery2)
			{
				Rot4 rot = Rot4.Random;
				ThingDef artyDef = artyDefs.RandomElement();
				IntVec3 artySpot = SiegeBlueprintPlacer.FindArtySpot(artyDef, rot, map);
				if (artySpot.IsValid)
				{
					yield return GenConstruct.PlaceBlueprintForBuild(artyDef, artySpot, map, rot, SiegeBlueprintPlacer.faction, ThingDefOf.Steel);
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
		}

		private static IntVec3 FindSandbagRoot(Map map)
		{
			CellRect cellRect = CellRect.CenteredOn(SiegeBlueprintPlacer.center, 13);
			cellRect.ClipInsideMap(map);
			CellRect cellRect2 = CellRect.CenteredOn(SiegeBlueprintPlacer.center, 8);
			cellRect2.ClipInsideMap(map);
			int num = 0;
			goto IL_002e;
			IL_002e:
			IntVec3 result;
			while (true)
			{
				num++;
				if (num > 200)
				{
					result = IntVec3.Invalid;
				}
				else
				{
					IntVec3 randomCell = cellRect.RandomCell;
					if (cellRect2.Contains(randomCell))
						continue;
					if (!map.reachability.CanReach(randomCell, SiegeBlueprintPlacer.center, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Deadly))
						continue;
					if (!SiegeBlueprintPlacer.CanPlaceBlueprintAt(randomCell, Rot4.North, ThingDefOf.Sandbags, map))
						continue;
					bool flag = false;
					for (int i = 0; i < SiegeBlueprintPlacer.placedSandbagLocs.Count; i++)
					{
						float num2 = (float)(SiegeBlueprintPlacer.placedSandbagLocs[i] - randomCell).LengthHorizontalSquared;
						if (num2 < 36.0)
						{
							flag = true;
						}
					}
					if (flag)
						continue;
					result = randomCell;
				}
				break;
			}
			return result;
			IL_010c:
			goto IL_002e;
		}

		private static IntVec3 FindArtySpot(ThingDef artyDef, Rot4 rot, Map map)
		{
			CellRect cellRect = CellRect.CenteredOn(SiegeBlueprintPlacer.center, 8);
			cellRect.ClipInsideMap(map);
			int num = 0;
			goto IL_0018;
			IL_0018:
			IntVec3 result;
			while (true)
			{
				num++;
				if (num > 200)
				{
					result = IntVec3.Invalid;
				}
				else
				{
					IntVec3 randomCell = cellRect.RandomCell;
					if (!map.reachability.CanReach(randomCell, SiegeBlueprintPlacer.center, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Deadly))
						continue;
					if (randomCell.Roofed(map))
						continue;
					if (!SiegeBlueprintPlacer.CanPlaceBlueprintAt(randomCell, rot, artyDef, map))
						continue;
					result = randomCell;
				}
				break;
			}
			return result;
			IL_008a:
			goto IL_0018;
		}
	}
}
