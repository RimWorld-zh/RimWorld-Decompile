using System;
using RimWorld;

namespace Verse.AI
{
	public class MentalState_PanicFlee : MentalState
	{
		public MentalState_PanicFlee()
		{
		}

		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
