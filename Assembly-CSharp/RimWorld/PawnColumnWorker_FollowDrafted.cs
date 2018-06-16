using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200087F RID: 2175
	public class PawnColumnWorker_FollowDrafted : PawnColumnWorker_Checkbox
	{
		// Token: 0x0600319C RID: 12700 RVA: 0x001AE0AC File Offset: 0x001AC4AC
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		// Token: 0x0600319D RID: 12701 RVA: 0x001AE0F4 File Offset: 0x001AC4F4
		protected override bool GetValue(Pawn pawn)
		{
			return pawn.playerSettings.followDrafted;
		}

		// Token: 0x0600319E RID: 12702 RVA: 0x001AE114 File Offset: 0x001AC514
		protected override void SetValue(Pawn pawn, bool value)
		{
			pawn.playerSettings.followDrafted = value;
		}
	}
}
