using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	public class SampleSustainer : Sample
	{
		public SubSustainer subSustainer;

		public float scheduledEndTime;

		public bool resolvedSkipAttack;

		public override float ParentStartRealTime
		{
			get
			{
				return this.subSustainer.creationRealTime;
			}
		}

		public override float ParentStartTick
		{
			get
			{
				return (float)this.subSustainer.creationTick;
			}
		}

		public override float ParentHashCode
		{
			get
			{
				return (float)this.subSustainer.GetHashCode();
			}
		}

		public override SoundParams ExternalParams
		{
			get
			{
				return this.subSustainer.ExternalParams;
			}
		}

		public override SoundInfo Info
		{
			get
			{
				return this.subSustainer.Info;
			}
		}

		protected override float Volume
		{
			get
			{
				float num = base.Volume * this.subSustainer.parent.scopeFader.inScopePercent;
				float num2 = 1f;
				if (this.subSustainer.parent.Ended)
				{
					num2 = (float)(1.0 - Mathf.Min(this.subSustainer.parent.TimeSinceEnd / base.subDef.parentDef.sustainFadeoutTime, 1f));
				}
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				if (base.AgeRealTime < base.subDef.sustainAttack)
				{
					if (!this.resolvedSkipAttack && !(base.subDef.sustainAttack < 0.0099999997764825821))
					{
						float f = base.AgeRealTime / base.subDef.sustainAttack;
						f = Mathf.Sqrt(f);
						return Mathf.Lerp(0f, num, f) * num2;
					}
					return num * num2;
				}
				if (realtimeSinceStartup > this.scheduledEndTime - base.subDef.sustainRelease)
				{
					float num3 = (realtimeSinceStartup - (this.scheduledEndTime - base.subDef.sustainRelease)) / base.subDef.sustainRelease;
					num3 = (float)(1.0 - num3);
					num3 = Mathf.Max(num3, 0f);
					num3 = Mathf.Sqrt(num3);
					num3 = (float)(1.0 - num3);
					return Mathf.Lerp(num, 0f, num3) * num2;
				}
				return num * num2;
			}
		}

		private SampleSustainer(SubSoundDef def)
			: base(def)
		{
		}

		public static SampleSustainer TryMakeAndPlay(SubSustainer subSus, AudioClip clip, float scheduledEndTime)
		{
			SampleSustainer sampleSustainer = new SampleSustainer(subSus.subDef);
			sampleSustainer.subSustainer = subSus;
			sampleSustainer.scheduledEndTime = scheduledEndTime;
			GameObject gameObject = new GameObject("SampleSource_" + sampleSustainer.subDef.name + "_" + sampleSustainer.startRealTime);
			GameObject gameObject2 = (!subSus.subDef.onCamera) ? subSus.parent.worldRootObject : Find.Camera.gameObject;
			gameObject.transform.parent = gameObject2.transform;
			gameObject.transform.localPosition = Vector3.zero;
			sampleSustainer.source = AudioSourceMaker.NewAudioSourceOn(gameObject);
			if ((Object)sampleSustainer.source == (Object)null)
			{
				if ((Object)gameObject != (Object)null)
				{
					Object.Destroy(gameObject);
				}
				return null;
			}
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
			return sampleSustainer;
		}

		public override void SampleCleanup()
		{
			base.SampleCleanup();
			if ((Object)base.source != (Object)null && (Object)base.source.gameObject != (Object)null)
			{
				Object.Destroy(base.source.gameObject);
			}
		}
	}
}
