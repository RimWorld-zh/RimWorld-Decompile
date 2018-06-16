using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200043E RID: 1086
	public class RoomStatWorker_GraveVisitingJoyGainFactor : RoomStatWorker
	{
		// Token: 0x060012DE RID: 4830 RVA: 0x000A2EDC File Offset: 0x000A12DC
		public override float GetScore(Room room)
		{
			float stat = room.GetStat(RoomStatDefOf.Impressiveness);
			return Mathf.Lerp(0.8f, 1.2f, Mathf.InverseLerp(-150f, 150f, stat));
		}
	}
}
