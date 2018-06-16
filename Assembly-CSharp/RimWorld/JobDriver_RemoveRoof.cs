using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000046 RID: 70
	public class JobDriver_RemoveRoof : JobDriver_AffectRoof
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000248 RID: 584 RVA: 0x00017F54 File Offset: 0x00016354
		protected override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00017F6C File Offset: 0x0001636C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !base.Map.areaManager.NoRoof[base.Cell]);
			foreach (Toil t in this.<MakeNewToils>__BaseCallProxy0())
			{
				yield return t;
			}
			yield break;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00017F98 File Offset: 0x00016398
		protected override void DoEffect()
		{
			JobDriver_RemoveRoof.removedRoofs.Clear();
			base.Map.roofGrid.SetRoof(base.Cell, null);
			JobDriver_RemoveRoof.removedRoofs.Add(base.Cell);
			RoofCollapseCellsFinder.CheckCollapseFlyingRoofs(JobDriver_RemoveRoof.removedRoofs, base.Map, true);
			JobDriver_RemoveRoof.removedRoofs.Clear();
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00017FF4 File Offset: 0x000163F4
		protected override bool DoWorkFailOn()
		{
			return !base.Cell.Roofed(base.Map);
		}

		// Token: 0x040001D8 RID: 472
		private static List<IntVec3> removedRoofs = new List<IntVec3>();
	}
}
