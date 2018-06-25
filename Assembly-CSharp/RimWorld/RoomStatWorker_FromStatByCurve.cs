using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200043F RID: 1087
	public class RoomStatWorker_FromStatByCurve : RoomStatWorker
	{
		// Token: 0x060012DF RID: 4831 RVA: 0x000A33D0 File Offset: 0x000A17D0
		public override float GetScore(Room room)
		{
			return this.def.curve.Evaluate(room.GetStat(this.def.inputStat));
		}
	}
}
