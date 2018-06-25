using System;

namespace Verse.AI
{
	// Token: 0x02000A45 RID: 2629
	public interface IJobEndable
	{
		// Token: 0x06003A81 RID: 14977
		Pawn GetActor();

		// Token: 0x06003A82 RID: 14978
		void AddEndCondition(Func<JobCondition> newEndCondition);
	}
}
