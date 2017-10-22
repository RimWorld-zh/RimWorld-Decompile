using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_RaidEnemy : IncidentWorker_Raid
	{
		protected override bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
		{
			return base.FactionCanBeGroupSource(f, map, desperate) && f.HostileTo(Faction.OfPlayer) && (desperate || (float)GenDate.DaysPassed >= f.def.earliestRaidDays);
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			bool result;
			if (!base.TryExecuteWorker(parms))
			{
				result = false;
			}
			else
			{
				Find.TickManager.slower.SignalForceNormalSpeedShort();
				Find.StoryWatcher.statsRecord.numRaidsEnemy++;
				result = true;
			}
			return result;
		}

		protected override bool TryResolveRaidFaction(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			bool result;
			if (parms.faction != null)
			{
				result = true;
			}
			else
			{
				float num = parms.points;
				if (num <= 0.0)
				{
					num = 999999f;
				}
				result = ((byte)((PawnGroupMakerUtility.TryGetRandomFactionForNormalPawnGroup(num, out parms.faction, (Predicate<Faction>)((Faction f) => this.FactionCanBeGroupSource(f, map, false)), true, true, true, true) || PawnGroupMakerUtility.TryGetRandomFactionForNormalPawnGroup(num, out parms.faction, (Predicate<Faction>)((Faction f) => this.FactionCanBeGroupSource(f, map, true)), true, true, true, true)) ? 1 : 0) != 0);
			}
			return result;
		}

		protected override void ResolveRaidPoints(IncidentParms parms)
		{
			if (parms.points <= 0.0)
			{
				Log.Error("RaidEnemy is resolving raid points. They should always be set before initiating the incident.");
				parms.points = (float)Rand.Range(50, 300);
			}
		}

		protected override void ResolveRaidStrategy(IncidentParms parms)
		{
			if (parms.raidStrategy == null)
			{
				Map map = (Map)parms.target;
				parms.raidStrategy = (from d in DefDatabase<RaidStrategyDef>.AllDefs
				where d.Worker.CanUseWith(parms)
				select d).RandomElementByWeight((Func<RaidStrategyDef, float>)((RaidStrategyDef d) => d.Worker.SelectionChance(map)));
			}
		}

		protected override string GetLetterLabel(IncidentParms parms)
		{
			return parms.raidStrategy.letterLabelEnemy;
		}

		protected override string GetLetterText(IncidentParms parms, List<Pawn> pawns)
		{
			string str = (string)null;
			switch (parms.raidArrivalMode)
			{
			case PawnsArriveMode.EdgeWalkIn:
			{
				str = "EnemyRaidWalkIn".Translate(parms.faction.def.pawnsPlural, parms.faction.Name);
				break;
			}
			case PawnsArriveMode.EdgeDrop:
			{
				str = "EnemyRaidEdgeDrop".Translate(parms.faction.def.pawnsPlural, parms.faction.Name);
				break;
			}
			case PawnsArriveMode.CenterDrop:
			{
				str = "EnemyRaidCenterDrop".Translate(parms.faction.def.pawnsPlural, parms.faction.Name);
				break;
			}
			}
			str += "\n\n";
			str += parms.raidStrategy.arrivalTextEnemy;
			Pawn pawn = pawns.Find((Predicate<Pawn>)((Pawn x) => x.Faction.leader == x));
			if (pawn != null)
			{
				str += "\n\n";
				str += "EnemyRaidLeaderPresent".Translate(pawn.Faction.def.pawnsPlural, pawn.LabelShort);
			}
			return str;
		}

		protected override LetterDef GetLetterDef()
		{
			return LetterDefOf.ThreatBig;
		}

		protected override string GetRelatedPawnsInfoLetterText(IncidentParms parms)
		{
			return "LetterRelatedPawnsRaidEnemy".Translate(parms.faction.def.pawnsPlural);
		}
	}
}
