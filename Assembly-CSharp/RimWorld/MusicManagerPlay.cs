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

		private const float FadeoutDuration = 10f;

		private AudioSource audioSource;

		private MusicManagerState state;

		private float fadeoutFactor = 1f;

		private float nextSongStartTime = 12f;

		private SongDef lastStartedSong;

		private Queue<SongDef> recentSongs = new Queue<SongDef>();

		public bool disabled;

		private SongDef forcedNextSong;

		private bool songWasForced;

		private bool ignorePrefsVolumeThisSong;

		public float subtleAmbienceSoundVolumeMultiplier = 1f;

		private bool gameObjectCreated;

		private static readonly FloatRange SongIntervalRelax = new FloatRange(85f, 105f);

		private static readonly FloatRange SongIntervalTension = new FloatRange(2f, 5f);

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
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome && maps[i].dangerWatcher.DangerRating == StoryDanger.High)
					{
						return true;
					}
				}
				return false;
			}
		}

		public float CurVolume
		{
			get
			{
				float num = (float)((!this.ignorePrefsVolumeThisSong) ? Prefs.VolumeMusic : 1.0);
				if (this.lastStartedSong == null)
				{
					return num;
				}
				return this.lastStartedSong.volume * num * this.fadeoutFactor;
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
						goto IL_0102;
					}
					if (!this.DangerMusicMode && this.lastStartedSong.tense)
						goto IL_0102;
				}
				goto IL_0109;
			}
			return;
			IL_0109:
			this.audioSource.volume = this.CurVolume;
			if (this.audioSource.isPlaying)
			{
				if (this.state == MusicManagerState.Fadeout)
				{
					this.fadeoutFactor -= (float)(Time.deltaTime / 10.0);
					if (this.fadeoutFactor <= 0.0)
					{
						this.audioSource.Stop();
						this.ignorePrefsVolumeThisSong = false;
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
					this.StartNewSong();
				}
			}
			return;
			IL_0102:
			this.state = MusicManagerState.Fadeout;
			goto IL_0109;
		}

		private void UpdateSubtleAmbienceSoundVolumeMultiplier()
		{
			if (this.IsPlaying && this.CurVolume > 0.0010000000474974513)
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
			this.audioSource.volume = this.CurVolume;
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
			if (this.forcedNextSong != null)
			{
				SongDef result = this.forcedNextSong;
				this.forcedNextSong = null;
				this.songWasForced = true;
				return result;
			}
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
				return DefDatabase<SongDef>.GetRandom();
			}
			return source.RandomElementByWeight((Func<SongDef, float>)((SongDef s) => s.commonality));
		}

		private bool AppropriateNow(SongDef song)
		{
			if (!song.playOnMap)
			{
				return false;
			}
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
			Map map = Find.AnyPlayerHomeMap ?? Find.VisibleMap;
			if (!song.allowedSeasons.NullOrEmpty())
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
				return false;
			}
			if (song.allowedTimeOfDay != TimeOfDay.Any)
			{
				if (map == null)
				{
					return true;
				}
				if (song.allowedTimeOfDay == TimeOfDay.Night)
				{
					return GenLocalDate.DayPercent(map) < 0.20000000298023224 || GenLocalDate.DayPercent(map) > 0.699999988079071;
				}
				return GenLocalDate.DayPercent(map) > 0.20000000298023224 && GenLocalDate.DayPercent(map) < 0.699999988079071;
			}
			return true;
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
			Queue<SongDef>.Enumerator enumerator2 = this.recentSongs.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					SongDef current2 = enumerator2.Current;
					stringBuilder.AppendLine("   " + current2.defName);
				}
			}
			finally
			{
				((IDisposable)(object)enumerator2).Dispose();
			}
			Log.Message(stringBuilder.ToString());
		}
	}
}
