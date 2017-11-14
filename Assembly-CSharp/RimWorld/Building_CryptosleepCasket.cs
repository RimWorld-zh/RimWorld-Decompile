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
			if (!myPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				FloatMenuOption failer = new FloatMenuOption("CannotUseNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
				yield return failer;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			_003CGetFloatMenuOptions_003Ec__Iterator0 _003CGetFloatMenuOptions_003Ec__Iterator2 = (_003CGetFloatMenuOptions_003Ec__Iterator0)/*Error near IL_017a: stateMachine*/;
			JobDef jobDef = JobDefOf.EnterCryptosleepCasket;
			string jobStr = "EnterCryptosleepCasket".Translate();
			Action jobAction = delegate
			{
				Job job = new Job(jobDef, _003CGetFloatMenuOptions_003Ec__Iterator2._0024this);
				myPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			};
			yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(jobStr, jobAction, MenuOptionPriority.Default, null, null, 0f, null, null), myPawn, this, "ReservedBy");
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0226:
			/*Error near IL_0227: Unexpected return in MoveNext()*/;
		}

		public override IEnumerable<Gizmo> GetGizmos()
		{
			using (IEnumerator<Gizmo> enumerator = base.GetGizmos().GetEnumerator())
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
				action = ((Building_Casket)this).EjectContents,
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
			IL_01c4:
			/*Error near IL_01c5: Unexpected return in MoveNext()*/;
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
						pawn.health.AddHediff(HediffDefOf.CryptosleepSickness, null, null);
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
				Building_CryptosleepCasket building_CryptosleepCasket = (Building_CryptosleepCasket)GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForDef(item), PathEndMode.InteractionCell, TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, delegate(Thing x)
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
