using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public abstract class IncidentWorker_NeutralGroup : IncidentWorker_PawnsArrive
	{
		protected IncidentWorker_NeutralGroup()
		{
		}

		protected virtual PawnGroupKindDef PawnGroupKindDef
		{
			get
			{
				return PawnGroupKindDefOf.Peaceful;
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
			if (!parms.spawnCenter.IsValid)
			{
				if (!RCellFinder.TryFindRandomPawnEntryCell(out parms.spawnCenter, map, CellFinder.EdgeRoadChance_Neutral, null))
				{
					return false;
				}
			}
			if (parms.faction == null)
			{
				if (!base.CandidateFactions(map, false).TryRandomElement(out parms.faction))
				{
					if (!base.CandidateFactions(map, true).TryRandomElement(out parms.faction))
					{
						return false;
					}
				}
			}
			return true;
		}

		protected abstract void ResolveParmsPoints(IncidentParms parms);

		protected List<Pawn> SpawnPawns(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(this.PawnGroupKindDef, parms, true);
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms, false).ToList<Pawn>();
			foreach (Pawn newThing in list)
			{
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(parms.spawnCenter, map, 5, null);
				GenSpawn.Spawn(newThing, loc, map, WipeMode.Vanish);
			}
			return list;
		}
	}
}
