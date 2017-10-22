using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_TradeWithPawn : JobDriver
	{
		private Pawn Trader
		{
			get
			{
				return (Pawn)base.TargetThingA;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOn((Func<bool>)(() => !((_003CMakeNewToils_003Ec__Iterator3F)/*Error near IL_0059: stateMachine*/)._003C_003Ef__this.Trader.CanTradeNow));
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					Pawn actor = ((_003CMakeNewToils_003Ec__Iterator3F)/*Error near IL_008c: stateMachine*/)._003Ctrade_003E__0.actor;
					if (((_003CMakeNewToils_003Ec__Iterator3F)/*Error near IL_008c: stateMachine*/)._003C_003Ef__this.Trader.CanTradeNow)
					{
						Find.WindowStack.Add(new Dialog_Trade(actor, ((_003CMakeNewToils_003Ec__Iterator3F)/*Error near IL_008c: stateMachine*/)._003C_003Ef__this.Trader));
					}
				}
			};
		}
	}
}
