using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A2B RID: 2603
	public class Trigger_PawnExperiencingDangerousTemperatures : Trigger
	{
		// Token: 0x060039D6 RID: 14806 RVA: 0x001E8E78 File Offset: 0x001E7278
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

		// Token: 0x040024B7 RID: 9399
		private float temperatureHediffThreshold = 0.15f;
	}
}
