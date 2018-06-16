using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000894 RID: 2196
	public class PawnColumnWorker_HostilityResponse : PawnColumnWorker
	{
		// Token: 0x06003214 RID: 12820 RVA: 0x001AF879 File Offset: 0x001ADC79
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.RaceProps.Humanlike)
			{
				HostilityResponseModeUtility.DrawResponseButton(rect, pawn, true);
			}
		}

		// Token: 0x06003215 RID: 12821 RVA: 0x001AF89C File Offset: 0x001ADC9C
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), Mathf.CeilToInt(24f) + 3);
		}

		// Token: 0x06003216 RID: 12822 RVA: 0x001AF8CC File Offset: 0x001ADCCC
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 24);
		}

		// Token: 0x06003217 RID: 12823 RVA: 0x001AF8F0 File Offset: 0x001ADCF0
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06003218 RID: 12824 RVA: 0x001AF918 File Offset: 0x001ADD18
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06003219 RID: 12825 RVA: 0x001AF944 File Offset: 0x001ADD44
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

		// Token: 0x04001ADC RID: 6876
		private const int TopPadding = 3;

		// Token: 0x04001ADD RID: 6877
		private const int Width = 24;
	}
}
