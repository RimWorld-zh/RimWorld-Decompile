using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200086E RID: 2158
	[StaticConstructorOnStartup]
	public class MainTabWindow_History : MainTabWindow
	{
		// Token: 0x04001A85 RID: 6789
		private HistoryAutoRecorderGroup historyAutoRecorderGroup = null;

		// Token: 0x04001A86 RID: 6790
		private FloatRange graphSection;

		// Token: 0x04001A87 RID: 6791
		private Vector2 messagesScrollPos;

		// Token: 0x04001A88 RID: 6792
		private float messagesLastHeight;

		// Token: 0x04001A89 RID: 6793
		private static MainTabWindow_History.HistoryTab curTab = MainTabWindow_History.HistoryTab.Graph;

		// Token: 0x04001A8A RID: 6794
		private static bool showLetters = true;

		// Token: 0x04001A8B RID: 6795
		private static bool showMessages;

		// Token: 0x04001A8C RID: 6796
		private const float MessagesRowHeight = 30f;

		// Token: 0x04001A8D RID: 6797
		private const float PinColumnSize = 30f;

		// Token: 0x04001A8E RID: 6798
		private const float PinSize = 22f;

		// Token: 0x04001A8F RID: 6799
		private const float IconColumnSize = 30f;

		// Token: 0x04001A90 RID: 6800
		private const float DateSize = 200f;

		// Token: 0x04001A91 RID: 6801
		private const float SpaceBetweenColumns = 10f;

		// Token: 0x04001A92 RID: 6802
		private static readonly Texture2D PinTex = ContentFinder<Texture2D>.Get("UI/Icons/Pin", true);

		// Token: 0x04001A93 RID: 6803
		private static List<CurveMark> marks = new List<CurveMark>();

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x06003115 RID: 12565 RVA: 0x001AA69C File Offset: 0x001A8A9C
		public override Vector2 RequestedTabSize
		{
			get
			{
				return new Vector2(1010f, 640f);
			}
		}

		// Token: 0x06003116 RID: 12566 RVA: 0x001AA6C0 File Offset: 0x001A8AC0
		public override void PreOpen()
		{
			base.PreOpen();
			this.historyAutoRecorderGroup = Find.History.Groups().FirstOrDefault<HistoryAutoRecorderGroup>();
			if (this.historyAutoRecorderGroup != null)
			{
				this.graphSection = new FloatRange(0f, (float)Find.TickManager.TicksGame / 60000f);
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].wealthWatcher.ForceRecount(false);
			}
		}

		// Token: 0x06003117 RID: 12567 RVA: 0x001AA748 File Offset: 0x001A8B48
		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			Rect rect2 = rect;
			rect2.yMin += 45f;
			List<TabRecord> list = new List<TabRecord>();
			list.Add(new TabRecord("Graph".Translate(), delegate()
			{
				MainTabWindow_History.curTab = MainTabWindow_History.HistoryTab.Graph;
			}, MainTabWindow_History.curTab == MainTabWindow_History.HistoryTab.Graph));
			list.Add(new TabRecord("Messages".Translate(), delegate()
			{
				MainTabWindow_History.curTab = MainTabWindow_History.HistoryTab.Messages;
			}, MainTabWindow_History.curTab == MainTabWindow_History.HistoryTab.Messages));
			list.Add(new TabRecord("Statistics".Translate(), delegate()
			{
				MainTabWindow_History.curTab = MainTabWindow_History.HistoryTab.Statistics;
			}, MainTabWindow_History.curTab == MainTabWindow_History.HistoryTab.Statistics));
			TabDrawer.DrawTabs(rect2, list, 200f);
			MainTabWindow_History.HistoryTab historyTab = MainTabWindow_History.curTab;
			if (historyTab != MainTabWindow_History.HistoryTab.Graph)
			{
				if (historyTab != MainTabWindow_History.HistoryTab.Messages)
				{
					if (historyTab == MainTabWindow_History.HistoryTab.Statistics)
					{
						this.DoStatisticsPage(rect2);
					}
				}
				else
				{
					this.DoMessagesPage(rect2);
				}
			}
			else
			{
				this.DoGraphPage(rect2);
			}
		}

		// Token: 0x06003118 RID: 12568 RVA: 0x001AA878 File Offset: 0x001A8C78
		private void DoStatisticsPage(Rect rect)
		{
			rect.yMin += 17f;
			GUI.BeginGroup(rect);
			StringBuilder stringBuilder = new StringBuilder();
			TimeSpan timeSpan = new TimeSpan(0, 0, (int)Find.GameInfo.RealPlayTimeInteracting);
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Playtime".Translate(),
				": ",
				timeSpan.Days,
				"LetterDay".Translate(),
				" ",
				timeSpan.Hours,
				"LetterHour".Translate(),
				" ",
				timeSpan.Minutes,
				"LetterMinute".Translate(),
				" ",
				timeSpan.Seconds,
				"LetterSecond".Translate()
			}));
			stringBuilder.AppendLine("Storyteller".Translate() + ": " + Find.Storyteller.def.LabelCap);
			DifficultyDef difficulty = Find.Storyteller.difficulty;
			stringBuilder.AppendLine("Difficulty".Translate() + ": " + difficulty.LabelCap);
			if (Find.CurrentMap != null)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("ThisMapColonyWealthTotal".Translate() + ": " + Find.CurrentMap.wealthWatcher.WealthTotal.ToString("F0"));
				stringBuilder.AppendLine("ThisMapColonyWealthItems".Translate() + ": " + Find.CurrentMap.wealthWatcher.WealthItems.ToString("F0"));
				stringBuilder.AppendLine("ThisMapColonyWealthBuildings".Translate() + ": " + Find.CurrentMap.wealthWatcher.WealthBuildings.ToString("F0"));
				stringBuilder.AppendLine("ThisMapColonyWealthTameAnimals".Translate() + ": " + Find.CurrentMap.wealthWatcher.WealthTameAnimals.ToString("F0"));
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("NumThreatBigs".Translate() + ": " + Find.StoryWatcher.statsRecord.numThreatBigs);
			stringBuilder.AppendLine("NumEnemyRaids".Translate() + ": " + Find.StoryWatcher.statsRecord.numRaidsEnemy);
			stringBuilder.AppendLine();
			if (Find.CurrentMap != null)
			{
				stringBuilder.AppendLine("ThisMapDamageTaken".Translate() + ": " + Find.CurrentMap.damageWatcher.DamageTakenEver);
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

		// Token: 0x06003119 RID: 12569 RVA: 0x001AABFC File Offset: 0x001A8FFC
		private void DoMessagesPage(Rect rect)
		{
			rect.yMin += 10f;
			Widgets.CheckboxLabeled(new Rect(rect.x, rect.y, 200f, 30f), "ShowLetters".Translate(), ref MainTabWindow_History.showLetters, false, null, null, true);
			Widgets.CheckboxLabeled(new Rect(rect.x + 200f, rect.y, 200f, 30f), "ShowMessages".Translate(), ref MainTabWindow_History.showMessages, false, null, null, true);
			rect.yMin += 40f;
			bool flag = false;
			Rect outRect = rect;
			Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, this.messagesLastHeight);
			Widgets.BeginScrollView(outRect, ref this.messagesScrollPos, viewRect, true);
			float num = 0f;
			List<IArchivable> archivablesListForReading = Find.Archive.ArchivablesListForReading;
			for (int i = archivablesListForReading.Count - 1; i >= 0; i--)
			{
				if (MainTabWindow_History.showLetters || (!(archivablesListForReading[i] is Letter) && !(archivablesListForReading[i] is ArchivedDialog)))
				{
					if (MainTabWindow_History.showMessages || !(archivablesListForReading[i] is Message))
					{
						flag = true;
						if (num + 30f >= this.messagesScrollPos.y && num <= this.messagesScrollPos.y + outRect.height)
						{
							this.DoArchivableRow(new Rect(0f, num, viewRect.width, 30f), archivablesListForReading[i], i);
						}
						num += 30f;
					}
				}
			}
			this.messagesLastHeight = num;
			Widgets.EndScrollView();
			if (!flag)
			{
				Widgets.NoneLabel(rect.yMin + 3f, rect.width, "(" + "NoMessages".Translate() + ")");
			}
		}

		// Token: 0x0600311A RID: 12570 RVA: 0x001AAE08 File Offset: 0x001A9208
		private void DoArchivableRow(Rect rect, IArchivable archivable, int index)
		{
			if (index % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			Widgets.DrawHighlightIfMouseover(rect);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Rect rect2 = rect;
			Rect rect3 = rect2;
			rect3.width = 30f;
			rect2.xMin += 40f;
			float num;
			if (Find.Archive.IsPinned(archivable))
			{
				num = 1f;
			}
			else if (Mouse.IsOver(rect3))
			{
				num = 0.25f;
			}
			else
			{
				num = 0f;
			}
			if (num > 0f)
			{
				GUI.color = new Color(1f, 1f, 1f, num);
				GUI.DrawTexture(new Rect(rect3.x + (rect3.width - 22f) / 2f, rect3.y + (rect3.height - 22f) / 2f, 22f, 22f).Rounded(), MainTabWindow_History.PinTex);
				GUI.color = Color.white;
			}
			Rect rect4 = rect2;
			Rect outerRect = rect2;
			outerRect.width = 30f;
			rect2.xMin += 40f;
			Texture archivedIcon = archivable.ArchivedIcon;
			if (archivedIcon != null)
			{
				GUI.color = archivable.ArchivedIconColor;
				Widgets.DrawTextureFitted(outerRect, archivedIcon, 0.8f);
				GUI.color = Color.white;
			}
			Rect rect5 = rect2;
			rect5.width = 200f;
			rect2.xMin += 210f;
			Vector2 location = (Find.CurrentMap == null) ? default(Vector2) : Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile);
			GUI.color = new Color(0.75f, 0.75f, 0.75f);
			int num2 = GenDate.TickGameToAbs(archivable.CreatedTicksGame);
			string str = string.Concat(new object[]
			{
				GenDate.DateFullStringAt((long)num2, location),
				", ",
				GenDate.HourInteger((long)num2, location.x),
				"LetterHour".Translate()
			});
			Widgets.Label(rect5, str.Truncate(rect5.width, null));
			GUI.color = Color.white;
			Rect rect6 = rect2;
			Widgets.Label(rect6, archivable.ArchivedLabel.Truncate(rect6.width, null));
			GenUI.ResetLabelAlign();
			Text.WordWrap = true;
			TooltipHandler.TipRegion(rect3, "PinArchivableTip".Translate(new object[]
			{
				200
			}));
			if (Mouse.IsOver(rect4))
			{
				TooltipHandler.TipRegion(rect4, archivable.ArchivedTooltip);
			}
			if (Widgets.ButtonInvisible(rect3, false))
			{
				if (Find.Archive.IsPinned(archivable))
				{
					Find.Archive.Unpin(archivable);
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
				else
				{
					Find.Archive.Pin(archivable);
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
			}
			if (Widgets.ButtonInvisible(rect4, false))
			{
				if (Event.current.button == 1)
				{
					LookTargets lookTargets = archivable.LookTargets;
					if (CameraJumper.CanJump(lookTargets.TryGetPrimaryTarget()))
					{
						CameraJumper.TryJumpAndSelect(lookTargets.TryGetPrimaryTarget());
						Find.MainTabsRoot.EscapeCurrentTab(true);
					}
				}
				else
				{
					archivable.OpenArchived();
				}
			}
		}

		// Token: 0x0600311B RID: 12571 RVA: 0x001AB174 File Offset: 0x001A9574
		private void DoGraphPage(Rect rect)
		{
			rect.yMin += 17f;
			GUI.BeginGroup(rect);
			Rect graphRect = new Rect(0f, 0f, rect.width, 450f);
			Rect legendRect = new Rect(0f, graphRect.yMax, rect.width / 2f, 40f);
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
						float x = (float)GenDate.TickAbsToGame(tale.date) / 60000f;
						MainTabWindow_History.marks.Add(new CurveMark(x, tale.ShortSummary, tale.def.historyGraphColor));
					}
				}
				this.historyAutoRecorderGroup.DrawGraph(graphRect, legendRect, this.graphSection, MainTabWindow_History.marks);
			}
			Text.Font = GameFont.Small;
			float num = (float)Find.TickManager.TicksGame / 60000f;
			if (Widgets.ButtonText(new Rect(legendRect.xMin + legendRect.width, legendRect.yMin, 110f, 40f), "Last30Days".Translate(), true, false, true))
			{
				this.graphSection = new FloatRange(Mathf.Max(0f, num - 30f), num);
			}
			if (Widgets.ButtonText(new Rect(legendRect.xMin + legendRect.width + 110f + 4f, legendRect.yMin, 110f, 40f), "Last100Days".Translate(), true, false, true))
			{
				this.graphSection = new FloatRange(Mathf.Max(0f, num - 100f), num);
			}
			if (Widgets.ButtonText(new Rect(legendRect.xMin + legendRect.width + 228f, legendRect.yMin, 110f, 40f), "Last300Days".Translate(), true, false, true))
			{
				this.graphSection = new FloatRange(Mathf.Max(0f, num - 300f), num);
			}
			if (Widgets.ButtonText(new Rect(legendRect.xMin + legendRect.width + 342f, legendRect.yMin, 110f, 40f), "AllDays".Translate(), true, false, true))
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
					list.Add(new FloatMenuOption(groupLocal.def.LabelCap, delegate()
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

		// Token: 0x0200086F RID: 2159
		private enum HistoryTab : byte
		{
			// Token: 0x04001A98 RID: 6808
			Graph,
			// Token: 0x04001A99 RID: 6809
			Messages,
			// Token: 0x04001A9A RID: 6810
			Statistics
		}
	}
}
