using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class MainTabWindow_History : MainTabWindow
	{
		private enum HistoryTab : byte
		{
			Graph = 0,
			Statistics = 1
		}

		private HistoryTab curTab = HistoryTab.Graph;

		private HistoryAutoRecorderGroup historyAutoRecorderGroup = null;

		private FloatRange graphSection;

		private static List<CurveMark> marks = new List<CurveMark>();

		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(1010f, 640f);
			}
		}

		public override void PreOpen()
		{
			base.PreOpen();
			this.historyAutoRecorderGroup = Find.History.Groups().FirstOrDefault();
			if (this.historyAutoRecorderGroup != null)
			{
				this.graphSection = new FloatRange(0f, (float)((float)Find.TickManager.TicksGame / 60000.0));
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].wealthWatcher.ForceRecount(false);
			}
		}

		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			Rect rect2 = rect;
			rect2.yMin += 45f;
			List<TabRecord> list = new List<TabRecord>();
			list.Add(new TabRecord("Graph".Translate(), (Action)delegate
			{
				this.curTab = HistoryTab.Graph;
			}, this.curTab == HistoryTab.Graph));
			list.Add(new TabRecord("Statistics".Translate(), (Action)delegate
			{
				this.curTab = HistoryTab.Statistics;
			}, this.curTab == HistoryTab.Statistics));
			TabDrawer.DrawTabs(rect2, list);
			switch (this.curTab)
			{
			case HistoryTab.Graph:
			{
				this.DoGraphPage(rect2);
				break;
			}
			case HistoryTab.Statistics:
			{
				this.DoStatisticsPage(rect2);
				break;
			}
			}
		}

		private void DoStatisticsPage(Rect rect)
		{
			rect.yMin += 17f;
			GUI.BeginGroup(rect);
			StringBuilder stringBuilder = new StringBuilder();
			TimeSpan timeSpan = new TimeSpan(0, 0, (int)Find.GameInfo.RealPlayTimeInteracting);
			stringBuilder.AppendLine("Playtime".Translate() + ": " + timeSpan.Days + "LetterDay".Translate() + " " + timeSpan.Hours + "LetterHour".Translate() + " " + timeSpan.Minutes + "LetterMinute".Translate() + " " + timeSpan.Seconds + "LetterSecond".Translate());
			stringBuilder.AppendLine("Storyteller".Translate() + ": " + Find.Storyteller.def.label);
			DifficultyDef difficulty = Find.Storyteller.difficulty;
			stringBuilder.AppendLine("Difficulty".Translate() + ": " + difficulty.label);
			if (Find.VisibleMap != null)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("ThisMapColonyWealthTotal".Translate() + ": " + Find.VisibleMap.wealthWatcher.WealthTotal.ToString("F0"));
				stringBuilder.AppendLine("ThisMapColonyWealthItems".Translate() + ": " + Find.VisibleMap.wealthWatcher.WealthItems.ToString("F0"));
				stringBuilder.AppendLine("ThisMapColonyWealthBuildings".Translate() + ": " + Find.VisibleMap.wealthWatcher.WealthBuildings.ToString("F0"));
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("NumThreatBigs".Translate() + ": " + Find.StoryWatcher.statsRecord.numThreatBigs);
			stringBuilder.AppendLine("NumEnemyRaids".Translate() + ": " + Find.StoryWatcher.statsRecord.numRaidsEnemy);
			stringBuilder.AppendLine();
			if (Find.VisibleMap != null)
			{
				stringBuilder.AppendLine("ThisMapDamageTaken".Translate() + ": " + Find.VisibleMap.damageWatcher.DamageTakenEver);
			}
			stringBuilder.AppendLine("ColonistsKilled".Translate() + ": " + Find.StoryWatcher.statsRecord.colonistsKilled);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("ColonistsLaunched".Translate() + ": " + Find.StoryWatcher.statsRecord.colonistsLaunched);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect2 = new Rect(0f, 0f, 400f, 400f);
			Widgets.Label(rect2, stringBuilder.ToString());
			GUI.EndGroup();
		}

		private void DoGraphPage(Rect rect)
		{
			rect.yMin += 17f;
			GUI.BeginGroup(rect);
			Rect graphRect = new Rect(0f, 0f, rect.width, 450f);
			Rect legendRect = new Rect(0f, graphRect.yMax, (float)(rect.width / 2.0), 40f);
			Rect rect2 = new Rect(0f, legendRect.yMax, rect.width, 40f);
			if (this.historyAutoRecorderGroup != null)
			{
				MainTabWindow_History.marks.Clear();
				List<Tale> allTalesListForReading = Find.TaleManager.AllTalesListForReading;
				for (int i = 0; i < allTalesListForReading.Count; i++)
				{
					Tale tale = allTalesListForReading[i];
					if (tale.def.type == TaleType.PermanentHistorical)
					{
						float x = (float)((float)GenDate.TickAbsToGame(tale.date) / 60000.0);
						MainTabWindow_History.marks.Add(new CurveMark(x, tale.ShortSummary, tale.def.historyGraphColor));
					}
				}
				this.historyAutoRecorderGroup.DrawGraph(graphRect, legendRect, this.graphSection, MainTabWindow_History.marks);
			}
			Text.Font = GameFont.Small;
			float num = (float)((float)Find.TickManager.TicksGame / 60000.0);
			if (Widgets.ButtonText(new Rect(legendRect.xMin + legendRect.width, legendRect.yMin, 110f, 40f), "Last30Days".Translate(), true, false, true))
			{
				this.graphSection = new FloatRange(Mathf.Max(0f, (float)(num - 30.0)), num);
			}
			if (Widgets.ButtonText(new Rect((float)(legendRect.xMin + legendRect.width + 110.0 + 4.0), legendRect.yMin, 110f, 40f), "Last100Days".Translate(), true, false, true))
			{
				this.graphSection = new FloatRange(Mathf.Max(0f, (float)(num - 100.0)), num);
			}
			if (Widgets.ButtonText(new Rect((float)(legendRect.xMin + legendRect.width + 228.0), legendRect.yMin, 110f, 40f), "Last300Days".Translate(), true, false, true))
			{
				this.graphSection = new FloatRange(Mathf.Max(0f, (float)(num - 300.0)), num);
			}
			if (Widgets.ButtonText(new Rect((float)(legendRect.xMin + legendRect.width + 342.0), legendRect.yMin, 110f, 40f), "AllDays".Translate(), true, false, true))
			{
				this.graphSection = new FloatRange(0f, num);
			}
			if (Widgets.ButtonText(new Rect(rect2.x, rect2.y, 110f, 40f), "SelectGraph".Translate(), true, false, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				List<HistoryAutoRecorderGroup> list2 = Find.History.Groups();
				for (int j = 0; j < list2.Count; j++)
				{
					HistoryAutoRecorderGroup groupLocal = list2[j];
					list.Add(new FloatMenuOption(groupLocal.def.LabelCap, (Action)delegate
					{
						this.historyAutoRecorderGroup = groupLocal;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				FloatMenu window = new FloatMenu(list, "SelectGraph".Translate(), false);
				Find.WindowStack.Add(window);
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.HistoryTab, KnowledgeAmount.Total);
			}
			GUI.EndGroup();
		}
	}
}
