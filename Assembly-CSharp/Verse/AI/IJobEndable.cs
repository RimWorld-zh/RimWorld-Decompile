using System;

namespace Verse.AI
{
	// Token: 0x02000A46 RID: 2630
	public interface IJobEndable
	{
		// Token: 0x06003A82 RID: 14978
		Pawn GetActor();

		// Token: 0x06003A83 RID: 14979
		void AddEndCondition(Func<JobCondition> newEndCondition);
	}
}
