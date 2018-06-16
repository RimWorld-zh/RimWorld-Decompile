using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000076 RID: 118
	public class JobDriver_Open : JobDriver
	{
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000333 RID: 819 RVA: 0x000234A0 File Offset: 0x000218A0
		private IOpenable Openable
		{
			get
			{
				return (IOpenable)this.job.targetA.Thing;
			}
		}

		// Token: 0x06000334 RID: 820 RVA: 0x000234CC File Offset: 0x000218CC
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00023500 File Offset: 0x00021900
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return new Toil
			{
				initAction = delegate()
				{
					if (!this.Openable.CanOpen)
					{
						Designation designation = base.Map.designationManager.DesignationOn(this.job.targetA.Thing, DesignationDefOf.Open);
						if (designation != null)
						{
							designation.Delete();
						}
					}
				}
			}.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnThingMissingDesignation(TargetIndex.A, DesignationDefOf.Open).FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_General.Wait(300).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f).FailOnDespawnedOrNull(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			yield return Toils_General.Open(TargetIndex.A);
			yield break;
		}

		// Token: 0x04000226 RID: 550
		public const int OpenTicks = 300;
	}
}
