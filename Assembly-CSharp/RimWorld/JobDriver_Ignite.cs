using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200006F RID: 111
	public class JobDriver_Ignite : JobDriver
	{
		// Token: 0x0600030C RID: 780 RVA: 0x00021154 File Offset: 0x0001F554
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0002116C File Offset: 0x0001F56C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnBurningImmobile(TargetIndex.A);
			yield return new Toil
			{
				initAction = delegate()
				{
					this.pawn.natives.TryStartIgnite(base.TargetThingA);
				}
			};
			yield break;
		}
	}
}
