using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Sound
{
	// Token: 0x02000DB0 RID: 3504
	public class SampleOneShotManager
	{
		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x06004E5C RID: 20060 RVA: 0x0028F964 File Offset: 0x0028DD64
		public IEnumerable<SampleOneShot> PlayingOneShots
		{
			get
			{
				return this.samples;
			}
		}

		// Token: 0x06004E5D RID: 20061 RVA: 0x0028F980 File Offset: 0x0028DD80
		private float CameraDistanceSquaredOf(SoundInfo info)
		{
			return (float)(Find.CameraDriver.MapPosition - info.Maker.Cell).LengthHorizontalSquared;
		}

		// Token: 0x06004E5E RID: 20062 RVA: 0x0028F9BC File Offset: 0x0028DDBC
		private float ImportanceOf(SampleOneShot sample)
		{
			return this.ImportanceOf(sample.subDef.parentDef, sample.info, sample.AgeRealTime);
		}

		// Token: 0x06004E5F RID: 20063 RVA: 0x0028F9F0 File Offset: 0x0028DDF0
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

		// Token: 0x06004E60 RID: 20064 RVA: 0x0028FA48 File Offset: 0x0028DE48
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

		// Token: 0x06004E61 RID: 20065 RVA: 0x0028FAFC File Offset: 0x0028DEFC
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

		// Token: 0x06004E62 RID: 20066 RVA: 0x0028FB94 File Offset: 0x0028DF94
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

		// Token: 0x06004E63 RID: 20067 RVA: 0x0028FC08 File Offset: 0x0028E008
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

		// Token: 0x04003432 RID: 13362
		private List<SampleOneShot> samples = new List<SampleOneShot>();

		// Token: 0x04003433 RID: 13363
		private List<SampleOneShot> cleanupList = new List<SampleOneShot>();
	}
}
