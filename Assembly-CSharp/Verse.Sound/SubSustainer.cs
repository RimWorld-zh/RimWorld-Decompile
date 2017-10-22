using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse.Sound
{
	public class SubSustainer
	{
		public Sustainer parent;

		public SubSoundDef subDef;

		private List<SampleSustainer> samples = new List<SampleSustainer>();

		private float nextSampleStartTime;

		public int creationFrame = -1;

		public int creationTick = -1;

		public float creationRealTime = -1f;

		private const float MinSampleStartInterval = 0.01f;

		public SoundInfo Info
		{
			get
			{
				return this.parent.info;
			}
		}

		public SoundParams ExternalParams
		{
			get
			{
				return this.parent.externalParams;
			}
		}

		public SubSustainer(Sustainer parent, SubSoundDef subSoundDef)
		{
			this.parent = parent;
			this.subDef = subSoundDef;
			LongEventHandler.ExecuteWhenFinished((Action)delegate
			{
				this.creationFrame = Time.frameCount;
				this.creationRealTime = Time.realtimeSinceStartup;
				if (Current.ProgramState == ProgramState.Playing)
				{
					this.creationTick = Find.TickManager.TicksGame;
				}
				if (this.subDef.startDelayRange.TrueMax < 0.0010000000474974513)
				{
					this.StartSample();
				}
				else
				{
					this.nextSampleStartTime = Time.realtimeSinceStartup + this.subDef.startDelayRange.RandomInRange;
				}
			});
		}

		private void StartSample()
		{
			ResolvedGrain resolvedGrain = this.subDef.RandomizedResolvedGrain();
			if (resolvedGrain == null)
			{
				Log.Error("SubSustainer for " + this.subDef + " of " + this.parent.def + " could not resolve any grains.");
				this.parent.End();
			}
			else
			{
				float num = (!this.subDef.sustainLoop) ? resolvedGrain.duration : this.subDef.sustainLoopDurationRange.RandomInRange;
				float num2 = Time.realtimeSinceStartup + num;
				this.nextSampleStartTime = num2 + this.subDef.sustainIntervalRange.RandomInRange;
				if (this.nextSampleStartTime < Time.realtimeSinceStartup + 0.0099999997764825821)
				{
					this.nextSampleStartTime = (float)(Time.realtimeSinceStartup + 0.0099999997764825821);
				}
				if (!(resolvedGrain is ResolvedGrain_Silence))
				{
					SampleSustainer sampleSustainer = SampleSustainer.TryMakeAndPlay(this, ((ResolvedGrain_Clip)resolvedGrain).clip, num2);
					if (sampleSustainer != null)
					{
						if (this.subDef.sustainSkipFirstAttack && Time.frameCount == this.creationFrame)
						{
							sampleSustainer.resolvedSkipAttack = true;
						}
						this.samples.Add(sampleSustainer);
					}
				}
			}
		}

		public void SubSustainerUpdate()
		{
			for (int num = this.samples.Count - 1; num >= 0; num--)
			{
				if (Time.realtimeSinceStartup > this.samples[num].scheduledEndTime)
				{
					this.EndSample(this.samples[num]);
				}
			}
			if (Time.realtimeSinceStartup > this.nextSampleStartTime)
			{
				this.StartSample();
			}
			for (int i = 0; i < this.samples.Count; i++)
			{
				this.samples[i].Update();
			}
		}

		private void EndSample(SampleSustainer samp)
		{
			this.samples.Remove(samp);
			samp.SampleCleanup();
		}

		public virtual void Cleanup()
		{
			while (this.samples.Count > 0)
			{
				this.EndSample(this.samples[0]);
			}
		}

		public override string ToString()
		{
			return this.subDef.name + "_" + this.creationFrame;
		}

		public override int GetHashCode()
		{
			return Gen.HashCombine(this.creationRealTime.GetHashCode(), this.subDef);
		}

		public string SamplesDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (SampleSustainer sample in this.samples)
			{
				stringBuilder.AppendLine(sample.ToString());
			}
			return stringBuilder.ToString();
		}
	}
}
