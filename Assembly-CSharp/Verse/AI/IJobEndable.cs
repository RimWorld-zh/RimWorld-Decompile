using System;

namespace Verse.AI
{
	// Token: 0x02000A46 RID: 2630
	public interface IJobEndable
	{
		// Token: 0x06003A80 RID: 14976
		Pawn GetActor();

		// Token: 0x06003A81 RID: 14977
		void AddEndCondition(Func<JobCondition> newEndCondition);
	}
}
