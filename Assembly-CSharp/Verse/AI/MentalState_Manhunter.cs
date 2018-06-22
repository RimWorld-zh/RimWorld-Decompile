using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A79 RID: 2681
	public class MentalState_Manhunter : MentalState
	{
		// Token: 0x06003B9B RID: 15259 RVA: 0x001F7F32 File Offset: 0x001F6332
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.AnimalsDontAttackDoors, OpportunityType.Critical);
		}

		// Token: 0x06003B9C RID: 15260 RVA: 0x001F7F48 File Offset: 0x001F6348
		public override bool ForceHostileTo(Thing t)
		{
			return t.Faction != null && this.ForceHostileTo(t.Faction);
		}

		// Token: 0x06003B9D RID: 15261 RVA: 0x001F7F78 File Offset: 0x001F6378
		public override bool ForceHostileTo(Faction f)
		{
			return true;
		}

		// Token: 0x06003B9E RID: 15262 RVA: 0x001F7F90 File Offset: 0x001F6390
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
