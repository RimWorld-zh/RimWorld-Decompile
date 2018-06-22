using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002A8 RID: 680
	public class InteractionWorker
	{
		// Token: 0x06000B64 RID: 2916 RVA: 0x00066D94 File Offset: 0x00065194
		public virtual float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			return 0f;
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x00066DAE File Offset: 0x000651AE
		public virtual void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef)
		{
			letterText = null;
			letterLabel = null;
			letterDef = null;
		}
	}
}
