using RimWorld.Planet;
using System;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	public class CompSpawnerMechanoidsOnDamaged : ThingComp
	{
		public float pointsLeft;

		private Lord lord;

		private const float MechanoidsDefendRadius = 21f;

		public static readonly string MemoDamaged = "ShipPartDamaged";

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Lord>(ref this.lord, "defenseLord", false);
			Scribe_Values.Look<float>(ref this.pointsLeft, "mechanoidPointsLeft", 0f, false);
		}

		public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			base.PostPreApplyDamage(dinfo, out absorbed);
			if (!absorbed)
			{
				if (dinfo.Def.harmsHealth)
				{
					if (this.lord != null)
					{
						this.lord.ReceiveMemo(CompSpawnerMechanoidsOnDamaged.MemoDamaged);
					}
					float num = (float)(base.parent.HitPoints - dinfo.Amount);
					if (num < (float)base.parent.MaxHitPoints * 0.98000001907348633 && dinfo.Instigator != null && dinfo.Instigator.Faction != null)
					{
						goto IL_00a4;
					}
					if (num < (float)base.parent.MaxHitPoints * 0.89999997615814209)
						goto IL_00a4;
				}
				goto IL_00ad;
			}
			return;
			IL_00ad:
			absorbed = false;
			return;
			IL_00a4:
			this.TrySpawnMechanoids();
			goto IL_00ad;
		}

		public void Notify_BlueprintReplacedWithSolidThingNearby(Pawn by)
		{
			if (by.Faction != Faction.OfMechanoids)
			{
				this.TrySpawnMechanoids();
			}
		}

		private void TrySpawnMechanoids()
		{
			if (!(this.pointsLeft <= 0.0) && base.parent.Spawned)
			{
				if (this.lord == null)
				{
					IntVec3 invalid = default(IntVec3);
					if (!CellFinder.TryFindRandomCellNear(base.parent.Position, base.parent.Map, 5, (Predicate<IntVec3>)((IntVec3 c) => c.Standable(base.parent.Map) && base.parent.Map.reachability.CanReach(c, (Thing)base.parent, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false))), out invalid))
					{
						Log.Error("Found no place for mechanoids to defend " + this);
						invalid = IntVec3.Invalid;
					}
					LordJob_MechanoidsDefendShip lordJob = new LordJob_MechanoidsDefendShip(base.parent, base.parent.Faction, 21f, invalid);
					this.lord = LordMaker.MakeNewLord(Faction.OfMechanoids, lordJob, base.parent.Map, null);
				}
				PawnKindDef kindDef = default(PawnKindDef);
				IntVec3 center = default(IntVec3);
				while ((from def in DefDatabase<PawnKindDef>.AllDefs
				where def.RaceProps.IsMechanoid && def.isFighter && def.combatPower <= this.pointsLeft
				select def).TryRandomElement<PawnKindDef>(out kindDef) && (from cell in GenAdj.CellsAdjacent8Way(base.parent)
				where this.CanSpawnMechanoidAt(cell)
				select cell).TryRandomElement<IntVec3>(out center))
				{
					Pawn pawn = PawnGenerator.GeneratePawn(kindDef, Faction.OfMechanoids);
					if (!GenPlace.TryPlaceThing(pawn, center, base.parent.Map, ThingPlaceMode.Near, null))
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
						break;
					}
					this.lord.AddPawn(pawn);
					this.pointsLeft -= pawn.kindDef.combatPower;
				}
				this.pointsLeft = 0f;
				SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(base.parent.Map);
			}
		}

		private bool CanSpawnMechanoidAt(IntVec3 c)
		{
			return c.Walkable(base.parent.Map);
		}
	}
}
