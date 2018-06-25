using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000E0 RID: 224
	public class JobGiver_ExtinguishSelf : ThinkNode_JobGiver
	{
		// Token: 0x040002B6 RID: 694
		private const float ActivateChance = 0.1f;

		// Token: 0x060004E3 RID: 1251 RVA: 0x00036708 File Offset: 0x00034B08
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (Rand.Value < 0.1f)
			{
				Fire fire = (Fire)pawn.GetAttachment(ThingDefOf.Fire);
				if (fire != null)
				{
					return new Job(JobDefOf.ExtinguishSelf, fire);
				}
			}
			return null;
		}
	}
}
