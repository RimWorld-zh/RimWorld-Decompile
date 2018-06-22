using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200088C RID: 2188
	public class PawnColumnWorker_AllowedArea : PawnColumnWorker
	{
		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x060031F6 RID: 12790 RVA: 0x001AF194 File Offset: 0x001AD594
		protected override GameFont DefaultHeaderFont
		{
			get
			{
				return GameFont.Tiny;
			}
		}

		// Token: 0x060031F7 RID: 12791 RVA: 0x001AF1AC File Offset: 0x001AD5AC
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 200);
		}

		// Token: 0x060031F8 RID: 12792 RVA: 0x001AF1D4 File Offset: 0x001AD5D4
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(273, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x060031F9 RID: 12793 RVA: 0x001AF204 File Offset: 0x001AD604
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x060031FA RID: 12794 RVA: 0x001AF227 File Offset: 0x001AD627
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.Faction == Faction.OfPlayer)
			{
				AreaAllowedGUI.DoAllowedAreaSelectors(rect, pawn);
			}
		}

		// Token: 0x060031FB RID: 12795 RVA: 0x001AF248 File Offset: 0x001AD648
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			Rect rect2 = new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f);
			if (Widgets.ButtonText(rect2, "ManageAreas".Translate(), true, false, true))
			{
				Find.WindowStack.Add(new Dialog_ManageAreas(Find.CurrentMap));
			}
		}

		// Token: 0x060031FC RID: 12796 RVA: 0x001AF2C4 File Offset: 0x001AD6C4
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x060031FD RID: 12797 RVA: 0x001AF2F0 File Offset: 0x001AD6F0
		private int GetValueToCompare(Pawn pawn)
		{
			int result;
			if (pawn.Faction != Faction.OfPlayer)
			{
				result = int.MinValue;
			}
			else
			{
				Area areaRestriction = pawn.playerSettings.AreaRestriction;
				result = ((areaRestriction == null) ? -2147483647 : areaRestriction.ID);
			}
			return result;
		}

		// Token: 0x060031FE RID: 12798 RVA: 0x001AF344 File Offset: 0x001AD744
		protected override void HeaderClicked(Rect headerRect, PawnTable table)
		{
			base.HeaderClicked(headerRect, table);
			if (Event.current.shift && Find.CurrentMap != null)
			{
				List<Pawn> pawnsListForReading = table.PawnsListForReading;
				for (int i = 0; i < pawnsListForReading.Count; i++)
				{
					if (pawnsListForReading[i].Faction != Faction.OfPlayer)
					{
						return;
					}
					if (Event.current.button == 0)
					{
						pawnsListForReading[i].playerSettings.AreaRestriction = Find.CurrentMap.areaManager.Home;
					}
					else if (Event.current.button == 1)
					{
						pawnsListForReading[i].playerSettings.AreaRestriction = null;
					}
				}
				if (Event.current.button == 0)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
				else if (Event.current.button == 1)
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x060031FF RID: 12799 RVA: 0x001AF440 File Offset: 0x001AD840
		protected override string GetHeaderTip(PawnTable table)
		{
			return base.GetHeaderTip(table) + "\n" + "AllowedAreaShiftClickTip".Translate();
		}

		// Token: 0x04001AD2 RID: 6866
		private const int TopAreaHeight = 65;

		// Token: 0x04001AD3 RID: 6867
		private const int ManageAreasButtonHeight = 32;
	}
}
