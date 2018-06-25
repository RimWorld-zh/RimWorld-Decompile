using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200034A RID: 842
	public abstract class IncidentWorker_Raid : IncidentWorker_PawnsArrive
	{
		// Token: 0x06000E70 RID: 3696
		protected abstract bool TryResolveRaidFaction(IncidentParms parms);

		// Token: 0x06000E71 RID: 3697
		protected abstract void ResolveRaidStrategy(IncidentParms parms, PawnGroupKindDef groupKind);

		// Token: 0x06000E72 RID: 3698
		protected abstract string GetLetterLabel(IncidentParms parms);

		// Token: 0x06000E73 RID: 3699
		protected abstract string GetLetterText(IncidentParms parms, List<Pawn> pawns);

		// Token: 0x06000E74 RID: 3700
		protected abstract LetterDef GetLetterDef();

		// Token: 0x06000E75 RID: 3701
		protected abstract string GetRelatedPawnsInfoLetterText(IncidentParms parms);

		// Token: 0x06000E76 RID: 3702
		protected abstract void ResolveRaidPoints(IncidentParms parms);

		// Token: 0x06000E77 RID: 3703 RVA: 0x00079944 File Offset: 0x00077D44
		protected virtual void ResolveRaidArriveMode(IncidentParms parms)
		{
			if (parms.raidArrivalMode == null)
			{
				if (parms.raidArrivalModeForQuickMilitaryAid)
				{
					if (!(from d in DefDatabase<PawnsArrivalModeDef>.AllDefs
					where d.forQuickMilitaryAid
					select d).Any((PawnsArrivalModeDef d) => d.Worker.GetSelectionWeight(parms) > 0f))
					{
						parms.raidArrivalMode = ((Rand.Value >= 0.6f) ? PawnsArrivalModeDefOf.CenterDrop : PawnsArrivalModeDefOf.EdgeDrop);
						return;
					}
				}
				if (!(from x in parms.raidStrategy.arriveModes
				where x.Worker.CanUseWith(parms)
				select x).TryRandomElementByWeight((PawnsArrivalModeDef x) => x.Worker.GetSelectionWeight(parms), out parms.raidArrivalMode))
				{
					Log.Error("Could not resolve arrival mode for raid. Defaulting to EdgeWalkIn. parms=" + parms, false);
					parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
				}
			}
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x00079A5C File Offset: 0x00077E5C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			this.ResolveRaidPoints(parms);
			bool result;
			if (!this.TryResolveRaidFaction(parms))
			{
				result = false;
			}
			else
			{
				PawnGroupKindDef combat = PawnGroupKindDefOf.Combat;
				this.ResolveRaidStrategy(parms, combat);
				this.ResolveRaidArriveMode(parms);
				if (!parms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(parms))
				{
					result = false;
				}
				else
				{
					parms.points *= parms.raidArrivalMode.pointsFactor;
					parms.points *= parms.raidStrategy.pointsFactor;
					parms.points = Mathf.Max(parms.points, parms.raidStrategy.Worker.MinimumPoints(parms.faction, combat) * 1.05f);
					PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(combat, parms, false);
					List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms, true).ToList<Pawn>();
					if (list.Count == 0)
					{
						Log.Error("Got no pawns spawning raid from parms " + parms, false);
						result = false;
					}
					else
					{
						parms.raidArrivalMode.Worker.Arrive(list, parms);
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.AppendLine("Points = " + parms.points.ToString("F0"));
						foreach (Pawn pawn in list)
						{
							string str = (pawn.equipment == null || pawn.equipment.Primary == null) ? "unarmed" : pawn.equipment.Primary.LabelCap;
							stringBuilder.AppendLine(pawn.KindLabel + " - " + str);
						}
						string letterLabel = this.GetLetterLabel(parms);
						string letterText = this.GetLetterText(parms, list);
						PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(list, ref letterLabel, ref letterText, this.GetRelatedPawnsInfoLetterText(parms), true, true);
						List<TargetInfo> list2 = new List<TargetInfo>();
						if (parms.pawnGroups != null)
						{
							List<List<Pawn>> list3 = IncidentParmsUtility.SplitIntoGroups(list, parms.pawnGroups);
							List<Pawn> list4 = list3.MaxBy((List<Pawn> x) => x.Count);
							if (list4.Any<Pawn>())
							{
								list2.Add(list4[0]);
							}
							for (int i = 0; i < list3.Count; i++)
							{
								if (list3[i] != list4)
								{
									if (list3[i].Any<Pawn>())
									{
										list2.Add(list3[i][0]);
									}
								}
							}
						}
						else if (list.Any<Pawn>())
						{
							list2.Add(list[0]);
						}
						Find.LetterStack.ReceiveLetter(letterLabel, letterText, this.GetLetterDef(), list2, parms.faction, stringBuilder.ToString());
						if (this.GetLetterDef() == LetterDefOf.ThreatBig)
						{
							TaleRecorder.RecordTale(TaleDefOf.RaidArrived, new object[0]);
						}
						parms.raidStrategy.Worker.MakeLords(parms, list);
						AvoidGridMaker.RegenerateAvoidGridsFor(parms.faction, map);
						LessonAutoActivator.TeachOpportunity(ConceptDefOf.EquippingWeapons, OpportunityType.Critical);
						if (!PlayerKnowledgeDatabase.IsComplete(ConceptDefOf.ShieldBelts))
						{
							for (int j = 0; j < list.Count; j++)
							{
								Pawn pawn2 = list[j];
								if (pawn2.apparel.WornApparel.Any((Apparel ap) => ap is ShieldBelt))
								{
									LessonAutoActivator.TeachOpportunity(ConceptDefOf.ShieldBelts, OpportunityType.Critical);
									break;
								}
							}
						}
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x00079E48 File Offset: 0x00078248
		public void DoTable_RaidFactionSampled()
		{
			int ticksGame = Find.TickManager.TicksGame;
			Find.TickManager.DebugSetTicksGame(36000000);
			List<TableDataGetter<Faction>> list = new List<TableDataGetter<Faction>>();
			list.Add(new TableDataGetter<Faction>("name", (Faction f) => f.Name));
			foreach (float num in Dialog_DebugActionsMenu.PointsOptions(false))
			{
				float points = num;
				Dictionary<Faction, int> factionCount = new Dictionary<Faction, int>();
				foreach (Faction key in Find.FactionManager.AllFactions)
				{
					factionCount.Add(key, 0);
				}
				for (int i = 0; i < 500; i++)
				{
					IncidentParms incidentParms = new IncidentParms();
					incidentParms.target = Find.CurrentMap;
					incidentParms.points = points;
					if (this.TryResolveRaidFaction(incidentParms))
					{
						Dictionary<Faction, int> factionCount2;
						Faction faction;
						(factionCount2 = factionCount)[faction = incidentParms.faction] = factionCount2[faction] + 1;
					}
				}
				list.Add(new TableDataGetter<Faction>(points.ToString("F0"), delegate(Faction str)
				{
					int num2 = factionCount[str];
					return ((float)num2 / 500f).ToStringPercent();
				}));
			}
			Find.TickManager.DebugSetTicksGame(ticksGame);
			DebugTables.MakeTablesDialog<Faction>(Find.FactionManager.AllFactions, list.ToArray());
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x0007A028 File Offset: 0x00078428
		public void DoTable_RaidStrategySampled(Faction fac)
		{
			int ticksGame = Find.TickManager.TicksGame;
			Find.TickManager.DebugSetTicksGame(36000000);
			List<TableDataGetter<RaidStrategyDef>> list = new List<TableDataGetter<RaidStrategyDef>>();
			list.Add(new TableDataGetter<RaidStrategyDef>("defName", (RaidStrategyDef d) => d.defName));
			foreach (float num in Dialog_DebugActionsMenu.PointsOptions(false))
			{
				float points = num;
				Dictionary<RaidStrategyDef, int> strats = new Dictionary<RaidStrategyDef, int>();
				foreach (RaidStrategyDef key in DefDatabase<RaidStrategyDef>.AllDefs)
				{
					strats.Add(key, 0);
				}
				for (int i = 0; i < 500; i++)
				{
					IncidentParms incidentParms = new IncidentParms();
					incidentParms.target = Find.CurrentMap;
					incidentParms.points = points;
					incidentParms.faction = fac;
					if (this.TryResolveRaidFaction(incidentParms))
					{
						this.ResolveRaidStrategy(incidentParms, PawnGroupKindDefOf.Combat);
						if (incidentParms.raidStrategy != null)
						{
							Dictionary<RaidStrategyDef, int> strats2;
							RaidStrategyDef raidStrategy;
							(strats2 = strats)[raidStrategy = incidentParms.raidStrategy] = strats2[raidStrategy] + 1;
						}
					}
				}
				list.Add(new TableDataGetter<RaidStrategyDef>(points.ToString("F0"), delegate(RaidStrategyDef str)
				{
					int num2 = strats[str];
					return ((float)num2 / 500f).ToStringPercent();
				}));
			}
			Find.TickManager.DebugSetTicksGame(ticksGame);
			DebugTables.MakeTablesDialog<RaidStrategyDef>(DefDatabase<RaidStrategyDef>.AllDefs, list.ToArray());
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x0007A224 File Offset: 0x00078624
		public void DoTable_RaidArrivalModeSampled(Faction fac)
		{
			int ticksGame = Find.TickManager.TicksGame;
			Find.TickManager.DebugSetTicksGame(36000000);
			List<TableDataGetter<PawnsArrivalModeDef>> list = new List<TableDataGetter<PawnsArrivalModeDef>>();
			list.Add(new TableDataGetter<PawnsArrivalModeDef>("mode", (PawnsArrivalModeDef f) => f.defName));
			foreach (float num in Dialog_DebugActionsMenu.PointsOptions(false))
			{
				float points = num;
				Dictionary<PawnsArrivalModeDef, int> modeCount = new Dictionary<PawnsArrivalModeDef, int>();
				foreach (PawnsArrivalModeDef key in DefDatabase<PawnsArrivalModeDef>.AllDefs)
				{
					modeCount.Add(key, 0);
				}
				for (int i = 0; i < 500; i++)
				{
					IncidentParms incidentParms = new IncidentParms();
					incidentParms.target = Find.CurrentMap;
					incidentParms.points = points;
					incidentParms.faction = fac;
					if (this.TryResolveRaidFaction(incidentParms))
					{
						this.ResolveRaidStrategy(incidentParms, PawnGroupKindDefOf.Combat);
						this.ResolveRaidArriveMode(incidentParms);
						Dictionary<PawnsArrivalModeDef, int> modeCount2;
						PawnsArrivalModeDef raidArrivalMode;
						(modeCount2 = modeCount)[raidArrivalMode = incidentParms.raidArrivalMode] = modeCount2[raidArrivalMode] + 1;
					}
				}
				list.Add(new TableDataGetter<PawnsArrivalModeDef>(points.ToString("F0"), delegate(PawnsArrivalModeDef str)
				{
					int num2 = modeCount[str];
					return ((float)num2 / 500f).ToStringPercent();
				}));
			}
			Find.TickManager.DebugSetTicksGame(ticksGame);
			DebugTables.MakeTablesDialog<PawnsArrivalModeDef>(DefDatabase<PawnsArrivalModeDef>.AllDefs, list.ToArray());
		}
	}
}
