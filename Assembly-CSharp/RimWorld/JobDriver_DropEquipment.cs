using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_DropEquipment : JobDriver
	{
		private const int DurationTicks = 30;

		private ThingWithComps TargetEquipment
		{
			get
			{
				return (ThingWithComps)base.TargetA.Thing;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					((_003CMakeNewToils_003Ec__Iterator52)/*Error near IL_0036: stateMachine*/)._003C_003Ef__this.pawn.pather.StopDead();
				},
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = 30
			};
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					ThingWithComps thingWithComps = default(ThingWithComps);
					if (!((_003CMakeNewToils_003Ec__Iterator52)/*Error near IL_0089: stateMachine*/)._003C_003Ef__this.pawn.equipment.TryDropEquipment(((_003CMakeNewToils_003Ec__Iterator52)/*Error near IL_0089: stateMachine*/)._003C_003Ef__this.TargetEquipment, out thingWithComps, ((_003CMakeNewToils_003Ec__Iterator52)/*Error near IL_0089: stateMachine*/)._003C_003Ef__this.pawn.Position, true))
					{
						((_003CMakeNewToils_003Ec__Iterator52)/*Error near IL_0089: stateMachine*/)._003C_003Ef__this.EndJobWith(JobCondition.Incompletable);
					}
				}
			};
		}
	}
}
