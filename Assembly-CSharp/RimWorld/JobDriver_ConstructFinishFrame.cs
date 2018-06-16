using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000041 RID: 65
	public class JobDriver_ConstructFinishFrame : JobDriver
	{
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600022B RID: 555 RVA: 0x00016C78 File Offset: 0x00015078
		private Frame Frame
		{
			get
			{
				return (Frame)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00016CA8 File Offset: 0x000150A8
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00016CDC File Offset: 0x000150DC
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			Toil build = new Toil();
			build.initAction = delegate()
			{
				GenClamor.DoClamor(build.actor, 15f, ClamorDefOf.Construction);
			};
			build.tickAction = delegate()
			{
				Pawn actor = build.actor;
				Frame frame = this.Frame;
				if (frame.resourceContainer.Count > 0)
				{
					actor.skills.Learn(SkillDefOf.Construction, 0.275f, false);
				}
				float num = actor.GetStatValue(StatDefOf.ConstructionSpeed, true);
				if (frame.Stuff != null)
				{
					num *= frame.Stuff.GetStatValueAbstract(StatDefOf.ConstructionSpeedFactor, null);
				}
				float workToMake = frame.WorkToMake;
				if (actor.Faction == Faction.OfPlayer)
				{
					float statValue = actor.GetStatValue(StatDefOf.ConstructSuccessChance, true);
					if (Rand.Value < 1f - Mathf.Pow(statValue, num / workToMake))
					{
						frame.FailConstruction(actor);
						this.ReadyForNextToil();
						return;
					}
				}
				if (frame.def.entityDefToBuild is TerrainDef)
				{
					this.Map.snowGrid.SetDepth(frame.Position, 0f);
				}
				frame.workDone += num;
				if (frame.workDone >= workToMake)
				{
					frame.CompleteConstruction(actor);
					this.ReadyForNextToil();
				}
			};
			build.WithEffect(() => ((Frame)build.actor.jobs.curJob.GetTarget(TargetIndex.A).Thing).ConstructionEffect, TargetIndex.A);
			build.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			build.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			build.FailOn(() => !GenConstruct.CanConstruct(this.Frame, this.pawn, true, false));
			build.defaultCompleteMode = ToilCompleteMode.Delay;
			build.defaultDuration = 5000;
			build.activeSkill = (() => SkillDefOf.Construction);
			yield return build;
			yield break;
		}

		// Token: 0x040001D1 RID: 465
		private const int JobEndInterval = 5000;
	}
}
