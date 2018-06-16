using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200008C RID: 140
	public abstract class JobDriver_PlantWork : JobDriver
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000395 RID: 917 RVA: 0x000288D8 File Offset: 0x00026CD8
		protected Plant Plant
		{
			get
			{
				return (Plant)this.job.targetA.Thing;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000396 RID: 918 RVA: 0x00028904 File Offset: 0x00026D04
		protected virtual DesignationDef RequiredDesignation
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0002891C File Offset: 0x00026D1C
		public override bool TryMakePreToilReservations()
		{
			LocalTargetInfo target = this.job.GetTarget(TargetIndex.A);
			if (target.IsValid)
			{
				if (!this.pawn.Reserve(target, this.job, 1, -1, null))
				{
					return false;
				}
			}
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00028990 File Offset: 0x00026D90
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.Init();
			yield return Toils_JobTransforms.MoveCurrentTargetIntoQueue(TargetIndex.A);
			Toil initExtractTargetFromQueue = Toils_JobTransforms.ClearDespawnedNullOrForbiddenQueuedTargets(TargetIndex.A, (this.RequiredDesignation == null) ? null : new Func<Thing, bool>((Thing t) => this.Map.designationManager.DesignationOn(t, this.RequiredDesignation) != null));
			yield return initExtractTargetFromQueue;
			yield return Toils_JobTransforms.SucceedOnNoTargetInQueue(TargetIndex.A);
			yield return Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.A, true);
			Toil gotoThing = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).JumpIfDespawnedOrNullOrForbidden(TargetIndex.A, initExtractTargetFromQueue);
			if (this.RequiredDesignation != null)
			{
				gotoThing.FailOnThingMissingDesignation(TargetIndex.A, this.RequiredDesignation);
			}
			yield return gotoThing;
			Toil cut = new Toil();
			cut.tickAction = delegate()
			{
				Pawn actor = cut.actor;
				if (actor.skills != null)
				{
					actor.skills.Learn(SkillDefOf.Growing, this.xpPerTick, false);
				}
				float statValue = actor.GetStatValue(StatDefOf.PlantWorkSpeed, true);
				float num = statValue;
				Plant plant = this.Plant;
				num *= Mathf.Lerp(3.3f, 1f, plant.Growth);
				this.workDone += num;
				if (this.workDone >= plant.def.plant.harvestWork)
				{
					if (plant.def.plant.harvestedThingDef != null)
					{
						if (actor.RaceProps.Humanlike && plant.def.plant.harvestFailable && Rand.Value > actor.GetStatValue(StatDefOf.PlantHarvestYield, true))
						{
							Vector3 loc = (this.pawn.DrawPos + plant.DrawPos) / 2f;
							MoteMaker.ThrowText(loc, this.Map, "TextMote_HarvestFailed".Translate(), 3.65f);
						}
						else
						{
							int num2 = plant.YieldNow();
							if (num2 > 0)
							{
								Thing thing = ThingMaker.MakeThing(plant.def.plant.harvestedThingDef, null);
								thing.stackCount = num2;
								if (actor.Faction != Faction.OfPlayer)
								{
									thing.SetForbidden(true, true);
								}
								GenPlace.TryPlaceThing(thing, actor.Position, this.Map, ThingPlaceMode.Near, null, null);
								actor.records.Increment(RecordDefOf.PlantsHarvested);
							}
						}
					}
					plant.def.plant.soundHarvestFinish.PlayOneShot(actor);
					plant.PlantCollected();
					this.workDone = 0f;
					this.ReadyForNextToil();
				}
			};
			cut.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			if (this.RequiredDesignation != null)
			{
				cut.FailOnThingMissingDesignation(TargetIndex.A, this.RequiredDesignation);
			}
			cut.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			cut.defaultCompleteMode = ToilCompleteMode.Never;
			cut.WithEffect(EffecterDefOf.Harvest, TargetIndex.A);
			cut.WithProgressBar(TargetIndex.A, () => this.workDone / this.Plant.def.plant.harvestWork, true, -0.5f);
			cut.PlaySustainerOrSound(() => this.Plant.def.plant.soundHarvesting);
			cut.activeSkill = (() => SkillDefOf.Growing);
			yield return cut;
			Toil plantWorkDoneToil = this.PlantWorkDoneToil();
			if (plantWorkDoneToil != null)
			{
				yield return plantWorkDoneToil;
			}
			yield return Toils_Jump.Jump(initExtractTargetFromQueue);
			yield break;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x000289BA File Offset: 0x00026DBA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workDone, "workDone", 0f, false);
		}

		// Token: 0x0600039A RID: 922 RVA: 0x000289D9 File Offset: 0x00026DD9
		protected virtual void Init()
		{
		}

		// Token: 0x0600039B RID: 923 RVA: 0x000289DC File Offset: 0x00026DDC
		protected virtual Toil PlantWorkDoneToil()
		{
			return null;
		}

		// Token: 0x0400024B RID: 587
		private float workDone = 0f;

		// Token: 0x0400024C RID: 588
		protected float xpPerTick = 0f;

		// Token: 0x0400024D RID: 589
		protected const TargetIndex PlantInd = TargetIndex.A;
	}
}
