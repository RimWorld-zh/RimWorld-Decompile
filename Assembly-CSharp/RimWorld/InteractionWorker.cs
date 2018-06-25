using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002AA RID: 682
	public class InteractionWorker
	{
		// Token: 0x06000B67 RID: 2919 RVA: 0x00066EE0 File Offset: 0x000652E0
		public virtual float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0f;
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x00066EFA File Offset: 0x000652FA
		public virtual void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef)
		{
			letterText = null;
			letterLabel = null;
			letterDef = null;
		}
	}
}
