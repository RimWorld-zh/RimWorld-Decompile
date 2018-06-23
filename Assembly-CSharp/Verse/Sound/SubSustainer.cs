using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DBF RID: 3519
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

		// Token: 0x06004E95 RID: 20117 RVA: 0x00291004 File Offset: 0x0028F404
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

		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x06004E96 RID: 20118 RVA: 0x0029105C File Offset: 0x0028F45C
		public SoundInfo Info
		{
			get
			{
				return this.parent.info;
			}
		}

		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x06004E97 RID: 20119 RVA: 0x0029107C File Offset: 0x0028F47C
		public SoundParams ExternalParams
		{
			get
			{
				return this.parent.externalParams;
			}
		}

		// Token: 0x06004E98 RID: 20120 RVA: 0x0029109C File Offset: 0x0028F49C
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

		// Token: 0x06004E99 RID: 20121 RVA: 0x002911E4 File Offset: 0x0028F5E4
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

		// Token: 0x06004E9A RID: 20122 RVA: 0x00291284 File Offset: 0x0028F684
		private void EndSample(SampleSustainer samp)
		{
			this.samples.Remove(samp);
			samp.SampleCleanup();
		}

		// Token: 0x06004E9B RID: 20123 RVA: 0x0029129A File Offset: 0x0028F69A
		public virtual void Cleanup()
		{
			while (this.samples.Count > 0)
			{
				this.EndSample(this.samples[0]);
			}
		}

		// Token: 0x06004E9C RID: 20124 RVA: 0x002912C8 File Offset: 0x0028F6C8
		public override string ToString()
		{
			return this.subDef.name + "_" + this.creationFrame;
		}

		// Token: 0x06004E9D RID: 20125 RVA: 0x00291300 File Offset: 0x0028F700
		public override int GetHashCode()
		{
			return Gen.HashCombine<SubSoundDef>(this.creationRealTime.GetHashCode(), this.subDef);
		}

		// Token: 0x06004E9E RID: 20126 RVA: 0x00291334 File Offset: 0x0028F734
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
