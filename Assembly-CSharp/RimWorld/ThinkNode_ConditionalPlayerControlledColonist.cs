using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020001C4 RID: 452
	public class ThinkNode_ConditionalPlayerControlledColonist : ThinkNode_Conditional
	{
		// Token: 0x0600093D RID: 2365 RVA: 0x00056070 File Offset: 0x00054470
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsColonistPlayerControlled;
		}
	}
}
