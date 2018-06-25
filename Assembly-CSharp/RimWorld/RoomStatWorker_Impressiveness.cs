using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000440 RID: 1088
	public class RoomStatWorker_Impressiveness : RoomStatWorker
	{
		// Token: 0x060012E2 RID: 4834 RVA: 0x000A3210 File Offset: 0x000A1610
		public override float GetScore(Room room)
		{
			float factor = this.GetFactor(room.GetStat(RoomStatDefOf.Wealth) / 1500f);
			float factor2 = this.GetFactor(room.GetStat(RoomStatDefOf.Beauty) / 3f);
			float factor3 = this.GetFactor(room.GetStat(RoomStatDefOf.Space) / 125f);
			float factor4 = this.GetFactor(1f + Mathf.Min(room.GetStat(RoomStatDefOf.Cleanliness), 0f) / 2.5f);
			float a = (factor + factor2 + factor3 + factor4) / 4f;
			float b = Mathf.Min(factor, Mathf.Min(factor2, Mathf.Min(factor3, factor4)));
			float num = Mathf.Lerp(a, b, 0.35f);
			float num2 = factor3 * 5f;
			if (num > num2)
			{
				num = Mathf.Lerp(num, num2, 0.75f);
			}
			return num * 100f;
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x000A32F8 File Offset: 0x000A16F8
		private float GetFactor(float baseFactor)
		{
			float result;
			if (Mathf.Abs(baseFactor) < 1f)
			{
				result = baseFactor;
			}
			else if (baseFactor > 0f)
			{
				result = 1f + Mathf.Log(baseFactor);
			}
			else
			{
				result = -1f - Mathf.Log(-baseFactor);
			}
			return result;
		}
	}
}
