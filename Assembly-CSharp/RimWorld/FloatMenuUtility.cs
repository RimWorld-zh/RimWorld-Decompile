using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200083C RID: 2108
	public static class FloatMenuUtility
	{
		// Token: 0x06002FBD RID: 12221 RVA: 0x0019DB30 File Offset: 0x0019BF30
		public static void MakeMenu<T>(IEnumerable<T> objects, Func<T, string> labelGetter, Func<T, Action> actionGetter)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (T t in objects)
			{
				T arg = t;
				list.Add(new FloatMenuOption(labelGetter(arg), actionGetter(arg), MenuOptionPriority.Default, null, null, 0f, null, null));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x06002FBE RID: 12222 RVA: 0x0019DBBC File Offset: 0x0019BFBC
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
				if (primaryVerb.verbProps.IsMeleeAttack)
				{
					result = null;
				}
				else
				{
					if (!pawn.Drafted)
					{
						failStr = "IsNotDraftedLower".Translate(new object[]
						{
							pawn.LabelShort
						});
					}
					else if (!pawn.IsColonistPlayerControlled)
					{
						failStr = "CannotOrderNonControlledLower".Translate();
					}
					else if (target.IsValid && !pawn.equipment.PrimaryEq.PrimaryVerb.CanHitTarget(target))
					{
						if (!pawn.Position.InHorDistOf(target.Cell, primaryVerb.verbProps.range))
						{
							failStr = "OutOfRange".Translate();
						}
						else
						{
							failStr = "CannotHitTarget".Translate();
						}
					}
					else if (pawn.story.WorkTagIsDisabled(WorkTags.Violent))
					{
						failStr = "IsIncapableOfViolenceLower".Translate(new object[]
						{
							pawn.LabelShort
						});
					}
					else
					{
						if (pawn != target.Thing)
						{
							return delegate()
							{
								Job job = new Job(JobDefOf.AttackStatic, target);
								pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
							};
						}
						failStr = "CannotAttackSelf".Translate();
					}
					result = null;
				}
			}
			return result;
		}

		// Token: 0x06002FBF RID: 12223 RVA: 0x0019DD8C File Offset: 0x0019C18C
		public static Action GetMeleeAttackAction(Pawn pawn, LocalTargetInfo target, out string failStr)
		{
			failStr = "";
			if (!pawn.Drafted)
			{
				failStr = "IsNotDraftedLower".Translate(new object[]
				{
					pawn.LabelShort
				});
			}
			else if (!pawn.IsColonistPlayerControlled)
			{
				failStr = "CannotOrderNonControlledLower".Translate();
			}
			else if (target.IsValid && !pawn.CanReach(target, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				failStr = "NoPath".Translate();
			}
			else if (pawn.story.WorkTagIsDisabled(WorkTags.Violent))
			{
				failStr = "IsIncapableOfViolenceLower".Translate(new object[]
				{
					pawn.LabelShort
				});
			}
			else if (pawn.meleeVerbs.TryGetMeleeVerb(target.Thing) == null)
			{
				failStr = "Incapable".Translate();
			}
			else
			{
				if (pawn != target.Thing)
				{
					return delegate()
					{
						Job job = new Job(JobDefOf.AttackMelee, target);
						Pawn pawn2 = target.Thing as Pawn;
						if (pawn2 != null)
						{
							job.killIncappedTarget = pawn2.Downed;
						}
						pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
					};
				}
				failStr = "CannotAttackSelf".Translate();
			}
			return null;
		}

		// Token: 0x06002FC0 RID: 12224 RVA: 0x0019DEFC File Offset: 0x0019C2FC
		public static Action GetAttackAction(Pawn pawn, LocalTargetInfo target, out string failStr)
		{
			Action result;
			if (pawn.equipment.Primary != null && !pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.IsMeleeAttack)
			{
				result = FloatMenuUtility.GetRangedAttackAction(pawn, target, out failStr);
			}
			else
			{
				result = FloatMenuUtility.GetMeleeAttackAction(pawn, target, out failStr);
			}
			return result;
		}

		// Token: 0x06002FC1 RID: 12225 RVA: 0x0019DF58 File Offset: 0x0019C358
		public static FloatMenuOption DecoratePrioritizedTask(FloatMenuOption option, Pawn pawn, LocalTargetInfo target, string reservedText = "ReservedBy")
		{
			FloatMenuOption result;
			if (option.action == null)
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
						option.Label = option.Label + " (" + reservedText.Translate(new object[]
						{
							pawn2.LabelShort
						}) + ")";
					}
				}
				if (option.revalidateClickTarget != null && option.revalidateClickTarget != target.Thing)
				{
					Log.ErrorOnce(string.Format("Click target mismatch; {0} vs {1} in {2}", option.revalidateClickTarget, target.Thing, option.Label), 52753118, false);
				}
				option.revalidateClickTarget = target.Thing;
				result = option;
			}
			return result;
		}
	}
}
