using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	public class Building_CryptosleepCasket : Building_Casket
	{
		public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			if (base.TryAcceptThing(thing, allowSpecialEffects))
			{
				if (allowSpecialEffects)
				{
					SoundDef.Named("CryptosleepCasketAccept").PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
				return true;
			}
			return false;
		}

		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(myPawn))
			{
				yield return floatMenuOption;
			}
			if (base.innerContainer.Count == 0)
			{
				if (!myPawn.CanReach((Thing)this, PathEndMode.InteractionCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					FloatMenuOption failer = new FloatMenuOption("CannotUseNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
					yield return failer;
				}
				else
				{
					JobDef jobDef = JobDefOf.EnterCryptosleepCasket;
					string jobStr = "EnterCryptosleepCasket".Translate();
					Action jobAction = (Action)delegate
					{
						Job job = new Job(((_003CGetFloatMenuOptions_003Ec__Iterator150)/*Error near IL_0141: stateMachine*/)._003CjobDef_003E__3, (Thing)((_003CGetFloatMenuOptions_003Ec__Iterator150)/*Error near IL_0141: stateMachine*/)._003C_003Ef__this);
						((_003CGetFloatMenuOptions_003Ec__Iterator150)/*Error near IL_0141: stateMachine*/).myPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
					};
					yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(jobStr, jobAction, MenuOptionPriority.Default, null, null, 0f, null, null), myPawn, (Thing)this, "ReservedBy");
				}
			}
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			if (base.Faction == Faction.OfPlayer && base.innerContainer.Count > 0 && base.def.building.isPlayerEjectable)
			{
				Command_Action eject = new Command_Action
				{
					action = new Action(this.EjectContents),
					defaultLabel = "CommandPodEject".Translate(),
					defaultDesc = "CommandPodEjectDesc".Translate()
				};
				if (base.innerContainer.Count == 0)
				{
					eject.Disable("CommandPodEjectFailEmpty".Translate());
				}
				eject.hotKey = KeyBindingDefOf.Misc1;
				eject.icon = ContentFinder<Texture2D>.Get("UI/Commands/PodEject", true);
				yield return (Gizmo)eject;
			}
		}

		public override void EjectContents()
		{
			ThingDef filthSlime = ThingDefOf.FilthSlime;
			foreach (Thing item in (IEnumerable<Thing>)base.innerContainer)
			{
				Pawn pawn = item as Pawn;
				if (pawn != null)
				{
					PawnComponentsUtility.AddComponentsForSpawn(pawn);
					pawn.filth.GainFilth(filthSlime);
					pawn.health.AddHediff(HediffDefOf.CryptosleepSickness, null, default(DamageInfo?));
				}
			}
			if (!base.Destroyed)
			{
				SoundDef.Named("CryptosleepCasketEject").PlayOneShot(SoundInfo.InMap(new TargetInfo(base.Position, base.Map, false), MaintenanceType.None));
			}
			base.EjectContents();
		}

		public static Building_CryptosleepCasket FindCryptosleepCasketFor(Pawn p, Pawn traveler, bool ignoreOtherReservations = false)
		{
			IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
			where typeof(Building_CryptosleepCasket).IsAssignableFrom(def.thingClass)
			select def;
			foreach (ThingDef item in enumerable)
			{
				Predicate<Thing> validator = (Predicate<Thing>)delegate(Thing x)
				{
					int result;
					if (!((Building_CryptosleepCasket)x).HasAnyContents)
					{
						bool ignoreOtherReservations2 = ignoreOtherReservations;
						result = (traveler.CanReserve(x, 1, -1, null, ignoreOtherReservations2) ? 1 : 0);
					}
					else
					{
						result = 0;
					}
					return (byte)result != 0;
				};
				Building_CryptosleepCasket building_CryptosleepCasket = (Building_CryptosleepCasket)GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForDef(item), PathEndMode.InteractionCell, TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
				if (building_CryptosleepCasket != null)
				{
					return building_CryptosleepCasket;
				}
			}
			return null;
		}
	}
}
