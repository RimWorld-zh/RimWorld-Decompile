using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000851 RID: 2129
	public class ITab_Pawn_Log : ITab
	{
		// Token: 0x04001A1F RID: 6687
		public const float Width = 630f;

		// Token: 0x04001A20 RID: 6688
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowAllX = 60f;

		// Token: 0x04001A21 RID: 6689
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowAllWidth = 100f;

		// Token: 0x04001A22 RID: 6690
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowCombatX = 445f;

		// Token: 0x04001A23 RID: 6691
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowCombatWidth = 115f;

		// Token: 0x04001A24 RID: 6692
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowSocialX = 330f;

		// Token: 0x04001A25 RID: 6693
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowSocialWidth = 105f;

		// Token: 0x04001A26 RID: 6694
		[TweakValue("Interface", 0f, 20f)]
		private static float ToolbarHeight = 2f;

		// Token: 0x04001A27 RID: 6695
		[TweakValue("Interface", 0f, 100f)]
		private static float ButtonOffset = 60f;

		// Token: 0x04001A28 RID: 6696
		public bool showAll = false;

		// Token: 0x04001A29 RID: 6697
		public bool showCombat = true;

		// Token: 0x04001A2A RID: 6698
		public bool showSocial = true;

		// Token: 0x04001A2B RID: 6699
		public LogEntry logSeek = null;

		// Token: 0x04001A2C RID: 6700
		public ITab_Pawn_Log_Utility.LogDrawData data = new ITab_Pawn_Log_Utility.LogDrawData();

		// Token: 0x04001A2D RID: 6701
		public List<ITab_Pawn_Log_Utility.LogLineDisplayable> cachedLogDisplay;

		// Token: 0x04001A2E RID: 6702
		public int cachedLogDisplayLastTick = -1;

		// Token: 0x04001A2F RID: 6703
		public int cachedLogPlayLastTick = -1;

		// Token: 0x04001A30 RID: 6704
		private Vector2 scrollPosition = default(Vector2);

		// Token: 0x06003040 RID: 12352 RVA: 0x001A461C File Offset: 0x001A2A1C
		public ITab_Pawn_Log()
		{
			this.size = new Vector2(630f, 510f);
			this.labelKey = "TabLog";
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x06003041 RID: 12353 RVA: 0x001A4694 File Offset: 0x001A2A94
		private Pawn SelPawnForCombatInfo
		{
			get
			{
				Pawn result;
				if (base.SelPawn != null)
				{
					result = base.SelPawn;
				}
				else
				{
					Corpse corpse = base.SelThing as Corpse;
					if (corpse == null)
					{
						throw new InvalidOperationException("Social tab on non-pawn non-corpse " + base.SelThing);
					}
					result = corpse.InnerPawn;
				}
				return result;
			}
		}

		// Token: 0x06003042 RID: 12354 RVA: 0x001A46F0 File Offset: 0x001A2AF0
		protected override void FillTab()
		{
			Pawn selPawnForCombatInfo = this.SelPawnForCombatInfo;
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y);
			Rect rect2 = new Rect(ITab_Pawn_Log.ShowAllX, ITab_Pawn_Log.ToolbarHeight, ITab_Pawn_Log.ShowAllWidth, 24f);
			bool flag = this.showAll;
			Widgets.CheckboxLabeled(rect2, "ShowAll".Translate(), ref this.showAll, false, null, null, false);
			if (flag != this.showAll)
			{
				this.cachedLogDisplay = null;
			}
			Rect rect3 = new Rect(ITab_Pawn_Log.ShowCombatX, ITab_Pawn_Log.ToolbarHeight, ITab_Pawn_Log.ShowCombatWidth, 24f);
			bool flag2 = this.showCombat;
			Widgets.CheckboxLabeled(rect3, "ShowCombat".Translate(), ref this.showCombat, false, null, null, false);
			if (flag2 != this.showCombat)
			{
				this.cachedLogDisplay = null;
			}
			Rect rect4 = new Rect(ITab_Pawn_Log.ShowSocialX, ITab_Pawn_Log.ToolbarHeight, ITab_Pawn_Log.ShowSocialWidth, 24f);
			bool flag3 = this.showSocial;
			Widgets.CheckboxLabeled(rect4, "ShowSocial".Translate(), ref this.showSocial, false, null, null, false);
			if (flag3 != this.showSocial)
			{
				this.cachedLogDisplay = null;
			}
			if (this.cachedLogDisplay == null || this.cachedLogDisplayLastTick != selPawnForCombatInfo.records.LastBattleTick || this.cachedLogPlayLastTick != Find.PlayLog.LastTick)
			{
				this.cachedLogDisplay = ITab_Pawn_Log_Utility.GenerateLogLinesFor(selPawnForCombatInfo, this.showAll, this.showCombat, this.showSocial).ToList<ITab_Pawn_Log_Utility.LogLineDisplayable>();
				this.cachedLogDisplayLastTick = selPawnForCombatInfo.records.LastBattleTick;
				this.cachedLogPlayLastTick = Find.PlayLog.LastTick;
			}
			Rect rect5 = new Rect(rect.width - ITab_Pawn_Log.ButtonOffset, 0f, 18f, 24f);
			if (Widgets.ButtonImage(rect5, TexButton.Copy))
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (ITab_Pawn_Log_Utility.LogLineDisplayable logLineDisplayable in this.cachedLogDisplay)
				{
					logLineDisplayable.AppendTo(stringBuilder);
				}
				GUIUtility.systemCopyBuffer = stringBuilder.ToString();
			}
			TooltipHandler.TipRegion(rect5, "CopyLogTip".Translate());
			rect.yMin = 24f;
			rect = rect.ContractedBy(10f);
			float width = rect.width - 16f - 10f;
			float num = 0f;
			foreach (ITab_Pawn_Log_Utility.LogLineDisplayable logLineDisplayable2 in this.cachedLogDisplay)
			{
				if (logLineDisplayable2.Matches(this.logSeek))
				{
					this.scrollPosition.y = num - (logLineDisplayable2.GetHeight(width) + rect.height) / 2f;
				}
				num += logLineDisplayable2.GetHeight(width);
			}
			this.logSeek = null;
			if (num > 0f)
			{
				Rect viewRect = new Rect(0f, 0f, rect.width - 16f, num);
				this.data.StartNewDraw();
				Widgets.BeginScrollView(rect, ref this.scrollPosition, viewRect, true);
				float num2 = 0f;
				foreach (ITab_Pawn_Log_Utility.LogLineDisplayable logLineDisplayable3 in this.cachedLogDisplay)
				{
					logLineDisplayable3.Draw(num2, width, this.data);
					num2 += logLineDisplayable3.GetHeight(width);
				}
				Widgets.EndScrollView();
			}
			else
			{
				Text.Anchor = TextAnchor.MiddleCenter;
				Text.Font = GameFont.Medium;
				GUI.color = Color.grey;
				Widgets.Label(new Rect(0f, 0f, this.size.x, this.size.y), "(" + "NoRecentEntries".Translate() + ")");
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
			}
		}

		// Token: 0x06003043 RID: 12355 RVA: 0x001A4B50 File Offset: 0x001A2F50
		public void SeekTo(LogEntry entry)
		{
			this.logSeek = entry;
		}

		// Token: 0x06003044 RID: 12356 RVA: 0x001A4B5A File Offset: 0x001A2F5A
		public void Highlight(LogEntry entry)
		{
			this.data.highlightEntry = entry;
			this.data.highlightIntensity = 1f;
		}
	}
}
