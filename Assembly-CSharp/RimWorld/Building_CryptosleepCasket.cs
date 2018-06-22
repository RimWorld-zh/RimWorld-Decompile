using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020006A2 RID: 1698
	public class Building_CryptosleepCasket : Building_Casket
	{
		// Token: 0x0600242A RID: 9258 RVA: 0x00133214 File Offset: 0x00131614
		public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			bool result;
			if (base.TryAcceptThing(thing, allowSpecialEffects))
			{
				if (allowSpecialEffects)
				{
					SoundDefOf.CryptosleepCasket_Accept.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x00133268 File Offset: 0x00131668
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
		{
			foreach (FloatMenuOption o in this.<GetFloatMenuOptions>__BaseCallProxy0(myPawn))
			{
				yield return o;
			}
			if (this.innerContainer.Count == 0)
			{
				if (!myPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					FloatMenuOption failer = new FloatMenuOption("CannotUseNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
					yield return failer;
				}
				else
				{
					JobDef jobDef = JobDefOf.EnterCryptosleepCasket;
					string jobStr = "EnterCryptosleepCasket".Translate();
					Action jobAction = delegate()
					{
						Job job = new Job(jobDef, this.$this);
						myPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
					};
					yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(jobStr, jobAction, MenuOptionPriority.Default, null, null, 0f, null, null), myPawn, this, "ReservedBy");
				}
			}
			yield break;
		}

		// Token: 0x0600242C RID: 9260 RVA: 0x0013329C File Offset: 0x0013169C
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo c in this.<GetGizmos>__BaseCallProxy1())
			{
				yield return c;
			}
			if (base.Faction == Faction.OfPlayer && this.innerContainer.Count > 0 && this.def.building.isPlayerEjectable)
			{
				Command_Action eject = new Command_Action();
				eject.action = new Action(this.EjectContents);
				eject.defaultLabel = "CommandPodEject".Translate();
				eject.defaultDesc = "CommandPodEjectDesc".Translate();
				if (this.innerContainer.Count == 0)
				{
					eject.Disable("CommandPodEjectFailEmpty".Translate());
				}
				eject.hotKey = KeyBindingDefOf.Misc1;
				eject.icon = ContentFinder<Texture2D>.Get("UI/Commands/PodEject", true);
				yield return eject;
			}
			yield break;
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x001332C8 File Offset: 0x001316C8
		public override void EjectContents()
		{
			ThingDef filth_Slime = ThingDefOf.Filth_Slime;
			foreach (Thing thing in ((IEnumerable<Thing>)this.innerContainer))
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					PawnComponentsUtility.AddComponentsForSpawn(pawn);
					pawn.filth.GainFilth(filth_Slime);
					if (pawn.RaceProps.IsFlesh)
					{
						pawn.health.AddHediff(HediffDefOf.CryptosleepSickness, null, null, null);
					}
				}
			}
			if (!base.Destroyed)
			{
				SoundDefOf.CryptosleepCasket_Eject.PlayOneShot(SoundInfo.InMap(new TargetInfo(base.Position, base.Map, false), MaintenanceType.None));
			}
			base.EjectContents();
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x001333A8 File Offset: 0x001317A8
		public static Building_CryptosleepCasket FindCryptosleepCasketFor(Pawn p, Pawn traveler, bool ignoreOtherReservations = false)
		{
			IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
			where typeof(Building_CryptosleepCasket).IsAssignableFrom(def.thingClass)
			select def;
			foreach (ThingDef singleDef in enumerable)
			{
				Building_CryptosleepCasket building_CryptosleepCasket = (Building_CryptosleepCasket)GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForDef(singleDef), PathEndMode.InteractionCell, TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, delegate(Thing x)
				{
					bool result;
					if (!((Building_CryptosleepCasket)x).HasAnyContents)
					{
						Pawn traveler2 = traveler;
						LocalTargetInfo target = x;
						bool ignoreOtherReservations2 = ignoreOtherReservations;
						result = traveler2.CanReserve(target, 1, -1, null, ignoreOtherReservations2);
					}
					else
					{
						result = false;
					}
					return result;
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
