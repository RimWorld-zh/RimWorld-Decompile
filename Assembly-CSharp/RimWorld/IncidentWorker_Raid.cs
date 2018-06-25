using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public abstract class IncidentWorker_Raid : IncidentWorker_PawnsArrive
	{
		[CompilerGenerated]
		private static Func<PawnsArrivalModeDef, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<List<Pawn>, int> <>f__am$cache1;

		[CompilerGenerated]
		private static Predicate<Apparel> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<Faction, string> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<RaidStrategyDef, string> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<PawnsArrivalModeDef, string> <>f__am$cache5;

		protected IncidentWorker_Raid()
		{
		}

		protected abstract bool TryResolveRaidFaction(IncidentParms parms);

		protected abstract void ResolveRaidStrategy(IncidentParms parms, PawnGroupKindDef groupKind);

		protected abstract string GetLetterLabel(IncidentParms parms);

		protected abstract string GetLetterText(IncidentParms parms, List<Pawn> pawns);

		protected abstract LetterDef GetLetterDef();

		protected abstract string GetRelatedPawnsInfoLetterText(IncidentParms parms);

		protected abstract void ResolveRaidPoints(IncidentParms parms);

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

		[CompilerGenerated]
		private static bool <ResolveRaidArriveMode>m__0(PawnsArrivalModeDef d)
		{
			return d.forQuickMilitaryAid;
		}

		[CompilerGenerated]
		private static int <TryExecuteWorker>m__1(List<Pawn> x)
		{
			return x.Count;
		}

		[CompilerGenerated]
		private static bool <TryExecuteWorker>m__2(Apparel ap)
		{
			return ap is ShieldBelt;
		}

		[CompilerGenerated]
		private static string <DoTable_RaidFactionSampled>m__3(Faction f)
		{
			return f.Name;
		}

		[CompilerGenerated]
		private static string <DoTable_RaidStrategySampled>m__4(RaidStrategyDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <DoTable_RaidArrivalModeSampled>m__5(PawnsArrivalModeDef f)
		{
			return f.defName;
		}

		[CompilerGenerated]
		private sealed class <ResolveRaidArriveMode>c__AnonStorey0
		{
			internal IncidentParms parms;

			public <ResolveRaidArriveMode>c__AnonStorey0()
			{
			}

			internal bool <>m__0(PawnsArrivalModeDef d)
			{
				return d.Worker.GetSelectionWeight(this.parms) > 0f;
			}

			internal bool <>m__1(PawnsArrivalModeDef x)
			{
				return x.Worker.CanUseWith(this.parms);
			}

			internal float <>m__2(PawnsArrivalModeDef x)
			{
				return x.Worker.GetSelectionWeight(this.parms);
			}
		}

		[CompilerGenerated]
		private sealed class <DoTable_RaidFactionSampled>c__AnonStorey1
		{
			internal Dictionary<Faction, int> factionCount;

			public <DoTable_RaidFactionSampled>c__AnonStorey1()
			{
			}

			internal string <>m__0(Faction str)
			{
				int num = this.factionCount[str];
				return ((float)num / 500f).ToStringPercent();
			}
		}

		[CompilerGenerated]
		private sealed class <DoTable_RaidStrategySampled>c__AnonStorey2
		{
			internal Dictionary<RaidStrategyDef, int> strats;

			public <DoTable_RaidStrategySampled>c__AnonStorey2()
			{
			}

			internal string <>m__0(RaidStrategyDef str)
			{
				int num = this.strats[str];
				return ((float)num / 500f).ToStringPercent();
			}
		}

		[CompilerGenerated]
		private sealed class <DoTable_RaidArrivalModeSampled>c__AnonStorey3
		{
			internal Dictionary<PawnsArrivalModeDef, int> modeCount;

			public <DoTable_RaidArrivalModeSampled>c__AnonStorey3()
			{
			}

			internal string <>m__0(PawnsArrivalModeDef str)
			{
				int num = this.modeCount[str];
				return ((float)num / 500f).ToStringPercent();
			}
		}
	}
}
