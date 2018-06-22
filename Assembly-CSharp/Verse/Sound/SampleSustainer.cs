using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DB1 RID: 3505
	public class SampleSustainer : Sample
	{
		// Token: 0x06004E65 RID: 20069 RVA: 0x0028FDB3 File Offset: 0x0028E1B3
		private SampleSustainer(SubSoundDef def) : base(def)
		{
		}

		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x06004E66 RID: 20070 RVA: 0x0028FDC4 File Offset: 0x0028E1C4
		public override float ParentStartRealTime
		{
			get
			{
				return this.subSustainer.creationRealTime;
			}
		}

		// Token: 0x17000CA7 RID: 3239
		// (get) Token: 0x06004E67 RID: 20071 RVA: 0x0028FDE4 File Offset: 0x0028E1E4
		public override float ParentStartTick
		{
			get
			{
				return (float)this.subSustainer.creationTick;
			}
		}

		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x06004E68 RID: 20072 RVA: 0x0028FE08 File Offset: 0x0028E208
		public override float ParentHashCode
		{
			get
			{
				return (float)this.subSustainer.GetHashCode();
			}
		}

		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x06004E69 RID: 20073 RVA: 0x0028FE2C File Offset: 0x0028E22C
		public override SoundParams ExternalParams
		{
			get
			{
				return this.subSustainer.ExternalParams;
			}
		}

		// Token: 0x17000CAA RID: 3242
		// (get) Token: 0x06004E6A RID: 20074 RVA: 0x0028FE4C File Offset: 0x0028E24C
		public override SoundInfo Info
		{
			get
			{
				return this.subSustainer.Info;
			}
		}

		// Token: 0x17000CAB RID: 3243
		// (get) Token: 0x06004E6B RID: 20075 RVA: 0x0028FE6C File Offset: 0x0028E26C
		protected override float Volume
		{
			get
			{
				float num = base.Volume * this.subSustainer.parent.scopeFader.inScopePercent;
				float num2 = 1f;
				if (this.subSustainer.parent.Ended)
				{
					num2 = 1f - Mathf.Min(this.subSustainer.parent.TimeSinceEnd / this.subDef.parentDef.sustainFadeoutTime, 1f);
				}
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				float result;
				if (base.AgeRealTime < this.subDef.sustainAttack)
				{
					if (this.resolvedSkipAttack || this.subDef.sustainAttack < 0.01f)
					{
						result = num * num2;
					}
					else
					{
						float num3 = base.AgeRealTime / this.subDef.sustainAttack;
						num3 = Mathf.Sqrt(num3);
						result = Mathf.Lerp(0f, num, num3) * num2;
					}
				}
				else if (realtimeSinceStartup > this.scheduledEndTime - this.subDef.sustainRelease)
				{
					float num4 = (realtimeSinceStartup - (this.scheduledEndTime - this.subDef.sustainRelease)) / this.subDef.sustainRelease;
					num4 = 1f - num4;
					num4 = Mathf.Max(num4, 0f);
					num4 = Mathf.Sqrt(num4);
					num4 = 1f - num4;
					result = Mathf.Lerp(num, 0f, num4) * num2;
				}
				else
				{
					result = num * num2;
				}
				return result;
			}
		}

		// Token: 0x06004E6C RID: 20076 RVA: 0x0028FFE4 File Offset: 0x0028E3E4
		public static SampleSustainer TryMakeAndPlay(SubSustainer subSus, AudioClip clip, float scheduledEndTime)
		{
			SampleSustainer sampleSustainer = new SampleSustainer(subSus.subDef);
			sampleSustainer.subSustainer = subSus;
			sampleSustainer.scheduledEndTime = scheduledEndTime;
			GameObject gameObject = new GameObject(string.Concat(new object[]
			{
				"SampleSource_",
				sampleSustainer.subDef.name,
				"_",
				sampleSustainer.startRealTime
			}));
			GameObject gameObject2 = (!subSus.subDef.onCamera) ? subSus.parent.worldRootObject : Find.Camera.gameObject;
			gameObject.transform.parent = gameObject2.transform;
			gameObject.transform.localPosition = Vector3.zero;
			sampleSustainer.source = AudioSourceMaker.NewAudioSourceOn(gameObject);
			SampleSustainer result;
			if (sampleSustainer.source == null)
			{
				if (gameObject != null)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
				result = null;
			}
			else
			{
				sampleSustainer.source.clip = clip;
				sampleSustainer.source.volume = sampleSustainer.SanitizedVolume;
				sampleSustainer.source.pitch = sampleSustainer.SanitizedPitch;
				sampleSustainer.source.minDistance = sampleSustainer.subDef.distRange.TrueMin;
				sampleSustainer.source.maxDistance = sampleSustainer.subDef.distRange.TrueMax;
				sampleSustainer.source.spatialBlend = 1f;
				List<SoundFilter> filters = sampleSustainer.subDef.filters;
				for (int i = 0; i < filters.Count; i++)
				{
					filters[i].SetupOn(sampleSustainer.source);
				}
				if (sampleSustainer.subDef.sustainLoop)
				{
					sampleSustainer.source.loop = true;
				}
				sampleSustainer.Update();
				sampleSustainer.source.Play();
				sampleSustainer.source.Play();
				result = sampleSustainer;
			}
			return result;
		}

		// Token: 0x06004E6D RID: 20077 RVA: 0x002901C0 File Offset: 0x0028E5C0
		public override void SampleCleanup()
		{
			base.SampleCleanup();
			if (this.source != null && this.source.gameObject != null)
			{
				UnityEngine.Object.Destroy(this.source.gameObject);
			}
		}

		// Token: 0x04003434 RID: 13364
		public SubSustainer subSustainer;

		// Token: 0x04003435 RID: 13365
		public float scheduledEndTime;

		// Token: 0x04003436 RID: 13366
		public bool resolvedSkipAttack = false;
	}
}
