using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public static class VerbDefsHardcodedNative
	{
		public static IEnumerable<VerbProperties> AllVerbDefs()
		{
			yield return new VerbProperties
			{
				category = VerbCategory.BeatFire,
				label = "Beat fire",
				range = 1f,
				noiseRadius = 3f,
				targetParams = 
				{
					canTargetFires = true,
					canTargetPawns = false,
					canTargetBuildings = false,
					mapObjectTargetsMustBeAutoAttackable = false
				},
				warmupTime = 0f,
				defaultCooldownTime = 1.1f,
				soundCast = SoundDef.Named("Interact_BeatFire")
			};
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
