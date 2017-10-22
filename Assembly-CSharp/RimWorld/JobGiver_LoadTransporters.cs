using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_LoadTransporters : ThinkNode_JobGiver
	{
		private static List<CompTransporter> tmpTransporters = new List<CompTransporter>();

		protected override Job TryGiveJob(Pawn pawn)
		{
			int transportersGroup = pawn.mindState.duty.transportersGroup;
			TransporterUtility.GetTransportersInGroup(transportersGroup, pawn.Map, JobGiver_LoadTransporters.tmpTransporters);
			int num = 0;
			Job result;
			while (true)
			{
				if (num < JobGiver_LoadTransporters.tmpTransporters.Count)
				{
					CompTransporter transporter = JobGiver_LoadTransporters.tmpTransporters[num];
					if (LoadTransportersJobUtility.HasJobOnTransporter(pawn, transporter))
					{
						result = LoadTransportersJobUtility.JobOnTransporter(pawn, transporter);
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}
	}
}
