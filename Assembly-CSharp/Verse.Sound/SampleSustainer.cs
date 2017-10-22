using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	public class SampleSustainer : Sample
	{
		public SubSustainer subSustainer;

		public float scheduledEndTime;

		public bool resolvedSkipAttack;

		public override Map Map
		{
			get
			{
				return this.subSustainer.Info.Maker.Map;
			}
		}

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

		protected override bool TestPlaying
		{
			get
			{
				SoundInfo info = this.subSustainer.Info;
				return info.testPlay;
			}
		}

		private SampleSustainer(SubSoundDef def) : base(def)
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
			sampleSustainer.source.volume = sampleSustainer.resolvedVolume;
			sampleSustainer.source.pitch = sampleSustainer.resolvedPitch;
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
			sampleSustainer.ApplyMappedParameters();
			sampleSustainer.UpdateSourceVolume();
			sampleSustainer.source.Play();
			sampleSustainer.source.Play();
			return sampleSustainer;
		}

		public void UpdateSourceVolume()
		{
			float num = base.resolvedVolume * this.subSustainer.parent.scopeFader.inScopePercent * base.MappedVolumeMultiplier * base.ContextVolumeMultiplier;
			if (base.AgeRealTime < base.subDef.sustainAttack)
			{
				if (this.resolvedSkipAttack || base.subDef.sustainAttack < 0.0099999997764825821)
				{
					base.source.volume = num;
				}
				else
				{
					float f = base.AgeRealTime / base.subDef.sustainAttack;
					f = Mathf.Sqrt(f);
					base.source.volume = Mathf.Lerp(0f, num, f);
				}
			}
			else if (Time.realtimeSinceStartup > this.scheduledEndTime - base.subDef.sustainRelease)
			{
				float num2 = (Time.realtimeSinceStartup - (this.scheduledEndTime - base.subDef.sustainRelease)) / base.subDef.sustainRelease;
				num2 = (float)(1.0 - num2);
				num2 = Mathf.Sqrt(num2);
				num2 = (float)(1.0 - num2);
				base.source.volume = Mathf.Lerp(num, 0f, num2);
			}
			else
			{
				base.source.volume = num;
			}
			if (this.subSustainer.parent.Ended)
			{
				float num3 = (float)(1.0 - this.subSustainer.parent.TimeSinceEnd / base.subDef.parentDef.sustainFadeoutTime);
				base.source.volume *= num3;
			}
			if (base.source.volume < 0.0010000000474974513)
			{
				base.source.mute = true;
			}
			else
			{
				base.source.mute = false;
			}
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
