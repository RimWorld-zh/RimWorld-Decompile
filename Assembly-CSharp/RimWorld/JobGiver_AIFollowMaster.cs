using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020000BB RID: 187
	public class JobGiver_AIFollowMaster : JobGiver_AIFollowPawn
	{
		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x00032E10 File Offset: 0x00031210
		protected override int FollowJobExpireInterval
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00032E2C File Offset: 0x0003122C
		protected override Pawn GetFollowee(Pawn pawn)
		{
			Pawn result;
			if (pawn.playerSettings == null)
			{
				result = null;
			}
			else
			{
				result = pawn.playerSettings.Master;
			}
			return result;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00032E60 File Offset: 0x00031260
		protected override float GetRadius(Pawn pawn)
		{
			float result;
			if (pawn.playerSettings.Master.playerSettings.animalsReleased && pawn.training.HasLearned(TrainableDefOf.Release))
			{
				result = 50f;
			}
			else
			{
				result = 3f;
			}
			return result;
		}

		// Token: 0x0400028E RID: 654
		public const float RadiusUnreleased = 3f;

		// Token: 0x0400028F RID: 655
		public const float RadiusReleased = 50f;
	}
}
