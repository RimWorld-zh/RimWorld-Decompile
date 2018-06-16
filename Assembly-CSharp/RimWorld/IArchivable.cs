using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020002F5 RID: 757
	public interface IArchivable : IExposable
	{
		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000C9B RID: 3227
		Texture ArchivedIcon { get; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000C9C RID: 3228
		Color ArchivedIconColor { get; }

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000C9D RID: 3229
		string ArchivedLabel { get; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000C9E RID: 3230
		string ArchivedTooltip { get; }

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000C9F RID: 3231
		int CreatedTicksGame { get; }

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000CA0 RID: 3232
		bool CanCullArchivedNow { get; }

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000CA1 RID: 3233
		LookTargets LookTargets { get; }

		// Token: 0x06000CA2 RID: 3234
		void OpenArchived();
	}
}
