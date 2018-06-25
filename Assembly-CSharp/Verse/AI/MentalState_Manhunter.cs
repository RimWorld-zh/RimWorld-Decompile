using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A7B RID: 2683
	public class MentalState_Manhunter : MentalState
	{
		// Token: 0x06003B9F RID: 15263 RVA: 0x001F805E File Offset: 0x001F645E
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.AnimalsDontAttackDoors, OpportunityType.Critical);
		}

		// Token: 0x06003BA0 RID: 15264 RVA: 0x001F8074 File Offset: 0x001F6474
		public override bool ForceHostileTo(Thing t)
		{
			return t.Faction != null && this.ForceHostileTo(t.Faction);
		}

		// Token: 0x06003BA1 RID: 15265 RVA: 0x001F80A4 File Offset: 0x001F64A4
		public override bool ForceHostileTo(Faction f)
		{
			return true;
		}

		// Token: 0x06003BA2 RID: 15266 RVA: 0x001F80BC File Offset: 0x001F64BC
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
