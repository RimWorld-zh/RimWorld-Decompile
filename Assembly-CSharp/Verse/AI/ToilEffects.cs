using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse.AI
{
	// Token: 0x02000A49 RID: 2633
	public static class ToilEffects
	{
		// Token: 0x06003A8C RID: 14988 RVA: 0x001F00C0 File Offset: 0x001EE4C0
		public static Toil PlaySoundAtStart(this Toil toil, SoundDef sound)
		{
			toil.AddPreInitAction(delegate
			{
				sound.PlayOneShot(new TargetInfo(toil.GetActor().Position, toil.GetActor().Map, false));
			});
			return toil;
		}

		// Token: 0x06003A8D RID: 14989 RVA: 0x001F0108 File Offset: 0x001EE508
		public static Toil PlaySoundAtEnd(this Toil toil, SoundDef sound)
		{
			toil.AddFinishAction(delegate
			{
				sound.PlayOneShot(new TargetInfo(toil.GetActor().Position, toil.GetActor().Map, false));
			});
			return toil;
		}

		// Token: 0x06003A8E RID: 14990 RVA: 0x001F0150 File Offset: 0x001EE550
		public static Toil PlaySustainerOrSound(this Toil toil, SoundDef soundDef)
		{
			return toil.PlaySustainerOrSound(() => soundDef);
		}

		// Token: 0x06003A8F RID: 14991 RVA: 0x001F0184 File Offset: 0x001EE584
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

		// Token: 0x06003A90 RID: 14992 RVA: 0x001F01E8 File Offset: 0x001EE5E8
		public static Toil WithEffect(this Toil toil, EffecterDef effectDef, TargetIndex ind)
		{
			return toil.WithEffect(() => effectDef, ind);
		}

		// Token: 0x06003A91 RID: 14993 RVA: 0x001F0220 File Offset: 0x001EE620
		public static Toil WithEffect(this Toil toil, Func<EffecterDef> effecterDefGetter, TargetIndex ind)
		{
			return toil.WithEffect(effecterDefGetter, () => toil.actor.CurJob.GetTarget(ind));
		}

		// Token: 0x06003A92 RID: 14994 RVA: 0x001F0264 File Offset: 0x001EE664
		public static Toil WithEffect(this Toil toil, Func<EffecterDef> effecterDefGetter, Thing thing)
		{
			return toil.WithEffect(effecterDefGetter, () => thing);
		}

		// Token: 0x06003A93 RID: 14995 RVA: 0x001F029C File Offset: 0x001EE69C
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

		// Token: 0x06003A94 RID: 14996 RVA: 0x001F0308 File Offset: 0x001EE708
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

		// Token: 0x06003A95 RID: 14997 RVA: 0x001F0384 File Offset: 0x001EE784
		public static Toil WithProgressBarToilDelay(this Toil toil, TargetIndex ind, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f)
		{
			return toil.WithProgressBar(ind, () => 1f - (float)toil.actor.jobs.curDriver.ticksLeftThisToil / (float)toil.defaultDuration, interpolateBetweenActorAndTarget, offsetZ);
		}

		// Token: 0x06003A96 RID: 14998 RVA: 0x001F03C0 File Offset: 0x001EE7C0
		public static Toil WithProgressBarToilDelay(this Toil toil, TargetIndex ind, int toilDuration, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f)
		{
			return toil.WithProgressBar(ind, () => 1f - (float)toil.actor.jobs.curDriver.ticksLeftThisToil / (float)toilDuration, interpolateBetweenActorAndTarget, offsetZ);
		}
	}
}
