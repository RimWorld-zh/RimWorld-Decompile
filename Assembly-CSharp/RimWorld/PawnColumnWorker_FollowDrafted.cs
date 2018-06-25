using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200087D RID: 2173
	public class PawnColumnWorker_FollowDrafted : PawnColumnWorker_Checkbox
	{
		// Token: 0x0600319A RID: 12698 RVA: 0x001AE70C File Offset: 0x001ACB0C
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		// Token: 0x0600319B RID: 12699 RVA: 0x001AE754 File Offset: 0x001ACB54
		protected override bool GetValue(Pawn pawn)
		{
			return pawn.playerSettings.followDrafted;
		}

		// Token: 0x0600319C RID: 12700 RVA: 0x001AE774 File Offset: 0x001ACB74
		protected override void SetValue(Pawn pawn, bool value)
		{
			pawn.playerSettings.followDrafted = value;
		}
	}
}
