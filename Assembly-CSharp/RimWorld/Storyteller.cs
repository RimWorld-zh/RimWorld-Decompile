using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200035F RID: 863
	public class Storyteller : IExposable
	{
		// Token: 0x04000932 RID: 2354
		public StorytellerDef def;

		// Token: 0x04000933 RID: 2355
		public DifficultyDef difficulty;

		// Token: 0x04000934 RID: 2356
		public List<StorytellerComp> storytellerComps;

		// Token: 0x04000935 RID: 2357
		public IncidentQueue incidentQueue = new IncidentQueue();

		// Token: 0x04000936 RID: 2358
		public StoryIntender_Population intenderPopulation;

		// Token: 0x04000937 RID: 2359
		public static readonly Vector2 PortraitSizeTiny = new Vector2(116f, 124f);

		// Token: 0x04000938 RID: 2360
		public static readonly Vector2 PortraitSizeLarge = new Vector2(580f, 620f);

		// Token: 0x04000939 RID: 2361
		public const int IntervalsPerDay = 60;

		// Token: 0x0400093A RID: 2362
		public const int CheckInterval = 1000;

		// Token: 0x0400093B RID: 2363
		private static List<IIncidentTarget> tmpAllIncidentTargets = new List<IIncidentTarget>();

		// Token: 0x0400093C RID: 2364
		private string debugStringCached = "Generating data...";

		// Token: 0x06000EF6 RID: 3830 RVA: 0x0007E26C File Offset: 0x0007C66C
		public Storyteller()
		{
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x0007E28B File Offset: 0x0007C68B
		public Storyteller(StorytellerDef def, DifficultyDef difficulty)
		{
			this.def = def;
			this.difficulty = difficulty;
			this.intenderPopulation = new StoryIntender_Population(this);
			this.InitializeStorytellerComps();
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000EF8 RID: 3832 RVA: 0x0007E2CC File Offset: 0x0007C6CC
		public List<IIncidentTarget> AllIncidentTargets
		{
			get
			{
				Storyteller.tmpAllIncidentTargets.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					Storyteller.tmpAllIncidentTargets.Add(maps[i]);
				}
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int j = 0; j < caravans.Count; j++)
				{
					if (caravans[j].IsPlayerControlled)
					{
						Storyteller.tmpAllIncidentTargets.Add(caravans[j]);
					}
				}
				Storyteller.tmpAllIncidentTargets.Add(Find.World);
				return Storyteller.tmpAllIncidentTargets;
			}
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x0007E377 File Offset: 0x0007C777
		public static void StorytellerStaticUpdate()
		{
			Storyteller.tmpAllIncidentTargets.Clear();
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x0007E384 File Offset: 0x0007C784
		private void InitializeStorytellerComps()
		{
			this.storytellerComps = new List<StorytellerComp>();
			for (int i = 0; i < this.def.comps.Count; i++)
			{
				StorytellerComp storytellerComp = (StorytellerComp)Activator.CreateInstance(this.def.comps[i].compClass);
				storytellerComp.props = this.def.comps[i];
				this.storytellerComps.Add(storytellerComp);
			}
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x0007E404 File Offset: 0x0007C804
		public void ExposeData()
		{
			Scribe_Defs.Look<StorytellerDef>(ref this.def, "def");
			Scribe_Defs.Look<DifficultyDef>(ref this.difficulty, "difficulty");
			Scribe_Deep.Look<IncidentQueue>(ref this.incidentQueue, "incidentQueue", new object[0]);
			Scribe_Deep.Look<StoryIntender_Population>(ref this.intenderPopulation, "intenderPopulation", new object[]
			{
				this
			});
			if (this.difficulty == null)
			{
				Log.Error("Loaded storyteller without difficulty", false);
				this.difficulty = DefDatabase<DifficultyDef>.AllDefsListForReading[3];
			}
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				this.InitializeStorytellerComps();
			}
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x0007E49C File Offset: 0x0007C89C
		public void StorytellerTick()
		{
			this.incidentQueue.IncidentQueueTick();
			if (Find.TickManager.TicksGame % 1000 == 0)
			{
				if (DebugSettings.enableStoryteller)
				{
					foreach (FiringIncident fi in this.MakeIncidentsForInterval())
					{
						this.TryFire(fi);
					}
				}
			}
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x0007E52C File Offset: 0x0007C92C
		public void TryFire(FiringIncident fi)
		{
			if (fi.parms.forced || fi.def.Worker.CanFireNow(fi.parms))
			{
				if (fi.def.Worker.TryExecute(fi.parms))
				{
					fi.parms.target.StoryState.Notify_IncidentFired(fi);
				}
			}
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x0007E598 File Offset: 0x0007C998
		public IEnumerable<FiringIncident> MakeIncidentsForInterval()
		{
			List<IIncidentTarget> targets = this.AllIncidentTargets;
			for (int i = 0; i < this.storytellerComps.Count; i++)
			{
				foreach (FiringIncident incident in this.MakeIncidentsForInterval(this.storytellerComps[i], targets))
				{
					yield return incident;
				}
			}
			yield break;
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x0007E5C4 File Offset: 0x0007C9C4
		public IEnumerable<FiringIncident> MakeIncidentsForInterval(StorytellerComp comp, List<IIncidentTarget> targets)
		{
			if (GenDate.DaysPassedFloat <= comp.props.minDaysPassed)
			{
				yield break;
			}
			for (int i = 0; i < targets.Count; i++)
			{
				IIncidentTarget targ = targets[i];
				if (comp.props.allowedTargetTypes == null || comp.props.allowedTargetTypes.Count == 0 || comp.props.allowedTargetTypes.Intersect(targ.AcceptedTypes()).Any<IncidentTargetTypeDef>())
				{
					foreach (FiringIncident fi in comp.MakeIntervalIncidents(targ))
					{
						if (Find.Storyteller.difficulty.allowBigThreats || (fi.def.category != IncidentCategoryDefOf.ThreatBig && fi.def.category != IncidentCategoryDefOf.RaidBeacon))
						{
							yield return fi;
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x0007E5F5 File Offset: 0x0007C9F5
		public void Notify_DefChanged()
		{
			this.InitializeStorytellerComps();
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x0007E600 File Offset: 0x0007CA00
		public string DebugString()
		{
			if (Time.frameCount % 60 == 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Storyteller : " + this.def.label);
				stringBuilder.AppendLine("------------- Global threats data ---------------");
				stringBuilder.AppendLine("   NumRaidsEnemy: " + Find.StoryWatcher.statsRecord.numRaidsEnemy);
				stringBuilder.AppendLine("   TotalThreatPointsFactor: " + Find.StoryWatcher.watcherRampUp.TotalThreatPointsFactor.ToString("F5"));
				stringBuilder.AppendLine("      ShortTermFactor: " + Find.StoryWatcher.watcherRampUp.ShortTermFactor.ToString("F5"));
				stringBuilder.AppendLine("      LongTermFactor: " + Find.StoryWatcher.watcherRampUp.LongTermFactor.ToString("F5"));
				stringBuilder.AppendLine("   AllyAssistanceMTBMultiplier (ally): " + StorytellerUtility.AllyIncidentMTBMultiplier(false).ToString());
				stringBuilder.AppendLine("   AllyAssistanceMTBMultiplier (non-hostile): " + StorytellerUtility.AllyIncidentMTBMultiplier(true).ToString());
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("-------------- Global population data --------------");
				stringBuilder.AppendLine(this.intenderPopulation.DebugReadout);
				stringBuilder.AppendLine("------------- All incident targets --------------");
				for (int i = 0; i < this.AllIncidentTargets.Count; i++)
				{
					stringBuilder.AppendLine("   " + this.AllIncidentTargets[i].ToString());
				}
				IIncidentTarget incidentTarget = Find.WorldSelector.SingleSelectedObject as IIncidentTarget;
				if (incidentTarget == null)
				{
					incidentTarget = Find.CurrentMap;
				}
				if (incidentTarget != null)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("---------- Selected: " + incidentTarget + " --------");
					stringBuilder.AppendLine("   Wealth: " + incidentTarget.PlayerWealthForStoryteller.ToString("F0"));
					stringBuilder.AppendLine("   IncidentPointsRandomFactorRange: " + incidentTarget.IncidentPointsRandomFactorRange);
					stringBuilder.AppendLine("   Pawns-Humanlikes: " + (from p in incidentTarget.PlayerPawnsForStoryteller
					where p.def.race.Humanlike
					select p).Count<Pawn>());
					stringBuilder.AppendLine("   Pawns-Animals: " + (from p in incidentTarget.PlayerPawnsForStoryteller
					where p.def.race.Animal
					select p).Count<Pawn>());
					Map map = incidentTarget as Map;
					if (map != null)
					{
						stringBuilder.AppendLine("   StoryDanger: " + map.dangerWatcher.DangerRating);
						stringBuilder.AppendLine("   FireDanger: " + map.fireWatcher.FireDanger.ToString("F2"));
						stringBuilder.AppendLine("   DaysSinceSeriousDamage: " + map.damageWatcher.DaysSinceSeriousDamage.ToString("F1"));
						stringBuilder.AppendLine("   LastThreatBigQueueTick: " + map.storyState.LastThreatBigTick.ToStringTicksToPeriod());
					}
					stringBuilder.AppendLine("   Current points (ignoring early raid factors): " + StorytellerUtility.DefaultThreatPointsNow(incidentTarget).ToString("F0"));
					stringBuilder.AppendLine("   Current points for specific IncidentMakers:");
					for (int j = 0; j < this.storytellerComps.Count; j++)
					{
						IncidentParms incidentParms = this.storytellerComps[j].GenerateParms(IncidentCategoryDefOf.ThreatBig, incidentTarget);
						stringBuilder.AppendLine("      " + this.storytellerComps[j].GetType().ToString().Substring(23) + ": " + incidentParms.points.ToString("F0"));
					}
				}
				this.debugStringCached = stringBuilder.ToString();
			}
			return this.debugStringCached;
		}
	}
}
