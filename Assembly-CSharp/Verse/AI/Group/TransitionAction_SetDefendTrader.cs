using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A04 RID: 2564
	public class TransitionAction_SetDefendTrader : TransitionAction
	{
		// Token: 0x06003980 RID: 14720 RVA: 0x001E7C00 File Offset: 0x001E6000
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
	}
}
