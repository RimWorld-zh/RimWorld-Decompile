using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Sound
{
	// Token: 0x02000DB2 RID: 3506
	public class SampleOneShotManager
	{
		// Token: 0x04003432 RID: 13362
		private List<SampleOneShot> samples = new List<SampleOneShot>();

		// Token: 0x04003433 RID: 13363
		private List<SampleOneShot> cleanupList = new List<SampleOneShot>();

		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x06004E60 RID: 20064 RVA: 0x0028FA90 File Offset: 0x0028DE90
		public IEnumerable<SampleOneShot> PlayingOneShots
		{
			get
			{
				return this.samples;
			}
		}

		// Token: 0x06004E61 RID: 20065 RVA: 0x0028FAAC File Offset: 0x0028DEAC
		private float CameraDistanceSquaredOf(SoundInfo info)
		{
			return (float)(Find.CameraDriver.MapPosition - info.Maker.Cell).LengthHorizontalSquared;
		}

		// Token: 0x06004E62 RID: 20066 RVA: 0x0028FAE8 File Offset: 0x0028DEE8
		private float ImportanceOf(SampleOneShot sample)
		{
			return this.ImportanceOf(sample.subDef.parentDef, sample.info, sample.AgeRealTime);
		}

		// Token: 0x06004E63 RID: 20067 RVA: 0x0028FB1C File Offset: 0x0028DF1C
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

		// Token: 0x06004E64 RID: 20068 RVA: 0x0028FB74 File Offset: 0x0028DF74
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

		// Token: 0x06004E65 RID: 20069 RVA: 0x0028FC28 File Offset: 0x0028E028
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

		// Token: 0x06004E66 RID: 20070 RVA: 0x0028FCC0 File Offset: 0x0028E0C0
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

		// Token: 0x06004E67 RID: 20071 RVA: 0x0028FD34 File Offset: 0x0028E134
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
