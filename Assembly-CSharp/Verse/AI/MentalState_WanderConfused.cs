using System;
using RimWorld;

namespace Verse.AI
{
	public class MentalState_WanderConfused : MentalState
	{
		public MentalState_WanderConfused()
		{
		}

		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}
	}
}
