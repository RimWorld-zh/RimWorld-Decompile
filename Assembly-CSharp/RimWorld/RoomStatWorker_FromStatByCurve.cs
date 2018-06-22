using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200043D RID: 1085
	public class RoomStatWorker_FromStatByCurve : RoomStatWorker
	{
		// Token: 0x060012DC RID: 4828 RVA: 0x000A3080 File Offset: 0x000A1480
		public override float GetScore(Room room)
		{
			return this.def.curve.Evaluate(room.GetStat(this.def.inputStat));
		}
	}
}
