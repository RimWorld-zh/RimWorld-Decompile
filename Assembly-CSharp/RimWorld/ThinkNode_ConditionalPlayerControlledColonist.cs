using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001C4 RID: 452
	public class ThinkNode_ConditionalPlayerControlledColonist : ThinkNode_Conditional
	{
		// Token: 0x0600093C RID: 2364 RVA: 0x0005606C File Offset: 0x0005446C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsColonistPlayerControlled;
		}
	}
}
