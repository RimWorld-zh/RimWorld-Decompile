using System;

namespace Verse.AI
{
	// Token: 0x02000ADB RID: 2779
	public class JobGiver_WanderNearMaster : JobGiver_Wander
	{
		// Token: 0x06003D94 RID: 15764 RVA: 0x0020614C File Offset: 0x0020454C
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

		// Token: 0x06003D95 RID: 15765 RVA: 0x00206184 File Offset: 0x00204584
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.playerSettings.Master.PositionHeld, pawn);
		}

		// Token: 0x06003D96 RID: 15766 RVA: 0x002061B0 File Offset: 0x002045B0
		private bool MustUseRootRoom(Pawn pawn)
		{
			Pawn master = pawn.playerSettings.Master;
			return !master.playerSettings.animalsReleased;
		}
	}
}
