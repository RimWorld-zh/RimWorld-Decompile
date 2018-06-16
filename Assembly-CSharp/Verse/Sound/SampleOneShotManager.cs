using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Sound
{
	// Token: 0x02000DB4 RID: 3508
	public class SampleOneShotManager
	{
		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x06004E49 RID: 20041 RVA: 0x0028E3D4 File Offset: 0x0028C7D4
		public IEnumerable<SampleOneShot> PlayingOneShots
		{
			get
			{
				return this.samples;
			}
		}

		// Token: 0x06004E4A RID: 20042 RVA: 0x0028E3F0 File Offset: 0x0028C7F0
		private float CameraDistanceSquaredOf(SoundInfo info)
		{
			return (float)(Find.CameraDriver.MapPosition - info.Maker.Cell).LengthHorizontalSquared;
		}

		// Token: 0x06004E4B RID: 20043 RVA: 0x0028E42C File Offset: 0x0028C82C
		private float ImportanceOf(SampleOneShot sample)
		{
			return this.ImportanceOf(sample.subDef.parentDef, sample.info, sample.AgeRealTime);
		}

		// Token: 0x06004E4C RID: 20044 RVA: 0x0028E460 File Offset: 0x0028C860
		private float ImportanceOf(SoundDef def, SoundInfo info, float ageRealTime)
		{
			float result;
			if (def.priorityMode == VoicePriorityMode.PrioritizeNearest)
			{
				result = 1f / (this.CameraDistanceSquaredOf(info) + 1f);
			}
			else
			{
				if (def.priorityMode != VoicePriorityMode.PrioritizeNewest)
				{
					throw new NotImplementedException();
				}
				result = 1f / (ageRealTime + 1f);
			}
			return result;
		}

		// Token: 0x06004E4D RID: 20045 RVA: 0x0028E4B8 File Offset: 0x0028C8B8
		public bool CanAddPlayingOneShot(SoundDef def, SoundInfo info)
		{
			bool result;
			if (!SoundDefHelper.CorrectContextNow(def, info.Maker.Map))
			{
				result = false;
			}
			else if ((from s in this.samples
			where s.subDef.parentDef == def && s.AgeRealTime < 0.05f
			select s).Count<SampleOneShot>() >= def.MaxSimultaneousSamples)
			{
				result = false;
			}
			else
			{
				SampleOneShot sampleOneShot = this.LeastImportantOf(def);
				result = (sampleOneShot == null || this.ImportanceOf(def, info, 0f) >= this.ImportanceOf(sampleOneShot));
			}
			return result;
		}

		// Token: 0x06004E4E RID: 20046 RVA: 0x0028E56C File Offset: 0x0028C96C
		public void TryAddPlayingOneShot(SampleOneShot newSample)
		{
			int num = (from s in this.samples
			where s.subDef == newSample.subDef
			select s).Count<SampleOneShot>();
			if (num >= newSample.subDef.parentDef.maxVoices)
			{
				SampleOneShot sampleOneShot = this.LeastImportantOf(newSample.subDef.parentDef);
				sampleOneShot.source.Stop();
				this.samples.Remove(sampleOneShot);
			}
			this.samples.Add(newSample);
		}

		// Token: 0x06004E4F RID: 20047 RVA: 0x0028E604 File Offset: 0x0028CA04
		private SampleOneShot LeastImportantOf(SoundDef def)
		{
			SampleOneShot sampleOneShot = null;
			for (int i = 0; i < this.samples.Count; i++)
			{
				SampleOneShot sampleOneShot2 = this.samples[i];
				if (sampleOneShot2.subDef.parentDef == def)
				{
					if (sampleOneShot == null || this.ImportanceOf(sampleOneShot2) < this.ImportanceOf(sampleOneShot))
					{
						sampleOneShot = sampleOneShot2;
					}
				}
			}
			return sampleOneShot;
		}

		// Token: 0x06004E50 RID: 20048 RVA: 0x0028E678 File Offset: 0x0028CA78
		public void SampleOneShotManagerUpdate()
		{
			for (int i = 0; i < this.samples.Count; i++)
			{
				this.samples[i].Update();
			}
			this.cleanupList.Clear();
			for (int j = 0; j < this.samples.Count; j++)
			{
				SampleOneShot sampleOneShot = this.samples[j];
				if (sampleOneShot.source == null || !sampleOneShot.source.isPlaying || !SoundDefHelper.CorrectContextNow(sampleOneShot.subDef.parentDef, sampleOneShot.Map))
				{
					if (sampleOneShot.source != null && sampleOneShot.source.isPlaying)
					{
						sampleOneShot.source.Stop();
					}
					sampleOneShot.SampleCleanup();
					this.cleanupList.Add(sampleOneShot);
				}
			}
			if (this.cleanupList.Count > 0)
			{
				this.samples.RemoveAll((SampleOneShot s) => this.cleanupList.Contains(s));
			}
		}

		// Token: 0x04003429 RID: 13353
		private List<SampleOneShot> samples = new List<SampleOneShot>();

		// Token: 0x0400342A RID: 13354
		private List<SampleOneShot> cleanupList = new List<SampleOneShot>();
	}
}
