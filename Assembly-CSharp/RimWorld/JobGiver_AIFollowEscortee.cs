using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020000BA RID: 186
	public class JobGiver_AIFollowEscortee : JobGiver_AIFollowPawn
	{
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x00032D98 File Offset: 0x00031198
		protected override int FollowJobExpireInterval
		{
			get
			{
				return 120;
			}
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00032DB0 File Offset: 0x000311B0
		protected override Pawn GetFollowee(Pawn pawn)
		{
			return (Pawn)pawn.mindState.duty.focus.Thing;
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00032DE0 File Offset: 0x000311E0
		protected override float GetRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}
	}
}
