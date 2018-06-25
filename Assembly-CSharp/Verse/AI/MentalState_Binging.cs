using System;
using RimWorld;

namespace Verse.AI
{
	public class MentalState_Binging : MentalState
	{
		public MentalState_Binging()
		{
		}

		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
