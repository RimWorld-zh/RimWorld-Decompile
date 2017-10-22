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
			foreach (T item in objects)
			{
				T _ = item;
				list.Add(new FloatMenuOption(labelGetter(item), actionGetter(item), MenuOptionPriority.Default, null, null, 0f, null, null));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		public static Action GetRangedAttackAction(Pawn pawn, LocalTargetInfo target, out string failStr)
		{
			failStr = "";
			Action result;
			if (pawn.equipment.Primary == null)
			{
				result = null;
			}
			else
			{
				Verb primaryVerb = pawn.equipment.PrimaryEq.PrimaryVerb;
				if (primaryVerb.verbProps.MeleeRange)
				{
					result = null;
				}
				else
				{
					if (!pawn.Drafted)
					{
						failStr = "IsNotDraftedLower".Translate(pawn.NameStringShort);
						goto IL_01bb;
					}
					if (!pawn.IsColonistPlayerControlled)
					{
						failStr = "CannotOrderNonControlledLower".Translate();
						goto IL_01bb;
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
						goto IL_01bb;
					}
					if (pawn.story.WorkTagIsDisabled(WorkTags.Violent))
					{
						failStr = "IsIncapableOfViolenceLower".Translate(pawn.NameStringShort);
						goto IL_01bb;
					}
					if (pawn == target.Thing)
					{
						failStr = "CannotAttackSelf".Translate();
						goto IL_01bb;
					}
					result = (Action)delegate()
					{
						Job job = new Job(JobDefOf.AttackStatic, target);
						pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
					};
				}
			}
			goto IL_01c2;
			IL_01bb:
			result = null;
			goto IL_01c2;
			IL_01c2:
			return result;
		}

		public static Action GetMeleeAttackAction(Pawn pawn, LocalTargetInfo target, out string failStr)
		{
			failStr = "";
			if (!pawn.Drafted)
			{
				failStr = "IsNotDraftedLower".Translate(pawn.NameStringShort);
				goto IL_0150;
			}
			if (!pawn.IsColonistPlayerControlled)
			{
				failStr = "CannotOrderNonControlledLower".Translate();
				goto IL_0150;
			}
			if (target.IsValid && !pawn.CanReach(target, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				failStr = "NoPath".Translate();
				goto IL_0150;
			}
			if (pawn.story.WorkTagIsDisabled(WorkTags.Violent))
			{
				failStr = "IsIncapableOfViolenceLower".Translate(pawn.NameStringShort);
				goto IL_0150;
			}
			if (pawn.meleeVerbs.TryGetMeleeVerb() == null)
			{
				failStr = "Incapable".Translate();
				goto IL_0150;
			}
			if (pawn == target.Thing)
			{
				failStr = "CannotAttackSelf".Translate();
				goto IL_0150;
			}
			Action result = (Action)delegate()
			{
				Job job = new Job(JobDefOf.AttackMelee, target);
				Pawn pawn2 = target.Thing as Pawn;
				if (pawn2 != null)
				{
					job.killIncappedTarget = pawn2.Downed;
				}
				pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			};
			goto IL_0157;
			IL_0157:
			return result;
			IL_0150:
			result = null;
			goto IL_0157;
		}

		public static Action GetAttackAction(Pawn pawn, LocalTargetInfo target, out string failStr)
		{
			return (pawn.equipment.Primary == null || pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.MeleeRange) ? FloatMenuUtility.GetMeleeAttackAction(pawn, target, out failStr) : FloatMenuUtility.GetRangedAttackAction(pawn, target, out failStr);
		}

		public static FloatMenuOption DecoratePrioritizedTask(FloatMenuOption option, Pawn pawn, LocalTargetInfo target, string reservedText = "ReservedBy")
		{
			FloatMenuOption result;
			if ((object)option.action == null)
			{
				result = option;
			}
			else
			{
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
				result = option;
			}
			return result;
		}
	}
}
