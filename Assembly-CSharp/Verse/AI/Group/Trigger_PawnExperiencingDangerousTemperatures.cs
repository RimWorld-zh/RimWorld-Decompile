using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A2E RID: 2606
	public class Trigger_PawnExperiencingDangerousTemperatures : Trigger
	{
		// Token: 0x040024C8 RID: 9416
		private float temperatureHediffThreshold = 0.15f;

		// Token: 0x060039DB RID: 14811 RVA: 0x001E92D0 File Offset: 0x001E76D0
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 197 == 0)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn pawn = lord.ownedPawns[i];
					if (pawn.Spawned && !pawn.Dead && !pawn.Downed)
					{
						Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Heatstroke, false);
						if (firstHediffOfDef != null && firstHediffOfDef.Severity > this.temperatureHediffThreshold)
						{
							return true;
						}
						Hediff firstHediffOfDef2 = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia, false);
						if (firstHediffOfDef2 != null && firstHediffOfDef2.Severity > this.temperatureHediffThreshold)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
