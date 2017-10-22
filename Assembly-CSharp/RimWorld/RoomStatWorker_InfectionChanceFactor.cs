using UnityEngine;
using Verse;

namespace RimWorld
{
	public class RoomStatWorker_InfectionChanceFactor : RoomStatWorker
	{
		public override float GetScore(Room room)
		{
			float stat = room.GetStat(RoomStatDefOf.Cleanliness);
			float value = (!(stat >= 0.0)) ? GenMath.LerpDouble(-5f, 0f, 1f, 0.5f, stat) : GenMath.LerpDouble(0f, 1f, 0.5f, 0.2f, stat);
			return Mathf.Clamp(value, 0.2f, 1f);
		}
	}
}
