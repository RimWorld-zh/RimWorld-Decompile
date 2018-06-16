using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200009D RID: 157
	public class JobDriver_DropEquipment : JobDriver
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x0002EC8C File Offset: 0x0002D08C
		private ThingWithComps TargetEquipment
		{
			get
			{
				return (ThingWithComps)base.TargetA.Thing;
			}
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0002ECB4 File Offset: 0x0002D0B4
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0002ECCC File Offset: 0x0002D0CC
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			yield return new Toil
			{
				initAction = delegate()
				{
					this.pawn.pather.StopDead();
				},
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = 30
			};
			yield return new Toil
			{
				initAction = delegate()
				{
					ThingWithComps thingWithComps;
					if (!this.pawn.equipment.TryDropEquipment(this.TargetEquipment, out thingWithComps, this.pawn.Position, true))
					{
						base.EndJobWith(JobCondition.Incompletable);
					}
				}
			};
			yield break;
		}

		// Token: 0x04000268 RID: 616
		private const int DurationTicks = 30;
	}
}
