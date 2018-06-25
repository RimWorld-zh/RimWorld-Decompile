using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200045B RID: 1115
	public class DeathActionWorker_BigExplosion : DeathActionWorker
	{
		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06001390 RID: 5008 RVA: 0x000A91EC File Offset: 0x000A75EC
		public override RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_DiedExplosive;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06001391 RID: 5009 RVA: 0x000A9208 File Offset: 0x000A7608
		public override bool DangerousInMelee
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x000A9220 File Offset: 0x000A7620
		public override void PawnDied(Corpse corpse)
		{
			float radius;
			if (corpse.InnerPawn.ageTracker.CurLifeStageIndex == 0)
			{
				radius = 1.9f;
			}
			else if (corpse.InnerPawn.ageTracker.CurLifeStageIndex == 1)
			{
				radius = 2.9f;
			}
			else
			{
				radius = 4.9f;
			}
			GenExplosion.DoExplosion(corpse.Position, corpse.Map, radius, DamageDefOf.Flame, corpse.InnerPawn, -1, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
		}
	}
}
