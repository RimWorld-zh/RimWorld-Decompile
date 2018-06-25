using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class IncidentWorker_RaidFriendly : IncidentWorker_Raid
	{
		[CompilerGenerated]
		private static Func<IAttackTarget, Faction> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<IAttackTarget, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<IAttackTarget, float> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<Faction, float> <>f__am$cache3;

		[CompilerGenerated]
		private static Predicate<Pawn> <>f__am$cache4;

		public IncidentWorker_RaidFriendly()
		{
		}

		protected override bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
		{
			IEnumerable<Faction> source = (from p in map.attackTargetsCache.TargetsHostileToColony
			select ((Thing)p).Faction).Distinct<Faction>();
			return base.FactionCanBeGroupSource(f, map, desperate) && !f.def.hidden && f.PlayerRelationKind == FactionRelationKind.Ally && (!source.Any<Faction>() || source.Any((Faction hf) => hf.HostileTo(f)));
		}

		protected override bool CanFireNowSub(IncidentParms parms)
		{
			bool result;
			if (!base.CanFireNowSub(parms))
			{
				result = false;
			}
			else
			{
				Map map = (Map)parms.target;
				result = ((from p in map.attackTargetsCache.TargetsHostileToColony
				where GenHostility.IsActiveThreatToPlayer(p)
				select p).Sum(delegate(IAttackTarget p)
				{
					Pawn pawn = p as Pawn;
					float result2;
					if (pawn != null)
					{
						result2 = pawn.kindDef.combatPower;
					}
					else
					{
						result2 = 0f;
					}
					return result2;
				}) > 120f);
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
			else if (!base.CandidateFactions(map, false).Any<Faction>())
			{
				result = false;
			}
			else
			{
				parms.faction = base.CandidateFactions(map, false).RandomElementByWeight((Faction fac) => (float)fac.PlayerGoodwill + 120.000008f);
				result = true;
			}
			return result;
		}

		protected override void ResolveRaidStrategy(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			if (parms.raidStrategy == null)
			{
				parms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
			}
		}

		protected override void ResolveRaidPoints(IncidentParms parms)
		{
			if (parms.points <= 0f)
			{
				parms.points = Mathf.Min(StorytellerUtility.DefaultThreatPointsNow(parms.target), 500f);
			}
		}

		protected override string GetLetterLabel(IncidentParms parms)
		{
			return parms.raidStrategy.letterLabelFriendly;
		}

		protected override string GetLetterText(IncidentParms parms, List<Pawn> pawns)
		{
			string text = string.Format(parms.raidArrivalMode.textFriendly, parms.faction.def.pawnsPlural, parms.faction.Name);
			text += "\n\n";
			text += parms.raidStrategy.arrivalTextFriendly;
			Pawn pawn = pawns.Find((Pawn x) => x.Faction.leader == x);
			if (pawn != null)
			{
				text += "\n\n";
				text += "FriendlyRaidLeaderPresent".Translate(new object[]
				{
					pawn.Faction.def.pawnsPlural,
					pawn.LabelShort
				});
			}
			return text;
		}

		protected override LetterDef GetLetterDef()
		{
			return LetterDefOf.PositiveEvent;
		}

		protected override string GetRelatedPawnsInfoLetterText(IncidentParms parms)
		{
			return "LetterRelatedPawnsRaidFriendly".Translate(new object[]
			{
				Faction.OfPlayer.def.pawnsPlural,
				parms.faction.def.pawnsPlural
			});
		}

		[CompilerGenerated]
		private static Faction <FactionCanBeGroupSource>m__0(IAttackTarget p)
		{
			return ((Thing)p).Faction;
		}

		[CompilerGenerated]
		private static bool <CanFireNowSub>m__1(IAttackTarget p)
		{
			return GenHostility.IsActiveThreatToPlayer(p);
		}

		[CompilerGenerated]
		private static float <CanFireNowSub>m__2(IAttackTarget p)
		{
			Pawn pawn = p as Pawn;
			float result;
			if (pawn != null)
			{
				result = pawn.kindDef.combatPower;
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		[CompilerGenerated]
		private static float <TryResolveRaidFaction>m__3(Faction fac)
		{
			return (float)fac.PlayerGoodwill + 120.000008f;
		}

		[CompilerGenerated]
		private static bool <GetLetterText>m__4(Pawn x)
		{
			return x.Faction.leader == x;
		}

		[CompilerGenerated]
		private sealed class <FactionCanBeGroupSource>c__AnonStorey0
		{
			internal Faction f;

			public <FactionCanBeGroupSource>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Faction hf)
			{
				return hf.HostileTo(this.f);
			}
		}
	}
}
