using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalDangerousTemperature : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalDangerousTemperature()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.SafeTemperatureRange().Includes(pawn.AmbientTemperature);
		}
	}
}
