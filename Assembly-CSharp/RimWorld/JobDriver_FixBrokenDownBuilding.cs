using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000043 RID: 67
	public class JobDriver_FixBrokenDownBuilding : JobDriver
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000236 RID: 566 RVA: 0x000179C8 File Offset: 0x00015DC8
		private Building Building
		{
			get
			{
				return (Building)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000237 RID: 567 RVA: 0x000179F8 File Offset: 0x00015DF8
		private Thing Components
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00017A24 File Offset: 0x00015E24
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.Building, this.job, 1, -1, null) && this.pawn.Reserve(this.Components, this.job, 1, -1, null);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00017A80 File Offset: 0x00015E80
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
			Toil repair = Toils_General.Wait(1000);
			repair.FailOnDespawnedOrNull(TargetIndex.A);
			repair.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			repair.WithEffect(this.Building.def.repairEffect, TargetIndex.A);
			repair.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return repair;
			yield return new Toil
			{
				initAction = delegate()
				{
					this.Components.Destroy(DestroyMode.Vanish);
					if (Rand.Value > this.pawn.GetStatValue(StatDefOf.FixBrokenDownBuildingSuccessChance, true))
					{
						Vector3 loc = (this.pawn.DrawPos + this.Building.DrawPos) / 2f;
						MoteMaker.ThrowText(loc, base.Map, "TextMote_FixBrokenDownBuildingFail".Translate(), 3.65f);
					}
					else
					{
						this.Building.GetComp<CompBreakdownable>().Notify_Repaired();
					}
				}
			};
			yield break;
		}

		// Token: 0x040001D3 RID: 467
		private const TargetIndex BuildingInd = TargetIndex.A;

		// Token: 0x040001D4 RID: 468
		private const TargetIndex ComponentInd = TargetIndex.B;

		// Token: 0x040001D5 RID: 469
		private const int TicksDuration = 1000;
	}
}
