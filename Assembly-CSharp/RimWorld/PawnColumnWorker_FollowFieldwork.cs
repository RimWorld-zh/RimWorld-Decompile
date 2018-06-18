using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000880 RID: 2176
	public class PawnColumnWorker_FollowFieldwork : PawnColumnWorker_Checkbox
	{
		// Token: 0x060031A2 RID: 12706 RVA: 0x001AE1F4 File Offset: 0x001AC5F4
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		// Token: 0x060031A3 RID: 12707 RVA: 0x001AE23C File Offset: 0x001AC63C
		protected override bool GetValue(Pawn pawn)
		{
			return pawn.playerSettings.followFieldwork;
		}

		// Token: 0x060031A4 RID: 12708 RVA: 0x001AE25C File Offset: 0x001AC65C
		protected override void SetValue(Pawn pawn, bool value)
		{
			pawn.playerSettings.followFieldwork = value;
		}
	}
}
