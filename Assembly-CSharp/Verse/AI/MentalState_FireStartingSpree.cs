using System;
using RimWorld;

namespace Verse.AI
{
	public class MentalState_FireStartingSpree : MentalState
	{
		public MentalState_FireStartingSpree()
		{
		}

		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
