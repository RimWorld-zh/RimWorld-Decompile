using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalPlayerControlledColonist : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalPlayerControlledColonist()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsColonistPlayerControlled;
		}
	}
}
