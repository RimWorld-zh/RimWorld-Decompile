using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobDriver_Mine : JobDriver
	{
		private int ticksToPickHit = -1000;

		private Effecter effecter = null;

		public const int BaseTicksBetweenPickHits = 120;

		private const int BaseDamagePerPickHit_NaturalRock = 80;

		private const int BaseDamagePerPickHit_NotNaturalRock = 40;

		private const float MinMiningSpeedFactorForNPCs = 0.5f;

		public JobDriver_Mine()
		{
		}

		private Thing MineTarget
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		public override bool TryMakePreToilReservations()
		{
			return this.pawn.Reserve(this.MineTarget, this.job, 1, -1, null);
		}

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
					int num = (!mineTarget.def.building.isNaturalRock) ? 40 : 80;
					Mineable mineable = mineTarget as Mineable;
					if (mineable == null || mineTarget.HitPoints > num)
					{
						DamageDef mining = DamageDefOf.Mining;
						float amount = (float)num;
						Pawn actor2 = mine.actor;
						DamageInfo dinfo = new DamageInfo(mining, amount, 0f, -1f, actor2, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
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

		private void ResetTicksToPickHit()
		{
			float num = this.pawn.GetStatValue(StatDefOf.MiningSpeed, true);
			if (num < 0.5f && this.pawn.Faction != Faction.OfPlayer)
			{
				num = 0.5f;
			}
			this.ticksToPickHit = (int)Math.Round((double)(120f / num));
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToPickHit, "ticksToPickHit", 0, false);
		}

		[CompilerGenerated]
		private sealed class <MakeNewToils>c__Iterator0 : IEnumerable, IEnumerable<Toil>, IEnumerator, IDisposable, IEnumerator<Toil>
		{
			internal JobDriver_Mine $this;

			internal Toil $current;

			internal bool $disposing;

			internal int $PC;

			private JobDriver_Mine.<MakeNewToils>c__Iterator0.<MakeNewToils>c__AnonStorey1 $locvar0;

			private static Func<SkillDef> <>f__am$cache0;

			[DebuggerHidden]
			public <MakeNewToils>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
					this.FailOnCellMissingDesignation(TargetIndex.A, DesignationDefOf.Mine);
					this.$current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					<MakeNewToils>c__AnonStorey.mine = new Toil();
					<MakeNewToils>c__AnonStorey.mine.tickAction = delegate()
					{
						Pawn actor = <MakeNewToils>c__AnonStorey.mine.actor;
						Thing mineTarget = <MakeNewToils>c__AnonStorey.<>f__ref$0.$this.MineTarget;
						if (<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.ticksToPickHit < -100)
						{
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.ResetTicksToPickHit();
						}
						if (actor.skills != null && (mineTarget.Faction != actor.Faction || actor.Faction == null))
						{
							actor.skills.Learn(SkillDefOf.Mining, 0.077f, false);
						}
						<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.ticksToPickHit--;
						if (<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.ticksToPickHit <= 0)
						{
							IntVec3 position = mineTarget.Position;
							if (<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.effecter == null)
							{
								<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.effecter = EffecterDefOf.Mine.Spawn();
							}
							<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.effecter.Trigger(actor, mineTarget);
							int num2 = (!mineTarget.def.building.isNaturalRock) ? 40 : 80;
							Mineable mineable = mineTarget as Mineable;
							if (mineable == null || mineTarget.HitPoints > num2)
							{
								DamageDef mining = DamageDefOf.Mining;
								float amount = (float)num2;
								Pawn actor2 = <MakeNewToils>c__AnonStorey.mine.actor;
								DamageInfo dinfo = new DamageInfo(mining, amount, 0f, -1f, actor2, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
								mineTarget.TakeDamage(dinfo);
							}
							else
							{
								mineable.Notify_TookMiningDamage(mineTarget.HitPoints, <MakeNewToils>c__AnonStorey.mine.actor);
								mineable.HitPoints = 0;
								mineable.DestroyMined(actor);
							}
							if (mineTarget.Destroyed)
							{
								actor.Map.mineStrikeManager.CheckStruckOre(position, mineTarget.def, actor);
								actor.records.Increment(RecordDefOf.CellsMined);
								if (<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn.Faction != Faction.OfPlayer)
								{
									List<Thing> thingList = position.GetThingList(<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.Map);
									for (int i = 0; i < thingList.Count; i++)
									{
										thingList[i].SetForbidden(true, false);
									}
								}
								if (<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn.Faction == Faction.OfPlayer && MineStrikeManager.MineableIsVeryValuable(mineTarget.def))
								{
									TaleRecorder.RecordTale(TaleDefOf.MinedValuable, new object[]
									{
										<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn,
										mineTarget.def.building.mineableThing
									});
								}
								if (<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn.Faction == Faction.OfPlayer && MineStrikeManager.MineableIsValuable(mineTarget.def) && !<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn.Map.IsPlayerHome)
								{
									TaleRecorder.RecordTale(TaleDefOf.CaravanRemoteMining, new object[]
									{
										<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.pawn,
										mineTarget.def.building.mineableThing
									});
								}
								<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.ReadyForNextToil();
							}
							else
							{
								<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.ResetTicksToPickHit();
							}
						}
					};
					<MakeNewToils>c__AnonStorey.mine.defaultCompleteMode = ToilCompleteMode.Never;
					<MakeNewToils>c__AnonStorey.mine.WithProgressBar(TargetIndex.A, () => 1f - (float)<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.MineTarget.HitPoints / (float)<MakeNewToils>c__AnonStorey.<>f__ref$0.$this.MineTarget.MaxHitPoints, false, -0.5f);
					<MakeNewToils>c__AnonStorey.mine.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
					<MakeNewToils>c__AnonStorey.mine.activeSkill = (() => SkillDefOf.Mining);
					this.$current = <MakeNewToils>c__AnonStorey.mine;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			Toil IEnumerator<Toil>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.AI.Toil>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				JobDriver_Mine.<MakeNewToils>c__Iterator0 <MakeNewToils>c__Iterator = new JobDriver_Mine.<MakeNewToils>c__Iterator0();
				<MakeNewToils>c__Iterator.$this = this;
				return <MakeNewToils>c__Iterator;
			}

			private static SkillDef <>m__0()
			{
				return SkillDefOf.Mining;
			}

			private sealed class <MakeNewToils>c__AnonStorey1
			{
				internal Toil mine;

				internal JobDriver_Mine.<MakeNewToils>c__Iterator0 <>f__ref$0;

				public <MakeNewToils>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					Pawn actor = this.mine.actor;
					Thing mineTarget = this.<>f__ref$0.$this.MineTarget;
					if (this.<>f__ref$0.$this.ticksToPickHit < -100)
					{
						this.<>f__ref$0.$this.ResetTicksToPickHit();
					}
					if (actor.skills != null && (mineTarget.Faction != actor.Faction || actor.Faction == null))
					{
						actor.skills.Learn(SkillDefOf.Mining, 0.077f, false);
					}
					this.<>f__ref$0.$this.ticksToPickHit--;
					if (this.<>f__ref$0.$this.ticksToPickHit <= 0)
					{
						IntVec3 position = mineTarget.Position;
						if (this.<>f__ref$0.$this.effecter == null)
						{
							this.<>f__ref$0.$this.effecter = EffecterDefOf.Mine.Spawn();
						}
						this.<>f__ref$0.$this.effecter.Trigger(actor, mineTarget);
						int num = (!mineTarget.def.building.isNaturalRock) ? 40 : 80;
						Mineable mineable = mineTarget as Mineable;
						if (mineable == null || mineTarget.HitPoints > num)
						{
							DamageDef mining = DamageDefOf.Mining;
							float amount = (float)num;
							Pawn actor2 = this.mine.actor;
							DamageInfo dinfo = new DamageInfo(mining, amount, 0f, -1f, actor2, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
							mineTarget.TakeDamage(dinfo);
						}
						else
						{
							mineable.Notify_TookMiningDamage(mineTarget.HitPoints, this.mine.actor);
							mineable.HitPoints = 0;
							mineable.DestroyMined(actor);
						}
						if (mineTarget.Destroyed)
						{
							actor.Map.mineStrikeManager.CheckStruckOre(position, mineTarget.def, actor);
							actor.records.Increment(RecordDefOf.CellsMined);
							if (this.<>f__ref$0.$this.pawn.Faction != Faction.OfPlayer)
							{
								List<Thing> thingList = position.GetThingList(this.<>f__ref$0.$this.Map);
								for (int i = 0; i < thingList.Count; i++)
								{
									thingList[i].SetForbidden(true, false);
								}
							}
							if (this.<>f__ref$0.$this.pawn.Faction == Faction.OfPlayer && MineStrikeManager.MineableIsVeryValuable(mineTarget.def))
							{
								TaleRecorder.RecordTale(TaleDefOf.MinedValuable, new object[]
								{
									this.<>f__ref$0.$this.pawn,
									mineTarget.def.building.mineableThing
								});
							}
							if (this.<>f__ref$0.$this.pawn.Faction == Faction.OfPlayer && MineStrikeManager.MineableIsValuable(mineTarget.def) && !this.<>f__ref$0.$this.pawn.Map.IsPlayerHome)
							{
								TaleRecorder.RecordTale(TaleDefOf.CaravanRemoteMining, new object[]
								{
									this.<>f__ref$0.$this.pawn,
									mineTarget.def.building.mineableThing
								});
							}
							this.<>f__ref$0.$this.ReadyForNextToil();
						}
						else
						{
							this.<>f__ref$0.$this.ResetTicksToPickHit();
						}
					}
				}

				internal float <>m__1()
				{
					return 1f - (float)this.<>f__ref$0.$this.MineTarget.HitPoints / (float)this.<>f__ref$0.$this.MineTarget.MaxHitPoints;
				}
			}
		}
	}
}
