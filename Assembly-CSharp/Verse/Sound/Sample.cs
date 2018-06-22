using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DAE RID: 3502
	public abstract class Sample
	{
		// Token: 0x06004E3D RID: 20029 RVA: 0x0028EFF0 File Offset: 0x0028D3F0
		public Sample(SubSoundDef def)
		{
			this.subDef = def;
			this.resolvedVolume = def.RandomizedVolume();
			this.resolvedPitch = def.pitchRange.RandomInRange;
			this.startRealTime = Time.realtimeSinceStartup;
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.startTick = Find.TickManager.TicksGame;
			}
			else
			{
				this.startTick = 0;
			}
			foreach (SoundParamTarget_Volume key in (from m in this.subDef.paramMappings
			select m.outParam).OfType<SoundParamTarget_Volume>())
			{
				this.volumeInMappings.Add(key, 0f);
			}
		}

		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x06004E3E RID: 20030 RVA: 0x0028F0F4 File Offset: 0x0028D4F4
		public float AgeRealTime
		{
			get
			{
				return Time.realtimeSinceStartup - this.startRealTime;
			}
		}

		// Token: 0x17000C92 RID: 3218
		// (get) Token: 0x06004E3F RID: 20031 RVA: 0x0028F118 File Offset: 0x0028D518
		public int AgeTicks
		{
			get
			{
				int result;
				if (Current.ProgramState == ProgramState.Playing)
				{
					result = Find.TickManager.TicksGame - this.startTick;
				}
				else
				{
					result = (int)(this.AgeRealTime * 60f);
				}
				return result;
			}
		}

		// Token: 0x17000C93 RID: 3219
		// (get) Token: 0x06004E40 RID: 20032
		public abstract float ParentStartRealTime { get; }

		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x06004E41 RID: 20033
		public abstract float ParentStartTick { get; }

		// Token: 0x17000C95 RID: 3221
		// (get) Token: 0x06004E42 RID: 20034
		public abstract float ParentHashCode { get; }

		// Token: 0x17000C96 RID: 3222
		// (get) Token: 0x06004E43 RID: 20035
		public abstract SoundParams ExternalParams { get; }

		// Token: 0x17000C97 RID: 3223
		// (get) Token: 0x06004E44 RID: 20036
		public abstract SoundInfo Info { get; }

		// Token: 0x17000C98 RID: 3224
		// (get) Token: 0x06004E45 RID: 20037 RVA: 0x0028F15C File Offset: 0x0028D55C
		public Map Map
		{
			get
			{
				return this.Info.Maker.Map;
			}
		}

		// Token: 0x17000C99 RID: 3225
		// (get) Token: 0x06004E46 RID: 20038 RVA: 0x0028F188 File Offset: 0x0028D588
		protected bool TestPlaying
		{
			get
			{
				return this.Info.testPlay;
			}
		}

		// Token: 0x17000C9A RID: 3226
		// (get) Token: 0x06004E47 RID: 20039 RVA: 0x0028F1AC File Offset: 0x0028D5AC
		protected float MappedVolumeMultiplier
		{
			get
			{
				float num = 1f;
				foreach (float num2 in this.volumeInMappings.Values)
				{
					float num3 = num2;
					num *= num3;
				}
				return num;
			}
		}

		// Token: 0x17000C9B RID: 3227
		// (get) Token: 0x06004E48 RID: 20040 RVA: 0x0028F220 File Offset: 0x0028D620
		protected float ContextVolumeMultiplier
		{
			get
			{
				float result;
				if (SoundDefHelper.CorrectContextNow(this.subDef.parentDef, this.Map))
				{
					result = 1f;
				}
				else
				{
					result = 0f;
				}
				return result;
			}
		}

		// Token: 0x17000C9C RID: 3228
		// (get) Token: 0x06004E49 RID: 20041 RVA: 0x0028F260 File Offset: 0x0028D660
		protected virtual float Volume
		{
			get
			{
				float result;
				if (this.subDef.muteWhenPaused && Current.ProgramState == ProgramState.Playing && Find.TickManager.Paused && !this.TestPlaying)
				{
					result = 0f;
				}
				else
				{
					result = this.resolvedVolume * this.Info.volumeFactor * this.MappedVolumeMultiplier * this.ContextVolumeMultiplier;
				}
				return result;
			}
		}

		// Token: 0x17000C9D RID: 3229
		// (get) Token: 0x06004E4A RID: 20042 RVA: 0x0028F2DC File Offset: 0x0028D6DC
		public float SanitizedVolume
		{
			get
			{
				return AudioSourceUtility.GetSanitizedVolume(this.Volume, this.subDef.parentDef);
			}
		}

		// Token: 0x17000C9E RID: 3230
		// (get) Token: 0x06004E4B RID: 20043 RVA: 0x0028F308 File Offset: 0x0028D708
		protected virtual float Pitch
		{
			get
			{
				float num = this.resolvedPitch * this.Info.pitchFactor;
				if (this.subDef.tempoAffectedByGameSpeed && Current.ProgramState == ProgramState.Playing && !this.TestPlaying && !Find.TickManager.Paused)
				{
					num *= Find.TickManager.TickRateMultiplier;
				}
				return num;
			}
		}

		// Token: 0x17000C9F RID: 3231
		// (get) Token: 0x06004E4C RID: 20044 RVA: 0x0028F378 File Offset: 0x0028D778
		public float SanitizedPitch
		{
			get
			{
				return AudioSourceUtility.GetSanitizedPitch(this.Pitch, this.subDef.parentDef);
			}
		}

		// Token: 0x06004E4D RID: 20045 RVA: 0x0028F3A4 File Offset: 0x0028D7A4
		public virtual void Update()
		{
			this.source.pitch = this.SanitizedPitch;
			this.ApplyMappedParameters();
			this.source.volume = this.SanitizedVolume;
			if (this.source.volume < 0.001f)
			{
				this.source.mute = true;
			}
			else
			{
				this.source.mute = false;
			}
			if (this.subDef.tempoAffectedByGameSpeed && !this.TestPlaying)
			{
				if (Current.ProgramState == ProgramState.Playing && Find.TickManager.Paused)
				{
					if (this.source.isPlaying)
					{
						this.source.Pause();
					}
				}
				else if (!this.source.isPlaying)
				{
					this.source.UnPause();
				}
			}
		}

		// Token: 0x06004E4E RID: 20046 RVA: 0x0028F480 File Offset: 0x0028D880
		public void ApplyMappedParameters()
		{
			for (int i = 0; i < this.subDef.paramMappings.Count; i++)
			{
				SoundParameterMapping soundParameterMapping = this.subDef.paramMappings[i];
				if (soundParameterMapping.paramUpdateMode != SoundParamUpdateMode.OncePerSample || !this.mappingsApplied)
				{
					soundParameterMapping.Apply(this);
				}
			}
			this.mappingsApplied = true;
		}

		// Token: 0x06004E4F RID: 20047 RVA: 0x0028F4ED File Offset: 0x0028D8ED
		public void SignalMappedVolume(float value, SoundParamTarget sourceParam)
		{
			this.volumeInMappings[sourceParam] = value;
		}

		// Token: 0x06004E50 RID: 20048 RVA: 0x0028F500 File Offset: 0x0028D900
		public virtual void SampleCleanup()
		{
			for (int i = 0; i < this.subDef.paramMappings.Count; i++)
			{
				SoundParameterMapping soundParameterMapping = this.subDef.paramMappings[i];
				if (soundParameterMapping.curve.HasView)
				{
					soundParameterMapping.curve.View.ClearDebugInputFrom(this);
				}
			}
		}

		// Token: 0x06004E51 RID: 20049 RVA: 0x0028F564 File Offset: 0x0028D964
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Sample_",
				this.subDef.name,
				" volume=",
				this.source.volume,
				" at ",
				this.source.transform.position.ToIntVec3()
			});
		}

		// Token: 0x06004E52 RID: 20050 RVA: 0x0028F5DC File Offset: 0x0028D9DC
		public override int GetHashCode()
		{
			return Gen.HashCombine<SubSoundDef>(this.startRealTime.GetHashCode(), this.subDef);
		}

		// Token: 0x04003427 RID: 13351
		public SubSoundDef subDef;

		// Token: 0x04003428 RID: 13352
		public AudioSource source;

		// Token: 0x04003429 RID: 13353
		public float startRealTime;

		// Token: 0x0400342A RID: 13354
		public int startTick;

		// Token: 0x0400342B RID: 13355
		public float resolvedVolume;

		// Token: 0x0400342C RID: 13356
		public float resolvedPitch;

		// Token: 0x0400342D RID: 13357
		private bool mappingsApplied = false;

		// Token: 0x0400342E RID: 13358
		private Dictionary<SoundParamTarget, float> volumeInMappings = new Dictionary<SoundParamTarget, float>();
	}
}
