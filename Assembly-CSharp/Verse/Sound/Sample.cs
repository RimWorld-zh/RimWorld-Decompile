using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DB1 RID: 3505
	public abstract class Sample
	{
		// Token: 0x06004E28 RID: 20008 RVA: 0x0028DA40 File Offset: 0x0028BE40
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

		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x06004E29 RID: 20009 RVA: 0x0028DB44 File Offset: 0x0028BF44
		public float AgeRealTime
		{
			get
			{
				return Time.realtimeSinceStartup - this.startRealTime;
			}
		}

		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x06004E2A RID: 20010 RVA: 0x0028DB68 File Offset: 0x0028BF68
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

		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x06004E2B RID: 20011
		public abstract float ParentStartRealTime { get; }

		// Token: 0x17000C92 RID: 3218
		// (get) Token: 0x06004E2C RID: 20012
		public abstract float ParentStartTick { get; }

		// Token: 0x17000C93 RID: 3219
		// (get) Token: 0x06004E2D RID: 20013
		public abstract float ParentHashCode { get; }

		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x06004E2E RID: 20014
		public abstract SoundParams ExternalParams { get; }

		// Token: 0x17000C95 RID: 3221
		// (get) Token: 0x06004E2F RID: 20015
		public abstract SoundInfo Info { get; }

		// Token: 0x17000C96 RID: 3222
		// (get) Token: 0x06004E30 RID: 20016 RVA: 0x0028DBAC File Offset: 0x0028BFAC
		public Map Map
		{
			get
			{
				return this.Info.Maker.Map;
			}
		}

		// Token: 0x17000C97 RID: 3223
		// (get) Token: 0x06004E31 RID: 20017 RVA: 0x0028DBD8 File Offset: 0x0028BFD8
		protected bool TestPlaying
		{
			get
			{
				return this.Info.testPlay;
			}
		}

		// Token: 0x17000C98 RID: 3224
		// (get) Token: 0x06004E32 RID: 20018 RVA: 0x0028DBFC File Offset: 0x0028BFFC
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

		// Token: 0x17000C99 RID: 3225
		// (get) Token: 0x06004E33 RID: 20019 RVA: 0x0028DC70 File Offset: 0x0028C070
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

		// Token: 0x17000C9A RID: 3226
		// (get) Token: 0x06004E34 RID: 20020 RVA: 0x0028DCB0 File Offset: 0x0028C0B0
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

		// Token: 0x17000C9B RID: 3227
		// (get) Token: 0x06004E35 RID: 20021 RVA: 0x0028DD2C File Offset: 0x0028C12C
		public float SanitizedVolume
		{
			get
			{
				return AudioSourceUtility.GetSanitizedVolume(this.Volume, this.subDef.parentDef);
			}
		}

		// Token: 0x17000C9C RID: 3228
		// (get) Token: 0x06004E36 RID: 20022 RVA: 0x0028DD58 File Offset: 0x0028C158
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

		// Token: 0x17000C9D RID: 3229
		// (get) Token: 0x06004E37 RID: 20023 RVA: 0x0028DDC8 File Offset: 0x0028C1C8
		public float SanitizedPitch
		{
			get
			{
				return AudioSourceUtility.GetSanitizedPitch(this.Pitch, this.subDef.parentDef);
			}
		}

		// Token: 0x06004E38 RID: 20024 RVA: 0x0028DDF4 File Offset: 0x0028C1F4
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

		// Token: 0x06004E39 RID: 20025 RVA: 0x0028DED0 File Offset: 0x0028C2D0
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

		// Token: 0x06004E3A RID: 20026 RVA: 0x0028DF3D File Offset: 0x0028C33D
		public void SignalMappedVolume(float value, SoundParamTarget sourceParam)
		{
			this.volumeInMappings[sourceParam] = value;
		}

		// Token: 0x06004E3B RID: 20027 RVA: 0x0028DF50 File Offset: 0x0028C350
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

		// Token: 0x06004E3C RID: 20028 RVA: 0x0028DFB4 File Offset: 0x0028C3B4
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

		// Token: 0x06004E3D RID: 20029 RVA: 0x0028E02C File Offset: 0x0028C42C
		public override int GetHashCode()
		{
			return Gen.HashCombine<SubSoundDef>(this.startRealTime.GetHashCode(), this.subDef);
		}

		// Token: 0x0400341C RID: 13340
		public SubSoundDef subDef;

		// Token: 0x0400341D RID: 13341
		public AudioSource source;

		// Token: 0x0400341E RID: 13342
		public float startRealTime;

		// Token: 0x0400341F RID: 13343
		public int startTick;

		// Token: 0x04003420 RID: 13344
		public float resolvedVolume;

		// Token: 0x04003421 RID: 13345
		public float resolvedPitch;

		// Token: 0x04003422 RID: 13346
		private bool mappingsApplied = false;

		// Token: 0x04003423 RID: 13347
		private Dictionary<SoundParamTarget, float> volumeInMappings = new Dictionary<SoundParamTarget, float>();
	}
}
