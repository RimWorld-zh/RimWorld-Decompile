using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		[DebuggerHidden]
		public static IEnumerable<Blueprint_Build> PlaceBlueprints(IntVec3 placeCenter, Map map, Faction placeFaction, float points)
		{
			SiegeBlueprintPlacer.<PlaceBlueprints>c__Iterator69 <PlaceBlueprints>c__Iterator = new SiegeBlueprintPlacer.<PlaceBlueprints>c__Iterator69();
			<PlaceBlueprints>c__Iterator.placeCenter = placeCenter;
			<PlaceBlueprints>c__Iterator.placeFaction = placeFaction;
			<PlaceBlueprints>c__Iterator.map = map;
			<PlaceBlueprints>c__Iterator.points = points;
			<PlaceBlueprints>c__Iterator.<$>placeCenter = placeCenter;
			<PlaceBlueprints>c__Iterator.<$>placeFaction = placeFaction;
			<PlaceBlueprints>c__Iterator.<$>map = map;
			<PlaceBlueprints>c__Iterator.<$>points = points;
			SiegeBlueprintPlacer.<PlaceBlueprints>c__Iterator69 expr_3F = <PlaceBlueprints>c__Iterator;
			expr_3F.$PC = -2;
			return expr_3F;
		}

		private static bool CanPlaceBlueprintAt(IntVec3 root, Rot4 rot, ThingDef buildingDef, Map map)
		{
			return GenConstruct.CanPlaceBlueprintAt(buildingDef, root, rot, map, false, null).Accepted;
		}

		[DebuggerHidden]
		private static IEnumerable<Blueprint_Build> PlaceSandbagBlueprints(Map map)
		{
			SiegeBlueprintPlacer.<PlaceSandbagBlueprints>c__Iterator6A <PlaceSandbagBlueprints>c__Iterator6A = new SiegeBlueprintPlacer.<PlaceSandbagBlueprints>c__Iterator6A();
			<PlaceSandbagBlueprints>c__Iterator6A.map = map;
			<PlaceSandbagBlueprints>c__Iterator6A.<$>map = map;
			SiegeBlueprintPlacer.<PlaceSandbagBlueprints>c__Iterator6A expr_15 = <PlaceSandbagBlueprints>c__Iterator6A;
			expr_15.$PC = -2;
			return expr_15;
		}

		[DebuggerHidden]
		private static IEnumerable<Blueprint_Build> MakeSandbagLine(IntVec3 root, Map map, Rot4 growDir, int maxLength)
		{
			SiegeBlueprintPlacer.<MakeSandbagLine>c__Iterator6B <MakeSandbagLine>c__Iterator6B = new SiegeBlueprintPlacer.<MakeSandbagLine>c__Iterator6B();
			<MakeSandbagLine>c__Iterator6B.root = root;
			<MakeSandbagLine>c__Iterator6B.maxLength = maxLength;
			<MakeSandbagLine>c__Iterator6B.map = map;
			<MakeSandbagLine>c__Iterator6B.growDir = growDir;
			<MakeSandbagLine>c__Iterator6B.<$>root = root;
			<MakeSandbagLine>c__Iterator6B.<$>maxLength = maxLength;
			<MakeSandbagLine>c__Iterator6B.<$>map = map;
			<MakeSandbagLine>c__Iterator6B.<$>growDir = growDir;
			SiegeBlueprintPlacer.<MakeSandbagLine>c__Iterator6B expr_3F = <MakeSandbagLine>c__Iterator6B;
			expr_3F.$PC = -2;
			return expr_3F;
		}

		[DebuggerHidden]
		private static IEnumerable<Blueprint_Build> PlaceArtilleryBlueprints(float points, Map map)
		{
			SiegeBlueprintPlacer.<PlaceArtilleryBlueprints>c__Iterator6C <PlaceArtilleryBlueprints>c__Iterator6C = new SiegeBlueprintPlacer.<PlaceArtilleryBlueprints>c__Iterator6C();
			<PlaceArtilleryBlueprints>c__Iterator6C.points = points;
			<PlaceArtilleryBlueprints>c__Iterator6C.map = map;
			<PlaceArtilleryBlueprints>c__Iterator6C.<$>points = points;
			<PlaceArtilleryBlueprints>c__Iterator6C.<$>map = map;
			SiegeBlueprintPlacer.<PlaceArtilleryBlueprints>c__Iterator6C expr_23 = <PlaceArtilleryBlueprints>c__Iterator6C;
			expr_23.$PC = -2;
			return expr_23;
		}

		private static IntVec3 FindSandbagRoot(Map map)
		{
			CellRect cellRect = CellRect.CenteredOn(SiegeBlueprintPlacer.center, 13);
			cellRect.ClipInsideMap(map);
			CellRect cellRect2 = CellRect.CenteredOn(SiegeBlueprintPlacer.center, 8);
			cellRect2.ClipInsideMap(map);
			int num = 0;
			while (true)
			{
				num++;
				if (num > 200)
				{
					break;
				}
				IntVec3 randomCell = cellRect.RandomCell;
				if (!cellRect2.Contains(randomCell))
				{
					if (map.reachability.CanReach(randomCell, SiegeBlueprintPlacer.center, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Deadly))
					{
						if (SiegeBlueprintPlacer.CanPlaceBlueprintAt(randomCell, Rot4.North, ThingDefOf.Sandbags, map))
						{
							bool flag = false;
							for (int i = 0; i < SiegeBlueprintPlacer.placedSandbagLocs.Count; i++)
							{
								float num2 = (float)(SiegeBlueprintPlacer.placedSandbagLocs[i] - randomCell).LengthHorizontalSquared;
								if (num2 < 36f)
								{
									flag = true;
								}
							}
							if (!flag)
							{
								return randomCell;
							}
						}
					}
				}
			}
			return IntVec3.Invalid;
		}

		private static IntVec3 FindArtySpot(ThingDef artyDef, Rot4 rot, Map map)
		{
			CellRect cellRect = CellRect.CenteredOn(SiegeBlueprintPlacer.center, 8);
			cellRect.ClipInsideMap(map);
			int num = 0;
			while (true)
			{
				num++;
				if (num > 200)
				{
					break;
				}
				IntVec3 randomCell = cellRect.RandomCell;
				if (map.reachability.CanReach(randomCell, SiegeBlueprintPlacer.center, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Deadly))
				{
					if (!randomCell.Roofed(map))
					{
						if (SiegeBlueprintPlacer.CanPlaceBlueprintAt(randomCell, rot, artyDef, map))
						{
							return randomCell;
						}
					}
				}
			}
			return IntVec3.Invalid;
		}
	}
}
