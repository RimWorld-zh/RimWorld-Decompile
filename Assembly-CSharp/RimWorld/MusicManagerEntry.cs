using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000659 RID: 1625
	public class MusicManagerEntry
	{
		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x060021E2 RID: 8674 RVA: 0x0011F3CC File Offset: 0x0011D7CC
		private float CurVolume
		{
			get
			{
				return Prefs.VolumeMusic * SongDefOf.EntrySong.volume;
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x060021E3 RID: 8675 RVA: 0x0011F3F4 File Offset: 0x0011D7F4
		public float CurSanitizedVolume
		{
			get
			{
				return AudioSourceUtility.GetSanitizedVolume(this.CurVolume, "MusicManagerEntry");
			}
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x0011F419 File Offset: 0x0011D819
		public void MusicManagerEntryUpdate()
		{
			if (this.audioSource == null || !this.audioSource.isPlaying)
			{
				this.StartPlaying();
			}
			this.audioSource.volume = this.CurSanitizedVolume;
		}

		// Token: 0x060021E5 RID: 8677 RVA: 0x0011F454 File Offset: 0x0011D854
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
