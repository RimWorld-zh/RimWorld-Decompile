using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.Sound
{
	public class SampleOneShotManager
	{
		private List<SampleOneShot> samples = new List<SampleOneShot>();

		private List<SampleOneShot> cleanupList = new List<SampleOneShot>();

		public IEnumerable<SampleOneShot> PlayingOneShots
		{
			get
			{
				return this.samples;
			}
		}

		private float CameraDistanceSquaredOf(SoundInfo info)
		{
			return (float)(Find.CameraDriver.MapPosition - info.Maker.Cell).LengthHorizontalSquared;
		}

		private float ImportanceOf(SampleOneShot sample)
		{
			return this.ImportanceOf(sample.subDef.parentDef, sample.info, sample.AgeRealTime);
		}

		private float ImportanceOf(SoundDef def, SoundInfo info, float ageRealTime)
		{
			float result;
			if (def.priorityMode == VoicePriorityMode.PrioritizeNearest)
			{
				result = (float)(1.0 / (this.CameraDistanceSquaredOf(info) + 1.0));
				goto IL_004a;
			}
			if (def.priorityMode == VoicePriorityMode.PrioritizeNewest)
			{
				result = (float)(1.0 / (ageRealTime + 1.0));
				goto IL_004a;
			}
			throw new NotImplementedException();
			IL_004a:
			return result;
		}

		public bool CanAddPlayingOneShot(SoundDef def, SoundInfo info)
		{
			bool result;
			if (!SoundDefHelper.CorrectContextNow(def, info.Maker.Map))
			{
				result = false;
			}
			else if ((from s in this.samples
			where s.subDef.parentDef == def && s.AgeRealTime < 0.05000000074505806
			select s).Count() >= def.MaxSimultaneousSamples)
			{
				result = false;
			}
			else
			{
				SampleOneShot sampleOneShot = this.LeastImportantOf(def);
				result = ((byte)((sampleOneShot == null || !(this.ImportanceOf(def, info, 0f) < this.ImportanceOf(sampleOneShot))) ? 1 : 0) != 0);
			}
			return result;
		}

		public void TryAddPlayingOneShot(SampleOneShot newSample)
		{
			int num = (from s in this.samples
			where s.subDef == newSample.subDef
			select s).Count();
			if (num >= newSample.subDef.parentDef.maxVoices)
			{
				SampleOneShot sampleOneShot = this.LeastImportantOf(newSample.subDef.parentDef);
				sampleOneShot.source.Stop();
				this.samples.Remove(sampleOneShot);
			}
			this.samples.Add(newSample);
		}

		private SampleOneShot LeastImportantOf(SoundDef def)
		{
			SampleOneShot sampleOneShot = null;
			for (int i = 0; i < this.samples.Count; i++)
			{
				SampleOneShot sampleOneShot2 = this.samples[i];
				if (sampleOneShot2.subDef.parentDef == def && (sampleOneShot == null || this.ImportanceOf(sampleOneShot2) < this.ImportanceOf(sampleOneShot)))
				{
					sampleOneShot = sampleOneShot2;
				}
			}
			return sampleOneShot;
		}

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
				if ((UnityEngine.Object)sampleOneShot.source == (UnityEngine.Object)null || !sampleOneShot.source.isPlaying || !SoundDefHelper.CorrectContextNow(sampleOneShot.subDef.parentDef, sampleOneShot.Map))
				{
					if ((UnityEngine.Object)sampleOneShot.source != (UnityEngine.Object)null && sampleOneShot.source.isPlaying)
					{
						sampleOneShot.source.Stop();
					}
					sampleOneShot.SampleCleanup();
					this.cleanupList.Add(sampleOneShot);
				}
			}
			if (this.cleanupList.Count > 0)
			{
				this.samples.RemoveAll((Predicate<SampleOneShot>)((SampleOneShot s) => this.cleanupList.Contains(s)));
			}
		}
	}
}
