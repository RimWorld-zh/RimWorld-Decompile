using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DB2 RID: 3506
	public class SampleOneShot : Sample
	{
		// Token: 0x04003437 RID: 13367
		public SoundInfo info;

		// Token: 0x04003438 RID: 13368
		private SoundParams externalParams = new SoundParams();

		// Token: 0x06004E58 RID: 20056 RVA: 0x0028FA36 File Offset: 0x0028DE36
		private SampleOneShot(SubSoundDef def) : base(def)
		{
		}

		// Token: 0x17000C9F RID: 3231
		// (get) Token: 0x06004E59 RID: 20057 RVA: 0x0028FA4C File Offset: 0x0028DE4C
		public override float ParentStartRealTime
		{
			get
			{
				return this.startRealTime;
			}
		}

		// Token: 0x17000CA0 RID: 3232
		// (get) Token: 0x06004E5A RID: 20058 RVA: 0x0028FA68 File Offset: 0x0028DE68
		public override float ParentStartTick
		{
			get
			{
				return (float)this.startTick;
			}
		}

		// Token: 0x17000CA1 RID: 3233
		// (get) Token: 0x06004E5B RID: 20059 RVA: 0x0028FA84 File Offset: 0x0028DE84
		public override float ParentHashCode
		{
			get
			{
				return (float)this.GetHashCode();
			}
		}

		// Token: 0x17000CA2 RID: 3234
		// (get) Token: 0x06004E5C RID: 20060 RVA: 0x0028FAA0 File Offset: 0x0028DEA0
		public override SoundParams ExternalParams
		{
			get
			{
				return this.externalParams;
			}
		}

		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x06004E5D RID: 20061 RVA: 0x0028FABC File Offset: 0x0028DEBC
		public override SoundInfo Info
		{
			get
			{
				return this.info;
			}
		}

		// Token: 0x06004E5E RID: 20062 RVA: 0x0028FAD8 File Offset: 0x0028DED8
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
	}
}
