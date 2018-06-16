using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000443 RID: 1091
	public class RoomStatWorker_SurgerySuccessChanceCleanlinessFactor : RoomStatWorker
	{
		// Token: 0x060012EA RID: 4842 RVA: 0x000A325C File Offset: 0x000A165C
		public override float GetScore(Room room)
		{
			float stat = room.GetStat(RoomStatDefOf.Cleanliness);
			float value = GenMath.LerpDouble(-5f, 5f, 0.6f, 1.5f, stat);
			return Mathf.Clamp(value, 0.6f, 1.5f);
		}

		// Token: 0x04000B74 RID: 2932
		private const float MinFactor = 0.6f;

		// Token: 0x04000B75 RID: 2933
		private const float MaxFactor = 1.5f;

		// Token: 0x04000B76 RID: 2934
		private const float CleanlinessInfluence = 0.05f;
	}
}
