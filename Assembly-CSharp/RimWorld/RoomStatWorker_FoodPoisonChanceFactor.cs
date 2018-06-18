using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200043D RID: 1085
	public class RoomStatWorker_FoodPoisonChanceFactor : RoomStatWorker
	{
		// Token: 0x060012DC RID: 4828 RVA: 0x000A2E9C File Offset: 0x000A129C
		public override float GetScore(Room room)
		{
			float stat = room.GetStat(RoomStatDefOf.Cleanliness);
			float value = 1f / GenMath.UnboundedValueToFactor(stat * 0.21f);
			return Mathf.Clamp(value, 0.7f, 1.6f);
		}
	}
}
