using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_EnterTransporter : ThinkNode_JobGiver
	{
		private static List<CompTransporter> tmpTransporters = new List<CompTransporter>();

		protected override Job TryGiveJob(Pawn pawn)
		{
			int transportersGroup = pawn.mindState.duty.transportersGroup;
			TransporterUtility.GetTransportersInGroup(transportersGroup, pawn.Map, JobGiver_EnterTransporter.tmpTransporters);
			CompTransporter compTransporter = this.FindMyTransporter(JobGiver_EnterTransporter.tmpTransporters, pawn);
			return (compTransporter != null && pawn.CanReserveAndReach((Thing)compTransporter.parent, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false)) ? new Job(JobDefOf.EnterTransporter, (Thing)compTransporter.parent) : null;
		}

		private CompTransporter FindMyTransporter(List<CompTransporter> transporters, Pawn me)
		{
			int num = 0;
			CompTransporter result;
			while (true)
			{
				if (num < transporters.Count)
				{
					List<TransferableOneWay> leftToLoad = transporters[num].leftToLoad;
					if (leftToLoad != null)
					{
						for (int i = 0; i < leftToLoad.Count; i++)
						{
							if (leftToLoad[i].AnyThing is Pawn)
							{
								List<Thing> things = leftToLoad[i].things;
								for (int j = 0; j < things.Count; j++)
								{
									if (things[j] == me)
										goto IL_0064;
								}
							}
						}
					}
					num++;
					continue;
				}
				result = null;
				break;
				IL_0064:
				result = transporters[num];
				break;
			}
			return result;
		}
	}
}
