using System;

namespace Verse.AI
{
	// Token: 0x02000C29 RID: 3113
	public interface IAttackTarget : ILoadReferenceable
	{
		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x06004459 RID: 17497
		Thing Thing { get; }

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x0600445A RID: 17498
		LocalTargetInfo TargetCurrentlyAimingAt { get; }

		// Token: 0x0600445B RID: 17499
		bool ThreatDisabled(IAttackTargetSearcher disabledFor);
	}
}
