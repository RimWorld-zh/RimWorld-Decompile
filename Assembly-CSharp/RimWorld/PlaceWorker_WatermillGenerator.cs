using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C7C RID: 3196
	public class PlaceWorker_WatermillGenerator : PlaceWorker
	{
		// Token: 0x06004600 RID: 17920 RVA: 0x0024E84C File Offset: 0x0024CC4C
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			foreach (IntVec3 c in CompPowerPlantWater.GroundCells(loc, rot))
			{
				if (!map.terrainGrid.TerrainAt(c).affordances.Contains(TerrainAffordanceDefOf.Medium))
				{
					return new AcceptanceReport("TerrainCannotSupport".Translate());
				}
			}
			foreach (IntVec3 c2 in CompPowerPlantWater.WaterCells(loc, rot))
			{
				if (!map.terrainGrid.TerrainAt(c2).affordances.Contains(TerrainAffordanceDefOf.MovingFluid))
				{
					return new AcceptanceReport("MustBeOnMovingWater".Translate());
				}
			}
			return true;
		}

		// Token: 0x06004601 RID: 17921 RVA: 0x0024E968 File Offset: 0x0024CD68
		public override void DrawGhost(ThingDef def, IntVec3 loc, Rot4 rot, Color ghostCol)
		{
			GenDraw.DrawFieldEdges(CompPowerPlantWater.GroundCells(loc, rot).ToList<IntVec3>(), new Color(0.7f, 0.65f, 0.6f));
			GenDraw.DrawFieldEdges(CompPowerPlantWater.WaterCells(loc, rot).ToList<IntVec3>(), new Color(0.6f, 0.6f, 0.7f));
		}

		// Token: 0x06004602 RID: 17922 RVA: 0x0024E9C0 File Offset: 0x0024CDC0
		public override IEnumerable<TerrainAffordanceDef> DisplayAffordances()
		{
			yield return TerrainAffordanceDefOf.Medium;
			yield return TerrainAffordanceDefOf.MovingFluid;
			yield break;
		}
	}
}
