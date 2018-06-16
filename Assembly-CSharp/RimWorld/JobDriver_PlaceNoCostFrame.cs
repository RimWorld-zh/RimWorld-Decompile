using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000044 RID: 68
	public class JobDriver_PlaceNoCostFrame : JobDriver
	{
		// Token: 0x0600023B RID: 571 RVA: 0x00017D94 File Offset: 0x00016194
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00017DC8 File Offset: 0x000161C8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Goto.MoveOffTargetBlueprint(TargetIndex.A);
			yield return Toils_Construct.MakeSolidThingFromBlueprintIfNecessary(TargetIndex.A, TargetIndex.None);
			yield break;
		}
	}
}
