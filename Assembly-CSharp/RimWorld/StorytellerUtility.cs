using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class StorytellerUtility
	{
		private const float GlobalPointsMin = 35f;

		private const float GlobalPointsMax = 20000f;

		public const float BuildingWealthFactor = 0.5f;

		private static readonly SimpleCurve PointsPerWealthCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(14000f, 0f),
				true
			},
			{
				new CurvePoint(400000f, 2400f),
				true
			},
			{
				new CurvePoint(700000f, 3600f),
				true
			},
			{
				new CurvePoint(1000000f, 4200f),
				true
			}
		};

		private const float PointsPerTameNonDownedCombatTrainableAnimalCombatPower = 0.08f;

		private const float PointsPerPlayerPawnFactorInContainer = 0.3f;

		private const float PointsPerPlayerPawnHealthSummaryLerpAmount = 0.65f;

		private static readonly SimpleCurve PointsPerColonistByWealthCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 15f),
				true
			},
			{
				new CurvePoint(10000f, 15f),
				true
			},
			{
				new CurvePoint(400000f, 140f),
				true
			},
			{
				new CurvePoint(1000000f, 200f),
				true
			}
		};

		public const float CaravanWealthPointsFactor = 0.7f;

		public const float CaravanAnimalPointsFactor = 0.7f;

		public static readonly FloatRange CaravanPointsRandomFactorRange = new FloatRange(0.7f, 0.9f);

		private static readonly SimpleCurve AllyIncidentFractionFromAllyFraction = new SimpleCurve
		{
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(0.25f, 0.6f),
				true
			}
		};

		private static Dictionary<IIncidentTarget, StoryState> tmpOldStoryStates = new Dictionary<IIncidentTarget, StoryState>();

		[CompilerGenerated]
		private static Func<KeyValuePair<IIncidentTarget, int>, int> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Pair<IncidentDef, IncidentParms>, IncidentDef> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<IncidentDef, string> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<IncidentDef, string> <>f__am$cache3;

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
			}
			return incidentParms;
		}

		public static float DefaultThreatPointsNow(IIncidentTarget target)
		{
			float playerWealthForStoryteller = target.PlayerWealthForStoryteller;
			float num = StorytellerUtility.PointsPerWealthCurve.Evaluate(playerWealthForStoryteller);
			float num2 = 0f;
			foreach (Pawn pawn in target.PlayerPawnsForStoryteller)
			{
				float num3 = 0f;
				if (pawn.IsFreeColonist)
				{
					num3 = StorytellerUtility.PointsPerColonistByWealthCurve.Evaluate(playerWealthForStoryteller);
				}
				else if (pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && !pawn.Downed && pawn.training.CanAssignToTrain(TrainableDefOf.Release).Accepted)
				{
					num3 = 0.08f * pawn.kindDef.combatPower;
					if (target is Caravan)
					{
						num3 *= 0.7f;
					}
				}
				if (num3 > 0f)
				{
					if (pawn.ParentHolder != null && pawn.ParentHolder is Building_CryptosleepCasket)
					{
						num3 *= 0.3f;
					}
					num3 = Mathf.Lerp(num3, num3 * pawn.health.summaryHealth.SummaryHealthPercent, 0.65f);
					num2 += num3;
				}
			}
			float num4 = num + num2;
			num4 *= target.IncidentPointsRandomFactorRange.RandomInRange;
			float totalThreatPointsFactor = Find.StoryWatcher.watcherAdaptation.TotalThreatPointsFactor;
			float num5 = Mathf.Lerp(1f, totalThreatPointsFactor, Find.Storyteller.difficulty.adaptationEffectFactor);
			num4 *= num5;
			num4 *= Find.Storyteller.difficulty.threatScale;
			num4 *= Find.Storyteller.def.pointsFactorFromDaysPassed.Evaluate((float)GenDate.DaysPassed);
			return Mathf.Clamp(num4, 35f, 20000f);
		}

		public static float DefaultSiteThreatPointsNow()
		{
			return SiteTuning.ThreatPointsToSiteThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(Find.World)) * SiteTuning.SitePointRandomFactorRange.RandomInRange;
		}

		public static float AllyIncidentFraction(bool fullAlliesOnly)
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
					if (allFactionsListForReading[i].PlayerRelationKind == FactionRelationKind.Ally || (!fullAlliesOnly && !allFactionsListForReading[i].HostileTo(Faction.OfPlayer)))
					{
						num++;
					}
				}
			}
			if (num == 0)
			{
				return -1f;
			}
			float x = (float)num / Mathf.Max((float)num2, 1f);
			return StorytellerUtility.AllyIncidentFractionFromAllyFraction.Evaluate(x);
		}

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
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"  ",
					keyValuePair.Value,
					": ",
					keyValuePair.Key
				}));
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

		public static void DebugGetFutureIncidents(int numTestDays, bool currentMapOnly, out Dictionary<IIncidentTarget, int> incCountsForTarget, out int[] incCountsForComp, out List<Pair<IncidentDef, IncidentParms>> allIncidents, out int threatBigCount, StringBuilder outputSb = null, StorytellerComp onlyThisComp = null)
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
					if (firingIncident == null)
					{
						Log.Error("Null incident generated.", false);
					}
					if (!currentMapOnly || firingIncident.parms.target == Find.CurrentMap)
					{
						firingIncident.parms.target.StoryState.Notify_IncidentFired(firingIncident);
						allIncidents.Add(new Pair<IncidentDef, IncidentParms>(firingIncident.def, firingIncident.parms));
						firingIncident.parms.target.StoryState.Notify_IncidentFired(firingIncident);
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
						else if (firingIncident.def.category == IncidentCategoryDefOf.ThreatSmall)
						{
							text = "S";
						}
						int num2 = Find.Storyteller.storytellerComps.IndexOf(firingIncident.source);
						incCountsForComp[num2]++;
						if (outputSb != null)
						{
							outputSb.AppendLine(string.Concat(new object[]
							{
								"M",
								num2,
								" ",
								text,
								" ",
								Find.TickManager.TicksGame.TicksToDays().ToString("F1"),
								"d      [",
								Find.TickManager.TicksGame / 1000,
								"]",
								firingIncident
							}));
						}
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

		public static void DebugLogTestIncidentTargets()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Available incident targets:\n");
			foreach (IIncidentTarget incidentTarget in Find.Storyteller.AllIncidentTargets)
			{
				stringBuilder.AppendLine(incidentTarget.ToString());
				foreach (IncidentTargetTagDef arg in incidentTarget.IncidentTargetTags())
				{
					stringBuilder.AppendLine("  " + arg);
				}
				stringBuilder.AppendLine(string.Empty);
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static StorytellerUtility()
		{
		}

		[CompilerGenerated]
		private static int <DebugLogTestFutureIncidents>m__0(KeyValuePair<IIncidentTarget, int> kvp)
		{
			return kvp.Value;
		}

		[CompilerGenerated]
		private static IncidentDef <DebugLogTestFutureIncidents>m__1(Pair<IncidentDef, IncidentParms> x)
		{
			return x.First;
		}

		[CompilerGenerated]
		private static string <DebugLogTestFutureIncidents>m__2(IncidentDef x)
		{
			return x.category.defName;
		}

		[CompilerGenerated]
		private static string <DebugLogTestFutureIncidents>m__3(IncidentDef x)
		{
			return x.defName;
		}

		[CompilerGenerated]
		private sealed class <ShowFutureIncidentsDebugLogFloatMenu>c__AnonStorey0
		{
			internal bool currentMapOnly;

			public <ShowFutureIncidentsDebugLogFloatMenu>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				StorytellerUtility.DebugLogTestFutureIncidents(this.currentMapOnly, null);
			}
		}

		[CompilerGenerated]
		private sealed class <ShowFutureIncidentsDebugLogFloatMenu>c__AnonStorey1
		{
			internal StorytellerComp comp;

			internal StorytellerUtility.<ShowFutureIncidentsDebugLogFloatMenu>c__AnonStorey0 <>f__ref$0;

			public <ShowFutureIncidentsDebugLogFloatMenu>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				StorytellerUtility.DebugLogTestFutureIncidents(this.<>f__ref$0.currentMapOnly, this.comp);
			}
		}
	}
}
