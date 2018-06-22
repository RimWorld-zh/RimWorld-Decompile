using System;

namespace Verse.AI
{
	// Token: 0x02000AD7 RID: 2775
	public class JobGiver_WanderNearMaster : JobGiver_Wander
	{
		// Token: 0x06003D8F RID: 15759 RVA: 0x00206470 File Offset: 0x00204870
		public JobGiver_WanderNearMaster()
		{
			this.wanderRadius = 3f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
			this.wanderDestValidator = delegate(Pawn p, IntVec3 c, IntVec3 root)
			{
				if (this.MustUseRootRoom(p))
				{
					Room room = root.GetRoom(p.Map, RegionType.Set_Passable);
					if (room != null && !WanderRoomUtility.IsValidWanderDest(p, c, root))
					{
						return false;
					}
				}
				return true;
			};
		}

		// Token: 0x06003D90 RID: 15760 RVA: 0x002064A8 File Offset: 0x002048A8
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.playerSettings.Master.PositionHeld, pawn);
		}

		// Token: 0x06003D91 RID: 15761 RVA: 0x002064D4 File Offset: 0x002048D4
		private bool MustUseRootRoom(Pawn pawn)
		{
			Pawn master = pawn.playerSettings.Master;
			return !master.playerSettings.animalsReleased;
		}
	}
}
