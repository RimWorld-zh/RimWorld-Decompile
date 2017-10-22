using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Verse
{
	public class Dialog_DebugLogMenu : Dialog_DebugOptionLister
	{
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		public Dialog_DebugLogMenu()
		{
			base.forcePause = true;
		}

		protected override void DoListingItems()
		{
			base.DoLabel("Logs");
			MethodInfo[] methods = typeof(DataAnalysisLogger).GetMethods(BindingFlags.Static | BindingFlags.Public);
			for (int i = 0; i < methods.Length; i++)
			{
				MethodInfo mi = methods[i];
				string name = mi.Name;
				if (name.StartsWith("DoLog_"))
				{
					base.DebugAction(GenText.SplitCamelCase(name.Substring(6)), (Action)delegate
					{
						mi.Invoke(null, null);
					});
				}
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				base.DebugAction("Plant proportions", (Action)delegate
				{
					GenPlant.LogPlantProportions();
				});
				base.DebugAction("Database tales list", (Action)delegate
				{
					Find.TaleManager.LogTales();
				});
				base.DebugAction("Database tales interest", (Action)delegate
				{
					Find.TaleManager.LogTaleInterestSummary();
				});
				base.DebugAction("Database tales descs", (Action)delegate
				{
					TaleTester.LogTalesInDatabase();
				});
				base.DebugAction("Random tales descs", (Action)delegate
				{
					TaleTester.LogGeneratedTales(40);
				});
				base.DebugAction("Taleless descs", (Action)delegate
				{
					TaleTester.LogDescriptionsTaleless();
				});
				base.DebugAction("Temperature data", (Action)delegate
				{
					Find.VisibleMap.mapTemperature.DebugLogTemps();
				});
				base.DebugAction("Weather chances", (Action)delegate
				{
					Find.VisibleMap.weatherDecider.LogWeatherChances();
				});
				base.DebugAction("Celestial glow", (Action)delegate
				{
					GenCelestial.LogSunGlowForYear();
				});
				base.DebugAction("ListerPawns", (Action)delegate
				{
					Find.VisibleMap.mapPawns.LogListedPawns();
				});
				base.DebugAction("Wind speeds", (Action)delegate
				{
					Find.VisibleMap.windManager.LogWindSpeeds();
				});
				base.DebugAction("Kidnapped pawns", (Action)delegate
				{
					Find.FactionManager.LogKidnappedPawns();
				});
				base.DebugAction("World pawn list", (Action)delegate
				{
					Find.WorldPawns.LogWorldPawns();
				});
				base.DebugAction("World pawn mothball info", (Action)delegate
				{
					Find.WorldPawns.LogWorldPawnMothballPrevention();
				});
				base.DebugAction("World pawn GC breakdown", (Action)delegate
				{
					Find.WorldPawns.gc.LogGC();
				});
				base.DebugAction("World pawn dotgraph", (Action)delegate
				{
					Find.WorldPawns.gc.LogDotgraph();
				});
				base.DebugAction("Run world pawn GC", (Action)delegate
				{
					Find.WorldPawns.gc.RunGC();
				});
				base.DebugAction("Run world pawn mothball", (Action)delegate
				{
					Find.WorldPawns.DebugRunMothballProcessing();
				});
				base.DebugAction("Draw list", (Action)delegate
				{
					Find.VisibleMap.dynamicDrawManager.LogDynamicDrawThings();
				});
				base.DebugAction("Future incidents", (Action)delegate
				{
					StorytellerUtility.DebugLogTestFutureIncidents(false);
				});
				base.DebugAction("Future incidents (visible map)", (Action)delegate
				{
					StorytellerUtility.DebugLogTestFutureIncidents(true);
				});
				base.DebugAction("Incident targets", (Action)delegate
				{
					StorytellerUtility.DebugLogTestIncidentTargets();
				});
				base.DebugAction("Map pawns", (Action)delegate
				{
					Find.VisibleMap.mapPawns.LogListedPawns();
				});
			}
			base.DoGap();
			Text.Font = GameFont.Small;
			base.DoLabel("Tables");
			MethodInfo[] methods2 = typeof(DataAnalysisTableMaker).GetMethods(BindingFlags.Static | BindingFlags.Public);
			for (int j = 0; j < methods2.Length; j++)
			{
				MethodInfo mi2 = methods2[j];
				string name2 = mi2.Name;
				if (name2.StartsWith("DoTable_"))
				{
					base.DebugAction(GenText.SplitCamelCase(name2.Substring(8)), (Action)delegate
					{
						mi2.Invoke(null, null);
					});
				}
			}
			base.DoGap();
			base.DoLabel("UI");
			base.DebugAction("Pawn column", (Action)delegate
			{
				List<DebugMenuOption> list = new List<DebugMenuOption>();
				List<PawnColumnDef> allDefsListForReading = DefDatabase<PawnColumnDef>.AllDefsListForReading;
				for (int k = 0; k < allDefsListForReading.Count; k++)
				{
					PawnColumnDef localDef = allDefsListForReading[k];
					list.Add(new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Action, (Action)delegate
					{
						Find.WindowStack.Add(new Dialog_PawnTableTest(localDef));
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
			});
		}
	}
}
