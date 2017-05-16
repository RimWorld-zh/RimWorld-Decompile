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
			JobDriver_Goto.<MakeNewToils>c__Iterator1B2 <MakeNewToils>c__Iterator1B = new JobDriver_Goto.<MakeNewToils>c__Iterator1B2();
			<MakeNewToils>c__Iterator1B.<>f__this = this;
			JobDriver_Goto.<MakeNewToils>c__Iterator1B2 expr_0E = <MakeNewToils>c__Iterator1B;
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
