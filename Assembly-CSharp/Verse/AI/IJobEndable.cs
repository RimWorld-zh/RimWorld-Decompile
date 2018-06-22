using System;

namespace Verse.AI
{
	// Token: 0x02000A42 RID: 2626
	public interface IJobEndable
	{
		// Token: 0x06003A7C RID: 14972
		Pawn GetActor();

		// Token: 0x06003A7D RID: 14973
		void AddEndCondition(Func<JobCondition> newEndCondition);
	}
}
