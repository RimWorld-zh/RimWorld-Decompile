using System;
using System.Collections.Generic;
using Verse.Sound;

namespace Verse.AI
{
	public class JobDriver_Equip : JobDriver
	{
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					ThingWithComps thingWithComps = (ThingWithComps)((_003CMakeNewToils_003Ec__Iterator1B1)/*Error near IL_0074: stateMachine*/)._003C_003Ef__this.CurJob.targetA.Thing;
					ThingWithComps thingWithComps2 = null;
					if (thingWithComps.def.stackLimit > 1 && thingWithComps.stackCount > 1)
					{
						thingWithComps2 = (ThingWithComps)thingWithComps.SplitOff(1);
					}
					else
					{
						thingWithComps2 = thingWithComps;
						thingWithComps2.DeSpawn();
					}
					((_003CMakeNewToils_003Ec__Iterator1B1)/*Error near IL_0074: stateMachine*/)._003C_003Ef__this.pawn.equipment.MakeRoomFor(thingWithComps2);
					((_003CMakeNewToils_003Ec__Iterator1B1)/*Error near IL_0074: stateMachine*/)._003C_003Ef__this.pawn.equipment.AddEquipment(thingWithComps2);
					if (thingWithComps.def.soundInteract != null)
					{
						thingWithComps.def.soundInteract.PlayOneShot(new TargetInfo(((_003CMakeNewToils_003Ec__Iterator1B1)/*Error near IL_0074: stateMachine*/)._003C_003Ef__this.pawn.Position, ((_003CMakeNewToils_003Ec__Iterator1B1)/*Error near IL_0074: stateMachine*/)._003C_003Ef__this.pawn.Map, false));
					}
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
}
