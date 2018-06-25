using System;

namespace Verse.AI
{
	// Token: 0x02000C28 RID: 3112
	public interface IAttackTarget : ILoadReferenceable
	{
		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x06004465 RID: 17509
		Thing Thing { get; }

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x06004466 RID: 17510
		LocalTargetInfo TargetCurrentlyAimingAt { get; }

		// Token: 0x06004467 RID: 17511
		bool ThreatDisabled(IAttackTargetSearcher disabledFor);
	}
}
