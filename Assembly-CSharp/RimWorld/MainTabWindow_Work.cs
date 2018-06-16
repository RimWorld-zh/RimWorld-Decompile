using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200087A RID: 2170
	public class MainTabWindow_Work : MainTabWindow_PawnTable
	{
		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06003176 RID: 12662 RVA: 0x001AD224 File Offset: 0x001AB624
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Work;
			}
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06003177 RID: 12663 RVA: 0x001AD240 File Offset: 0x001AB640
		protected override float ExtraTopSpace
		{
			get
			{
				return 40f;
			}
		}

		// Token: 0x06003178 RID: 12664 RVA: 0x001AD25A File Offset: 0x001AB65A
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		// Token: 0x06003179 RID: 12665 RVA: 0x001AD274 File Offset: 0x001AB674
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

		// Token: 0x0600317A RID: 12666 RVA: 0x001AD360 File Offset: 0x001AB760
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

		// Token: 0x04001AC3 RID: 6851
		private const int SpaceBetweenPriorityArrowsAndWorkLabels = 40;
	}
}
