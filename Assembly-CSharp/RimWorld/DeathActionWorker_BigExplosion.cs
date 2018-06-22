using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000459 RID: 1113
	public class DeathActionWorker_BigExplosion : DeathActionWorker
	{
		// Token: 0x170002AD RID: 685
		// (get) Token: 0x0600138C RID: 5004 RVA: 0x000A909C File Offset: 0x000A749C
		public override RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_DiedExplosive;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x0600138D RID: 5005 RVA: 0x000A90B8 File Offset: 0x000A74B8
		public override bool DangerousInMelee
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x000A90D0 File Offset: 0x000A74D0
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
