using System;
using Verse;

namespace RimWorld
{
	public static class PlayerPawnsArriveMethodExtension
	{
		public static string ToStringHuman(this PlayerPawnsArriveMethod method)
		{
			string result;
			if (method != PlayerPawnsArriveMethod.Standing)
			{
				if (method != PlayerPawnsArriveMethod.DropPods)
				{
					throw new NotImplementedException();
				}
				result = "PlayerPawnsArriveMethod_DropPods".Translate();
			}
			else
			{
				result = "PlayerPawnsArriveMethod_Standing".Translate();
			}
			return result;
		}
	}
}
