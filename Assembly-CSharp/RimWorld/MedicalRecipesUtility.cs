using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000464 RID: 1124
	internal class MedicalRecipesUtility
	{
		// Token: 0x060013C2 RID: 5058 RVA: 0x000ABBD0 File Offset: 0x000A9FD0
		public static bool IsCleanAndDroppable(Pawn pawn, BodyPartRecord part)
		{
			return !pawn.Dead && !pawn.RaceProps.Animal && part.def.spawnThingOnRemoved != null && MedicalRecipesUtility.IsClean(pawn, part);
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x000ABC28 File Offset: 0x000AA028
		public static bool IsClean(Pawn pawn, BodyPartRecord part)
		{
			return !pawn.Dead && !(from x in pawn.health.hediffSet.hediffs
			where x.Part == part
			select x).Any<Hediff>();
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x000ABC85 File Offset: 0x000AA085
		public static void RestorePartAndSpawnAllPreviousParts(Pawn pawn, BodyPartRecord part, IntVec3 pos, Map map)
		{
			MedicalRecipesUtility.SpawnNaturalPartIfClean(pawn, part, pos, map);
			MedicalRecipesUtility.SpawnThingsFromHediffs(pawn, part, pos, map);
			pawn.health.RestorePart(part, null, true);
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x000ABCAC File Offset: 0x000AA0AC
		public static Thing SpawnNaturalPartIfClean(Pawn pawn, BodyPartRecord part, IntVec3 pos, Map map)
		{
			Thing result;
			if (MedicalRecipesUtility.IsCleanAndDroppable(pawn, part))
			{
				result = GenSpawn.Spawn(part.def.spawnThingOnRemoved, pos, map, WipeMode.Vanish);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x000ABCE8 File Offset: 0x000AA0E8
		public static void SpawnThingsFromHediffs(Pawn pawn, BodyPartRecord part, IntVec3 pos, Map map)
		{
			if (pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null).Contains(part))
			{
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
		}
	}
}
