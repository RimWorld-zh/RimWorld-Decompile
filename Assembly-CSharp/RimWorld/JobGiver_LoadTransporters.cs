using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000D5 RID: 213
	public class JobGiver_LoadTransporters : ThinkNode_JobGiver
	{
		// Token: 0x060004BE RID: 1214 RVA: 0x0003566C File Offset: 0x00033A6C
		protected override Job TryGiveJob(Pawn pawn)
		{
			int transportersGroup = pawn.mindState.duty.transportersGroup;
			TransporterUtility.GetTransportersInGroup(transportersGroup, pawn.Map, JobGiver_LoadTransporters.tmpTransporters);
			for (int i = 0; i < JobGiver_LoadTransporters.tmpTransporters.Count; i++)
			{
				CompTransporter transporter = JobGiver_LoadTransporters.tmpTransporters[i];
				if (LoadTransportersJobUtility.HasJobOnTransporter(pawn, transporter))
				{
					return LoadTransportersJobUtility.JobOnTransporter(pawn, transporter);
				}
			}
			return null;
		}

		// Token: 0x040002A6 RID: 678
		private static List<CompTransporter> tmpTransporters = new List<CompTransporter>();
	}
}
