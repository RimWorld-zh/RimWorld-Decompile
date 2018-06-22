using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000CF RID: 207
	public class JobGiver_EnterTransporter : ThinkNode_JobGiver
	{
		// Token: 0x060004AE RID: 1198 RVA: 0x00034F24 File Offset: 0x00033324
		protected override Job TryGiveJob(Pawn pawn)
		{
			int transportersGroup = pawn.mindState.duty.transportersGroup;
			TransporterUtility.GetTransportersInGroup(transportersGroup, pawn.Map, JobGiver_EnterTransporter.tmpTransporters);
			CompTransporter compTransporter = this.FindMyTransporter(JobGiver_EnterTransporter.tmpTransporters, pawn);
			Job result;
			if (compTransporter == null || !pawn.CanReach(compTransporter.parent, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				result = null;
			}
			else
			{
				result = new Job(JobDefOf.EnterTransporter, compTransporter.parent);
			}
			return result;
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00034FA4 File Offset: 0x000333A4
		private CompTransporter FindMyTransporter(List<CompTransporter> transporters, Pawn me)
		{
			for (int i = 0; i < transporters.Count; i++)
			{
				List<TransferableOneWay> leftToLoad = transporters[i].leftToLoad;
				if (leftToLoad != null)
				{
					for (int j = 0; j < leftToLoad.Count; j++)
					{
						if (leftToLoad[j].AnyThing is Pawn)
						{
							List<Thing> things = leftToLoad[j].things;
							for (int k = 0; k < things.Count; k++)
							{
								if (things[k] == me)
								{
									return transporters[i];
								}
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0400029D RID: 669
		private static List<CompTransporter> tmpTransporters = new List<CompTransporter>();
	}
}
