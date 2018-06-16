using System;

namespace Verse.AI
{
	// Token: 0x02000C2A RID: 3114
	public interface IAttackTarget : ILoadReferenceable
	{
		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x0600445B RID: 17499
		Thing Thing { get; }

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x0600445C RID: 17500
		LocalTargetInfo TargetCurrentlyAimingAt { get; }

		// Token: 0x0600445D RID: 17501
		bool ThreatDisabled(IAttackTargetSearcher disabledFor);
	}
}
