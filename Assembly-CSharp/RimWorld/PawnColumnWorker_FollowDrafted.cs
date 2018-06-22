using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200087B RID: 2171
	public class PawnColumnWorker_FollowDrafted : PawnColumnWorker_Checkbox
	{
		// Token: 0x06003197 RID: 12695 RVA: 0x001AE35C File Offset: 0x001AC75C
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		// Token: 0x06003198 RID: 12696 RVA: 0x001AE3A4 File Offset: 0x001AC7A4
		protected override bool GetValue(Pawn pawn)
		{
			return pawn.playerSettings.followDrafted;
		}

		// Token: 0x06003199 RID: 12697 RVA: 0x001AE3C4 File Offset: 0x001AC7C4
		protected override void SetValue(Pawn pawn, bool value)
		{
			pawn.playerSettings.followDrafted = value;
		}
	}
}
