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
			yield return new VerbProperties
			{
				category = VerbCategory.Ignite,
				label = "Ignite",
				range = 1f,
				noiseRadius = 3f,
				targetParams = 
				{
					onlyTargetFlammables = true,
					canTargetBuildings = true,
					canTargetPawns = false,
					mapObjectTargetsMustBeAutoAttackable = false
				},
				warmupTime = 3f,
				defaultCooldownTime = 1.3f,
				soundCast = SoundDef.Named("Interact_Ignite")
			};
		}
	}
}
