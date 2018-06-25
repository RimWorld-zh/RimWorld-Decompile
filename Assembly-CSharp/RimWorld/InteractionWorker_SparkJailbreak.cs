using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020004BC RID: 1212
	public class InteractionWorker_SparkJailbreak : InteractionWorker
	{
		// Token: 0x06001599 RID: 5529 RVA: 0x000C04C0 File Offset: 0x000BE8C0
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
