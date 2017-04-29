using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class PawnColumnWorker_AllowedArea : PawnColumnWorker
	{
		private const int TopAreaHeight = 65;

		private const int ManageAreasButtonHeight = 32;

		protected override GameFont DefaultHeaderFont
		{
			get
			{
				return GameFont.Tiny;
			}
		}

		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 200);
		}

		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(273, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.Faction != Faction.OfPlayer)
			{
				return;
			}
			AllowedAreaMode mode = (!pawn.RaceProps.Humanlike) ? AllowedAreaMode.Animal : AllowedAreaMode.Humanlike;
			AreaAllowedGUI.DoAllowedAreaSelectors(rect, pawn, mode);
		}

		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			Rect rect2 = new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f);
			if (Widgets.ButtonText(rect2, "ManageAreas".Translate(), true, false, true))
			{
				Find.WindowStack.Add(new Dialog_ManageAreas(Find.VisibleMap));
			}
		}

		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.Faction != Faction.OfPlayer)
			{
				return -2147483648;
			}
			Area areaRestriction = pawn.playerSettings.AreaRestriction;
			return (areaRestriction == null) ? -2147483647 : areaRestriction.ID;
		}
	}
}
