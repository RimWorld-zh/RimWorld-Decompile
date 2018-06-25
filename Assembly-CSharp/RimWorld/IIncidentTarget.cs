using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000318 RID: 792
	public interface IIncidentTarget : ILoadReferenceable
	{
		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000D66 RID: 3430
		int Tile { get; }

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000D67 RID: 3431
		StoryState StoryState { get; }

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000D68 RID: 3432
		GameConditionManager GameConditionManager { get; }

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000D69 RID: 3433
		float PlayerWealthForStoryteller { get; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000D6A RID: 3434
		IEnumerable<Pawn> PlayerPawnsForStoryteller { get; }

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000D6B RID: 3435
		FloatRange IncidentPointsRandomFactorRange { get; }

		// Token: 0x06000D6C RID: 3436
		IEnumerable<IncidentTargetTypeDef> AcceptedTypes();
	}
}
