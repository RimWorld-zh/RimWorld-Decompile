using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_BuildRoof : JobDriver_AffectRoof
	{
		private static List<IntVec3> builtRoofs = new List<IntVec3>();

		protected override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !((Area)((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0029: stateMachine*/)._0024this.Map.areaManager.BuildRoof)[((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0029: stateMachine*/)._0024this.Cell]);
			this.FailOn(() => !RoofCollapseUtility.WithinRangeOfRoofHolder(((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0041: stateMachine*/)._0024this.Cell, ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0041: stateMachine*/)._0024this.Map));
			this.FailOn(() => !RoofCollapseUtility.ConnectedToRoofHolder(((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0059: stateMachine*/)._0024this.Cell, ((_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_0059: stateMachine*/)._0024this.Map, true));
			using (IEnumerator<Toil> enumerator = base.MakeNewToils().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Toil t = enumerator.Current;
					yield return t;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_0101:
			/*Error near IL_0102: Unexpected return in MoveNext()*/;
		}

		protected override void DoEffect()
		{
			JobDriver_BuildRoof.builtRoofs.Clear();
			for (int i = 0; i < 9; i++)
			{
				IntVec3 intVec = base.Cell + GenAdj.AdjacentCellsAndInside[i];
				if (intVec.InBounds(base.Map) && ((Area)base.Map.areaManager.BuildRoof)[intVec] && !intVec.Roofed(base.Map) && RoofCollapseUtility.WithinRangeOfRoofHolder(intVec, base.Map))
				{
					base.Map.roofGrid.SetRoof(intVec, RoofDefOf.RoofConstructed);
					MoteMaker.PlaceTempRoof(intVec, base.Map);
					JobDriver_BuildRoof.builtRoofs.Add(intVec);
				}
			}
			JobDriver_BuildRoof.builtRoofs.Clear();
		}

		protected override bool DoWorkFailOn()
		{
			return base.Cell.Roofed(base.Map);
		}
	}
}
