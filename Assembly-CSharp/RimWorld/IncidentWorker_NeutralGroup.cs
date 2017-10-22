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
			if (!this.TryResolveParmsGeneral(parms))
			{
				return false;
			}
			this.ResolveParmsPoints(parms);
			return true;
		}

		protected virtual bool TryResolveParmsGeneral(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!parms.spawnCenter.IsValid && !RCellFinder.TryFindRandomPawnEntryCell(out parms.spawnCenter, map, CellFinder.EdgeRoadChance_Neutral, (Predicate<IntVec3>)null))
			{
				return false;
			}
			if (parms.faction == null && !base.CandidateFactions(map, false).TryRandomElement<Faction>(out parms.faction) && !base.CandidateFactions(map, true).TryRandomElement<Faction>(out parms.faction))
			{
				return false;
			}
			return true;
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
			PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(parms);
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(this.PawnGroupKindDef, defaultPawnGroupMakerParms, false).ToList();
			List<Pawn>.Enumerator enumerator = list.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Pawn current = enumerator.Current;
					IntVec3 loc = CellFinder.RandomClosewalkCellNear(parms.spawnCenter, map, 5, null);
					GenSpawn.Spawn(current, loc, map);
				}
				return list;
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
		}
	}
}
