using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000880 RID: 2176
	public class PawnColumnWorker_FollowFieldwork : PawnColumnWorker_Checkbox
	{
		// Token: 0x060031A0 RID: 12704 RVA: 0x001AE12C File Offset: 0x001AC52C
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		// Token: 0x060031A1 RID: 12705 RVA: 0x001AE174 File Offset: 0x001AC574
		protected override bool GetValue(Pawn pawn)
		{
			return pawn.playerSettings.followFieldwork;
		}

		// Token: 0x060031A2 RID: 12706 RVA: 0x001AE194 File Offset: 0x001AC594
		protected override void SetValue(Pawn pawn, bool value)
		{
			pawn.playerSettings.followFieldwork = value;
		}
	}
}
