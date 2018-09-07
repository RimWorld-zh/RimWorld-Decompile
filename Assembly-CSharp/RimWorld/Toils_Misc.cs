using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class Toils_Misc
	{
		public static Toil Learn(SkillDef skill, float xp)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				toil.actor.skills.Learn(skill, xp, false);
			};
			return toil;
		}

		public static Toil SetForbidden(TargetIndex ind, bool forbidden)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				toil.actor.CurJob.GetTarget(ind).Thing.SetForbidden(forbidden, true);
			};
			return toil;
		}

		public static Toil TakeItemFromInventoryToCarrier(Pawn pawn, TargetIndex itemInd)
		{
			return new Toil
			{
				initAction = delegate()
				{
					Job curJob = pawn.CurJob;
					Thing thing = (Thing)curJob.GetTarget(itemInd);
					int count = Mathf.Min(thing.stackCount, curJob.count);
					pawn.inventory.innerContainer.TryTransferToContainer(thing, pawn.carryTracker.innerContainer, count, true);
					curJob.SetTarget(itemInd, pawn.carryTracker.CarriedThing);
				}
			};
		}

		public static Toil ThrowColonistAttackingMote(TargetIndex target)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.CurJob;
				if (actor.playerSettings != null && actor.playerSettings.UsesConfigurableHostilityResponse && !actor.Drafted && !actor.InMentalState && !curJob.playerForced && actor.HostileTo(curJob.GetTarget(target).Thing))
				{
					MoteMaker.MakeColonistActionOverlay(actor, ThingDefOf.Mote_ColonistAttacking);
				}
			};
			return toil;
		}

		public static Toil FindRandomAdjacentReachableCell(TargetIndex adjacentToInd, TargetIndex cellInd)
		{
			Toil findCell = new Toil();
			findCell.initAction = delegate()
			{
				Pawn actor = findCell.actor;
				Job curJob = actor.CurJob;
				LocalTargetInfo target = curJob.GetTarget(adjacentToInd);
				if (target.HasThing && (!target.Thing.Spawned || target.Thing.Map != actor.Map))
				{
					Log.Error(string.Concat(new object[]
					{
						actor,
						" could not find standable cell adjacent to ",
						target,
						" because this thing is either unspawned or spawned somewhere else."
					}), false);
					actor.jobs.curDriver.EndJobWith(JobCondition.Errored);
					return;
				}
				int num = 0;
				IntVec3 c;
				for (;;)
				{
					num++;
					if (num > 100)
					{
						break;
					}
					if (target.HasThing)
					{
						c = target.Thing.RandomAdjacentCell8Way();
					}
					else
					{
						c = target.Cell.RandomAdjacentCell8Way();
					}
					if (c.Standable(actor.Map) && actor.CanReserve(c, 1, -1, null, false) && actor.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						goto Block_7;
					}
				}
				Log.Error(actor + " could not find standable cell adjacent to " + target, false);
				actor.jobs.curDriver.EndJobWith(JobCondition.Errored);
				return;
				Block_7:
				curJob.SetTarget(cellInd, c);
			};
			return findCell;
		}

		[CompilerGenerated]
		private sealed class <Learn>c__AnonStorey0
		{
			internal Toil toil;

			internal SkillDef skill;

			internal float xp;

			public <Learn>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				this.toil.actor.skills.Learn(this.skill, this.xp, false);
			}
		}

		[CompilerGenerated]
		private sealed class <SetForbidden>c__AnonStorey1
		{
			internal Toil toil;

			internal TargetIndex ind;

			internal bool forbidden;

			public <SetForbidden>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				this.toil.actor.CurJob.GetTarget(this.ind).Thing.SetForbidden(this.forbidden, true);
			}
		}

		[CompilerGenerated]
		private sealed class <TakeItemFromInventoryToCarrier>c__AnonStorey2
		{
			internal Pawn pawn;

			internal TargetIndex itemInd;

			public <TakeItemFromInventoryToCarrier>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				Job curJob = this.pawn.CurJob;
				Thing thing = (Thing)curJob.GetTarget(this.itemInd);
				int count = Mathf.Min(thing.stackCount, curJob.count);
				this.pawn.inventory.innerContainer.TryTransferToContainer(thing, this.pawn.carryTracker.innerContainer, count, true);
				curJob.SetTarget(this.itemInd, this.pawn.carryTracker.CarriedThing);
			}
		}

		[CompilerGenerated]
		private sealed class <ThrowColonistAttackingMote>c__AnonStorey3
		{
			internal Toil toil;

			internal TargetIndex target;

			public <ThrowColonistAttackingMote>c__AnonStorey3()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Job curJob = actor.CurJob;
				if (actor.playerSettings != null && actor.playerSettings.UsesConfigurableHostilityResponse && !actor.Drafted && !actor.InMentalState && !curJob.playerForced && actor.HostileTo(curJob.GetTarget(this.target).Thing))
				{
					MoteMaker.MakeColonistActionOverlay(actor, ThingDefOf.Mote_ColonistAttacking);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <FindRandomAdjacentReachableCell>c__AnonStorey4
		{
			internal Toil findCell;

			internal TargetIndex adjacentToInd;

			internal TargetIndex cellInd;

			public <FindRandomAdjacentReachableCell>c__AnonStorey4()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.findCell.actor;
				Job curJob = actor.CurJob;
				LocalTargetInfo target = curJob.GetTarget(this.adjacentToInd);
				if (target.HasThing && (!target.Thing.Spawned || target.Thing.Map != actor.Map))
				{
					Log.Error(string.Concat(new object[]
					{
						actor,
						" could not find standable cell adjacent to ",
						target,
						" because this thing is either unspawned or spawned somewhere else."
					}), false);
					actor.jobs.curDriver.EndJobWith(JobCondition.Errored);
					return;
				}
				int num = 0;
				IntVec3 c;
				for (;;)
				{
					num++;
					if (num > 100)
					{
						break;
					}
					if (target.HasThing)
					{
						c = target.Thing.RandomAdjacentCell8Way();
					}
					else
					{
						c = target.Cell.RandomAdjacentCell8Way();
					}
					if (c.Standable(actor.Map) && actor.CanReserve(c, 1, -1, null, false) && actor.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						goto Block_7;
					}
				}
				Log.Error(actor + " could not find standable cell adjacent to " + target, false);
				actor.jobs.curDriver.EndJobWith(JobCondition.Errored);
				return;
				Block_7:
				curJob.SetTarget(this.cellInd, c);
			}
		}
	}
}
