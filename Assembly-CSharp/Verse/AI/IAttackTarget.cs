using System;

namespace Verse.AI
{
	// Token: 0x02000C26 RID: 3110
	public interface IAttackTarget : ILoadReferenceable
	{
		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x06004462 RID: 17506
		Thing Thing { get; }

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x06004463 RID: 17507
		LocalTargetInfo TargetCurrentlyAimingAt { get; }

		// Token: 0x06004464 RID: 17508
		bool ThreatDisabled(IAttackTargetSearcher disabledFor);
	}
}
