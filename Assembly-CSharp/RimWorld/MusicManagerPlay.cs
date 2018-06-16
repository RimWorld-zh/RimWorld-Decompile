using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200065A RID: 1626
	[HasDebugOutput]
	public class MusicManagerPlay
	{
		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x060021E5 RID: 8677 RVA: 0x0011F550 File Offset: 0x0011D950
		private float CurTime
		{
			get
			{
				return Time.time;
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x060021E6 RID: 8678 RVA: 0x0011F56C File Offset: 0x0011D96C
		private bool DangerMusicMode
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].dangerWatcher.DangerRating == StoryDanger.High)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x060021E7 RID: 8679 RVA: 0x0011F5C0 File Offset: 0x0011D9C0
		private float CurVolume
		{
			get
			{
				float num = (!this.ignorePrefsVolumeThisSong) ? Prefs.VolumeMusic : 1f;
				float result;
				if (this.lastStartedSong == null)
				{
					result = num;
				}
				else
				{
					result = this.lastStartedSong.volume * num * this.fadeoutFactor;
				}
				return result;
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x060021E8 RID: 8680 RVA: 0x0011F618 File Offset: 0x0011DA18
		public float CurSanitizedVolume
		{
			get
			{
				return AudioSourceUtility.GetSanitizedVolume(this.CurVolume, "MusicManagerPlay");
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x060021E9 RID: 8681 RVA: 0x0011F640 File Offset: 0x0011DA40
		public bool IsPlaying
		{
			get
			{
				return this.audioSource.isPlaying;
			}
		}

		// Token: 0x060021EA RID: 8682 RVA: 0x0011F660 File Offset: 0x0011DA60
		public void ForceSilenceFor(float time)
		{
			this.nextSongStartTime = this.CurTime + time;
		}

		// Token: 0x060021EB RID: 8683 RVA: 0x0011F674 File Offset: 0x0011DA74
		public void MusicUpdate()
		{
			if (!this.gameObjectCreated)
			{
				this.gameObjectCreated = true;
				this.audioSource = new GameObject("MusicAudioSourceDummy")
				{
					transform = 
					{
						parent = Find.Root.soundRoot.sourcePool.sourcePoolCamera.cameraSourcesContainer.transform
					}
				}.AddComponent<AudioSource>();
				this.audioSource.bypassEffects = true;
				this.audioSource.bypassListenerEffects = true;
				this.audioSource.bypassReverbZones = true;
				this.audioSource.priority = 0;
			}
			this.UpdateSubtleAmbienceSoundVolumeMultiplier();
			if (!this.disabled)
			{
				if (this.songWasForced)
				{
					this.state = MusicManagerPlay.MusicManagerState.Normal;
					this.fadeoutFactor = 1f;
				}
				if (this.audioSource.isPlaying && !this.songWasForced)
				{
					if ((this.DangerMusicMode && !this.lastStartedSong.tense) || (!this.DangerMusicMode && this.lastStartedSong.tense))
					{
						this.state = MusicManagerPlay.MusicManagerState.Fadeout;
					}
				}
				this.audioSource.volume = this.CurSanitizedVolume;
				if (this.audioSource.isPlaying)
				{
					if (this.state == MusicManagerPlay.MusicManagerState.Fadeout)
					{
						this.fadeoutFactor -= Time.deltaTime / 10f;
						if (this.fadeoutFactor <= 0f)
						{
							this.audioSource.Stop();
							this.state = MusicManagerPlay.MusicManagerState.Normal;
							this.fadeoutFactor = 1f;
						}
					}
				}
				else
				{
					if (this.DangerMusicMode && this.nextSongStartTime > this.CurTime + MusicManagerPlay.SongIntervalTension.max)
					{
						this.nextSongStartTime = this.CurTime + MusicManagerPlay.SongIntervalTension.RandomInRange;
					}
					if (this.nextSongStartTime < this.CurTime - 5f)
					{
						float randomInRange;
						if (this.DangerMusicMode)
						{
							randomInRange = MusicManagerPlay.SongIntervalTension.RandomInRange;
						}
						else
						{
							randomInRange = MusicManagerPlay.SongIntervalRelax.RandomInRange;
						}
						this.nextSongStartTime = this.CurTime + randomInRange;
					}
					if (this.CurTime >= this.nextSongStartTime)
					{
						this.ignorePrefsVolumeThisSong = false;
						this.StartNewSong();
					}
				}
			}
		}

		// Token: 0x060021EC RID: 8684 RVA: 0x0011F8D0 File Offset: 0x0011DCD0
		private void UpdateSubtleAmbienceSoundVolumeMultiplier()
		{
			if (this.IsPlaying && this.CurSanitizedVolume > 0.001f)
			{
				this.subtleAmbienceSoundVolumeMultiplier -= Time.deltaTime * 0.1f;
			}
			else
			{
				this.subtleAmbienceSoundVolumeMultiplier += Time.deltaTime * 0.1f;
			}
			this.subtleAmbienceSoundVolumeMultiplier = Mathf.Clamp01(this.subtleAmbienceSoundVolumeMultiplier);
		}

		// Token: 0x060021ED RID: 8685 RVA: 0x0011F940 File Offset: 0x0011DD40
		private void StartNewSong()
		{
			this.lastStartedSong = this.ChooseNextSong();
			this.audioSource.clip = this.lastStartedSong.clip;
			this.audioSource.volume = this.CurSanitizedVolume;
			this.audioSource.spatialBlend = 0f;
			this.audioSource.Play();
			this.recentSongs.Enqueue(this.lastStartedSong);
		}

		// Token: 0x060021EE RID: 8686 RVA: 0x0011F9AD File Offset: 0x0011DDAD
		public void ForceStartSong(SongDef song, bool ignorePrefsVolume)
		{
			this.forcedNextSong = song;
			this.ignorePrefsVolumeThisSong = ignorePrefsVolume;
			this.StartNewSong();
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x0011F9C4 File Offset: 0x0011DDC4
		private SongDef ChooseNextSong()
		{
			this.songWasForced = false;
			SongDef result;
			if (this.forcedNextSong != null)
			{
				SongDef songDef = this.forcedNextSong;
				this.forcedNextSong = null;
				this.songWasForced = true;
				result = songDef;
			}
			else
			{
				IEnumerable<SongDef> source = from song in DefDatabase<SongDef>.AllDefs
				where this.AppropriateNow(song)
				select song;
				while (this.recentSongs.Count > 7)
				{
					this.recentSongs.Dequeue();
				}
				while (!source.Any<SongDef>() && this.recentSongs.Count > 0)
				{
					this.recentSongs.Dequeue();
				}
				if (!source.Any<SongDef>())
				{
					Log.Error("Could not get any appropriate song. Getting random and logging song selection data.", false);
					this.SongSelectionData();
					result = DefDatabase<SongDef>.GetRandom();
				}
				else
				{
					result = source.RandomElementByWeight((SongDef s) => s.commonality);
				}
			}
			return result;
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x0011FAC0 File Offset: 0x0011DEC0
		private bool AppropriateNow(SongDef song)
		{
			bool result;
			if (!song.playOnMap)
			{
				result = false;
			}
			else
			{
				if (this.DangerMusicMode)
				{
					if (!song.tense)
					{
						return false;
					}
				}
				else if (song.tense)
				{
					return false;
				}
				Map map = Find.AnyPlayerHomeMap ?? Find.CurrentMap;
				if (!song.allowedSeasons.NullOrEmpty<Season>())
				{
					if (map == null)
					{
						return false;
					}
					if (!song.allowedSeasons.Contains(GenLocalDate.Season(map)))
					{
						return false;
					}
				}
				if (this.recentSongs.Contains(song))
				{
					result = false;
				}
				else if (song.allowedTimeOfDay != TimeOfDay.Any)
				{
					if (map == null)
					{
						result = true;
					}
					else if (song.allowedTimeOfDay == TimeOfDay.Night)
					{
						result = (GenLocalDate.DayPercent(map) < 0.2f || GenLocalDate.DayPercent(map) > 0.7f);
					}
					else
					{
						result = (GenLocalDate.DayPercent(map) > 0.2f && GenLocalDate.DayPercent(map) < 0.7f);
					}
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x0011FBF8 File Offset: 0x0011DFF8
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("MusicManagerMap");
			stringBuilder.AppendLine("state: " + this.state);
			stringBuilder.AppendLine("lastStartedSong: " + this.lastStartedSong);
			stringBuilder.AppendLine("fadeoutFactor: " + this.fadeoutFactor);
			stringBuilder.AppendLine("nextSongStartTime: " + this.nextSongStartTime);
			stringBuilder.AppendLine("CurTime: " + this.CurTime);
			stringBuilder.AppendLine("recentSongs: " + (from s in this.recentSongs
			select s.defName).ToCommaList(true));
			stringBuilder.AppendLine("disabled: " + this.disabled);
			return stringBuilder.ToString();
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x0011FD08 File Offset: 0x0011E108
		[DebugOutput]
		public void SongSelectionData()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Most recent song: " + ((this.lastStartedSong == null) ? "None" : this.lastStartedSong.defName));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Songs appropriate to play now:");
			foreach (SongDef songDef in from s in DefDatabase<SongDef>.AllDefs
			where this.AppropriateNow(s)
			select s)
			{
				stringBuilder.AppendLine("   " + songDef.defName);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Recently played songs:");
			foreach (SongDef songDef2 in this.recentSongs)
			{
				stringBuilder.AppendLine("   " + songDef2.defName);
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x04001334 RID: 4916
		private AudioSource audioSource;

		// Token: 0x04001335 RID: 4917
		private MusicManagerPlay.MusicManagerState state = MusicManagerPlay.MusicManagerState.Normal;

		// Token: 0x04001336 RID: 4918
		private float fadeoutFactor = 1f;

		// Token: 0x04001337 RID: 4919
		private float nextSongStartTime = 12f;

		// Token: 0x04001338 RID: 4920
		private SongDef lastStartedSong;

		// Token: 0x04001339 RID: 4921
		private Queue<SongDef> recentSongs = new Queue<SongDef>();

		// Token: 0x0400133A RID: 4922
		public bool disabled = false;

		// Token: 0x0400133B RID: 4923
		private SongDef forcedNextSong = null;

		// Token: 0x0400133C RID: 4924
		private bool songWasForced = false;

		// Token: 0x0400133D RID: 4925
		private bool ignorePrefsVolumeThisSong = false;

		// Token: 0x0400133E RID: 4926
		public float subtleAmbienceSoundVolumeMultiplier = 1f;

		// Token: 0x0400133F RID: 4927
		private bool gameObjectCreated;

		// Token: 0x04001340 RID: 4928
		private static readonly FloatRange SongIntervalRelax = new FloatRange(85f, 105f);

		// Token: 0x04001341 RID: 4929
		private static readonly FloatRange SongIntervalTension = new FloatRange(2f, 5f);

		// Token: 0x04001342 RID: 4930
		private const float FadeoutDuration = 10f;

		// Token: 0x0200065B RID: 1627
		private enum MusicManagerState
		{
			// Token: 0x04001346 RID: 4934
			Normal,
			// Token: 0x04001347 RID: 4935
			Fadeout
		}
	}
}
