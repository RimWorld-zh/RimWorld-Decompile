using System;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
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

		public CompSpawnerMechanoidsOnDamaged()
		{
		}

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
					float num = (float)this.parent.HitPoints - dinfo.Amount;
					if ((num < (float)this.parent.MaxHitPoints * 0.98f && dinfo.Instigator != null && dinfo.Instigator.Faction != null) || num < (float)this.parent.MaxHitPoints * 0.9f)
					{
						this.TrySpawnMechanoids();
					}
				}
				absorbed = false;
			}
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
			if (this.pointsLeft > 0f)
			{
				if (this.parent.Spawned)
				{
					if (this.lord == null)
					{
						IntVec3 invalid;
						if (!CellFinder.TryFindRandomCellNear(this.parent.Position, this.parent.Map, 5, (IntVec3 c) => c.Standable(this.parent.Map) && this.parent.Map.reachability.CanReach(c, this.parent, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)), out invalid, -1))
						{
							Log.Error("Found no place for mechanoids to defend " + this, false);
							invalid = IntVec3.Invalid;
						}
						LordJob_MechanoidsDefendShip lordJob = new LordJob_MechanoidsDefendShip(this.parent, this.parent.Faction, 21f, invalid);
						this.lord = LordMaker.MakeNewLord(Faction.OfMechanoids, lordJob, this.parent.Map, null);
					}
					PawnKindDef kind;
					while ((from def in DefDatabase<PawnKindDef>.AllDefs
					where def.RaceProps.IsMechanoid && def.isFighter && def.combatPower <= this.pointsLeft
					select def).TryRandomElement(out kind))
					{
						IntVec3 center;
						if ((from cell in GenAdj.CellsAdjacent8Way(this.parent)
						where this.CanSpawnMechanoidAt(cell)
						select cell).TryRandomElement(out center))
						{
							PawnGenerationRequest request = new PawnGenerationRequest(kind, Faction.OfMechanoids, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, false, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
							Pawn pawn = PawnGenerator.GeneratePawn(request);
							if (GenPlace.TryPlaceThing(pawn, center, this.parent.Map, ThingPlaceMode.Near, null, null))
							{
								this.lord.AddPawn(pawn);
								this.pointsLeft -= pawn.kindDef.combatPower;
								continue;
							}
							Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
						}
						IL_1CA:
						this.pointsLeft = 0f;
						SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(this.parent.Map);
						return;
					}
					goto IL_1CA;
				}
			}
		}

		private bool CanSpawnMechanoidAt(IntVec3 c)
		{
			return c.Walkable(this.parent.Map);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static CompSpawnerMechanoidsOnDamaged()
		{
		}

		[CompilerGenerated]
		private bool <TrySpawnMechanoids>m__0(IntVec3 c)
		{
			return c.Standable(this.parent.Map) && this.parent.Map.reachability.CanReach(c, this.parent, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false));
		}

		[CompilerGenerated]
		private bool <TrySpawnMechanoids>m__1(PawnKindDef def)
		{
			return def.RaceProps.IsMechanoid && def.isFighter && def.combatPower <= this.pointsLeft;
		}

		[CompilerGenerated]
		private bool <TrySpawnMechanoids>m__2(IntVec3 cell)
		{
			return this.CanSpawnMechanoidAt(cell);
		}
	}
}
