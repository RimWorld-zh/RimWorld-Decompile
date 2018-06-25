using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002AA RID: 682
	public class InteractionWorker
	{
		// Token: 0x06000B68 RID: 2920 RVA: 0x00066EE4 File Offset: 0x000652E4
		public virtual float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0f;
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x00066EFE File Offset: 0x000652FE
		public virtual void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef)
		{
			letterText = null;
			letterLabel = null;
			letterDef = null;
		}
	}
}
