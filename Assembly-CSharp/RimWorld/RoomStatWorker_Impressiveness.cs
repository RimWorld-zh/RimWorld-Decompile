using UnityEngine;
using Verse;

namespace RimWorld
{
	public class RoomStatWorker_Impressiveness : RoomStatWorker
	{
		public override float GetScore(Room room)
		{
			float factor = this.GetFactor((float)(room.GetStat(RoomStatDefOf.Wealth) / 1500.0));
			float factor2 = this.GetFactor((float)(room.GetStat(RoomStatDefOf.Beauty) / 3.0));
			float factor3 = this.GetFactor((float)(room.GetStat(RoomStatDefOf.Space) / 125.0));
			float factor4 = this.GetFactor((float)(1.0 + room.GetStat(RoomStatDefOf.Cleanliness) / 2.5));
			float a = (float)((factor + factor2 + factor3 + factor4) / 4.0);
			float b = Mathf.Min(factor, Mathf.Min(factor2, Mathf.Min(factor3, factor4)));
			float num = Mathf.Lerp(a, b, 0.35f);
			float num2 = (float)(factor3 * 5.0);
			if (num > num2)
			{
				num = Mathf.Lerp(num, num2, 0.75f);
			}
			return (float)(num * 100.0);
		}

		private float GetFactor(float baseFactor)
		{
			if (Mathf.Abs(baseFactor) < 1.0)
			{
				return baseFactor;
			}
			if (baseFactor > 0.0)
			{
				return (float)(1.0 + Mathf.Log(baseFactor));
			}
			return (float)(-1.0 - Mathf.Log((float)(0.0 - baseFactor)));
		}
	}
}
