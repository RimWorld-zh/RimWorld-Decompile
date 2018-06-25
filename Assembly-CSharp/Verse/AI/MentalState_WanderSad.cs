using System;
using RimWorld;

namespace Verse.AI
{
	public class MentalState_WanderSad : MentalState
	{
		public MentalState_WanderSad()
		{
		}

		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
