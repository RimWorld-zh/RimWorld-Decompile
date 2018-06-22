using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200087C RID: 2172
	public class PawnColumnWorker_FollowFieldwork : PawnColumnWorker_Checkbox
	{
		// Token: 0x0600319B RID: 12699 RVA: 0x001AE3DC File Offset: 0x001AC7DC
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer && pawn.training.HasLearned(TrainableDefOf.Obedience);
		}

		// Token: 0x0600319C RID: 12700 RVA: 0x001AE424 File Offset: 0x001AC824
		protected override bool GetValue(Pawn pawn)
		{
			return pawn.playerSettings.followFieldwork;
		}

		// Token: 0x0600319D RID: 12701 RVA: 0x001AE444 File Offset: 0x001AC844
		protected override void SetValue(Pawn pawn, bool value)
		{
			pawn.playerSettings.followFieldwork = value;
		}
	}
}
