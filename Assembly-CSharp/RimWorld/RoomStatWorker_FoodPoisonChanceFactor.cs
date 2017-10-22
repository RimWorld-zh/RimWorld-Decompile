using UnityEngine;
using Verse;

namespace RimWorld
{
	public class RoomStatWorker_FoodPoisonChanceFactor : RoomStatWorker
	{
		public override float GetScore(Room room)
		{
			float stat = room.GetStat(RoomStatDefOf.Cleanliness);
			float value = (float)(1.0 / GenMath.UnboundedValueToFactor((float)(stat * 0.20999999344348907)));
			return Mathf.Clamp(value, 0.7f, 1.6f);
		}
	}
}
