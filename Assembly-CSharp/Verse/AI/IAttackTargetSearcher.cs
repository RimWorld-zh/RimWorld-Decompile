using System;

namespace Verse.AI
{
	// Token: 0x02000ADD RID: 2781
	public interface IAttackTargetSearcher
	{
		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x06003DA1 RID: 15777
		Thing Thing { get; }

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x06003DA2 RID: 15778
		Verb CurrentEffectiveVerb { get; }

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x06003DA3 RID: 15779
		LocalTargetInfo LastAttackedTarget { get; }

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x06003DA4 RID: 15780
		int LastAttackTargetTick { get; }
	}
}
