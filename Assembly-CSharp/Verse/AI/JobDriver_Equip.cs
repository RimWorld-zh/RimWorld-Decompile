using System;
using System.Collections.Generic;
using Verse.Sound;

namespace Verse.AI
{
	// Token: 0x02000A34 RID: 2612
	public class JobDriver_Equip : JobDriver
	{
		// Token: 0x060039F3 RID: 14835 RVA: 0x001E9AE4 File Offset: 0x001E7EE4
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		// Token: 0x060039F4 RID: 14836 RVA: 0x001E9B18 File Offset: 0x001E7F18
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return new Toil
			{
				initAction = delegate()
				{
					ThingWithComps thingWithComps = (ThingWithComps)this.job.targetA.Thing;
					ThingWithComps thingWithComps2;
					if (thingWithComps.def.stackLimit > 1 && thingWithComps.stackCount > 1)
					{
						thingWithComps2 = (ThingWithComps)thingWithComps.SplitOff(1);
					}
					else
					{
						thingWithComps2 = thingWithComps;
						thingWithComps2.DeSpawn(DestroyMode.Vanish);
					}
					this.pawn.equipment.MakeRoomFor(thingWithComps2);
					this.pawn.equipment.AddEquipment(thingWithComps2);
					if (thingWithComps.def.soundInteract != null)
					{
						thingWithComps.def.soundInteract.PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}
	}
}
