using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001AA RID: 426
	public static class SiegeBlueprintPlacer
	{
		// Token: 0x060008CB RID: 2251 RVA: 0x000528E8 File Offset: 0x00050CE8
		public static IEnumerable<Blueprint_Build> PlaceBlueprints(IntVec3 placeCenter, Map map, Faction placeFaction, float points)
		{
			SiegeBlueprintPlacer.center = placeCenter;
			SiegeBlueprintPlacer.faction = placeFaction;
			foreach (Blueprint_Build blue in SiegeBlueprintPlacer.PlaceSandbagBlueprints(map))
			{
				yield return blue;
			}
			foreach (Blueprint_Build blue2 in SiegeBlueprintPlacer.PlaceArtilleryBlueprints(points, map))
			{
				yield return blue2;
			}
			yield break;
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x00052928 File Offset: 0x00050D28
		private static bool CanPlaceBlueprintAt(IntVec3 root, Rot4 rot, ThingDef buildingDef, Map map)
		{
			return GenConstruct.CanPlaceBlueprintAt(buildingDef, root, rot, map, false, null).Accepted;
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x00052950 File Offset: 0x00050D50
		private static IEnumerable<Blueprint_Build> PlaceSandbagBlueprints(Map map)
		{
			SiegeBlueprintPlacer.placedSandbagLocs.Clear();
			int numSandbags = SiegeBlueprintPlacer.NumSandbagRange.RandomInRange;
			for (int i = 0; i < numSandbags; i++)
			{
				IntVec3 bagRoot = SiegeBlueprintPlacer.FindSandbagRoot(map);
				if (!bagRoot.IsValid)
				{
					yield break;
				}
				Rot4 growDirA;
				if (bagRoot.x > SiegeBlueprintPlacer.center.x)
				{
					growDirA = Rot4.West;
				}
				else
				{
					growDirA = Rot4.East;
				}
				Rot4 growDirB;
				if (bagRoot.z > SiegeBlueprintPlacer.center.z)
				{
					growDirB = Rot4.South;
				}
				else
				{
					growDirB = Rot4.North;
				}
				foreach (Blueprint_Build bag in SiegeBlueprintPlacer.MakeSandbagLine(bagRoot, map, growDirA, SiegeBlueprintPlacer.SandbagLengthRange.RandomInRange))
				{
					yield return bag;
				}
				bagRoot += growDirB.FacingCell;
				foreach (Blueprint_Build bag2 in SiegeBlueprintPlacer.MakeSandbagLine(bagRoot, map, growDirB, SiegeBlueprintPlacer.SandbagLengthRange.RandomInRange))
				{
					yield return bag2;
				}
			}
			yield break;
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0005297C File Offset: 0x00050D7C
		private static IEnumerable<Blueprint_Build> MakeSandbagLine(IntVec3 root, Map map, Rot4 growDir, int maxLength)
		{
			IntVec3 cur = root;
			for (int i = 0; i < maxLength; i++)
			{
				if (!SiegeBlueprintPlacer.CanPlaceBlueprintAt(cur, Rot4.North, ThingDefOf.Sandbags, map))
				{
					break;
				}
				yield return GenConstruct.PlaceBlueprintForBuild(ThingDefOf.Sandbags, cur, map, Rot4.North, SiegeBlueprintPlacer.faction, null);
				SiegeBlueprintPlacer.placedSandbagLocs.Add(cur);
				cur += growDir.FacingCell;
			}
			yield break;
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x000529BC File Offset: 0x00050DBC
		private static IEnumerable<Blueprint_Build> PlaceArtilleryBlueprints(float points, Map map)
		{
			IEnumerable<ThingDef> artyDefs = from def in DefDatabase<ThingDef>.AllDefs
			where def.building != null && def.building.buildingTags.Contains("Artillery_BaseDestroyer")
			select def;
			int numArtillery = Mathf.RoundToInt(points / 60f);
			numArtillery = Mathf.Clamp(numArtillery, 1, 2);
			for (int i = 0; i < numArtillery; i++)
			{
				Rot4 rot = Rot4.Random;
				ThingDef artyDef = artyDefs.RandomElement<ThingDef>();
				IntVec3 artySpot = SiegeBlueprintPlacer.FindArtySpot(artyDef, rot, map);
				if (!artySpot.IsValid)
				{
					yield break;
				}
				yield return GenConstruct.PlaceBlueprintForBuild(artyDef, artySpot, map, rot, SiegeBlueprintPlacer.faction, ThingDefOf.Steel);
				points -= 60f;
			}
			yield break;
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x000529F4 File Offset: 0x00050DF4
		private static IntVec3 FindSandbagRoot(Map map)
		{
			CellRect cellRect = CellRect.CenteredOn(SiegeBlueprintPlacer.center, 13);
			cellRect.ClipInsideMap(map);
			CellRect cellRect2 = CellRect.CenteredOn(SiegeBlueprintPlacer.center, 8);
			cellRect2.ClipInsideMap(map);
			int num = 0;
			IntVec3 randomCell;
			for (;;)
			{
				num++;
				if (num > 200)
				{
					break;
				}
				randomCell = cellRect.RandomCell;
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
								goto IL_104;
							}
						}
					}
				}
			}
			return IntVec3.Invalid;
			IL_104:
			return randomCell;
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x00052B14 File Offset: 0x00050F14
		private static IntVec3 FindArtySpot(ThingDef artyDef, Rot4 rot, Map map)
		{
			CellRect cellRect = CellRect.CenteredOn(SiegeBlueprintPlacer.center, 8);
			cellRect.ClipInsideMap(map);
			int num = 0;
			IntVec3 randomCell;
			for (;;)
			{
				num++;
				if (num > 200)
				{
					break;
				}
				randomCell = cellRect.RandomCell;
				if (map.reachability.CanReach(randomCell, SiegeBlueprintPlacer.center, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Deadly))
				{
					if (!randomCell.Roofed(map))
					{
						if (SiegeBlueprintPlacer.CanPlaceBlueprintAt(randomCell, rot, artyDef, map))
						{
							goto IL_83;
						}
					}
				}
			}
			return IntVec3.Invalid;
			IL_83:
			return randomCell;
		}

		// Token: 0x040003B0 RID: 944
		private static IntVec3 center;

		// Token: 0x040003B1 RID: 945
		private static Faction faction;

		// Token: 0x040003B2 RID: 946
		private static List<IntVec3> placedSandbagLocs = new List<IntVec3>();

		// Token: 0x040003B3 RID: 947
		private const int MaxArtyCount = 2;

		// Token: 0x040003B4 RID: 948
		public const float ArtyCost = 60f;

		// Token: 0x040003B5 RID: 949
		private const int MinSandbagDistSquared = 36;

		// Token: 0x040003B6 RID: 950
		private static readonly IntRange NumSandbagRange = new IntRange(2, 4);

		// Token: 0x040003B7 RID: 951
		private static readonly IntRange SandbagLengthRange = new IntRange(2, 7);
	}
}
