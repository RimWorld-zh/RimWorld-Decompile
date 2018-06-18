using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020004BE RID: 1214
	public class InteractionWorker_SparkJailbreak : InteractionWorker
	{
		// Token: 0x0600159E RID: 5534 RVA: 0x000C0384 File Offset: 0x000BE784
		public override void Interacted(Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks, out string letterText, out string letterLabel, out LetterDef letterDef)
		{
			if (!recipient.IsPrisoner || !recipient.guest.PrisonerIsSecure || !PrisonBreakUtility.CanParticipateInPrisonBreak(recipient))
			{
				letterText = null;
				letterLabel = null;
				letterDef = null;
			}
			else
			{
				PrisonBreakUtility.StartPrisonBreak(recipient, out letterText, out letterLabel, out letterDef);
				MentalState_Jailbreaker mentalState_Jailbreaker = initiator.MentalState as MentalState_Jailbreaker;
				if (mentalState_Jailbreaker != null)
				{
					mentalState_Jailbreaker.Notify_InducedPrisonerToEscape();
				}
			}
		}
	}
}
