using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_RemoveRoof : JobDriver_AffectRoof
	{
		private static List<IntVec3> removedRoofs = new List<IntVec3>();

		protected override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn((Func<bool>)(() => !((Area)((_003CMakeNewToils_003Ec__Iterator12)/*Error near IL_0029: stateMachine*/)._003C_003Ef__this.Map.areaManager.NoRoof)[((_003CMakeNewToils_003Ec__Iterator12)/*Error near IL_0029: stateMachine*/)._003C_003Ef__this.Cell]));
			foreach (Toil item in base.MakeNewToils())
			{
				yield return item;
			}
		}

		protected override void DoEffect()
		{
			JobDriver_RemoveRoof.removedRoofs.Clear();
			base.Map.roofGrid.SetRoof(base.Cell, null);
			JobDriver_RemoveRoof.removedRoofs.Add(base.Cell);
			RoofCollapseCellsFinder.CheckCollapseFlyingRoofs(JobDriver_RemoveRoof.removedRoofs, base.Map, true);
			JobDriver_RemoveRoof.removedRoofs.Clear();
		}

		protected override bool DoWorkFailOn()
		{
			return !base.Cell.Roofed(base.Map);
		}
	}
}
