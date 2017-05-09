using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

		[DebuggerHidden]
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
		{
			Building_CryptosleepCasket.<GetFloatMenuOptions>c__Iterator14F <GetFloatMenuOptions>c__Iterator14F = new Building_CryptosleepCasket.<GetFloatMenuOptions>c__Iterator14F();
			<GetFloatMenuOptions>c__Iterator14F.myPawn = myPawn;
			<GetFloatMenuOptions>c__Iterator14F.<$>myPawn = myPawn;
			<GetFloatMenuOptions>c__Iterator14F.<>f__this = this;
			Building_CryptosleepCasket.<GetFloatMenuOptions>c__Iterator14F expr_1C = <GetFloatMenuOptions>c__Iterator14F;
			expr_1C.$PC = -2;
			return expr_1C;
		}

		[DebuggerHidden]
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_CryptosleepCasket.<GetGizmos>c__Iterator150 <GetGizmos>c__Iterator = new Building_CryptosleepCasket.<GetGizmos>c__Iterator150();
			<GetGizmos>c__Iterator.<>f__this = this;
			Building_CryptosleepCasket.<GetGizmos>c__Iterator150 expr_0E = <GetGizmos>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public override void EjectContents()
		{
			ThingDef filthSlime = ThingDefOf.FilthSlime;
			foreach (Thing current in ((IEnumerable<Thing>)this.innerContainer))
			{
				Pawn pawn = current as Pawn;
				if (pawn != null)
				{
					PawnComponentsUtility.AddComponentsForSpawn(pawn);
					pawn.filth.GainFilth(filthSlime);
					pawn.health.AddHediff(HediffDefOf.CryptosleepSickness, null, null);
				}
			}
			if (!base.Destroyed)
			{
				SoundDef.Named("CryptosleepCasketEject").PlayOneShot(SoundInfo.InMap(new TargetInfo(base.Position, base.Map, false), MaintenanceType.None));
			}
			base.EjectContents();
		}

		public static Building_CryptosleepCasket FindCryptosleepCasketFor(Pawn p, Pawn traveler)
		{
			IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
			where typeof(Building_CryptosleepCasket).IsAssignableFrom(def.thingClass)
			select def;
			foreach (ThingDef current in enumerable)
			{
				Predicate<Thing> validator = (Thing x) => !((Building_CryptosleepCasket)x).HasAnyContents && traveler.CanReserve(x, 1, -1, null, false);
				Building_CryptosleepCasket building_CryptosleepCasket = (Building_CryptosleepCasket)GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForDef(current), PathEndMode.InteractionCell, TraverseParms.For(traveler, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null, -1, false, RegionType.Set_Passable, false);
				if (building_CryptosleepCasket != null)
				{
					return building_CryptosleepCasket;
				}
			}
			return null;
		}
	}
}
