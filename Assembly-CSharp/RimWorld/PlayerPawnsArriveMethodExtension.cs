using System;
using Verse;

namespace RimWorld
{
	public static class PlayerPawnsArriveMethodExtension
	{
		public static string ToStringHuman(this PlayerPawnsArriveMethod method)
		{
			string result;
			switch (method)
			{
			case PlayerPawnsArriveMethod.Standing:
			{
				result = "PlayerPawnsArriveMethod_Standing".Translate();
				break;
			}
			case PlayerPawnsArriveMethod.DropPods:
			{
				result = "PlayerPawnsArriveMethod_DropPods".Translate();
				break;
			}
			default:
			{
				throw new NotImplementedException();
			}
			}
			return result;
		}
	}
}
