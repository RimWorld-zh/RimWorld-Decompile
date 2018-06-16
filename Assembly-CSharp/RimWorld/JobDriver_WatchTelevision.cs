using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000062 RID: 98
	public class JobDriver_WatchTelevision : JobDriver_WatchBuilding
	{
		// Token: 0x060002CC RID: 716 RVA: 0x0001DE7C File Offset: 0x0001C27C
		protected override void WatchTickAction()
		{
			Building thing = (Building)base.TargetA.Thing;
			if (!thing.TryGetComp<CompPowerTrader>().PowerOn)
			{
				base.EndJobWith(JobCondition.Incompletable);
			}
			else
			{
				base.WatchTickAction();
			}
		}
	}
}
