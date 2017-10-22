using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_ManTurret : JobDriver
	{
		private const float ShellSearchRadius = 40f;

		private const int MaxPawnAmmoReservations = 10;

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
				result = ((byte)((compChangeableProjectile != null && !compChangeableProjectile.Loaded) ? 1 : 0) != 0);
			}
			return result;
		}

		public static Thing FindAmmoForTurret(Pawn pawn, Building_TurretGun gun)
		{
			StorageSettings allowedShellsSettings = (!pawn.IsColonist) ? null : gun.gun.TryGetComp<CompChangeableProjectile>().allowedShellsSettings;
			Predicate<Thing> validator = (Predicate<Thing>)((Thing t) => (byte)((!t.IsForbidden(pawn)) ? (pawn.CanReserve(t, 10, 1, null, false) ? ((allowedShellsSettings == null || allowedShellsSettings.AllowedToAccept(t)) ? 1 : 0) : 0) : 0) != 0);
			return GenClosest.ClosestThingReachable(gun.Position, gun.Map, ThingRequest.ForGroup(ThingRequestGroup.Shell), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 40f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
		}

		public override bool TryMakePreToilReservations()
		{
			return base.pawn.Reserve(base.job.targetA, base.job, 1, -1, null);
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			_003CMakeNewToils_003Ec__Iterator0 _003CMakeNewToils_003Ec__Iterator = (_003CMakeNewToils_003Ec__Iterator0)/*Error near IL_004e: stateMachine*/;
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			Toil gotoTurret = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			Toil loadIfNeeded = new Toil();
			loadIfNeeded.initAction = (Action)delegate
			{
				Pawn actor = loadIfNeeded.actor;
				Building building = (Building)actor.CurJob.targetA.Thing;
				Building_TurretGun building_TurretGun = building as Building_TurretGun;
				if (!JobDriver_ManTurret.GunNeedsLoading(building))
				{
					_003CMakeNewToils_003Ec__Iterator._0024this.JumpToToil(gotoTurret);
				}
				else
				{
					Thing thing = JobDriver_ManTurret.FindAmmoForTurret(_003CMakeNewToils_003Ec__Iterator._0024this.pawn, building_TurretGun);
					if (thing == null)
					{
						if (actor.Faction == Faction.OfPlayer)
						{
							Messages.Message("MessageOutOfNearbyShellsFor".Translate(actor.LabelShort, building_TurretGun.Label).CapitalizeFirst(), (Thing)building_TurretGun, MessageTypeDefOf.NegativeEvent);
						}
						actor.jobs.EndCurrentJob(JobCondition.Incompletable, true);
					}
					actor.CurJob.targetB = thing;
					actor.CurJob.count = 1;
				}
			};
			yield return loadIfNeeded;
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
