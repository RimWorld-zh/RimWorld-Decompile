using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000346 RID: 838
	public class IncidentWorker_RaidEnemy : IncidentWorker_Raid
	{
		// Token: 0x06000E5C RID: 3676 RVA: 0x0007A7C4 File Offset: 0x00078BC4
		protected override bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
		{
			return base.FactionCanBeGroupSource(f, map, desperate) && f.HostileTo(Faction.OfPlayer) && (desperate || (float)GenDate.DaysPassed >= f.def.earliestRaidDays);
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x0007A81C File Offset: 0x00078C1C
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

		// Token: 0x06000E5E RID: 3678 RVA: 0x0007A86C File Offset: 0x00078C6C
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
				if (num <= 0f)
				{
					num = 999999f;
				}
				result = (PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(num, out parms.faction, (Faction f) => this.FactionCanBeGroupSource(f, map, false), true, true, true, true) || PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(num, out parms.faction, (Faction f) => this.FactionCanBeGroupSource(f, map, true), true, true, true, true));
			}
			return result;
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x0007A91A File Offset: 0x00078D1A
		protected override void ResolveRaidPoints(IncidentParms parms)
		{
			if (parms.points <= 0f)
			{
				Log.Error("RaidEnemy is resolving raid points. They should always be set before initiating the incident.", false);
				parms.points = StorytellerUtility.DefaultThreatPointsNow(parms.target);
			}
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x0007A94C File Offset: 0x00078D4C
		protected override void ResolveRaidStrategy(IncidentParms parms)
		{
			if (parms.raidStrategy == null)
			{
				Map map = (Map)parms.target;
				if (!(from d in DefDatabase<RaidStrategyDef>.AllDefs
				where d.Worker.CanUseWith(parms) && (parms.raidArrivalMode != null || (d.arriveModes != null && d.arriveModes.Any((PawnsArrivalModeDef x) => x.Worker.CanUseWith(parms))))
				select d).TryRandomElementByWeight((RaidStrategyDef d) => d.Worker.SelectionWeight(map, parms.points), out parms.raidStrategy))
				{
					Log.Error(string.Concat(new object[]
					{
						"No raid stategy for ",
						parms.faction,
						" with points ",
						parms.points,
						"\nparms=",
						parms
					}), false);
					if (!Prefs.DevMode)
					{
						parms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
					}
				}
			}
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x0007AA38 File Offset: 0x00078E38
		protected override string GetLetterLabel(IncidentParms parms)
		{
			return parms.raidStrategy.letterLabelEnemy;
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x0007AA58 File Offset: 0x00078E58
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

		// Token: 0x06000E63 RID: 3683 RVA: 0x0007AB24 File Offset: 0x00078F24
		protected override LetterDef GetLetterDef()
		{
			return LetterDefOf.ThreatBig;
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x0007AB40 File Offset: 0x00078F40
		protected override string GetRelatedPawnsInfoLetterText(IncidentParms parms)
		{
			return "LetterRelatedPawnsRaidEnemy".Translate(new object[]
			{
				Faction.OfPlayer.def.pawnsPlural,
				parms.faction.def.pawnsPlural
			});
		}
	}
}
