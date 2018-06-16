using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000890 RID: 2192
	public class PawnColumnWorker_AllowedArea : PawnColumnWorker
	{
		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x060031FB RID: 12795 RVA: 0x001AEEE4 File Offset: 0x001AD2E4
		protected override GameFont DefaultHeaderFont
		{
			get
			{
				return GameFont.Tiny;
			}
		}

		// Token: 0x060031FC RID: 12796 RVA: 0x001AEEFC File Offset: 0x001AD2FC
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 200);
		}

		// Token: 0x060031FD RID: 12797 RVA: 0x001AEF24 File Offset: 0x001AD324
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(273, this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x060031FE RID: 12798 RVA: 0x001AEF54 File Offset: 0x001AD354
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x060031FF RID: 12799 RVA: 0x001AEF77 File Offset: 0x001AD377
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.Faction == Faction.OfPlayer)
			{
				AreaAllowedGUI.DoAllowedAreaSelectors(rect, pawn);
			}
		}

		// Token: 0x06003200 RID: 12800 RVA: 0x001AEF98 File Offset: 0x001AD398
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			Rect rect2 = new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f);
			if (Widgets.ButtonText(rect2, "ManageAreas".Translate(), true, false, true))
			{
				Find.WindowStack.Add(new Dialog_ManageAreas(Find.CurrentMap));
			}
		}

		// Token: 0x06003201 RID: 12801 RVA: 0x001AF014 File Offset: 0x001AD414
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06003202 RID: 12802 RVA: 0x001AF040 File Offset: 0x001AD440
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

		// Token: 0x06003203 RID: 12803 RVA: 0x001AF094 File Offset: 0x001AD494
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

		// Token: 0x06003204 RID: 12804 RVA: 0x001AF190 File Offset: 0x001AD590
		protected override string GetHeaderTip(PawnTable table)
		{
			return base.GetHeaderTip(table) + "\n" + "AllowedAreaShiftClickTip".Translate();
		}

		// Token: 0x04001AD4 RID: 6868
		private const int TopAreaHeight = 65;

		// Token: 0x04001AD5 RID: 6869
		private const int ManageAreasButtonHeight = 32;
	}
}
