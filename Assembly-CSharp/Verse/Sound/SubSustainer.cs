using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DC2 RID: 3522
	public class SubSustainer
	{
		// Token: 0x04003459 RID: 13401
		public Sustainer parent;

		// Token: 0x0400345A RID: 13402
		public SubSoundDef subDef;

		// Token: 0x0400345B RID: 13403
		private List<SampleSustainer> samples = new List<SampleSustainer>();

		// Token: 0x0400345C RID: 13404
		private float nextSampleStartTime;

		// Token: 0x0400345D RID: 13405
		public int creationFrame = -1;

		// Token: 0x0400345E RID: 13406
		public int creationTick = -1;

		// Token: 0x0400345F RID: 13407
		public float creationRealTime = -1f;

		// Token: 0x04003460 RID: 13408
		private const float MinSampleStartInterval = 0.01f;

		// Token: 0x06004E99 RID: 20121 RVA: 0x00291410 File Offset: 0x0028F810
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
		// (get) Token: 0x06004E9A RID: 20122 RVA: 0x00291468 File Offset: 0x0028F868
		public SoundInfo Info
		{
			get
			{
				return this.parent.info;
			}
		}

		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x06004E9B RID: 20123 RVA: 0x00291488 File Offset: 0x0028F888
		public SoundParams ExternalParams
		{
			get
			{
				return this.parent.externalParams;
			}
		}

		// Token: 0x06004E9C RID: 20124 RVA: 0x002914A8 File Offset: 0x0028F8A8
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

		// Token: 0x06004E9D RID: 20125 RVA: 0x002915F0 File Offset: 0x0028F9F0
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

		// Token: 0x06004E9E RID: 20126 RVA: 0x00291690 File Offset: 0x0028FA90
		private void EndSample(SampleSustainer samp)
		{
			this.samples.Remove(samp);
			samp.SampleCleanup();
		}

		// Token: 0x06004E9F RID: 20127 RVA: 0x002916A6 File Offset: 0x0028FAA6
		public virtual void Cleanup()
		{
			while (this.samples.Count > 0)
			{
				this.EndSample(this.samples[0]);
			}
		}

		// Token: 0x06004EA0 RID: 20128 RVA: 0x002916D4 File Offset: 0x0028FAD4
		public override string ToString()
		{
			return this.subDef.name + "_" + this.creationFrame;
		}

		// Token: 0x06004EA1 RID: 20129 RVA: 0x0029170C File Offset: 0x0028FB0C
		public override int GetHashCode()
		{
			return Gen.HashCombine<SubSoundDef>(this.creationRealTime.GetHashCode(), this.subDef);
		}

		// Token: 0x06004EA2 RID: 20130 RVA: 0x00291740 File Offset: 0x0028FB40
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
