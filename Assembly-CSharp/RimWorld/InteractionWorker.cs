using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002A8 RID: 680
	public class InteractionWorker
	{
		// Token: 0x06000B66 RID: 2918 RVA: 0x00066D2C File Offset: 0x0006512C
		public virtual float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0f;
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x00066D46 File Offset: 0x00065146
		public virtual void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef)
		{
			letterText = null;
			letterLabel = null;
			letterDef = null;
		}
	}
}
