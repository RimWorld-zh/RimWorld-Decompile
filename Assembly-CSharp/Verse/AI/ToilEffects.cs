using System;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse.AI
{
	public static class ToilEffects
	{
		public static Toil PlaySoundAtStart(this Toil toil, SoundDef sound)
		{
			toil.AddPreInitAction(delegate
			{
				sound.PlayOneShot(new TargetInfo(toil.GetActor().Position, toil.GetActor().Map, false));
			});
			return toil;
		}

		public static Toil PlaySoundAtEnd(this Toil toil, SoundDef sound)
		{
			toil.AddFinishAction(delegate
			{
				sound.PlayOneShot(new TargetInfo(toil.GetActor().Position, toil.GetActor().Map, false));
			});
			return toil;
		}

		public static Toil PlaySustainerOrSound(this Toil toil, SoundDef soundDef)
		{
			return toil.PlaySustainerOrSound(() => soundDef);
		}

		public static Toil PlaySustainerOrSound(this Toil toil, Func<SoundDef> soundDefGetter)
		{
			Sustainer sustainer = null;
			toil.AddPreInitAction(delegate
			{
				SoundDef soundDef = soundDefGetter();
				if (soundDef != null && !soundDef.sustain)
				{
					soundDef.PlayOneShot(new TargetInfo(toil.GetActor().Position, toil.GetActor().Map, false));
				}
			});
			toil.AddPreTickAction(delegate
			{
				if (sustainer == null || sustainer.Ended)
				{
					SoundDef soundDef = soundDefGetter();
					if (soundDef != null && soundDef.sustain)
					{
						SoundInfo info = SoundInfo.InMap(toil.actor, MaintenanceType.PerTick);
						sustainer = soundDef.TrySpawnSustainer(info);
					}
				}
				else
				{
					sustainer.Maintain();
				}
			});
			return toil;
		}

		public static Toil WithEffect(this Toil toil, EffecterDef effectDef, TargetIndex ind)
		{
			return toil.WithEffect(() => effectDef, ind);
		}

		public static Toil WithEffect(this Toil toil, Func<EffecterDef> effecterDefGetter, TargetIndex ind)
		{
			return toil.WithEffect(effecterDefGetter, () => toil.actor.CurJob.GetTarget(ind));
		}

		public static Toil WithEffect(this Toil toil, Func<EffecterDef> effecterDefGetter, Thing thing)
		{
			return toil.WithEffect(effecterDefGetter, () => thing);
		}

		public static Toil WithEffect(this Toil toil, Func<EffecterDef> effecterDefGetter, Func<LocalTargetInfo> effectTargetGetter)
		{
			Effecter effecter = null;
			toil.AddPreTickAction(delegate
			{
				if (effecter == null)
				{
					EffecterDef effecterDef = effecterDefGetter();
					if (effecterDef != null)
					{
						effecter = effecterDef.Spawn();
					}
				}
				else
				{
					effecter.EffectTick(toil.actor, effectTargetGetter().ToTargetInfo(toil.actor.Map));
				}
			});
			toil.AddFinishAction(delegate
			{
				if (effecter != null)
				{
					effecter.Cleanup();
					effecter = null;
				}
			});
			return toil;
		}

		public static Toil WithProgressBar(this Toil toil, TargetIndex ind, Func<float> progressGetter, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f)
		{
			Effecter effecter = null;
			toil.AddPreTickAction(delegate
			{
				if (toil.actor.Faction == Faction.OfPlayer)
				{
					if (effecter == null)
					{
						EffecterDef progressBar = EffecterDefOf.ProgressBar;
						effecter = progressBar.Spawn();
					}
					else
					{
						LocalTargetInfo target = toil.actor.CurJob.GetTarget(ind);
						if (!target.IsValid || (target.HasThing && !target.Thing.Spawned))
						{
							effecter.EffectTick(toil.actor, TargetInfo.Invalid);
						}
						else if (interpolateBetweenActorAndTarget)
						{
							effecter.EffectTick(toil.actor.CurJob.GetTarget(ind).ToTargetInfo(toil.actor.Map), toil.actor);
						}
						else
						{
							effecter.EffectTick(toil.actor.CurJob.GetTarget(ind).ToTargetInfo(toil.actor.Map), TargetInfo.Invalid);
						}
						MoteProgressBar mote = ((SubEffecter_ProgressBar)effecter.children[0]).mote;
						if (mote != null)
						{
							mote.progress = Mathf.Clamp01(progressGetter());
							mote.offsetZ = offsetZ;
						}
					}
				}
			});
			toil.AddFinishAction(delegate
			{
				if (effecter != null)
				{
					effecter.Cleanup();
					effecter = null;
				}
			});
			return toil;
		}

		public static Toil WithProgressBarToilDelay(this Toil toil, TargetIndex ind, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f)
		{
			return toil.WithProgressBar(ind, () => 1f - (float)toil.actor.jobs.curDriver.ticksLeftThisToil / (float)toil.defaultDuration, interpolateBetweenActorAndTarget, offsetZ);
		}

		public static Toil WithProgressBarToilDelay(this Toil toil, TargetIndex ind, int toilDuration, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f)
		{
			return toil.WithProgressBar(ind, () => 1f - (float)toil.actor.jobs.curDriver.ticksLeftThisToil / (float)toilDuration, interpolateBetweenActorAndTarget, offsetZ);
		}

		[CompilerGenerated]
		private sealed class <PlaySoundAtStart>c__AnonStorey0
		{
			internal SoundDef sound;

			internal Toil toil;

			public <PlaySoundAtStart>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				this.sound.PlayOneShot(new TargetInfo(this.toil.GetActor().Position, this.toil.GetActor().Map, false));
			}
		}

		[CompilerGenerated]
		private sealed class <PlaySoundAtEnd>c__AnonStorey1
		{
			internal SoundDef sound;

			internal Toil toil;

			public <PlaySoundAtEnd>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				this.sound.PlayOneShot(new TargetInfo(this.toil.GetActor().Position, this.toil.GetActor().Map, false));
			}
		}

		[CompilerGenerated]
		private sealed class <PlaySustainerOrSound>c__AnonStorey2
		{
			internal SoundDef soundDef;

			public <PlaySustainerOrSound>c__AnonStorey2()
			{
			}

			internal SoundDef <>m__0()
			{
				return this.soundDef;
			}
		}

		[CompilerGenerated]
		private sealed class <PlaySustainerOrSound>c__AnonStorey3
		{
			internal Func<SoundDef> soundDefGetter;

			internal Toil toil;

			internal Sustainer sustainer;

			public <PlaySustainerOrSound>c__AnonStorey3()
			{
			}

			internal void <>m__0()
			{
				SoundDef soundDef = this.soundDefGetter();
				if (soundDef != null && !soundDef.sustain)
				{
					soundDef.PlayOneShot(new TargetInfo(this.toil.GetActor().Position, this.toil.GetActor().Map, false));
				}
			}

			internal void <>m__1()
			{
				if (this.sustainer == null || this.sustainer.Ended)
				{
					SoundDef soundDef = this.soundDefGetter();
					if (soundDef != null && soundDef.sustain)
					{
						SoundInfo info = SoundInfo.InMap(this.toil.actor, MaintenanceType.PerTick);
						this.sustainer = soundDef.TrySpawnSustainer(info);
					}
				}
				else
				{
					this.sustainer.Maintain();
				}
			}
		}

		[CompilerGenerated]
		private sealed class <WithEffect>c__AnonStorey4
		{
			internal EffecterDef effectDef;

			public <WithEffect>c__AnonStorey4()
			{
			}

			internal EffecterDef <>m__0()
			{
				return this.effectDef;
			}
		}

		[CompilerGenerated]
		private sealed class <WithEffect>c__AnonStorey5
		{
			internal Toil toil;

			internal TargetIndex ind;

			public <WithEffect>c__AnonStorey5()
			{
			}

			internal LocalTargetInfo <>m__0()
			{
				return this.toil.actor.CurJob.GetTarget(this.ind);
			}
		}

		[CompilerGenerated]
		private sealed class <WithEffect>c__AnonStorey6
		{
			internal Thing thing;

			public <WithEffect>c__AnonStorey6()
			{
			}

			internal LocalTargetInfo <>m__0()
			{
				return this.thing;
			}
		}

		[CompilerGenerated]
		private sealed class <WithEffect>c__AnonStorey7
		{
			internal Effecter effecter;

			internal Func<EffecterDef> effecterDefGetter;

			internal Toil toil;

			internal Func<LocalTargetInfo> effectTargetGetter;

			public <WithEffect>c__AnonStorey7()
			{
			}

			internal void <>m__0()
			{
				if (this.effecter == null)
				{
					EffecterDef effecterDef = this.effecterDefGetter();
					if (effecterDef != null)
					{
						this.effecter = effecterDef.Spawn();
					}
				}
				else
				{
					this.effecter.EffectTick(this.toil.actor, this.effectTargetGetter().ToTargetInfo(this.toil.actor.Map));
				}
			}

			internal void <>m__1()
			{
				if (this.effecter != null)
				{
					this.effecter.Cleanup();
					this.effecter = null;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <WithProgressBar>c__AnonStorey8
		{
			internal Toil toil;

			internal Effecter effecter;

			internal TargetIndex ind;

			internal bool interpolateBetweenActorAndTarget;

			internal Func<float> progressGetter;

			internal float offsetZ;

			public <WithProgressBar>c__AnonStorey8()
			{
			}

			internal void <>m__0()
			{
				if (this.toil.actor.Faction == Faction.OfPlayer)
				{
					if (this.effecter == null)
					{
						EffecterDef progressBar = EffecterDefOf.ProgressBar;
						this.effecter = progressBar.Spawn();
					}
					else
					{
						LocalTargetInfo target = this.toil.actor.CurJob.GetTarget(this.ind);
						if (!target.IsValid || (target.HasThing && !target.Thing.Spawned))
						{
							this.effecter.EffectTick(this.toil.actor, TargetInfo.Invalid);
						}
						else if (this.interpolateBetweenActorAndTarget)
						{
							this.effecter.EffectTick(this.toil.actor.CurJob.GetTarget(this.ind).ToTargetInfo(this.toil.actor.Map), this.toil.actor);
						}
						else
						{
							this.effecter.EffectTick(this.toil.actor.CurJob.GetTarget(this.ind).ToTargetInfo(this.toil.actor.Map), TargetInfo.Invalid);
						}
						MoteProgressBar mote = ((SubEffecter_ProgressBar)this.effecter.children[0]).mote;
						if (mote != null)
						{
							mote.progress = Mathf.Clamp01(this.progressGetter());
							mote.offsetZ = this.offsetZ;
						}
					}
				}
			}

			internal void <>m__1()
			{
				if (this.effecter != null)
				{
					this.effecter.Cleanup();
					this.effecter = null;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <WithProgressBarToilDelay>c__AnonStorey9
		{
			internal Toil toil;

			public <WithProgressBarToilDelay>c__AnonStorey9()
			{
			}

			internal float <>m__0()
			{
				return 1f - (float)this.toil.actor.jobs.curDriver.ticksLeftThisToil / (float)this.toil.defaultDuration;
			}
		}

		[CompilerGenerated]
		private sealed class <WithProgressBarToilDelay>c__AnonStoreyA
		{
			internal Toil toil;

			internal int toilDuration;

			public <WithProgressBarToilDelay>c__AnonStoreyA()
			{
			}

			internal float <>m__0()
			{
				return 1f - (float)this.toil.actor.jobs.curDriver.ticksLeftThisToil / (float)this.toilDuration;
			}
		}
	}
}
