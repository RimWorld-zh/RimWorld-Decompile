using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DC2 RID: 3522
	public class SubSustainer
	{
		// Token: 0x06004E80 RID: 20096 RVA: 0x0028FA54 File Offset: 0x0028DE54
		public SubSustainer(Sustainer parent, SubSoundDef subSoundDef)
		{
			this.parent = parent;
			this.subDef = subSoundDef;
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.creationFrame = Time.frameCount;
				this.creationRealTime = Time.realtimeSinceStartup;
				if (Current.ProgramState == ProgramState.Playing)
				{
					this.creationTick = Find.TickManager.TicksGame;
				}
				if (this.subDef.startDelayRange.TrueMax < 0.001f)
				{
					this.StartSample();
				}
				else
				{
					this.nextSampleStartTime = Time.realtimeSinceStartup + this.subDef.startDelayRange.RandomInRange;
				}
			});
		}

		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x06004E81 RID: 20097 RVA: 0x0028FAAC File Offset: 0x0028DEAC
		public SoundInfo Info
		{
			get
			{
				return this.parent.info;
			}
		}

		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x06004E82 RID: 20098 RVA: 0x0028FACC File Offset: 0x0028DECC
		public SoundParams ExternalParams
		{
			get
			{
				return this.parent.externalParams;
			}
		}

		// Token: 0x06004E83 RID: 20099 RVA: 0x0028FAEC File Offset: 0x0028DEEC
		private void StartSample()
		{
			ResolvedGrain resolvedGrain = this.subDef.RandomizedResolvedGrain();
			if (resolvedGrain == null)
			{
				Log.Error(string.Concat(new object[]
				{
					"SubSustainer for ",
					this.subDef,
					" of ",
					this.parent.def,
					" could not resolve any grains."
				}), false);
				this.parent.End();
			}
			else
			{
				float num;
				if (this.subDef.sustainLoop)
				{
					num = this.subDef.sustainLoopDurationRange.RandomInRange;
				}
				else
				{
					num = resolvedGrain.duration;
				}
				float num2 = Time.realtimeSinceStartup + num;
				this.nextSampleStartTime = num2 + this.subDef.sustainIntervalRange.RandomInRange;
				if (this.nextSampleStartTime < Time.realtimeSinceStartup + 0.01f)
				{
					this.nextSampleStartTime = Time.realtimeSinceStartup + 0.01f;
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

		// Token: 0x06004E84 RID: 20100 RVA: 0x0028FC34 File Offset: 0x0028E034
		public void SubSustainerUpdate()
		{
			for (int i = this.samples.Count - 1; i >= 0; i--)
			{
				if (Time.realtimeSinceStartup > this.samples[i].scheduledEndTime)
				{
					this.EndSample(this.samples[i]);
				}
			}
			if (Time.realtimeSinceStartup > this.nextSampleStartTime)
			{
				this.StartSample();
			}
			for (int j = 0; j < this.samples.Count; j++)
			{
				this.samples[j].Update();
			}
		}

		// Token: 0x06004E85 RID: 20101 RVA: 0x0028FCD4 File Offset: 0x0028E0D4
		private void EndSample(SampleSustainer samp)
		{
			this.samples.Remove(samp);
			samp.SampleCleanup();
		}

		// Token: 0x06004E86 RID: 20102 RVA: 0x0028FCEA File Offset: 0x0028E0EA
		public virtual void Cleanup()
		{
			while (this.samples.Count > 0)
			{
				this.EndSample(this.samples[0]);
			}
		}

		// Token: 0x06004E87 RID: 20103 RVA: 0x0028FD18 File Offset: 0x0028E118
		public override string ToString()
		{
			return this.subDef.name + "_" + this.creationFrame;
		}

		// Token: 0x06004E88 RID: 20104 RVA: 0x0028FD50 File Offset: 0x0028E150
		public override int GetHashCode()
		{
			return Gen.HashCombine<SubSoundDef>(this.creationRealTime.GetHashCode(), this.subDef);
		}

		// Token: 0x06004E89 RID: 20105 RVA: 0x0028FD84 File Offset: 0x0028E184
		public string SamplesDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (SampleSustainer sampleSustainer in this.samples)
			{
				stringBuilder.AppendLine(sampleSustainer.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04003447 RID: 13383
		public Sustainer parent;

		// Token: 0x04003448 RID: 13384
		public SubSoundDef subDef;

		// Token: 0x04003449 RID: 13385
		private List<SampleSustainer> samples = new List<SampleSustainer>();

		// Token: 0x0400344A RID: 13386
		private float nextSampleStartTime;

		// Token: 0x0400344B RID: 13387
		public int creationFrame = -1;

		// Token: 0x0400344C RID: 13388
		public int creationTick = -1;

		// Token: 0x0400344D RID: 13389
		public float creationRealTime = -1f;

		// Token: 0x0400344E RID: 13390
		private const float MinSampleStartInterval = 0.01f;
	}
}
