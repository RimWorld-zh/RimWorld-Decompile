using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200009E RID: 158
	public class JobDriver_RemoveApparel : JobDriver
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x0002EF00 File Offset: 0x0002D300
		private Apparel Apparel
		{
			get
			{
				return (Apparel)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0002EF2E File Offset: 0x0002D32E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0002EF4C File Offset: 0x0002D34C
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0002EF62 File Offset: 0x0002D362
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.duration = (int)(this.Apparel.GetStatValue(StatDefOf.EquipDelay, true) * 60f);
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0002EF8C File Offset: 0x0002D38C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			yield return Toils_General.Wait(this.duration).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return Toils_General.Do(delegate
			{
				if (this.pawn.apparel.WornApparel.Contains(this.Apparel))
				{
					Apparel apparel;
					if (this.pawn.apparel.TryDrop(this.Apparel, out apparel))
					{
						this.job.targetA = apparel;
						if (this.job.haulDroppedApparel)
						{
							apparel.SetForbidden(false, false);
							StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(apparel);
							IntVec3 c;
							if (StoreUtility.TryFindBestBetterStoreCellFor(apparel, this.pawn, base.Map, currentPriority, this.pawn.Faction, out c, true))
							{
								this.job.count = apparel.stackCount;
								this.job.targetB = c;
							}
							else
							{
								base.EndJobWith(JobCondition.Incompletable);
							}
						}
						else
						{
							base.EndJobWith(JobCondition.Succeeded);
						}
					}
					else
					{
						base.EndJobWith(JobCondition.Incompletable);
					}
				}
				else
				{
					base.EndJobWith(JobCondition.Incompletable);
				}
			});
			if (this.job.haulDroppedApparel)
			{
				yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
				yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
				yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false).FailOn(() => !this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation));
				Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
				yield return carryToCell;
				yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true);
			}
			yield break;
		}

		// Token: 0x04000269 RID: 617
		private int duration;

		// Token: 0x0400026A RID: 618
		private const TargetIndex ApparelInd = TargetIndex.A;
	}
}
