using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200045D RID: 1117
	public class DeathActionWorker_BigExplosion : DeathActionWorker
	{
		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06001395 RID: 5013 RVA: 0x000A9080 File Offset: 0x000A7480
		public override RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_DiedExplosive;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06001396 RID: 5014 RVA: 0x000A909C File Offset: 0x000A749C
		public override bool DangerousInMelee
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x000A90B4 File Offset: 0x000A74B4
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
