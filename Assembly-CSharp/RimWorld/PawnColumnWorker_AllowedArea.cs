using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

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
			if (pawn.Faction == Faction.OfPlayer)
			{
				AllowedAreaMode mode = (AllowedAreaMode)(pawn.RaceProps.Humanlike ? 1 : 2);
				AreaAllowedGUI.DoAllowedAreaSelectors(rect, pawn, mode);
			}
		}

		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			Rect rect2 = new Rect(rect.x, (float)(rect.y + (rect.height - 65.0)), Mathf.Min(rect.width, 360f), 32f);
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
			return (areaRestriction == null) ? (-2147483647) : areaRestriction.ID;
		}

		protected override void HeaderClicked(Rect headerRect, PawnTable table)
		{
			base.HeaderClicked(headerRect, table);
			if (Event.current.shift && Find.VisibleMap != null)
			{
				List<Pawn> pawnsListForReading = table.PawnsListForReading;
				int num = 0;
				while (num < pawnsListForReading.Count)
				{
					if (pawnsListForReading[num].Faction == Faction.OfPlayer)
					{
						if (Event.current.button == 0)
						{
							pawnsListForReading[num].playerSettings.AreaRestriction = Find.VisibleMap.areaManager.Home;
						}
						else if (Event.current.button == 1)
						{
							pawnsListForReading[num].playerSettings.AreaRestriction = null;
						}
						num++;
						continue;
					}
					return;
				}
				if (Event.current.button == 0)
				{
					SoundDefOf.CheckboxTurnedOn.PlayOneShotOnCamera(null);
				}
				else if (Event.current.button == 1)
				{
					SoundDefOf.CheckboxTurnedOff.PlayOneShotOnCamera(null);
				}
			}
		}

		protected override string GetHeaderTip(PawnTable table)
		{
			return base.GetHeaderTip(table) + "\n" + "AllowedAreaShiftClickTip".Translate();
		}
	}
}
