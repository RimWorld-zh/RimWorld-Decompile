using System;
using RimWorld;

namespace Verse.AI
{
	public class MentalState_WanderPsychotic : MentalState
	{
		public MentalState_WanderPsychotic()
		{
		}

		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
