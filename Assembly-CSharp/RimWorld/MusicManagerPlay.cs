using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class MusicManagerPlay
	{
		private enum MusicManagerState
		{
			Normal = 0,
			Fadeout = 1
		}

		private AudioSource audioSource;

		private MusicManagerState state = MusicManagerState.Normal;

		private float fadeoutFactor = 1f;

		private float nextSongStartTime = 12f;

		private SongDef lastStartedSong;

		private Queue<SongDef> recentSongs = new Queue<SongDef>();

		public bool disabled = false;

		private SongDef forcedNextSong = null;

		private bool songWasForced = false;

		private bool ignorePrefsVolumeThisSong = false;

		public float subtleAmbienceSoundVolumeMultiplier = 1f;

		private bool gameObjectCreated;

		private static readonly FloatRange SongIntervalRelax = new FloatRange(85f, 105f);

		private static readonly FloatRange SongIntervalTension = new FloatRange(2f, 5f);

		private const float FadeoutDuration = 10f;

		private float CurTime
		{
			get
			{
				return Time.time;
			}
		}

		private bool DangerMusicMode
		{
			get
			{
				List<Map> maps = Find.Maps;
				int num = 0;
				bool result;
				while (true)
				{
					if (num < maps.Count)
					{
						if (maps[num].IsPlayerHome && maps[num].dangerWatcher.DangerRating == StoryDanger.High)
						{
							result = true;
							break;
						}
						num++;
						continue;
					}
					result = false;
					break;
				}
				return result;
			}
		}

		private float CurVolume
		{
			get
			{
				float num = (float)((!this.ignorePrefsVolumeThisSong) ? Prefs.VolumeMusic : 1.0);
				return (this.lastStartedSong != null) ? (this.lastStartedSong.volume * num * this.fadeoutFactor) : num;
			}
		}

		public float CurSanitizedVolume
		{
			get
			{
				return AudioSourceUtility.GetSanitizedVolume(this.CurVolume, "MusicManagerPlay");
			}
		}

		public bool IsPlaying
		{
			get
			{
				return this.audioSource.isPlaying;
			}
		}

		public void ForceSilenceFor(float time)
		{
			this.nextSongStartTime = this.CurTime + time;
		}

		public void MusicUpdate()
		{
			if (!this.gameObjectCreated)
			{
				this.gameObjectCreated = true;
				GameObject gameObject = new GameObject("MusicAudioSourceDummy");
				gameObject.transform.parent = Find.Root.soundRoot.sourcePool.sourcePoolCamera.cameraSourcesContainer.transform;
				this.audioSource = gameObject.AddComponent<AudioSource>();
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
					this.state = MusicManagerState.Normal;
					this.fadeoutFactor = 1f;
				}
				if (this.audioSource.isPlaying && !this.songWasForced)
				{
					if (this.DangerMusicMode && !this.lastStartedSong.tense)
					{
						goto IL_010c;
					}
					if (!this.DangerMusicMode && this.lastStartedSong.tense)
						goto IL_010c;
				}
				goto IL_0116;
			}
			return;
			IL_0116:
			this.audioSource.volume = this.CurSanitizedVolume;
			if (this.audioSource.isPlaying)
			{
				if (this.state == MusicManagerState.Fadeout)
				{
					this.fadeoutFactor -= (float)(Time.deltaTime / 10.0);
					if (this.fadeoutFactor <= 0.0)
					{
						this.audioSource.Stop();
						this.state = MusicManagerState.Normal;
						this.fadeoutFactor = 1f;
					}
				}
			}
			else
			{
				if (this.DangerMusicMode)
				{
					float num = this.nextSongStartTime;
					float curTime = this.CurTime;
					FloatRange songIntervalTension = MusicManagerPlay.SongIntervalTension;
					if (num > curTime + songIntervalTension.max)
					{
						this.nextSongStartTime = this.CurTime + MusicManagerPlay.SongIntervalTension.RandomInRange;
					}
				}
				if (this.nextSongStartTime < this.CurTime - 5.0)
				{
					float num2 = (!this.DangerMusicMode) ? MusicManagerPlay.SongIntervalRelax.RandomInRange : MusicManagerPlay.SongIntervalTension.RandomInRange;
					this.nextSongStartTime = this.CurTime + num2;
				}
				if (this.CurTime >= this.nextSongStartTime)
				{
					this.ignorePrefsVolumeThisSong = false;
					this.StartNewSong();
				}
			}
			return;
			IL_010c:
			this.state = MusicManagerState.Fadeout;
			goto IL_0116;
		}

		private void UpdateSubtleAmbienceSoundVolumeMultiplier()
		{
			if (this.IsPlaying && this.CurSanitizedVolume > 0.0010000000474974513)
			{
				this.subtleAmbienceSoundVolumeMultiplier -= (float)(Time.deltaTime * 0.10000000149011612);
			}
			else
			{
				this.subtleAmbienceSoundVolumeMultiplier += (float)(Time.deltaTime * 0.10000000149011612);
			}
			this.subtleAmbienceSoundVolumeMultiplier = Mathf.Clamp01(this.subtleAmbienceSoundVolumeMultiplier);
		}

		private void StartNewSong()
		{
			this.lastStartedSong = this.ChooseNextSong();
			this.audioSource.clip = this.lastStartedSong.clip;
			this.audioSource.volume = this.CurSanitizedVolume;
			this.audioSource.spatialBlend = 0f;
			this.audioSource.Play();
			this.recentSongs.Enqueue(this.lastStartedSong);
		}

		public void ForceStartSong(SongDef song, bool ignorePrefsVolume)
		{
			this.forcedNextSong = song;
			this.ignorePrefsVolumeThisSong = ignorePrefsVolume;
			this.StartNewSong();
		}

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
				while (!source.Any() && this.recentSongs.Count > 0)
				{
					this.recentSongs.Dequeue();
				}
				if (!source.Any())
				{
					Log.Error("Could not get any appropriate song. Getting random and logging song selection data.");
					this.LogSongSelectionData();
					result = DefDatabase<SongDef>.GetRandom();
				}
				else
				{
					result = source.RandomElementByWeight((Func<SongDef, float>)((SongDef s) => s.commonality));
				}
			}
			return result;
		}

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
						result = false;
						goto IL_0129;
					}
				}
				else if (song.tense)
				{
					result = false;
					goto IL_0129;
				}
				Map map = Find.AnyPlayerHomeMap ?? Find.VisibleMap;
				if (!song.allowedSeasons.NullOrEmpty())
				{
					if (map == null)
					{
						result = false;
						goto IL_0129;
					}
					if (!song.allowedSeasons.Contains(GenLocalDate.Season(map)))
					{
						result = false;
						goto IL_0129;
					}
				}
				result = (!this.recentSongs.Contains(song) && (song.allowedTimeOfDay == TimeOfDay.Any || map == null || ((song.allowedTimeOfDay != 0) ? (GenLocalDate.DayPercent(map) > 0.20000000298023224 && GenLocalDate.DayPercent(map) < 0.699999988079071) : (GenLocalDate.DayPercent(map) < 0.20000000298023224 || GenLocalDate.DayPercent(map) > 0.699999988079071))));
			}
			goto IL_0129;
			IL_0129:
			return result;
		}

		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("MusicManagerMap");
			stringBuilder.AppendLine("state: " + this.state);
			stringBuilder.AppendLine("lastStartedSong: " + this.lastStartedSong);
			stringBuilder.AppendLine("fadeoutFactor: " + this.fadeoutFactor);
			stringBuilder.AppendLine("nextSongStartTime: " + this.nextSongStartTime);
			stringBuilder.AppendLine("CurTime: " + this.CurTime);
			stringBuilder.AppendLine("recentSongs: " + GenText.ToCommaList(from s in this.recentSongs
			select s.defName, true));
			stringBuilder.AppendLine("disabled: " + this.disabled);
			return stringBuilder.ToString();
		}

		public void LogSongSelectionData()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Most recent song: " + ((this.lastStartedSong == null) ? "None" : this.lastStartedSong.defName));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Songs appropriate to play now:");
			foreach (SongDef item in from s in DefDatabase<SongDef>.AllDefs
			where this.AppropriateNow(s)
			select s)
			{
				stringBuilder.AppendLine("   " + item.defName);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Recently played songs:");
			foreach (SongDef recentSong in this.recentSongs)
			{
				stringBuilder.AppendLine("   " + recentSong.defName);
			}
			Log.Message(stringBuilder.ToString());
		}
	}
}
