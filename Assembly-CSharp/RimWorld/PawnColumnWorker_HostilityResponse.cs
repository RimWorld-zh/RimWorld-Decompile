using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000892 RID: 2194
	public class PawnColumnWorker_HostilityResponse : PawnColumnWorker
	{
		// Token: 0x04001ADE RID: 6878
		private const int TopPadding = 3;

		// Token: 0x04001ADF RID: 6879
		private const int Width = 24;

		// Token: 0x06003212 RID: 12818 RVA: 0x001AFED1 File Offset: 0x001AE2D1
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.RaceProps.Humanlike)
			{
				HostilityResponseModeUtility.DrawResponseButton(rect, pawn, true);
			}
		}

		// Token: 0x06003213 RID: 12819 RVA: 0x001AFEF4 File Offset: 0x001AE2F4
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), Mathf.CeilToInt(24f) + 3);
		}

		// Token: 0x06003214 RID: 12820 RVA: 0x001AFF24 File Offset: 0x001AE324
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 24);
		}

		// Token: 0x06003215 RID: 12821 RVA: 0x001AFF48 File Offset: 0x001AE348
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06003216 RID: 12822 RVA: 0x001AFF70 File Offset: 0x001AE370
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06003217 RID: 12823 RVA: 0x001AFF9C File Offset: 0x001AE39C
		private int GetValueToCompare(Pawn pawn)
		{
			int result;
			if (pawn.playerSettings == null)
			{
				result = int.MinValue;
			}
			else
			{
				result = (int)pawn.playerSettings.hostilityResponse;
			}
			return result;
		}
	}
}
