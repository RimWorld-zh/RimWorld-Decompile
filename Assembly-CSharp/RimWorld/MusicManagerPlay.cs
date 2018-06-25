using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[HasDebugOutput]
	public class MusicManagerPlay
	{
		private AudioSource audioSource;

		private MusicManagerPlay.MusicManagerState state = MusicManagerPlay.MusicManagerState.Normal;

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

		[CompilerGenerated]
		private static Func<SongDef, float> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<SongDef, string> <>f__am$cache1;

		public MusicManagerPlay()
		{
		}

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
					if (maps[i].dangerWatcher.DangerRating == StoryDanger.High)
					{
						return true;
					}
				}
				return false;
			}
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static MusicManagerPlay()
		{
		}

		[CompilerGenerated]
		private bool <ChooseNextSong>m__0(SongDef song)
		{
			return this.AppropriateNow(song);
		}

		[CompilerGenerated]
		private static float <ChooseNextSong>m__1(SongDef s)
		{
			return s.commonality;
		}

		[CompilerGenerated]
		private static string <DebugString>m__2(SongDef s)
		{
			return s.defName;
		}

		[CompilerGenerated]
		private bool <SongSelectionData>m__3(SongDef s)
		{
			return this.AppropriateNow(s);
		}

		private enum MusicManagerState
		{
			Normal,
			Fadeout
		}
	}
}
