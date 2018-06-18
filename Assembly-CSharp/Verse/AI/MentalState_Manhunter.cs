using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A7D RID: 2685
	public class MentalState_Manhunter : MentalState
	{
		// Token: 0x06003BA0 RID: 15264 RVA: 0x001F7C1E File Offset: 0x001F601E
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.AnimalsDontAttackDoors, OpportunityType.Critical);
		}

		// Token: 0x06003BA1 RID: 15265 RVA: 0x001F7C34 File Offset: 0x001F6034
		public override bool ForceHostileTo(Thing t)
		{
			return t.Faction != null && this.ForceHostileTo(t.Faction);
		}

		// Token: 0x06003BA2 RID: 15266 RVA: 0x001F7C64 File Offset: 0x001F6064
		public override bool ForceHostileTo(Faction f)
		{
			return true;
		}

		// Token: 0x06003BA3 RID: 15267 RVA: 0x001F7C7C File Offset: 0x001F607C
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
