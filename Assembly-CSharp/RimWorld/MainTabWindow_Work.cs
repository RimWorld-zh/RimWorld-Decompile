using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000878 RID: 2168
	public class MainTabWindow_Work : MainTabWindow_PawnTable
	{
		// Token: 0x04001AC1 RID: 6849
		private const int SpaceBetweenPriorityArrowsAndWorkLabels = 40;

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06003175 RID: 12661 RVA: 0x001AD61C File Offset: 0x001ABA1C
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Work;
			}
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06003176 RID: 12662 RVA: 0x001AD638 File Offset: 0x001ABA38
		protected override float ExtraTopSpace
		{
			get
			{
				return 40f;
			}
		}

		// Token: 0x06003177 RID: 12663 RVA: 0x001AD652 File Offset: 0x001ABA52
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		// Token: 0x06003178 RID: 12664 RVA: 0x001AD66C File Offset: 0x001ABA6C
		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			if (Event.current.type != EventType.Layout)
			{
				this.DoManualPrioritiesCheckbox();
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
				Text.Anchor = TextAnchor.UpperCenter;
				Text.Font = GameFont.Tiny;
				Rect rect2 = new Rect(370f, rect.y + 5f, 160f, 30f);
				Widgets.Label(rect2, "<= " + "HigherPriority".Translate());
				Rect rect3 = new Rect(630f, rect.y + 5f, 160f, 30f);
				Widgets.Label(rect3, "LowerPriority".Translate() + " =>");
				GUI.color = Color.white;
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.UpperLeft;
			}
		}

		// Token: 0x06003179 RID: 12665 RVA: 0x001AD758 File Offset: 0x001ABB58
		private void DoManualPrioritiesCheckbox()
		{
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect = new Rect(5f, 5f, 140f, 30f);
			bool useWorkPriorities = Current.Game.playSettings.useWorkPriorities;
			Widgets.CheckboxLabeled(rect, "ManualPriorities".Translate(), ref Current.Game.playSettings.useWorkPriorities, false, null, null, false);
			if (useWorkPriorities != Current.Game.playSettings.useWorkPriorities)
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_Alive)
				{
					if (pawn.Faction == Faction.OfPlayer && pawn.workSettings != null)
					{
						pawn.workSettings.Notify_UseWorkPrioritiesChanged();
					}
				}
			}
			if (!Current.Game.playSettings.useWorkPriorities)
			{
				UIHighlighter.HighlightOpportunity(rect, "ManualPriorities-Off");
			}
		}
	}
}
