using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI.Group
{
	public class TransitionAction_SetDefendTrader : TransitionAction
	{
		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

		public TransitionAction_SetDefendTrader()
		{
		}

		public override void DoAction(Transition trans)
		{
			LordToil_DefendPoint lordToil_DefendPoint = (LordToil_DefendPoint)trans.target;
			Pawn pawn = TraderCaravanUtility.FindTrader(lordToil_DefendPoint.lord);
			if (pawn != null)
			{
				lordToil_DefendPoint.SetDefendPoint(pawn.Position);
			}
			else
			{
				IEnumerable<Pawn> source = from x in lordToil_DefendPoint.lord.ownedPawns
				where x.GetTraderCaravanRole() == TraderCaravanRole.Carrier
				select x;
				if (source.Any<Pawn>())
				{
					lordToil_DefendPoint.SetDefendPoint(source.RandomElement<Pawn>().Position);
				}
				else
				{
					lordToil_DefendPoint.SetDefendPoint(lordToil_DefendPoint.lord.ownedPawns.RandomElement<Pawn>().Position);
				}
			}
		}

		[CompilerGenerated]
		private static bool <DoAction>m__0(Pawn x)
		{
			return x.GetTraderCaravanRole() == TraderCaravanRole.Carrier;
		}
	}
}
