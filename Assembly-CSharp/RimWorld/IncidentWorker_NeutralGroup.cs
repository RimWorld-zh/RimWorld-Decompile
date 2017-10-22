using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_NeutralGroup : IncidentWorker_PawnsArrive
	{
		protected virtual PawnGroupKindDef PawnGroupKindDef
		{
			get
			{
				return PawnGroupKindDefOf.Normal;
			}
		}

		protected override bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
		{
			return base.FactionCanBeGroupSource(f, map, desperate) && !f.def.hidden && !f.HostileTo(Faction.OfPlayer);
		}

		protected bool TryResolveParms(IncidentParms parms)
		{
			bool result;
			if (!this.TryResolveParmsGeneral(parms))
			{
				result = false;
			}
			else
			{
				this.ResolveParmsPoints(parms);
				result = true;
			}
			return result;
		}

		protected virtual bool TryResolveParmsGeneral(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return (byte)((parms.spawnCenter.IsValid || RCellFinder.TryFindRandomPawnEntryCell(out parms.spawnCenter, map, CellFinder.EdgeRoadChance_Neutral, (Predicate<IntVec3>)null)) ? ((parms.faction != null || base.CandidateFactions(map, false).TryRandomElement<Faction>(out parms.faction) || base.CandidateFactions(map, true).TryRandomElement<Faction>(out parms.faction)) ? 1 : 0) : 0) != 0;
		}

		protected virtual void ResolveParmsPoints(IncidentParms parms)
		{
			if (parms.points < 0.0)
			{
				float value = Rand.Value;
				if (value < 0.40000000596046448)
				{
					parms.points = (float)Rand.Range(40, 140);
				}
				else if (value < 0.800000011920929)
				{
					parms.points = (float)Rand.Range(140, 200);
				}
				else
				{
					parms.points = (float)Rand.Range(200, 500);
				}
			}
			IncidentParmsUtility.AdjustPointsForGroupArrivalParams(parms);
		}

		protected List<Pawn> SpawnPawns(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(parms, false);
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(this.PawnGroupKindDef, defaultPawnGroupMakerParms, false).ToList();
			foreach (Pawn item in list)
			{
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(parms.spawnCenter, map, 5, null);
				GenSpawn.Spawn(item, loc, map);
			}
			return list;
		}
	}
}
