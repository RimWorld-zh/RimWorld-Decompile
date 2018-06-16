using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000659 RID: 1625
	public class MusicManagerEntry
	{
		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x060021E0 RID: 8672 RVA: 0x0011F354 File Offset: 0x0011D754
		private float CurVolume
		{
			get
			{
				return Prefs.VolumeMusic * SongDefOf.EntrySong.volume;
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x060021E1 RID: 8673 RVA: 0x0011F37C File Offset: 0x0011D77C
		public float CurSanitizedVolume
		{
			get
			{
				return AudioSourceUtility.GetSanitizedVolume(this.CurVolume, "MusicManagerEntry");
			}
		}

		// Token: 0x060021E2 RID: 8674 RVA: 0x0011F3A1 File Offset: 0x0011D7A1
		public void MusicManagerEntryUpdate()
		{
			if (this.audioSource == null || !this.audioSource.isPlaying)
			{
				this.StartPlaying();
			}
			this.audioSource.volume = this.CurSanitizedVolume;
		}

		// Token: 0x060021E3 RID: 8675 RVA: 0x0011F3DC File Offset: 0x0011D7DC
		private void StartPlaying()
		{
			if (this.audioSource != null && !this.audioSource.isPlaying)
			{
				this.audioSource.Play();
			}
			else if (GameObject.Find("MusicAudioSourceDummy") != null)
			{
				Log.Error("MusicManagerEntry did StartPlaying but there is already a music source GameObject.", false);
			}
			else
			{
				this.audioSource = new GameObject("MusicAudioSourceDummy")
				{
					transform = 
					{
						parent = Camera.main.transform
					}
				}.AddComponent<AudioSource>();
				this.audioSource.bypassEffects = true;
				this.audioSource.bypassListenerEffects = true;
				this.audioSource.bypassReverbZones = true;
				this.audioSource.priority = 0;
				this.audioSource.clip = SongDefOf.EntrySong.clip;
				this.audioSource.volume = this.CurSanitizedVolume;
				this.audioSource.loop = true;
				this.audioSource.spatialBlend = 0f;
				this.audioSource.Play();
			}
		}

		// Token: 0x04001332 RID: 4914
		private AudioSource audioSource;

		// Token: 0x04001333 RID: 4915
		private const string SourceGameObjectName = "MusicAudioSourceDummy";
	}
}
