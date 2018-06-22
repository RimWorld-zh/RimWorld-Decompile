using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C79 RID: 3193
	public class PlaceWorker_WatermillGenerator : PlaceWorker
	{
		// Token: 0x060045FD RID: 17917 RVA: 0x0024E490 File Offset: 0x0024C890
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			foreach (IntVec3 c in CompPowerPlantWater.GroundPoints(loc, rot))
			{
				if (!map.terrainGrid.TerrainAt(c).affordances.Contains(TerrainAffordanceDefOf.Medium))
				{
					return new AcceptanceReport("TerrainCannotSupport".Translate());
				}
			}
			foreach (IntVec3 c2 in CompPowerPlantWater.WaterPoints(loc, rot))
			{
				if (!map.terrainGrid.TerrainAt(c2).affordances.Contains(TerrainAffordanceDefOf.MovingFluid))
				{
					return new AcceptanceReport("MustBeOnMovingWater".Translate());
				}
			}
			return true;
		}

		// Token: 0x060045FE RID: 17918 RVA: 0x0024E5AC File Offset: 0x0024C9AC
		public override void DrawGhost(ThingDef def, IntVec3 loc, Rot4 rot, Color ghostCol)
		{
			GenDraw.DrawFieldEdges(CompPowerPlantWater.GroundPoints(loc, rot).ToList<IntVec3>(), new Color(0.7f, 0.65f, 0.6f));
			GenDraw.DrawFieldEdges(CompPowerPlantWater.WaterPoints(loc, rot).ToList<IntVec3>(), new Color(0.6f, 0.6f, 0.7f));
		}

		// Token: 0x060045FF RID: 17919 RVA: 0x0024E604 File Offset: 0x0024CA04
		public override IEnumerable<TerrainAffordanceDef> DisplayAffordances()
		{
			yield return TerrainAffordanceDefOf.Medium;
			yield return TerrainAffordanceDefOf.MovingFluid;
			yield break;
		}
	}
}
