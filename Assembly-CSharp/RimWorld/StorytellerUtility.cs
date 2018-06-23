using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000378 RID: 888
	public static class StorytellerUtility
	{
		// Token: 0x04000964 RID: 2404
		private const float PointsPer1000Wealth = 7.8f;

		// Token: 0x04000965 RID: 2405
		public const float BuildingWealthFactor = 0.5f;

		// Token: 0x04000966 RID: 2406
		private const float PointsPerColonist = 34f;

		// Token: 0x04000967 RID: 2407
		private const float PointsPerTameAnimalCombatPower = 0.09f;

		// Token: 0x04000968 RID: 2408
		private const float PointsPerPlayerPawnFactorInContainer = 0.3f;

		// Token: 0x04000969 RID: 2409
		private const float PointsPerPlayerPawnHealthSummaryLerpAmount = 0.5f;

		// Token: 0x0400096A RID: 2410
		private static readonly SimpleCurve PostProcessCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 35f),
				true
			},
			{
				new CurvePoint(100f, 35f),
				true
			},
			{
				new CurvePoint(1000f, 1000f),
				true
			},
			{
				new CurvePoint(2000f, 2000f),
				true
			},
			{
				new CurvePoint(4000f, 3400f),
				true
			},
			{
				new CurvePoint(5000f, 4100f),
				true
			},
			{
				new CurvePoint(100000f, 70000f),
				true
			}
		};

		// Token: 0x0400096B RID: 2411
		public const float CaravanWealthPointsFactor = 0.5f;

		// Token: 0x0400096C RID: 2412
		public static readonly FloatRange CaravanPointsRandomFactorRange = new FloatRange(0.6f, 1f);

		// Token: 0x0400096D RID: 2413
		private static Dictionary<IIncidentTarget, StoryState> tmpOldStoryStates = new Dictionary<IIncidentTarget, StoryState>();

		// Token: 0x06000F55 RID: 3925 RVA: 0x00081EA4 File Offset: 0x000802A4
		public static IncidentParms DefaultParmsNow(IncidentCategoryDef incCat, IIncidentTarget target)
		{
			if (incCat == null)
			{
				Log.Warning("Trying to get default parms for null incident category.", false);
			}
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = target;
			if (incCat.needsParmsPoints)
			{
				incidentParms.points = StorytellerUtility.DefaultThreatPointsNow(target);
				if (target is Map && incCat == IncidentCategoryDefOf.ThreatBig)
				{
					switch (Find.StoryWatcher.statsRecord.numThreatBigs)
					{
					case 0:
						incidentParms.points = 38f;
						incidentParms.raidForceOneIncap = true;
						incidentParms.raidNeverFleeIndividual = true;
						break;
					case 1:
						incidentParms.points *= 0.5f;
						break;
					case 2:
						incidentParms.points *= 0.7f;
						break;
					case 3:
						incidentParms.points *= 0.8f;
						break;
					case 4:
						incidentParms.points *= 0.9f;
						break;
					default:
						incidentParms.points *= 1f;
						break;
					}
				}
			}
			return incidentParms;
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x00081FCC File Offset: 0x000803CC
		public static float DefaultThreatPointsNow(IIncidentTarget target)
		{
			float num = target.PlayerWealthForStoryteller;
			num = Mathf.Max(num, 0f);
			float num2 = num / 1000f * 7.8f;
			float num3 = 0f;
			foreach (Pawn pawn in target.PlayerPawnsForStoryteller)
			{
				float num4 = 0f;
				if (pawn.IsFreeColonist)
				{
					num4 = 34f;
				}
				else if (pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && !pawn.Downed && pawn.training.CanAssignToTrain(TrainableDefOf.Release).Accepted)
				{
					num4 = 0.09f * pawn.kindDef.combatPower;
				}
				if (num4 > 0f)
				{
					if (pawn.ParentHolder != null && pawn.ParentHolder is Building_CryptosleepCasket)
					{
						num4 *= 0.3f;
					}
					num4 = Mathf.Lerp(num4, num4 * pawn.health.summaryHealth.SummaryHealthPercent, 0.5f);
					num3 += num4;
				}
			}
			float num5 = num2 + num3;
			num5 *= Find.StoryWatcher.watcherRampUp.TotalThreatPointsFactor;
			num5 *= Find.Storyteller.difficulty.threatScale;
			num5 *= target.IncidentPointsRandomFactorRange.RandomInRange;
			return StorytellerUtility.PostProcessCurve.Evaluate(num5);
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x00082180 File Offset: 0x00080580
		public static float AllyIncidentMTBMultiplier(bool enoughIfNonHostile)
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (!allFactionsListForReading[i].def.hidden && !allFactionsListForReading[i].IsPlayer)
				{
					if (allFactionsListForReading[i].def.CanEverBeNonHostile)
					{
						num2++;
					}
					if (allFactionsListForReading[i].PlayerRelationKind == FactionRelationKind.Ally || (enoughIfNonHostile && !allFactionsListForReading[i].HostileTo(Faction.OfPlayer)))
					{
						num++;
					}
				}
			}
			float result;
			if (num == 0)
			{
				result = -1f;
			}
			else
			{
				float num3 = (float)num / Mathf.Max((float)num2, 1f);
				result = 1f / num3;
			}
			return result;
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x00082264 File Offset: 0x00080664
		public static void ShowFutureIncidentsDebugLogFloatMenu(bool currentMapOnly)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption("-All comps-", delegate()
			{
				StorytellerUtility.DebugLogTestFutureIncidents(currentMapOnly, null);
			}, MenuOptionPriority.Default, null, null, 0f, null, null));
			List<StorytellerComp> storytellerComps = Find.Storyteller.storytellerComps;
			for (int i = 0; i < storytellerComps.Count; i++)
			{
				StorytellerComp comp = storytellerComps[i];
				list.Add(new FloatMenuOption(comp.ToString(), delegate()
				{
					StorytellerUtility.DebugLogTestFutureIncidents(currentMapOnly, comp);
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x0008232C File Offset: 0x0008072C
		public static void DebugLogTestFutureIncidents(bool currentMapOnly, StorytellerComp onlyThisComp = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = "Test future incidents for " + Find.Storyteller.def;
			string text2;
			if (onlyThisComp != null)
			{
				text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					" (",
					onlyThisComp,
					")"
				});
			}
			text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				" (",
				Find.TickManager.TicksGame.TicksToDays().ToString("F1"),
				"d - ",
				(Find.TickManager.TicksGame + 6000000).TicksToDays().ToString("F1"),
				"d)"
			});
			stringBuilder.AppendLine(text + ":");
			Dictionary<IIncidentTarget, int> source;
			int[] array;
			List<Pair<IncidentDef, IncidentParms>> source2;
			int num;
			StorytellerUtility.DebugGetFutureIncidents(100, currentMapOnly, out source, out array, out source2, out num, stringBuilder, onlyThisComp);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Target totals:");
			foreach (KeyValuePair<IIncidentTarget, int> keyValuePair in from kvp in source
			orderby kvp.Value
			select kvp)
			{
				stringBuilder.AppendLine(string.Format("  {0}: {1}", keyValuePair.Value, keyValuePair.Key));
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Incident totals:");
			for (int i = 0; i < array.Length; i++)
			{
				float f = (float)array[i] / (float)array.Sum();
				float num2 = (float)array[i] / 100f;
				float num3 = 1f / num2;
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"   M",
					i,
					": ",
					array[i],
					"  (",
					f.ToStringPercent("F2"),
					" of total, avg ",
					num2.ToString("F2"),
					" per day, avg interval ",
					num3,
					")"
				}));
			}
			stringBuilder.AppendLine("Total threats: " + num);
			stringBuilder.AppendLine("Total threats avg per day: " + ((float)num / 100f).ToString("F2"));
			stringBuilder.AppendLine("Overall: " + array.Sum());
			stringBuilder.AppendLine("Overall avg per day: " + ((float)array.Sum() / 100f).ToString("F2"));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Incident defs used:");
			foreach (IncidentDef incidentDef in from x in (from x in source2
			select x.First).Distinct<IncidentDef>()
			orderby x.category.defName, x.defName
			select x)
			{
				stringBuilder.AppendLine(incidentDef.defName + " (" + incidentDef.category.defName + ")");
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x00082718 File Offset: 0x00080B18
		public static void DebugGetFutureIncidents(int numTestDays, bool currentMapOnly, out Dictionary<IIncidentTarget, int> incCountsForTarget, out int[] incCountsForComp, out List<Pair<IncidentDef, IncidentParms>> allIncidents, out int threatBigCount, StringBuilder incDebugInfo = null, StorytellerComp onlyThisComp = null)
		{
			int ticksGame = Find.TickManager.TicksGame;
			IncidentQueue incidentQueue = Find.Storyteller.incidentQueue;
			List<IIncidentTarget> allIncidentTargets = Find.Storyteller.AllIncidentTargets;
			StorytellerUtility.tmpOldStoryStates.Clear();
			for (int i = 0; i < allIncidentTargets.Count; i++)
			{
				IIncidentTarget incidentTarget = allIncidentTargets[i];
				StorytellerUtility.tmpOldStoryStates.Add(incidentTarget, incidentTarget.StoryState);
				new StoryState(incidentTarget).CopyTo(incidentTarget.StoryState);
			}
			Find.Storyteller.incidentQueue = new IncidentQueue();
			int num = numTestDays * 60;
			incCountsForComp = new int[Find.Storyteller.storytellerComps.Count];
			incCountsForTarget = new Dictionary<IIncidentTarget, int>();
			allIncidents = new List<Pair<IncidentDef, IncidentParms>>();
			threatBigCount = 0;
			for (int j = 0; j < num; j++)
			{
				IEnumerable<FiringIncident> enumerable = (onlyThisComp == null) ? Find.Storyteller.MakeIncidentsForInterval() : Find.Storyteller.MakeIncidentsForInterval(onlyThisComp, Find.Storyteller.AllIncidentTargets);
				foreach (FiringIncident firingIncident in enumerable)
				{
					if (!currentMapOnly || firingIncident.parms.target == Find.CurrentMap)
					{
						if (!incCountsForTarget.ContainsKey(firingIncident.parms.target))
						{
							incCountsForTarget[firingIncident.parms.target] = 0;
						}
						Dictionary<IIncidentTarget, int> dictionary;
						IIncidentTarget target;
						(dictionary = incCountsForTarget)[target = firingIncident.parms.target] = dictionary[target] + 1;
						string text = "  ";
						if (firingIncident.def.category == IncidentCategoryDefOf.ThreatBig || firingIncident.def.category == IncidentCategoryDefOf.RaidBeacon)
						{
							threatBigCount++;
							text = "T";
						}
						allIncidents.Add(new Pair<IncidentDef, IncidentParms>(firingIncident.def, firingIncident.parms));
						int num2 = Find.Storyteller.storytellerComps.IndexOf(firingIncident.source);
						incCountsForComp[num2]++;
						if (incDebugInfo != null)
						{
							incDebugInfo.AppendLine(string.Concat(new object[]
							{
								"M",
								num2,
								" ",
								text,
								" ",
								Find.TickManager.TicksGame.TicksToDays().ToString("F1"),
								"d      ",
								firingIncident
							}));
						}
						firingIncident.parms.target.StoryState.Notify_IncidentFired(firingIncident);
					}
				}
				Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 1000);
			}
			Find.TickManager.DebugSetTicksGame(ticksGame);
			Find.Storyteller.incidentQueue = incidentQueue;
			for (int k = 0; k < allIncidentTargets.Count; k++)
			{
				StorytellerUtility.tmpOldStoryStates[allIncidentTargets[k]].CopyTo(allIncidentTargets[k].StoryState);
			}
			StorytellerUtility.tmpOldStoryStates.Clear();
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x00082A6C File Offset: 0x00080E6C
		public static void DebugLogTestIncidentTargets()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Available incident targets:\n");
			foreach (IIncidentTarget incidentTarget in Find.Storyteller.AllIncidentTargets)
			{
				stringBuilder.AppendLine(incidentTarget.ToString());
				foreach (IncidentTargetTypeDef arg in incidentTarget.AcceptedTypes())
				{
					stringBuilder.AppendLine("  " + arg);
				}
				stringBuilder.AppendLine("");
			}
			Log.Message(stringBuilder.ToString(), false);
		}
	}
}
