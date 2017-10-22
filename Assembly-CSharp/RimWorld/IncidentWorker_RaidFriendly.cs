using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class IncidentWorker_RaidFriendly : IncidentWorker_Raid
	{
		protected override bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
		{
			IEnumerable<Faction> source = (from p in map.attackTargetsCache.TargetsHostileToColony
			select ((Thing)p).Faction).Distinct();
			return base.FactionCanBeGroupSource(f, map, desperate) && !f.def.hidden && !f.HostileTo(Faction.OfPlayer) && (!source.Any() || source.Any((Func<Faction, bool>)((Faction hf) => hf.HostileTo(f))));
		}

		protected override bool CanFireNowSub(IIncidentTarget target)
		{
			bool result;
			if (!base.CanFireNowSub(target))
			{
				result = false;
			}
			else
			{
				Map map = (Map)target;
				result = ((from p in map.attackTargetsCache.TargetsHostileToColony
				where GenHostility.IsActiveThreatToPlayer(p)
				select p).Sum((Func<IAttackTarget, float>)delegate(IAttackTarget p)
				{
					Pawn pawn = p as Pawn;
					return (float)((pawn == null) ? 0.0 : pawn.kindDef.combatPower);
				}) > 120.0);
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
			else if (!base.CandidateFactions(map, false).Any())
			{
				result = false;
			}
			else
			{
				parms.faction = base.CandidateFactions(map, false).RandomElementByWeight((Func<Faction, float>)((Faction fac) => (float)(fac.PlayerGoodwill + 120.00000762939453)));
				result = true;
			}
			return result;
		}

		protected override void ResolveRaidStrategy(IncidentParms parms)
		{
			if (parms.raidStrategy == null)
			{
				parms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
			}
		}

		protected override void ResolveRaidPoints(IncidentParms parms)
		{
			parms.points = (float)Rand.Range(400, 800);
		}

		protected override string GetLetterLabel(IncidentParms parms)
		{
			return parms.raidStrategy.letterLabelFriendly;
		}

		protected override string GetLetterText(IncidentParms parms, List<Pawn> pawns)
		{
			string str = (string)null;
			switch (parms.raidArrivalMode)
			{
			case PawnsArriveMode.EdgeWalkIn:
			{
				str = "FriendlyRaidWalkIn".Translate(parms.faction.def.pawnsPlural, parms.faction.Name);
				break;
			}
			case PawnsArriveMode.EdgeDrop:
			{
				str = "FriendlyRaidEdgeDrop".Translate(parms.faction.def.pawnsPlural, parms.faction.Name);
				break;
			}
			case PawnsArriveMode.CenterDrop:
			{
				str = "FriendlyRaidCenterDrop".Translate(parms.faction.def.pawnsPlural, parms.faction.Name);
				break;
			}
			}
			str += "\n\n";
			str += parms.raidStrategy.arrivalTextFriendly;
			Pawn pawn = pawns.Find((Predicate<Pawn>)((Pawn x) => x.Faction.leader == x));
			if (pawn != null)
			{
				str += "\n\n";
				str += "FriendlyRaidLeaderPresent".Translate(pawn.Faction.def.pawnsPlural, pawn.LabelShort);
			}
			return str;
		}

		protected override LetterDef GetLetterDef()
		{
			return LetterDefOf.PositiveEvent;
		}

		protected override string GetRelatedPawnsInfoLetterText(IncidentParms parms)
		{
			return "LetterRelatedPawnsRaidFriendly".Translate(parms.faction.def.pawnsPlural);
		}
	}
}
