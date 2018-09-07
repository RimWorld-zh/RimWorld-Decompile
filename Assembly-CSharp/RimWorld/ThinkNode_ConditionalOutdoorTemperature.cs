using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalOutdoorTemperature : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalOutdoorTemperature()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Position.UsesOutdoorTemperature(pawn.Map);
		}
	}
}
