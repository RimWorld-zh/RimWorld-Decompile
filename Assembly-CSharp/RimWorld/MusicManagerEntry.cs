using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000657 RID: 1623
	public class MusicManagerEntry
	{
		// Token: 0x0400132F RID: 4911
		private AudioSource audioSource;

		// Token: 0x04001330 RID: 4912
		private const string SourceGameObjectName = "MusicAudioSourceDummy";

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x060021DE RID: 8670 RVA: 0x0011F61C File Offset: 0x0011DA1C
		private float CurVolume
		{
			get
			{
				return Prefs.VolumeMusic * SongDefOf.EntrySong.volume;
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x060021DF RID: 8671 RVA: 0x0011F644 File Offset: 0x0011DA44
		public float CurSanitizedVolume
		{
			get
			{
				return AudioSourceUtility.GetSanitizedVolume(this.CurVolume, "MusicManagerEntry");
			}
		}

		// Token: 0x060021E0 RID: 8672 RVA: 0x0011F669 File Offset: 0x0011DA69
		public void MusicManagerEntryUpdate()
		{
			if (this.audioSource == null || !this.audioSource.isPlaying)
			{
				this.StartPlaying();
			}
			this.audioSource.volume = this.CurSanitizedVolume;
		}

		// Token: 0x060021E1 RID: 8673 RVA: 0x0011F6A4 File Offset: 0x0011DAA4
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
