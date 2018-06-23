using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000876 RID: 2166
	public class MainTabWindow_Work : MainTabWindow_PawnTable
	{
		// Token: 0x04001AC1 RID: 6849
		private const int SpaceBetweenPriorityArrowsAndWorkLabels = 40;

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06003171 RID: 12657 RVA: 0x001AD4D4 File Offset: 0x001AB8D4
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Work;
			}
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06003172 RID: 12658 RVA: 0x001AD4F0 File Offset: 0x001AB8F0
		protected override float ExtraTopSpace
		{
			get
			{
				return 40f;
			}
		}

		// Token: 0x06003173 RID: 12659 RVA: 0x001AD50A File Offset: 0x001AB90A
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		// Token: 0x06003174 RID: 12660 RVA: 0x001AD524 File Offset: 0x001AB924
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

		// Token: 0x06003175 RID: 12661 RVA: 0x001AD610 File Offset: 0x001ABA10
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
