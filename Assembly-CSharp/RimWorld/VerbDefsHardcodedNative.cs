using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200098F RID: 2447
	public static class VerbDefsHardcodedNative
	{
		// Token: 0x06003709 RID: 14089 RVA: 0x001D6890 File Offset: 0x001D4C90
		public static IEnumerable<VerbProperties> AllVerbDefs()
		{
			VerbProperties d = new VerbProperties();
			d.category = VerbCategory.BeatFire;
			d.label = "Beat fire";
			d.range = 1.42f;
			d.noiseRadius = 3f;
			d.targetParams.canTargetFires = true;
			d.targetParams.canTargetPawns = false;
			d.targetParams.canTargetBuildings = false;
			d.targetParams.mapObjectTargetsMustBeAutoAttackable = false;
			d.warmupTime = 0f;
			d.defaultCooldownTime = 1.1f;
			d.soundCast = SoundDefOf.Interact_BeatFire;
			yield return d;
			d = new VerbProperties();
			d.category = VerbCategory.Ignite;
			d.label = "Ignite";
			d.range = 1.42f;
			d.noiseRadius = 3f;
			d.targetParams.onlyTargetFlammables = true;
			d.targetParams.canTargetBuildings = true;
			d.targetParams.canTargetPawns = false;
			d.targetParams.mapObjectTargetsMustBeAutoAttackable = false;
			d.warmupTime = 3f;
			d.defaultCooldownTime = 1.3f;
			d.soundCast = SoundDefOf.Interact_Ignite;
			yield return d;
			yield break;
		}
	}
}
