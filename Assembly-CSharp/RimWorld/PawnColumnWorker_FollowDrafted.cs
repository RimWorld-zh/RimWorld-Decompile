using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200087F RID: 2175
	public class PawnColumnWorker_FollowDrafted : PawnColumnWorker_Checkbox
	{
		// Token: 0x0600319E RID: 12702 RVA: 0x001AE174 File Offset: 0x001AC574
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		// Token: 0x0600319F RID: 12703 RVA: 0x001AE1BC File Offset: 0x001AC5BC
		protected override bool GetValue(Pawn pawn)
		{
			return pawn.playerSettings.followDrafted;
		}

		// Token: 0x060031A0 RID: 12704 RVA: 0x001AE1DC File Offset: 0x001AC5DC
		protected override void SetValue(Pawn pawn, bool value)
		{
			pawn.playerSettings.followDrafted = value;
		}
	}
}
