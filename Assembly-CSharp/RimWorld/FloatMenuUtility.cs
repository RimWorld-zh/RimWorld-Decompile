using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class FloatMenuUtility
	{
		public static void MakeMenu<T>(IEnumerable<T> objects, Func<T, string> labelGetter, Func<T, Action> actionGetter)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (T @object in objects)
			{
				T arg = @object;
				list.Add(new FloatMenuOption(labelGetter(arg), actionGetter(arg), MenuOptionPriority.Default, null, null, 0f, null, null));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		public static Action GetRangedAttackAction(Pawn pawn, LocalTargetInfo target, out string failStr)
		{
			failStr = string.Empty;
			if (pawn.equipment.Primary == null)
			{
				return null;
			}
			Verb primaryVerb = pawn.equipment.PrimaryEq.PrimaryVerb;
			if (primaryVerb.verbProps.MeleeRange)
			{
				return null;
			}
			if (!pawn.Drafted)
			{
				failStr = "IsNotDraftedLower".Translate(pawn.NameStringShort);
				goto IL_01a4;
			}
			if (!pawn.IsColonistPlayerControlled)
			{
				failStr = "CannotOrderNonControlledLower".Translate();
				goto IL_01a4;
			}
			if (target.IsValid && !pawn.equipment.PrimaryEq.PrimaryVerb.CanHitTarget(target))
			{
				if (!pawn.Position.InHorDistOf(target.Cell, primaryVerb.verbProps.range))
				{
					failStr = "OutOfRange".Translate();
				}
				else
				{
					failStr = "CannotHitTarget".Translate();
				}
				goto IL_01a4;
			}
			if (pawn.story.WorkTagIsDisabled(WorkTags.Violent))
			{
				failStr = "IsIncapableOfViolenceLower".Translate(pawn.NameStringShort);
				goto IL_01a4;
			}
			if (pawn == target.Thing)
			{
				failStr = "CannotAttackSelf".Translate();
				goto IL_01a4;
			}
			return delegate
			{
				Job job = new Job(JobDefOf.AttackStatic, target);
				pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			};
			IL_01a4:
			return null;
		}

		public static Action GetMeleeAttackAction(Pawn pawn, LocalTargetInfo target, out string failStr)
		{
			failStr = string.Empty;
			if (!pawn.Drafted)
			{
				failStr = "IsNotDraftedLower".Translate(pawn.NameStringShort);
				goto IL_0141;
			}
			if (!pawn.IsColonistPlayerControlled)
			{
				failStr = "CannotOrderNonControlledLower".Translate();
				goto IL_0141;
			}
			if (target.IsValid && !pawn.CanReach(target, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				failStr = "NoPath".Translate();
				goto IL_0141;
			}
			if (pawn.story.WorkTagIsDisabled(WorkTags.Violent))
			{
				failStr = "IsIncapableOfViolenceLower".Translate(pawn.NameStringShort);
				goto IL_0141;
			}
			if (pawn.meleeVerbs.TryGetMeleeVerb() == null)
			{
				failStr = "Incapable".Translate();
				goto IL_0141;
			}
			if (pawn == target.Thing)
			{
				failStr = "CannotAttackSelf".Translate();
				goto IL_0141;
			}
			return delegate
			{
				Job job = new Job(JobDefOf.AttackMelee, target);
				Pawn pawn2 = target.Thing as Pawn;
				if (pawn2 != null)
				{
					job.killIncappedTarget = pawn2.Downed;
				}
				pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			};
			IL_0141:
			return null;
		}

		public static Action GetAttackAction(Pawn pawn, LocalTargetInfo target, out string failStr)
		{
			if (pawn.equipment.Primary != null && !pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.MeleeRange)
			{
				return FloatMenuUtility.GetRangedAttackAction(pawn, target, out failStr);
			}
			return FloatMenuUtility.GetMeleeAttackAction(pawn, target, out failStr);
		}

		public static FloatMenuOption DecoratePrioritizedTask(FloatMenuOption option, Pawn pawn, LocalTargetInfo target, string reservedText = "ReservedBy")
		{
			if (option.action == null)
			{
				return option;
			}
			if (pawn != null && !pawn.CanReserve(target, 1, -1, null, false) && pawn.CanReserve(target, 1, -1, null, true))
			{
				Pawn pawn2 = pawn.Map.reservationManager.FirstRespectedReserver(target, pawn);
				if (pawn2 == null)
				{
					pawn2 = pawn.Map.physicalInteractionReservationManager.FirstReserverOf(target);
				}
				if (pawn2 != null)
				{
					option.Label = option.Label + " (" + reservedText.Translate(pawn2.LabelShort) + ")";
				}
			}
			return option;
		}
	}
}
