using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse.AI
{
	// Token: 0x02000A45 RID: 2629
	public static class ToilEffects
	{
		// Token: 0x06003A86 RID: 14982 RVA: 0x001F0300 File Offset: 0x001EE700
		public static Toil PlaySoundAtStart(this Toil toil, SoundDef sound)
		{
			toil.AddPreInitAction(delegate
			{
				sound.PlayOneShot(new TargetInfo(toil.GetActor().Position, toil.GetActor().Map, false));
			});
			return toil;
		}

		// Token: 0x06003A87 RID: 14983 RVA: 0x001F0348 File Offset: 0x001EE748
		public static Toil PlaySoundAtEnd(this Toil toil, SoundDef sound)
		{
			toil.AddFinishAction(delegate
			{
				sound.PlayOneShot(new TargetInfo(toil.GetActor().Position, toil.GetActor().Map, false));
			});
			return toil;
		}

		// Token: 0x06003A88 RID: 14984 RVA: 0x001F0390 File Offset: 0x001EE790
		public static Toil PlaySustainerOrSound(this Toil toil, SoundDef soundDef)
		{
			return toil.PlaySustainerOrSound(() => soundDef);
		}

		// Token: 0x06003A89 RID: 14985 RVA: 0x001F03C4 File Offset: 0x001EE7C4
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

		// Token: 0x06003A8A RID: 14986 RVA: 0x001F0428 File Offset: 0x001EE828
		public static Toil WithEffect(this Toil toil, EffecterDef effectDef, TargetIndex ind)
		{
			return toil.WithEffect(() => effectDef, ind);
		}

		// Token: 0x06003A8B RID: 14987 RVA: 0x001F0460 File Offset: 0x001EE860
		public static Toil WithEffect(this Toil toil, Func<EffecterDef> effecterDefGetter, TargetIndex ind)
		{
			return toil.WithEffect(effecterDefGetter, () => toil.actor.CurJob.GetTarget(ind));
		}

		// Token: 0x06003A8C RID: 14988 RVA: 0x001F04A4 File Offset: 0x001EE8A4
		public static Toil WithEffect(this Toil toil, Func<EffecterDef> effecterDefGetter, Thing thing)
		{
			return toil.WithEffect(effecterDefGetter, () => thing);
		}

		// Token: 0x06003A8D RID: 14989 RVA: 0x001F04DC File Offset: 0x001EE8DC
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

		// Token: 0x06003A8E RID: 14990 RVA: 0x001F0548 File Offset: 0x001EE948
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

		// Token: 0x06003A8F RID: 14991 RVA: 0x001F05C4 File Offset: 0x001EE9C4
		public static Toil WithProgressBarToilDelay(this Toil toil, TargetIndex ind, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f)
		{
			return toil.WithProgressBar(ind, () => 1f - (float)toil.actor.jobs.curDriver.ticksLeftThisToil / (float)toil.defaultDuration, interpolateBetweenActorAndTarget, offsetZ);
		}

		// Token: 0x06003A90 RID: 14992 RVA: 0x001F0600 File Offset: 0x001EEA00
		public static Toil WithProgressBarToilDelay(this Toil toil, TargetIndex ind, int toilDuration, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f)
		{
			return toil.WithProgressBar(ind, () => 1f - (float)toil.actor.jobs.curDriver.ticksLeftThisToil / (float)toilDuration, interpolateBetweenActorAndTarget, offsetZ);
		}
	}
}
