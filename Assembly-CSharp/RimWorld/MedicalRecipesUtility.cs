using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class MedicalRecipesUtility
	{
		public MedicalRecipesUtility()
		{
		}

		public static bool IsCleanAndDroppable(Pawn pawn, BodyPartRecord part)
		{
			return !pawn.Dead && !pawn.RaceProps.Animal && part.def.spawnThingOnRemoved != null && MedicalRecipesUtility.IsClean(pawn, part);
		}

		public static bool IsClean(Pawn pawn, BodyPartRecord part)
		{
			return !pawn.Dead && !(from x in pawn.health.hediffSet.hediffs
			where x.Part == part
			select x).Any<Hediff>();
		}

		public static void RestorePartAndSpawnAllPreviousParts(Pawn pawn, BodyPartRecord part, IntVec3 pos, Map map)
		{
			MedicalRecipesUtility.SpawnNaturalPartIfClean(pawn, part, pos, map);
			MedicalRecipesUtility.SpawnThingsFromHediffs(pawn, part, pos, map);
			pawn.health.RestorePart(part, null, true);
		}

		public static Thing SpawnNaturalPartIfClean(Pawn pawn, BodyPartRecord part, IntVec3 pos, Map map)
		{
			if (MedicalRecipesUtility.IsCleanAndDroppable(pawn, part))
			{
				return GenSpawn.Spawn(part.def.spawnThingOnRemoved, pos, map, WipeMode.Vanish);
			}
			return null;
		}

		public static void SpawnThingsFromHediffs(Pawn pawn, BodyPartRecord part, IntVec3 pos, Map map)
		{
			if (!pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Contains(part))
			{
				return;
			}
			IEnumerable<Hediff> enumerable = from x in pawn.health.hediffSet.hediffs
			where x.Part == part
			select x;
			foreach (Hediff hediff in enumerable)
			{
				if (hediff.def.spawnThingOnRemoved != null)
				{
					GenSpawn.Spawn(hediff.def.spawnThingOnRemoved, pos, map, WipeMode.Vanish);
				}
			}
			for (int i = 0; i < part.parts.Count; i++)
			{
				MedicalRecipesUtility.SpawnThingsFromHediffs(pawn, part.parts[i], pos, map);
			}
		}

		[CompilerGenerated]
		private sealed class <IsClean>c__AnonStorey0
		{
			internal BodyPartRecord part;

			public <IsClean>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Hediff x)
			{
				return x.Part == this.part;
			}
		}

		[CompilerGenerated]
		private sealed class <SpawnThingsFromHediffs>c__AnonStorey1
		{
			internal BodyPartRecord part;

			public <SpawnThingsFromHediffs>c__AnonStorey1()
			{
			}

			internal bool <>m__0(Hediff x)
			{
				return x.Part == this.part;
			}
		}
	}
}
