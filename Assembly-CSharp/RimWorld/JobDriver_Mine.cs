using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Mine : JobDriver
	{
		public const int BaseTicksBetweenPickHits = 120;

		private const int BaseDamagePerPickHit = 80;

		private const float MinMiningSpeedForNPCs = 0.5f;

		private int ticksToPickHit = -1000;

		private Effecter effecter;

		private Thing MineTarget
		{
			get
			{
				return base.CurJob.GetTarget(TargetIndex.A).Thing;
			}
		}

		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnCellMissingDesignation(TargetIndex.A, DesignationDefOf.Mine);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil mine = new Toil
			{
				tickAction = (Action)delegate
				{
					Pawn actor = ((_003CMakeNewToils_003Ec__Iterator35)/*Error near IL_008d: stateMachine*/)._003Cmine_003E__0.actor;
					Thing mineTarget = ((_003CMakeNewToils_003Ec__Iterator35)/*Error near IL_008d: stateMachine*/)._003C_003Ef__this.MineTarget;
					if (((_003CMakeNewToils_003Ec__Iterator35)/*Error near IL_008d: stateMachine*/)._003C_003Ef__this.ticksToPickHit < -100)
					{
						((_003CMakeNewToils_003Ec__Iterator35)/*Error near IL_008d: stateMachine*/)._003C_003Ef__this.ResetTicksToPickHit();
					}
					if (actor.skills != null)
					{
						actor.skills.Learn(SkillDefOf.Mining, 0.11f, false);
					}
					((_003CMakeNewToils_003Ec__Iterator35)/*Error near IL_008d: stateMachine*/)._003C_003Ef__this.ticksToPickHit--;
					if (((_003CMakeNewToils_003Ec__Iterator35)/*Error near IL_008d: stateMachine*/)._003C_003Ef__this.ticksToPickHit <= 0)
					{
						IntVec3 position = mineTarget.Position;
						if (((_003CMakeNewToils_003Ec__Iterator35)/*Error near IL_008d: stateMachine*/)._003C_003Ef__this.effecter == null)
						{
							((_003CMakeNewToils_003Ec__Iterator35)/*Error near IL_008d: stateMachine*/)._003C_003Ef__this.effecter = EffecterDefOf.Mine.Spawn();
						}
						((_003CMakeNewToils_003Ec__Iterator35)/*Error near IL_008d: stateMachine*/)._003C_003Ef__this.effecter.Trigger((Thing)actor, mineTarget);
						int num = 80;
						Mineable mineable = mineTarget as Mineable;
						if (mineable == null || mineTarget.HitPoints > num)
						{
							Pawn actor2 = ((_003CMakeNewToils_003Ec__Iterator35)/*Error near IL_008d: stateMachine*/)._003Cmine_003E__0.actor;
							DamageInfo dinfo = new DamageInfo(DamageDefOf.Mining, num, -1f, actor2, null, null, DamageInfo.SourceCategory.ThingOrUnknown);
							mineTarget.TakeDamage(dinfo);
						}
						else
						{
							mineable.DestroyMined(actor);
						}
						if (mineTarget.Destroyed)
						{
							actor.Map.mineStrikeManager.CheckStruckOre(position, mineTarget.def, actor);
							actor.records.Increment(RecordDefOf.CellsMined);
							if (((_003CMakeNewToils_003Ec__Iterator35)/*Error near IL_008d: stateMachine*/)._003C_003Ef__this.pawn.Faction != Faction.OfPlayer)
							{
								List<Thing> thingList = position.GetThingList(((_003CMakeNewToils_003Ec__Iterator35)/*Error near IL_008d: stateMachine*/)._003C_003Ef__this.Map);
								for (int i = 0; i < thingList.Count; i++)
								{
									thingList[i].SetForbidden(true, false);
								}
							}
							((_003CMakeNewToils_003Ec__Iterator35)/*Error near IL_008d: stateMachine*/)._003C_003Ef__this.ReadyForNextToil();
						}
						else
						{
							((_003CMakeNewToils_003Ec__Iterator35)/*Error near IL_008d: stateMachine*/)._003C_003Ef__this.ResetTicksToPickHit();
						}
					}
				},
				defaultCompleteMode = ToilCompleteMode.Never
			};
			mine.WithProgressBar(TargetIndex.A, (Func<float>)(() => (float)(1.0 - (float)((_003CMakeNewToils_003Ec__Iterator35)/*Error near IL_00b1: stateMachine*/)._003C_003Ef__this.MineTarget.HitPoints / (float)((_003CMakeNewToils_003Ec__Iterator35)/*Error near IL_00b1: stateMachine*/)._003C_003Ef__this.MineTarget.MaxHitPoints)), false, -0.5f);
			mine.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			yield return mine;
		}

		private void ResetTicksToPickHit()
		{
			float num = base.pawn.GetStatValue(StatDefOf.MiningSpeed, true);
			if (num < 0.5 && base.pawn.Faction != Faction.OfPlayer)
			{
				num = 0.5f;
			}
			this.ticksToPickHit = (int)Math.Round(120.0 / num);
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToPickHit, "ticksToPickHit", 0, false);
		}
	}
}
