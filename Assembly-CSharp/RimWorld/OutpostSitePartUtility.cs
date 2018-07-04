using System;
using RimWorld.Planet;

namespace RimWorld
{
	public static class OutpostSitePartUtility
	{
		public static int GetPawnGroupMakerSeed(SiteCoreOrPartParams parms)
		{
			return parms.randomValue;
		}
	}
}
