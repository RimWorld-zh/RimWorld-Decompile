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
			if (!base.CanFireNowSub(target))
			{
				return false;
			}
			Map map = (Map)target;
			return (from p in map.attackTargetsCache.TargetsHostileToColony
			where !p.ThreatDisabled()
			select p).Sum((Func<IAttackTarget, float>)delegate(IAttackTarget p)
			{
				Pawn pawn = p as Pawn;
				if (pawn != null)
				{
					return pawn.kindDef.combatPower;
				}
				return 0f;
			}) > 120.0;
		}

		protected override bool TryResolveRaidFaction(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (parms.faction != null)
			{
				return true;
			}
			if (!base.CandidateFactions(map, false).Any())
			{
				return false;
			}
			parms.faction = base.CandidateFactions(map, false).RandomElementByWeight((Func<Faction, float>)((Faction fac) => (float)(fac.PlayerGoodwill + 120.00000762939453)));
			return true;
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
			return LetterDefOf.BadNonUrgent;
		}

		protected override string GetRelatedPawnsInfoLetterText(IncidentParms parms)
		{
			return "LetterRelatedPawnsRaidFriendly".Translate(parms.faction.def.pawnsPlural);
		}
	}
}
