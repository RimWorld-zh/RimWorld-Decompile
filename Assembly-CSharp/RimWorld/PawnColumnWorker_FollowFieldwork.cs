using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200087E RID: 2174
	public class PawnColumnWorker_FollowFieldwork : PawnColumnWorker_Checkbox
	{
		// Token: 0x0600319F RID: 12703 RVA: 0x001AE524 File Offset: 0x001AC924
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		// Token: 0x060031A0 RID: 12704 RVA: 0x001AE56C File Offset: 0x001AC96C
		protected override bool GetValue(Pawn pawn)
		{
			return pawn.playerSettings.followFieldwork;
		}

		// Token: 0x060031A1 RID: 12705 RVA: 0x001AE58C File Offset: 0x001AC98C
		protected override void SetValue(Pawn pawn, bool value)
		{
			pawn.playerSettings.followFieldwork = value;
		}
	}
}
