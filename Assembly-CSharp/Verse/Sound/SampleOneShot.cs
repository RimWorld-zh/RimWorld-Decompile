using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DB2 RID: 3506
	public class SampleOneShot : Sample
	{
		// Token: 0x06004E3F RID: 20031 RVA: 0x0028E07A File Offset: 0x0028C47A
		private SampleOneShot(SubSoundDef def) : base(def)
		{
		}

		// Token: 0x17000C9E RID: 3230
		// (get) Token: 0x06004E40 RID: 20032 RVA: 0x0028E090 File Offset: 0x0028C490
		public override float ParentStartRealTime
		{
			get
			{
				return this.startRealTime;
			}
		}

		// Token: 0x17000C9F RID: 3231
		// (get) Token: 0x06004E41 RID: 20033 RVA: 0x0028E0AC File Offset: 0x0028C4AC
		public override float ParentStartTick
		{
			get
			{
				return (float)this.startTick;
			}
		}

		// Token: 0x17000CA0 RID: 3232
		// (get) Token: 0x06004E42 RID: 20034 RVA: 0x0028E0C8 File Offset: 0x0028C4C8
		public override float ParentHashCode
		{
			get
			{
				return (float)this.GetHashCode();
			}
		}

		// Token: 0x17000CA1 RID: 3233
		// (get) Token: 0x06004E43 RID: 20035 RVA: 0x0028E0E4 File Offset: 0x0028C4E4
		public override SoundParams ExternalParams
		{
			get
			{
				return this.externalParams;
			}
		}

		// Token: 0x17000CA2 RID: 3234
		// (get) Token: 0x06004E44 RID: 20036 RVA: 0x0028E100 File Offset: 0x0028C500
		public override SoundInfo Info
		{
			get
			{
				return this.info;
			}
		}

		// Token: 0x06004E45 RID: 20037 RVA: 0x0028E11C File Offset: 0x0028C51C
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

		// Token: 0x04003425 RID: 13349
		public SoundInfo info;

		// Token: 0x04003426 RID: 13350
		private SoundParams externalParams = new SoundParams();
	}
}
