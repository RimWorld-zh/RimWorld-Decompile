using RimWorld.Planet;
using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class MainTabWindow_Work : MainTabWindow_PawnTable
	{
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Work;
			}
		}

		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			if (Event.current.type == EventType.Layout)
			{
				return;
			}
			this.DoManualPrioritiesCheckbox();
		}

		private void DoManualPrioritiesCheckbox()
		{
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect = new Rect(5f, 5f, 140f, 30f);
			bool useWorkPriorities = Current.Game.playSettings.useWorkPriorities;
			Widgets.CheckboxLabeled(rect, "ManualPriorities".Translate(), ref Current.Game.playSettings.useWorkPriorities, false);
			if (useWorkPriorities != Current.Game.playSettings.useWorkPriorities)
			{
				foreach (Pawn current in PawnsFinder.AllMapsAndWorld_Alive)
				{
					if (current.Faction == Faction.OfPlayer && current.workSettings != null)
					{
						current.workSettings.Notify_UseWorkPrioritiesChanged();
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
