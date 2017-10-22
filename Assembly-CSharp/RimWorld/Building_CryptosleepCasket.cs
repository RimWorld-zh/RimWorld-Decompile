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
			bool result;
			if (base.TryAcceptThing(thing, allowSpecialEffects))
			{
				if (allowSpecialEffects)
				{
					SoundDef.Named("CryptosleepCasketAccept").PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
		{
			_003CGetFloatMenuOptions_003Ec__Iterator0 _003CGetFloatMenuOptions_003Ec__Iterator = (_003CGetFloatMenuOptions_003Ec__Iterator0)/*Error near IL_003c: stateMachine*/;
			using (IEnumerator<FloatMenuOption> enumerator = this._003CGetFloatMenuOptions_003E__BaseCallProxy0(myPawn).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					FloatMenuOption o = enumerator.Current;
					yield return o;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (base.innerContainer.Count != 0)
				yield break;
			if (!myPawn.CanReach((Thing)this, PathEndMode.InteractionCell, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				FloatMenuOption failer = new FloatMenuOption("CannotUseNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
				yield return failer;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			_003CGetFloatMenuOptions_003Ec__Iterator0 _003CGetFloatMenuOptions_003Ec__Iterator2 = (_003CGetFloatMenuOptions_003Ec__Iterator0)/*Error near IL_0181: stateMachine*/;
			JobDef jobDef = JobDefOf.EnterCryptosleepCasket;
			string jobStr = "EnterCryptosleepCasket".Translate();
			Action jobAction = (Action)delegate()
			{
				Job job = new Job(jobDef, (Thing)_003CGetFloatMenuOptions_003Ec__Iterator2._0024this);
				myPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			};
			yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(jobStr, jobAction, MenuOptionPriority.Default, null, null, 0f, null, null), myPawn, (Thing)this, "ReservedBy");
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0230:
			/*Error near IL_0231: Unexpected return in MoveNext()*/;
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			using (IEnumerator<Gizmo> enumerator = this._003CGetGizmos_003E__BaseCallProxy1().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					Gizmo c = enumerator.Current;
					yield return c;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (base.Faction != Faction.OfPlayer)
				yield break;
			if (base.innerContainer.Count <= 0)
				yield break;
			if (!base.def.building.isPlayerEjectable)
				yield break;
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
			/*Error: Unable to find new state assignment for yield return*/;
			IL_01ca:
			/*Error near IL_01cb: Unexpected return in MoveNext()*/;
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
					if (pawn.RaceProps.IsFlesh)
					{
						pawn.health.AddHediff(HediffDefOf.CryptosleepSickness, null, default(DamageInfo?));
					}
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
				Building_CryptosleepCasket building_CryptosleepCasket = (Building_CryptosleepCasket)GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForDef(item), PathEndMode.InteractionCell, TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, (Predicate<Thing>)delegate(Thing x)
				{
					int result;
					if (!((Building_CryptosleepCasket)x).HasAnyContents)
					{
						Pawn p2 = traveler;
						LocalTargetInfo target = x;
						bool ignoreOtherReservations2 = ignoreOtherReservations;
						result = (p2.CanReserve(target, 1, -1, null, ignoreOtherReservations2) ? 1 : 0);
					}
					else
					{
						result = 0;
					}
					return (byte)result != 0;
				}, null, 0, -1, false, RegionType.Set_Passable, false);
				if (building_CryptosleepCasket != null)
				{
					return building_CryptosleepCasket;
				}
			}
			return null;
		}
	}
}
