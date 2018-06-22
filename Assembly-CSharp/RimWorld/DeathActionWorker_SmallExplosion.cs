using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200045A RID: 1114
	public class DeathActionWorker_SmallExplosion : DeathActionWorker
	{
		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06001390 RID: 5008 RVA: 0x000A9164 File Offset: 0x000A7564
		public override RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_DiedExplosive;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06001391 RID: 5009 RVA: 0x000A9180 File Offset: 0x000A7580
		public override bool DangerousInMelee
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x000A9198 File Offset: 0x000A7598
		public override void PawnDied(Corpse corpse)
		{
			GenExplosion.DoExplosion(corpse.Position, corpse.Map, 1.9f, DamageDefOf.Flame, corpse.InnerPawn, -1, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false);
		}
	}
}
