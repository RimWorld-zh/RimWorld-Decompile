using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DB5 RID: 3509
	public class SampleSustainer : Sample
	{
		// Token: 0x06004E52 RID: 20050 RVA: 0x0028E823 File Offset: 0x0028CC23
		private SampleSustainer(SubSoundDef def) : base(def)
		{
		}

		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x06004E53 RID: 20051 RVA: 0x0028E834 File Offset: 0x0028CC34
		public override float ParentStartRealTime
		{
			get
			{
				return this.subSustainer.creationRealTime;
			}
		}

		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x06004E54 RID: 20052 RVA: 0x0028E854 File Offset: 0x0028CC54
		public override float ParentStartTick
		{
			get
			{
				return (float)this.subSustainer.creationTick;
			}
		}

		// Token: 0x17000CA7 RID: 3239
		// (get) Token: 0x06004E55 RID: 20053 RVA: 0x0028E878 File Offset: 0x0028CC78
		public override float ParentHashCode
		{
			get
			{
				return (float)this.subSustainer.GetHashCode();
			}
		}

		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x06004E56 RID: 20054 RVA: 0x0028E89C File Offset: 0x0028CC9C
		public override SoundParams ExternalParams
		{
			get
			{
				return this.subSustainer.ExternalParams;
			}
		}

		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x06004E57 RID: 20055 RVA: 0x0028E8BC File Offset: 0x0028CCBC
		public override SoundInfo Info
		{
			get
			{
				return this.subSustainer.Info;
			}
		}

		// Token: 0x17000CAA RID: 3242
		// (get) Token: 0x06004E58 RID: 20056 RVA: 0x0028E8DC File Offset: 0x0028CCDC
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

		// Token: 0x06004E59 RID: 20057 RVA: 0x0028EA54 File Offset: 0x0028CE54
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

		// Token: 0x06004E5A RID: 20058 RVA: 0x0028EC30 File Offset: 0x0028D030
		public override void SampleCleanup()
		{
			base.SampleCleanup();
			if (this.source != null && this.source.gameObject != null)
			{
				UnityEngine.Object.Destroy(this.source.gameObject);
			}
		}

		// Token: 0x0400342B RID: 13355
		public SubSustainer subSustainer;

		// Token: 0x0400342C RID: 13356
		public float scheduledEndTime;

		// Token: 0x0400342D RID: 13357
		public bool resolvedSkipAttack = false;
	}
}
