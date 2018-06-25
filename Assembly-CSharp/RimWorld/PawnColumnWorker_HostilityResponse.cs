using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000892 RID: 2194
	public class PawnColumnWorker_HostilityResponse : PawnColumnWorker
	{
		// Token: 0x04001ADA RID: 6874
		private const int TopPadding = 3;

		// Token: 0x04001ADB RID: 6875
		private const int Width = 24;

		// Token: 0x06003213 RID: 12819 RVA: 0x001AFC69 File Offset: 0x001AE069
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.RaceProps.Humanlike)
			{
				HostilityResponseModeUtility.DrawResponseButton(rect, pawn, true);
			}
		}

		// Token: 0x06003214 RID: 12820 RVA: 0x001AFC8C File Offset: 0x001AE08C
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), Mathf.CeilToInt(24f) + 3);
		}

		// Token: 0x06003215 RID: 12821 RVA: 0x001AFCBC File Offset: 0x001AE0BC
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 24);
		}

		// Token: 0x06003216 RID: 12822 RVA: 0x001AFCE0 File Offset: 0x001AE0E0
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06003217 RID: 12823 RVA: 0x001AFD08 File Offset: 0x001AE108
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06003218 RID: 12824 RVA: 0x001AFD34 File Offset: 0x001AE134
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
