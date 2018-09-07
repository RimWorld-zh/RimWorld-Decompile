using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_RaidEnemy : IncidentWorker_Raid
	{
		[CompilerGenerated]
		private static Predicate<Pawn> <>f__am$cache0;

		public IncidentWorker_RaidEnemy()
		{
		}

		protected override bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
		{
			return base.FactionCanBeGroupSource(f, map, desperate) && f.HostileTo(Faction.OfPlayer) && (desperate || (float)GenDate.DaysPassed >= f.def.earliestRaidDays);
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			if (!base.TryExecuteWorker(parms))
			{
				return false;
			}
			Find.TickManager.slower.SignalForceNormalSpeedShort();
			Find.StoryWatcher.statsRecord.numRaidsEnemy++;
			return true;
		}

		protected override bool TryResolveRaidFaction(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (parms.faction != null)
			{
				return true;
			}
			float num = parms.points;
			if (num <= 0f)
			{
				num = 999999f;
			}
			return PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(num, out parms.faction, (Faction f) => this.FactionCanBeGroupSource(f, map, false), true, true, true, true) || PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(num, out parms.faction, (Faction f) => this.FactionCanBeGroupSource(f, map, true), true, true, true, true);
		}

		protected override void ResolveRaidPoints(IncidentParms parms)
		{
			if (parms.points <= 0f)
			{
				Log.Error("RaidEnemy is resolving raid points. They should always be set before initiating the incident.", false);
				parms.points = StorytellerUtility.DefaultThreatPointsNow(parms.target);
			}
		}

		protected override void ResolveRaidStrategy(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			if (parms.raidStrategy != null)
			{
				return;
			}
			Map map = (Map)parms.target;
			if (!(from d in DefDatabase<RaidStrategyDef>.AllDefs
			where d.Worker.CanUseWith(parms, groupKind) && (parms.raidArrivalMode != null || (d.arriveModes != null && d.arriveModes.Any((PawnsArrivalModeDef x) => x.Worker.CanUseWith(parms))))
			select d).TryRandomElementByWeight((RaidStrategyDef d) => d.Worker.SelectionWeight(map, parms.points), out parms.raidStrategy))
			{
				Log.Error(string.Concat(new object[]
				{
					"No raid stategy for ",
					parms.faction,
					" with points ",
					parms.points,
					", groupKind=",
					groupKind,
					"\nparms=",
					parms
				}), false);
				if (!Prefs.DevMode)
				{
					parms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
				}
			}
		}

		protected override string GetLetterLabel(IncidentParms parms)
		{
			return parms.raidStrategy.letterLabelEnemy;
		}

		protected override string GetLetterText(IncidentParms parms, List<Pawn> pawns)
		{
			string text = string.Format(parms.raidArrivalMode.textEnemy, parms.faction.def.pawnsPlural, parms.faction.Name);
			text += "\n\n";
			text += parms.raidStrategy.arrivalTextEnemy;
			Pawn pawn = pawns.Find((Pawn x) => x.Faction.leader == x);
			if (pawn != null)
			{
				text += "\n\n";
				text += "EnemyRaidLeaderPresent".Translate(new object[]
				{
					pawn.Faction.def.pawnsPlural,
					pawn.LabelShort
				});
			}
			return text;
		}

		protected override LetterDef GetLetterDef()
		{
			return LetterDefOf.ThreatBig;
		}

		protected override string GetRelatedPawnsInfoLetterText(IncidentParms parms)
		{
			return "LetterRelatedPawnsRaidEnemy".Translate(new object[]
			{
				Faction.OfPlayer.def.pawnsPlural,
				parms.faction.def.pawnsPlural
			});
		}

		[CompilerGenerated]
		private static bool <GetLetterText>m__0(Pawn x)
		{
			return x.Faction.leader == x;
		}

		[CompilerGenerated]
		private sealed class <TryResolveRaidFaction>c__AnonStorey0
		{
			internal Map map;

			internal IncidentWorker_RaidEnemy $this;

			public <TryResolveRaidFaction>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Faction f)
			{
				return this.$this.FactionCanBeGroupSource(f, this.map, false);
			}

			internal bool <>m__1(Faction f)
			{
				return this.$this.FactionCanBeGroupSource(f, this.map, true);
			}
		}

		[CompilerGenerated]
		private sealed class <ResolveRaidStrategy>c__AnonStorey1
		{
			internal IncidentParms parms;

			internal PawnGroupKindDef groupKind;

			internal Map map;

			public <ResolveRaidStrategy>c__AnonStorey1()
			{
			}

			internal bool <>m__0(RaidStrategyDef d)
			{
				return d.Worker.CanUseWith(this.parms, this.groupKind) && (this.parms.raidArrivalMode != null || (d.arriveModes != null && d.arriveModes.Any((PawnsArrivalModeDef x) => x.Worker.CanUseWith(this.parms))));
			}

			internal float <>m__1(RaidStrategyDef d)
			{
				return d.Worker.SelectionWeight(this.map, this.parms.points);
			}

			internal bool <>m__2(PawnsArrivalModeDef x)
			{
				return x.Worker.CanUseWith(this.parms);
			}
		}
	}
}
