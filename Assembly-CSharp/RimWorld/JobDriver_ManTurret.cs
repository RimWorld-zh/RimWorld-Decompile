using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	public class JobDriver_ManTurret : JobDriver
	{
		private const float ShellSearchRadius = 40f;

		private const int MaxPawnAmmoReservations = 10;

		private static bool GunNeedsLoading(Building b)
		{
			Building_TurretGun building_TurretGun = b as Building_TurretGun;
			if (building_TurretGun == null)
			{
				return false;
			}
			if (building_TurretGun.def.building.turretShellDef != null && !building_TurretGun.loaded)
			{
				return true;
			}
			return false;
		}

		public static Thing FindAmmoForTurret(Pawn pawn, Thing gun)
		{
			Predicate<Thing> validator = (Predicate<Thing>)delegate(Thing t)
			{
				if (t.IsForbidden(pawn))
				{
					return false;
				}
				if (!pawn.CanReserve(t, 10, 1, null, false))
				{
					return false;
				}
				return true;
			};
			return GenClosest.ClosestThingReachable(gun.Position, gun.Map, ThingRequest.ForDef(gun.def.building.turretShellDef), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 40f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			Toil gotoTurret = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					Pawn actor3 = ((_003CMakeNewToils_003Ec__Iterator34)/*Error near IL_0087: stateMachine*/)._003CloadIfNeeded_003E__1.actor;
					Building building3 = (Building)actor3.CurJob.targetA.Thing;
					Building_TurretGun building_TurretGun2 = building3 as Building_TurretGun;
					if (!JobDriver_ManTurret.GunNeedsLoading(building3))
					{
						((_003CMakeNewToils_003Ec__Iterator34)/*Error near IL_0087: stateMachine*/)._003C_003Ef__this.JumpToToil(((_003CMakeNewToils_003Ec__Iterator34)/*Error near IL_0087: stateMachine*/)._003CgotoTurret_003E__0);
					}
					else
					{
						Thing thing = JobDriver_ManTurret.FindAmmoForTurret(((_003CMakeNewToils_003Ec__Iterator34)/*Error near IL_0087: stateMachine*/)._003C_003Ef__this.pawn, building_TurretGun2);
						if (thing == null)
						{
							if (actor3.Faction == Faction.OfPlayer)
							{
								Messages.Message("MessageOutOfNearbyShellsFor".Translate(actor3.LabelShort, building_TurretGun2.Label).CapitalizeFirst(), (Thing)building_TurretGun2, MessageSound.Negative);
							}
							actor3.jobs.EndCurrentJob(JobCondition.Incompletable, true);
						}
						actor3.CurJob.targetB = thing;
						actor3.CurJob.count = 1;
					}
				}
			};
			yield return Toils_Reserve.Reserve(TargetIndex.B, 10, 1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return new Toil
			{
				initAction = (Action)delegate
				{
					Pawn actor2 = ((_003CMakeNewToils_003Ec__Iterator34)/*Error near IL_012f: stateMachine*/)._003CloadIfNeeded_003E__1.actor;
					Building building2 = (Building)actor2.CurJob.targetA.Thing;
					Building_TurretGun building_TurretGun = building2 as Building_TurretGun;
					SoundDefOf.ArtilleryShellLoaded.PlayOneShot(new TargetInfo(building_TurretGun.Position, building_TurretGun.Map, false));
					building_TurretGun.loaded = true;
					actor2.carryTracker.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
				}
			};
			yield return gotoTurret;
			Toil man = new Toil
			{
				tickAction = (Action)delegate
				{
					Pawn actor = ((_003CMakeNewToils_003Ec__Iterator34)/*Error near IL_0181: stateMachine*/)._003Cman_003E__3.actor;
					Building building = (Building)actor.CurJob.targetA.Thing;
					if (JobDriver_ManTurret.GunNeedsLoading(building))
					{
						((_003CMakeNewToils_003Ec__Iterator34)/*Error near IL_0181: stateMachine*/)._003C_003Ef__this.JumpToToil(((_003CMakeNewToils_003Ec__Iterator34)/*Error near IL_0181: stateMachine*/)._003CloadIfNeeded_003E__1);
					}
					else
					{
						building.GetComp<CompMannable>().ManForATick(actor);
					}
				},
				defaultCompleteMode = ToilCompleteMode.Never
			};
			man.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			yield return man;
		}
	}
}
