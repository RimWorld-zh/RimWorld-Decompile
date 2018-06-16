using System;

namespace Verse.AI
{
	// Token: 0x02000ADB RID: 2779
	public class JobGiver_WanderNearMaster : JobGiver_Wander
	{
		// Token: 0x06003D92 RID: 15762 RVA: 0x00206078 File Offset: 0x00204478
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

		// Token: 0x06003D93 RID: 15763 RVA: 0x002060B0 File Offset: 0x002044B0
		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			return WanderUtility.BestCloseWanderRoot(pawn.playerSettings.Master.PositionHeld, pawn);
		}

		// Token: 0x06003D94 RID: 15764 RVA: 0x002060DC File Offset: 0x002044DC
		private bool MustUseRootRoom(Pawn pawn)
		{
			Pawn master = pawn.playerSettings.Master;
			return !master.playerSettings.animalsReleased;
		}
	}
}
