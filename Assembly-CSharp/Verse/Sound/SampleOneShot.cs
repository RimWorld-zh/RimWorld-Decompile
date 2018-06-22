using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DAF RID: 3503
	public class SampleOneShot : Sample
	{
		// Token: 0x06004E54 RID: 20052 RVA: 0x0028F62A File Offset: 0x0028DA2A
		private SampleOneShot(SubSoundDef def) : base(def)
		{
		}

		// Token: 0x17000CA0 RID: 3232
		// (get) Token: 0x06004E55 RID: 20053 RVA: 0x0028F640 File Offset: 0x0028DA40
		public override float ParentStartRealTime
		{
			get
			{
				return this.startRealTime;
			}
		}

		// Token: 0x17000CA1 RID: 3233
		// (get) Token: 0x06004E56 RID: 20054 RVA: 0x0028F65C File Offset: 0x0028DA5C
		public override float ParentStartTick
		{
			get
			{
				return (float)this.startTick;
			}
		}

		// Token: 0x17000CA2 RID: 3234
		// (get) Token: 0x06004E57 RID: 20055 RVA: 0x0028F678 File Offset: 0x0028DA78
		public override float ParentHashCode
		{
			get
			{
				return (float)this.GetHashCode();
			}
		}

		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x06004E58 RID: 20056 RVA: 0x0028F694 File Offset: 0x0028DA94
		public override SoundParams ExternalParams
		{
			get
			{
				return this.externalParams;
			}
		}

		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x06004E59 RID: 20057 RVA: 0x0028F6B0 File Offset: 0x0028DAB0
		public override SoundInfo Info
		{
			get
			{
				return this.info;
			}
		}

		// Token: 0x06004E5A RID: 20058 RVA: 0x0028F6CC File Offset: 0x0028DACC
		public static SampleOneShot TryMakeAndPlay(SubSoundDef def, AudioClip clip, SoundInfo info)
		{
			SampleOneShot result;
			if ((double)info.pitchFactor <= 0.0001)
			{
				Log.ErrorOnce(string.Concat(new object[]
				{
					"Played sound with pitchFactor ",
					info.pitchFactor,
					": ",
					def,
					", ",
					info
				}), 632321, false);
				result = null;
			}
			else
			{
				SampleOneShot sampleOneShot = new SampleOneShot(def);
				sampleOneShot.info = info;
				sampleOneShot.source = Find.SoundRoot.sourcePool.GetSource(def.onCamera);
				if (sampleOneShot.source == null)
				{
					result = null;
				}
				else
				{
					sampleOneShot.source.clip = clip;
					sampleOneShot.source.volume = sampleOneShot.SanitizedVolume;
					sampleOneShot.source.pitch = sampleOneShot.SanitizedPitch;
					sampleOneShot.source.minDistance = sampleOneShot.subDef.distRange.TrueMin;
					sampleOneShot.source.maxDistance = sampleOneShot.subDef.distRange.TrueMax;
					if (!def.onCamera)
					{
						sampleOneShot.source.gameObject.transform.position = info.Maker.Cell.ToVector3ShiftedWithAltitude(0f);
						sampleOneShot.source.minDistance = def.distRange.TrueMin;
						sampleOneShot.source.maxDistance = def.distRange.TrueMax;
						sampleOneShot.source.spatialBlend = 1f;
					}
					else
					{
						sampleOneShot.source.spatialBlend = 0f;
					}
					for (int i = 0; i < def.filters.Count; i++)
					{
						def.filters[i].SetupOn(sampleOneShot.source);
					}
					foreach (KeyValuePair<string, float> keyValuePair in info.DefinedParameters)
					{
						sampleOneShot.externalParams[keyValuePair.Key] = keyValuePair.Value;
					}
					sampleOneShot.Update();
					sampleOneShot.source.Play();
					Find.SoundRoot.oneShotManager.TryAddPlayingOneShot(sampleOneShot);
					result = sampleOneShot;
				}
			}
			return result;
		}

		// Token: 0x04003430 RID: 13360
		public SoundInfo info;

		// Token: 0x04003431 RID: 13361
		private SoundParams externalParams = new SoundParams();
	}
}
