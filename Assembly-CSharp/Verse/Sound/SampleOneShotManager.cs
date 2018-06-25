using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Sound
{
	// Token: 0x02000DB3 RID: 3507
	public class SampleOneShotManager
	{
		// Token: 0x04003439 RID: 13369
		private List<SampleOneShot> samples = new List<SampleOneShot>();

		// Token: 0x0400343A RID: 13370
		private List<SampleOneShot> cleanupList = new List<SampleOneShot>();

		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x06004E60 RID: 20064 RVA: 0x0028FD70 File Offset: 0x0028E170
		public IEnumerable<SampleOneShot> PlayingOneShots
		{
			get
			{
				return this.samples;
			}
		}

		// Token: 0x06004E61 RID: 20065 RVA: 0x0028FD8C File Offset: 0x0028E18C
		private float CameraDistanceSquaredOf(SoundInfo info)
		{
			return (float)(Find.CameraDriver.MapPosition - info.Maker.Cell).LengthHorizontalSquared;
		}

		// Token: 0x06004E62 RID: 20066 RVA: 0x0028FDC8 File Offset: 0x0028E1C8
		private float ImportanceOf(SampleOneShot sample)
		{
			return this.ImportanceOf(sample.subDef.parentDef, sample.info, sample.AgeRealTime);
		}

		// Token: 0x06004E63 RID: 20067 RVA: 0x0028FDFC File Offset: 0x0028E1FC
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

		// Token: 0x06004E64 RID: 20068 RVA: 0x0028FE54 File Offset: 0x0028E254
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

		// Token: 0x06004E65 RID: 20069 RVA: 0x0028FF08 File Offset: 0x0028E308
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

		// Token: 0x06004E66 RID: 20070 RVA: 0x0028FFA0 File Offset: 0x0028E3A0
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

		// Token: 0x06004E67 RID: 20071 RVA: 0x00290014 File Offset: 0x0028E414
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
	}
}
