using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200087D RID: 2173
	public class PawnColumnWorker_FollowDrafted : PawnColumnWorker_Checkbox
	{
		// Token: 0x0600319B RID: 12699 RVA: 0x001AE4A4 File Offset: 0x001AC8A4
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		// Token: 0x0600319C RID: 12700 RVA: 0x001AE4EC File Offset: 0x001AC8EC
		protected override bool GetValue(Pawn pawn)
		{
			return pawn.playerSettings.followDrafted;
		}

		// Token: 0x0600319D RID: 12701 RVA: 0x001AE50C File Offset: 0x001AC90C
		protected override void SetValue(Pawn pawn, bool value)
		{
			pawn.playerSettings.followDrafted = value;
		}
	}
}
