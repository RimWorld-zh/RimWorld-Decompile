using System;
using RimWorld.Planet;

namespace RimWorld
{
	public static class SleepingMechanoidsSitePartUtility
	{
		public static int GetPawnGroupMakerSeed(SiteCoreOrPartParams parms)
		{
			return parms.randomValue;
		}
	}
}
