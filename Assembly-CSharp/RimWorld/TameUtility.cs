using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public static class TameUtility
	{
		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Pawn, int> <>f__am$cache1;

		public static void ShowDesignationWarnings(Pawn pawn, bool showManhunterOnTameFailWarning = true)
		{
			if (showManhunterOnTameFailWarning)
			{
				float manhunterOnTameFailChance = pawn.RaceProps.manhunterOnTameFailChance;
				if (manhunterOnTameFailChance >= 0.015f)
				{
					string text = "MessageAnimalManhuntsOnTameFailed".Translate(new object[]
					{
						pawn.kindDef.GetLabelPlural(-1).CapitalizeFirst(),
						manhunterOnTameFailChance.ToStringPercent()
					});
					Messages.Message(text, pawn, MessageTypeDefOf.CautionInput, false);
				}
			}
			IEnumerable<Pawn> source = from c in pawn.Map.mapPawns.FreeColonistsSpawned
			where c.workSettings.WorkIsActive(WorkTypeDefOf.Handling)
			select c;
			if (!source.Any<Pawn>())
			{
				source = pawn.Map.mapPawns.FreeColonistsSpawned;
			}
			if (source.Any<Pawn>())
			{
				Pawn pawn2 = source.MaxBy((Pawn c) => c.skills.GetSkill(SkillDefOf.Animals).Level);
				int level = pawn2.skills.GetSkill(SkillDefOf.Animals).Level;
				int num = TrainableUtility.MinimumHandlingSkill(pawn);
				if (num > level)
				{
					string text2 = "MessageNoHandlerSkilledEnough".Translate(new object[]
					{
						pawn.kindDef.label,
						num.ToStringCached(),
						SkillDefOf.Animals.LabelCap,
						pawn2.LabelShort,
						level
					});
					Messages.Message(text2, pawn, MessageTypeDefOf.CautionInput, false);
				}
			}
		}

		public static bool CanTame(Pawn pawn)
		{
			return pawn.AnimalOrWildMan() && pawn.Faction == null && pawn.RaceProps.wildness < 1f && !pawn.IsPrisonerInPrisonCell();
		}

		[CompilerGenerated]
		private static bool <ShowDesignationWarnings>m__0(Pawn c)
		{
			return c.workSettings.WorkIsActive(WorkTypeDefOf.Handling);
		}

		[CompilerGenerated]
		private static int <ShowDesignationWarnings>m__1(Pawn c)
		{
			return c.skills.GetSkill(SkillDefOf.Animals).Level;
		}
	}
}
