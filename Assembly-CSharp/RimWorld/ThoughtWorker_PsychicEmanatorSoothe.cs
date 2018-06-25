using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200022C RID: 556
	public class ThoughtWorker_PsychicEmanatorSoothe : ThoughtWorker
	{
		// Token: 0x040003EA RID: 1002
		private const float Radius = 15f;

		// Token: 0x06000A26 RID: 2598 RVA: 0x00059B2C File Offset: 0x00057F2C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtState result;
			if (!p.Spawned)
			{
				result = false;
			}
			else
			{
				List<Thing> list = p.Map.listerThings.ThingsOfDef(ThingDefOf.PsychicEmanator);
				for (int i = 0; i < list.Count; i++)
				{
					CompPowerTrader compPowerTrader = list[i].TryGetComp<CompPowerTrader>();
					if (compPowerTrader == null || compPowerTrader.PowerOn)
					{
						if (p.Position.InHorDistOf(list[i].Position, 15f))
						{
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}
	}
}
