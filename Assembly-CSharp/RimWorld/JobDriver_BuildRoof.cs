using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000040 RID: 64
	public class JobDriver_BuildRoof : JobDriver_AffectRoof
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000224 RID: 548 RVA: 0x00016830 File Offset: 0x00014C30
		protected override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00016848 File Offset: 0x00014C48
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !base.Map.areaManager.BuildRoof[base.Cell]);
			this.FailOn(() => !RoofCollapseUtility.WithinRangeOfRoofHolder(base.Cell, base.Map, false));
			this.FailOn(() => !RoofCollapseUtility.ConnectedToRoofHolder(base.Cell, base.Map, true));
			foreach (Toil t in this.<MakeNewToils>__BaseCallProxy0())
			{
				yield return t;
			}
			yield break;
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00016874 File Offset: 0x00014C74
		protected override void DoEffect()
		{
			JobDriver_BuildRoof.builtRoofs.Clear();
			for (int i = 0; i < 9; i++)
			{
				IntVec3 intVec = base.Cell + GenAdj.AdjacentCellsAndInside[i];
				if (intVec.InBounds(base.Map))
				{
					if (base.Map.areaManager.BuildRoof[intVec] && !intVec.Roofed(base.Map) && RoofCollapseUtility.WithinRangeOfRoofHolder(intVec, base.Map, false) && RoofUtility.FirstBlockingThing(intVec, base.Map) == null)
					{
						base.Map.roofGrid.SetRoof(intVec, RoofDefOf.RoofConstructed);
						MoteMaker.PlaceTempRoof(intVec, base.Map);
						JobDriver_BuildRoof.builtRoofs.Add(intVec);
					}
				}
			}
			JobDriver_BuildRoof.builtRoofs.Clear();
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0001695C File Offset: 0x00014D5C
		protected override bool DoWorkFailOn()
		{
			return base.Cell.Roofed(base.Map);
		}

		// Token: 0x040001D0 RID: 464
		private static List<IntVec3> builtRoofs = new List<IntVec3>();
	}
}
