using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000657 RID: 1623
	public class MusicManagerEntry
	{
		// Token: 0x04001333 RID: 4915
		private AudioSource audioSource;

		// Token: 0x04001334 RID: 4916
		private const string SourceGameObjectName = "MusicAudioSourceDummy";

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x060021DD RID: 8669 RVA: 0x0011F884 File Offset: 0x0011DC84
		private float CurVolume
		{
			get
			{
				return Prefs.VolumeMusic * SongDefOf.EntrySong.volume;
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x060021DE RID: 8670 RVA: 0x0011F8AC File Offset: 0x0011DCAC
		public float CurSanitizedVolume
		{
			get
			{
				return AudioSourceUtility.GetSanitizedVolume(this.CurVolume, "MusicManagerEntry");
			}
		}

		// Token: 0x060021DF RID: 8671 RVA: 0x0011F8D1 File Offset: 0x0011DCD1
		public void MusicManagerEntryUpdate()
		{
			if (this.audioSource == null || !this.audioSource.isPlaying)
			{
				this.StartPlaying();
			}
			this.audioSource.volume = this.CurSanitizedVolume;
		}

		// Token: 0x060021E0 RID: 8672 RVA: 0x0011F90C File Offset: 0x0011DD0C
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
	}
}
