using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000074 RID: 116
	public class JobDriver_ManTurret : JobDriver
	{
		// Token: 0x0400021F RID: 543
		private const float ShellSearchRadius = 40f;

		// Token: 0x04000220 RID: 544
		private const int MaxPawnAmmoReservations = 10;

		// Token: 0x06000328 RID: 808 RVA: 0x0002271C File Offset: 0x00020B1C
		private static bool GunNeedsLoading(Building b)
		{
			Building_TurretGun building_TurretGun = b as Building_TurretGun;
			bool result;
			if (building_TurretGun == null)
			{
				result = false;
			}
			else
			{
				CompChangeableProjectile compChangeableProjectile = building_TurretGun.gun.TryGetComp<CompChangeableProjectile>();
				result = (compChangeableProjectile != null && !compChangeableProjectile.Loaded);
			}
			return result;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0002276C File Offset: 0x00020B6C
		public static Thing FindAmmoForTurret(Pawn pawn, Building_TurretGun gun)
		{
			StorageSettings allowedShellsSettings = (!pawn.IsColonist) ? null : gun.gun.TryGetComp<CompChangeableProjectile>().allowedShellsSettings;
			Predicate<Thing> validator = (Thing t) => !t.IsForbidden(pawn) && pawn.CanReserve(t, 10, 1, null, false) && (allowedShellsSettings == null || allowedShellsSettings.AllowedToAccept(t));
			return GenClosest.ClosestThingReachable(gun.Position, gun.Map, ThingRequest.ForGroup(ThingRequestGroup.Shell), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 40f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x000227FC File Offset: 0x00020BFC
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00022830 File Offset: 0x00020C30
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			Toil gotoTurret = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			Toil loadIfNeeded = new Toil();
			loadIfNeeded.initAction = delegate()
			{
				Pawn actor = loadIfNeeded.actor;
				Building building = (Building)actor.CurJob.targetA.Thing;
				Building_TurretGun building_TurretGun = building as Building_TurretGun;
				if (!JobDriver_ManTurret.GunNeedsLoading(building))
				{
					this.JumpToToil(gotoTurret);
				}
				else
				{
					Thing thing = JobDriver_ManTurret.FindAmmoForTurret(this.pawn, building_TurretGun);
					if (thing == null)
					{
						if (actor.Faction == Faction.OfPlayer)
						{
							Messages.Message("MessageOutOfNearbyShellsFor".Translate(new object[]
							{
								actor.LabelShort,
								building_TurretGun.Label
							}).CapitalizeFirst(), building_TurretGun, MessageTypeDefOf.NegativeEvent, true);
						}
						actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
					}
					actor.CurJob.targetB = thing;
					actor.CurJob.count = 1;
				}
			};
			yield return loadIfNeeded;
			yield return Toils_Reserve.Reserve(TargetIndex.B, 10, 1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return new Toil
			{
				initAction = delegate()
				{
					Pawn actor = loadIfNeeded.actor;
					Building building = (Building)actor.CurJob.targetA.Thing;
					Building_TurretGun building_TurretGun = building as Building_TurretGun;
					SoundDefOf.Artillery_ShellLoaded.PlayOneShot(new TargetInfo(building_TurretGun.Position, building_TurretGun.Map, false));
					building_TurretGun.gun.TryGetComp<CompChangeableProjectile>().LoadShell(actor.CurJob.targetB.Thing.def, 1);
					actor.carryTracker.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
				}
			};
			yield return gotoTurret;
			Toil man = new Toil();
			man.tickAction = delegate()
			{
				Pawn actor = man.actor;
				Building building = (Building)actor.CurJob.targetA.Thing;
				if (JobDriver_ManTurret.GunNeedsLoading(building))
				{
					this.JumpToToil(loadIfNeeded);
				}
				else
				{
					building.GetComp<CompMannable>().ManForATick(actor);
				}
			};
			man.defaultCompleteMode = ToilCompleteMode.Never;
			man.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			yield return man;
			yield break;
		}
	}
}
