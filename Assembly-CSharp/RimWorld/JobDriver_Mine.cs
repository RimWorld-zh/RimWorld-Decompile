using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000075 RID: 117
	public class JobDriver_Mine : JobDriver
	{
		// Token: 0x04000221 RID: 545
		private int ticksToPickHit = -1000;

		// Token: 0x04000222 RID: 546
		private Effecter effecter = null;

		// Token: 0x04000223 RID: 547
		public const int BaseTicksBetweenPickHits = 120;

		// Token: 0x04000224 RID: 548
		private const int BaseDamagePerPickHit_NaturalRock = 80;

		// Token: 0x04000225 RID: 549
		private const int BaseDamagePerPickHit_NotNaturalRock = 35;

		// Token: 0x04000226 RID: 550
		private const float MinMiningSpeedFactorForNPCs = 0.5f;

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600032D RID: 813 RVA: 0x00022DE4 File Offset: 0x000211E4
		private Thing MineTarget
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00022E10 File Offset: 0x00021210
		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.MineTarget, this.job, 1, -1, null);
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00022E44 File Offset: 0x00021244
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnCellMissingDesignation(TargetIndex.A, DesignationDefOf.Mine);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil mine = new Toil();
			mine.tickAction = delegate()
			{
				Pawn actor = mine.actor;
				Thing mineTarget = this.MineTarget;
				if (this.ticksToPickHit < -100)
				{
					this.ResetTicksToPickHit();
				}
				if (actor.skills != null && (mineTarget.Faction != actor.Faction || actor.Faction == null))
				{
					actor.skills.Learn(SkillDefOf.Mining, 0.077f, false);
				}
				this.ticksToPickHit--;
				if (this.ticksToPickHit <= 0)
				{
					IntVec3 position = mineTarget.Position;
					if (this.effecter == null)
					{
						this.effecter = EffecterDefOf.Mine.Spawn();
					}
					this.effecter.Trigger(actor, mineTarget);
					int num = (!mineTarget.def.building.isNaturalRock) ? 35 : 80;
					Mineable mineable = mineTarget as Mineable;
					if (mineable == null || mineTarget.HitPoints > num)
					{
						DamageDef mining = DamageDefOf.Mining;
						float amount = (float)num;
						Pawn actor2 = mine.actor;
						DamageInfo dinfo = new DamageInfo(mining, amount, -1f, actor2, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
						mineTarget.TakeDamage(dinfo);
					}
					else
					{
						mineable.Notify_TookMiningDamage(mineTarget.HitPoints, mine.actor);
						mineable.HitPoints = 0;
						mineable.DestroyMined(actor);
					}
					if (mineTarget.Destroyed)
					{
						actor.Map.mineStrikeManager.CheckStruckOre(position, mineTarget.def, actor);
						actor.records.Increment(RecordDefOf.CellsMined);
						if (this.pawn.Faction != Faction.OfPlayer)
						{
							List<Thing> thingList = position.GetThingList(this.Map);
							for (int i = 0; i < thingList.Count; i++)
							{
								thingList[i].SetForbidden(true, false);
							}
						}
						if (this.pawn.Faction == Faction.OfPlayer && MineStrikeManager.MineableIsVeryValuable(mineTarget.def))
						{
							TaleRecorder.RecordTale(TaleDefOf.MinedValuable, new object[]
							{
								this.pawn,
								mineTarget.def.building.mineableThing
							});
						}
						if (this.pawn.Faction == Faction.OfPlayer && MineStrikeManager.MineableIsValuable(mineTarget.def) && !this.pawn.Map.IsPlayerHome)
						{
							TaleRecorder.RecordTale(TaleDefOf.CaravanRemoteMining, new object[]
							{
								this.pawn,
								mineTarget.def.building.mineableThing
							});
						}
						this.ReadyForNextToil();
					}
					else
					{
						this.ResetTicksToPickHit();
					}
				}
			};
			mine.defaultCompleteMode = ToilCompleteMode.Never;
			mine.WithProgressBar(TargetIndex.A, () => 1f - (float)this.MineTarget.HitPoints / (float)this.MineTarget.MaxHitPoints, false, -0.5f);
			mine.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			mine.activeSkill = (() => SkillDefOf.Mining);
			yield return mine;
			yield break;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x00022E70 File Offset: 0x00021270
		private void ResetTicksToPickHit()
		{
			float num = this.pawn.GetStatValue(StatDefOf.MiningSpeed, true);
			if (num < 0.5f && this.pawn.Faction != Faction.OfPlayer)
			{
				num = 0.5f;
			}
			this.ticksToPickHit = (int)Math.Round((double)(120f / num));
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00022ECA File Offset: 0x000212CA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToPickHit, "ticksToPickHit", 0, false);
		}
	}
}
