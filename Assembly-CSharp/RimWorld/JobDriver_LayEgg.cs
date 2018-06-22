using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200002D RID: 45
	public class JobDriver_LayEgg : JobDriver
	{
		// Token: 0x060001AE RID: 430 RVA: 0x000120C0 File Offset: 0x000104C0
		public override bool TryMakePreToilReservations()
		{
			return true;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x000120D8 File Offset: 0x000104D8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			yield return Toils_General.Wait(500);
			yield return Toils_General.Do(delegate
			{
				Thing forbiddenIfOutsideHomeArea = GenSpawn.Spawn(this.pawn.GetComp<CompEggLayer>().ProduceEgg(), this.pawn.Position, base.Map, WipeMode.Vanish);
				forbiddenIfOutsideHomeArea.SetForbiddenIfOutsideHomeArea();
			});
			yield break;
		}

		// Token: 0x040001AB RID: 427
		private const int LayEgg = 500;

		// Token: 0x040001AC RID: 428
		private const TargetIndex LaySpotInd = TargetIndex.A;
	}
}
