using System;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200073C RID: 1852
	public class CompSpawnerMechanoidsOnDamaged : ThingComp
	{
		// Token: 0x04001665 RID: 5733
		public float pointsLeft;

		// Token: 0x04001666 RID: 5734
		private Lord lord;

		// Token: 0x04001667 RID: 5735
		private const float MechanoidsDefendRadius = 21f;

		// Token: 0x04001668 RID: 5736
		public static readonly string MemoDamaged = "ShipPartDamaged";

		// Token: 0x060028E5 RID: 10469 RVA: 0x0015D0A3 File Offset: 0x0015B4A3
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Lord>(ref this.lord, "defenseLord", false);
			Scribe_Values.Look<float>(ref this.pointsLeft, "mechanoidPointsLeft", 0f, false);
		}

		// Token: 0x060028E6 RID: 10470 RVA: 0x0015D0D4 File Offset: 0x0015B4D4
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

		// Token: 0x060028E7 RID: 10471 RVA: 0x0015D191 File Offset: 0x0015B591
		public void Notify_BlueprintReplacedWithSolidThingNearby(Pawn by)
		{
			if (by.Faction != Faction.OfMechanoids)
			{
				this.TrySpawnMechanoids();
			}
		}

		// Token: 0x060028E8 RID: 10472 RVA: 0x0015D1AC File Offset: 0x0015B5AC
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

		// Token: 0x060028E9 RID: 10473 RVA: 0x0015D3A4 File Offset: 0x0015B7A4
		private bool CanSpawnMechanoidAt(IntVec3 c)
		{
			return c.Walkable(this.parent.Map);
		}
	}
}
