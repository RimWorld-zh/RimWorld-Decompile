using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200009D RID: 157
	public class JobDriver_DropEquipment : JobDriver
	{
		// Token: 0x04000269 RID: 617
		private const int DurationTicks = 30;

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x0002EC84 File Offset: 0x0002D084
		private ThingWithComps TargetEquipment
		{
			get
			{
				return (ThingWithComps)base.TargetA.Thing;
			}
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0002ECAC File Offset: 0x0002D0AC
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0002ECC4 File Offset: 0x0002D0C4
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
	}
}
