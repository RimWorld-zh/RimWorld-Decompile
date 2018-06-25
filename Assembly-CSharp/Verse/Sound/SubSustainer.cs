using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DC1 RID: 3521
	public class SubSustainer
	{
		// Token: 0x04003452 RID: 13394
		public Sustainer parent;

		// Token: 0x04003453 RID: 13395
		public SubSoundDef subDef;

		// Token: 0x04003454 RID: 13396
		private List<SampleSustainer> samples = new List<SampleSustainer>();

		// Token: 0x04003455 RID: 13397
		private float nextSampleStartTime;

		// Token: 0x04003456 RID: 13398
		public int creationFrame = -1;

		// Token: 0x04003457 RID: 13399
		public int creationTick = -1;

		// Token: 0x04003458 RID: 13400
		public float creationRealTime = -1f;

		// Token: 0x04003459 RID: 13401
		private const float MinSampleStartInterval = 0.01f;

		// Token: 0x06004E99 RID: 20121 RVA: 0x00291130 File Offset: 0x0028F530
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

		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x06004E9A RID: 20122 RVA: 0x00291188 File Offset: 0x0028F588
		public SoundInfo Info
		{
			get
			{
				return this.parent.info;
			}
		}

		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x06004E9B RID: 20123 RVA: 0x002911A8 File Offset: 0x0028F5A8
		public SoundParams ExternalParams
		{
			get
			{
				return this.parent.externalParams;
			}
		}

		// Token: 0x06004E9C RID: 20124 RVA: 0x002911C8 File Offset: 0x0028F5C8
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

		// Token: 0x06004E9D RID: 20125 RVA: 0x00291310 File Offset: 0x0028F710
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

		// Token: 0x06004E9E RID: 20126 RVA: 0x002913B0 File Offset: 0x0028F7B0
		private void EndSample(SampleSustainer samp)
		{
			this.samples.Remove(samp);
			samp.SampleCleanup();
		}

		// Token: 0x06004E9F RID: 20127 RVA: 0x002913C6 File Offset: 0x0028F7C6
		public virtual void Cleanup()
		{
			while (this.samples.Count > 0)
			{
				this.EndSample(this.samples[0]);
			}
		}

		// Token: 0x06004EA0 RID: 20128 RVA: 0x002913F4 File Offset: 0x0028F7F4
		public override string ToString()
		{
			return this.subDef.name + "_" + this.creationFrame;
		}

		// Token: 0x06004EA1 RID: 20129 RVA: 0x0029142C File Offset: 0x0028F82C
		public override int GetHashCode()
		{
			return Gen.HashCombine<SubSoundDef>(this.creationRealTime.GetHashCode(), this.subDef);
		}

		// Token: 0x06004EA2 RID: 20130 RVA: 0x00291460 File Offset: 0x0028F860
		public string SamplesDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (SampleSustainer sampleSustainer in this.samples)
			{
				stringBuilder.AppendLine(sampleSustainer.ToString());
			}
			return stringBuilder.ToString();
		}
	}
}
