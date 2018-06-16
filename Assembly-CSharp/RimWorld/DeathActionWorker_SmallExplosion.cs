using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200045E RID: 1118
	public class DeathActionWorker_SmallExplosion : DeathActionWorker
	{
		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06001399 RID: 5017 RVA: 0x000A9148 File Offset: 0x000A7548
		public override RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_DiedExplosive;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x0600139A RID: 5018 RVA: 0x000A9164 File Offset: 0x000A7564
		public override bool DangerousInMelee
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x000A917C File Offset: 0x000A757C
		public override void PawnDied(Corpse corpse)
		{
			GenExplosion.DoExplosion(corpse.Position, corpse.Map, 1.9f, DamageDefOf.Flame, corpse.InnerPawn, -1, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
		}
	}
}
