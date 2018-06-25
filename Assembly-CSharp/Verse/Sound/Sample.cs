using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DB0 RID: 3504
	public abstract class Sample
	{
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

		// Token: 0x06004E41 RID: 20033 RVA: 0x0028F11C File Offset: 0x0028D51C
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

		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x06004E42 RID: 20034 RVA: 0x0028F220 File Offset: 0x0028D620
		public float AgeRealTime
		{
			get
			{
				return Time.realtimeSinceStartup - this.startRealTime;
			}
		}

		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x06004E43 RID: 20035 RVA: 0x0028F244 File Offset: 0x0028D644
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

		// Token: 0x17000C92 RID: 3218
		// (get) Token: 0x06004E44 RID: 20036
		public abstract float ParentStartRealTime { get; }

		// Token: 0x17000C93 RID: 3219
		// (get) Token: 0x06004E45 RID: 20037
		public abstract float ParentStartTick { get; }

		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x06004E46 RID: 20038
		public abstract float ParentHashCode { get; }

		// Token: 0x17000C95 RID: 3221
		// (get) Token: 0x06004E47 RID: 20039
		public abstract SoundParams ExternalParams { get; }

		// Token: 0x17000C96 RID: 3222
		// (get) Token: 0x06004E48 RID: 20040
		public abstract SoundInfo Info { get; }

		// Token: 0x17000C97 RID: 3223
		// (get) Token: 0x06004E49 RID: 20041 RVA: 0x0028F288 File Offset: 0x0028D688
		public Map Map
		{
			get
			{
				return this.Info.Maker.Map;
			}
		}

		// Token: 0x17000C98 RID: 3224
		// (get) Token: 0x06004E4A RID: 20042 RVA: 0x0028F2B4 File Offset: 0x0028D6B4
		protected bool TestPlaying
		{
			get
			{
				return this.Info.testPlay;
			}
		}

		// Token: 0x17000C99 RID: 3225
		// (get) Token: 0x06004E4B RID: 20043 RVA: 0x0028F2D8 File Offset: 0x0028D6D8
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

		// Token: 0x17000C9A RID: 3226
		// (get) Token: 0x06004E4C RID: 20044 RVA: 0x0028F34C File Offset: 0x0028D74C
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

		// Token: 0x17000C9B RID: 3227
		// (get) Token: 0x06004E4D RID: 20045 RVA: 0x0028F38C File Offset: 0x0028D78C
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

		// Token: 0x17000C9C RID: 3228
		// (get) Token: 0x06004E4E RID: 20046 RVA: 0x0028F408 File Offset: 0x0028D808
		public float SanitizedVolume
		{
			get
			{
				return AudioSourceUtility.GetSanitizedVolume(this.Volume, this.subDef.parentDef);
			}
		}

		// Token: 0x17000C9D RID: 3229
		// (get) Token: 0x06004E4F RID: 20047 RVA: 0x0028F434 File Offset: 0x0028D834
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

		// Token: 0x17000C9E RID: 3230
		// (get) Token: 0x06004E50 RID: 20048 RVA: 0x0028F4A4 File Offset: 0x0028D8A4
		public float SanitizedPitch
		{
			get
			{
				return AudioSourceUtility.GetSanitizedPitch(this.Pitch, this.subDef.parentDef);
			}
		}

		// Token: 0x06004E51 RID: 20049 RVA: 0x0028F4D0 File Offset: 0x0028D8D0
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

		// Token: 0x06004E52 RID: 20050 RVA: 0x0028F5AC File Offset: 0x0028D9AC
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

		// Token: 0x06004E53 RID: 20051 RVA: 0x0028F619 File Offset: 0x0028DA19
		public void SignalMappedVolume(float value, SoundParamTarget sourceParam)
		{
			this.volumeInMappings[sourceParam] = value;
		}

		// Token: 0x06004E54 RID: 20052 RVA: 0x0028F62C File Offset: 0x0028DA2C
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

		// Token: 0x06004E55 RID: 20053 RVA: 0x0028F690 File Offset: 0x0028DA90
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

		// Token: 0x06004E56 RID: 20054 RVA: 0x0028F708 File Offset: 0x0028DB08
		public override int GetHashCode()
		{
			return Gen.HashCombine<SubSoundDef>(this.startRealTime.GetHashCode(), this.subDef);
		}
	}
}
