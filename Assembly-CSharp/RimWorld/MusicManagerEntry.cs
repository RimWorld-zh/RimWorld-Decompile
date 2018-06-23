using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000655 RID: 1621
	public class MusicManagerEntry
	{
		// Token: 0x0400132F RID: 4911
		private AudioSource audioSource;

		// Token: 0x04001330 RID: 4912
		private const string SourceGameObjectName = "MusicAudioSourceDummy";

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x060021DA RID: 8666 RVA: 0x0011F4CC File Offset: 0x0011D8CC
		private float CurVolume
		{
			get
			{
				return Prefs.VolumeMusic * SongDefOf.EntrySong.volume;
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x060021DB RID: 8667 RVA: 0x0011F4F4 File Offset: 0x0011D8F4
		public float CurSanitizedVolume
		{
			get
			{
				return AudioSourceUtility.GetSanitizedVolume(this.CurVolume, "MusicManagerEntry");
			}
		}

		// Token: 0x060021DC RID: 8668 RVA: 0x0011F519 File Offset: 0x0011D919
		public void MusicManagerEntryUpdate()
		{
			if (this.audioSource == null || !this.audioSource.isPlaying)
			{
				this.StartPlaying();
			}
			this.audioSource.volume = this.CurSanitizedVolume;
		}

		// Token: 0x060021DD RID: 8669 RVA: 0x0011F554 File Offset: 0x0011D954
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
