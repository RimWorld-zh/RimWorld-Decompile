using System;
using RimWorld;

namespace Verse.AI
{
	public class MentalState_GiveUpExit : MentalState
	{
		public MentalState_GiveUpExit()
		{
		}

		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
