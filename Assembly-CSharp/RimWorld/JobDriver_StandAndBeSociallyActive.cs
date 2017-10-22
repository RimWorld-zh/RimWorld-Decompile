using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_StandAndBeSociallyActive : JobDriver
	{
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return new Toil
			{
				tickAction = (Action)delegate
				{
					Pawn pawn = ((_003CMakeNewToils_003Ec__Iterator18)/*Error near IL_0032: stateMachine*/)._003C_003Ef__this.FindClosePawn();
					if (pawn != null)
					{
						((_003CMakeNewToils_003Ec__Iterator18)/*Error near IL_0032: stateMachine*/)._003C_003Ef__this.pawn.Drawer.rotator.FaceCell(pawn.Position);
					}
					((_003CMakeNewToils_003Ec__Iterator18)/*Error near IL_0032: stateMachine*/)._003C_003Ef__this.pawn.GainComfortFromCellIfPossible();
				},
				socialMode = RandomSocialMode.SuperActive,
				defaultCompleteMode = ToilCompleteMode.Never,
				handlingFacing = true
			};
		}

		private Pawn FindClosePawn()
		{
			IntVec3 position = base.pawn.Position;
			for (int i = 0; i < 24; i++)
			{
				IntVec3 intVec = position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(base.Map))
				{
					Thing thing = intVec.GetThingList(base.Map).Find((Predicate<Thing>)((Thing x) => x is Pawn));
					if (thing != null && thing != base.pawn && GenSight.LineOfSight(position, intVec, base.Map, false, null, 0, 0))
					{
						return (Pawn)thing;
					}
				}
			}
			return null;
		}
	}
}
