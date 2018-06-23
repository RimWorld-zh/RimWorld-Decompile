using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000890 RID: 2192
	public class PawnColumnWorker_HostilityResponse : PawnColumnWorker
	{
		// Token: 0x04001ADA RID: 6874
		private const int TopPadding = 3;

		// Token: 0x04001ADB RID: 6875
		private const int Width = 24;

		// Token: 0x0600320F RID: 12815 RVA: 0x001AFB29 File Offset: 0x001ADF29
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.RaceProps.Humanlike)
			{
				HostilityResponseModeUtility.DrawResponseButton(rect, pawn, true);
			}
		}

		// Token: 0x06003210 RID: 12816 RVA: 0x001AFB4C File Offset: 0x001ADF4C
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), Mathf.CeilToInt(24f) + 3);
		}

		// Token: 0x06003211 RID: 12817 RVA: 0x001AFB7C File Offset: 0x001ADF7C
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 24);
		}

		// Token: 0x06003212 RID: 12818 RVA: 0x001AFBA0 File Offset: 0x001ADFA0
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06003213 RID: 12819 RVA: 0x001AFBC8 File Offset: 0x001ADFC8
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06003214 RID: 12820 RVA: 0x001AFBF4 File Offset: 0x001ADFF4
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
