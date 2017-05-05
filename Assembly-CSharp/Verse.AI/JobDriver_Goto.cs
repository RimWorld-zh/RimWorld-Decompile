using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Verse.AI
{
	public class JobDriver_Goto : JobDriver
	{
		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_Goto.<MakeNewToils>c__Iterator1AE <MakeNewToils>c__Iterator1AE = new JobDriver_Goto.<MakeNewToils>c__Iterator1AE();
			<MakeNewToils>c__Iterator1AE.<>f__this = this;
			JobDriver_Goto.<MakeNewToils>c__Iterator1AE expr_0E = <MakeNewToils>c__Iterator1AE;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		private void TryExitMap()
		{
			if (base.CurJob.failIfCantJoinOrCreateCaravan && !CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(this.pawn))
			{
				return;
			}
			this.pawn.ExitMap(true);
		}
	}
}
