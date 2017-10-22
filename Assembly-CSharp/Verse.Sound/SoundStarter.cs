using RimWorld.Planet;

namespace Verse.Sound
{
	public static class SoundStarter
	{
		public static void PlayOneShotOnCamera(this SoundDef soundDef, Map onlyThisMap = null)
		{
			if (UnityData.IsInMainThread)
			{
				if (onlyThisMap != null)
				{
					if (Find.VisibleMap != onlyThisMap)
						return;
					if (WorldRendererUtility.WorldRenderedNow)
						return;
				}
				if (soundDef != null)
				{
					if (soundDef.subSounds.Count > 0)
					{
						bool flag = false;
						int num = 0;
						while (num < soundDef.subSounds.Count)
						{
							if (!soundDef.subSounds[num].onCamera)
							{
								num++;
								continue;
							}
							flag = true;
							break;
						}
						if (!flag)
						{
							Log.Error("Tried to play " + soundDef + " on camera but it has no on-camera subSounds.");
						}
					}
					soundDef.PlayOneShot(SoundInfo.OnCamera(MaintenanceType.None));
				}
			}
		}

		public static void PlayOneShot(this SoundDef soundDef, SoundInfo info)
		{
			if (UnityData.IsInMainThread)
			{
				if (soundDef == null)
				{
					Log.Error("Tried to PlayOneShot with null SoundDef. Info=" + info);
				}
				else
				{
					DebugSoundEventsLog.Notify_SoundEvent(soundDef, info);
					if (soundDef != null)
					{
						if (soundDef.isUndefined)
						{
							if (Prefs.DevMode && Find.WindowStack.IsOpen(typeof(EditWindow_DefEditor)))
							{
								DefDatabase<SoundDef>.Clear();
								DefDatabase<SoundDef>.AddAllInMods();
								SoundDef soundDef2 = SoundDef.Named(soundDef.defName);
								if (!soundDef2.isUndefined)
								{
									soundDef2.PlayOneShot(info);
								}
							}
						}
						else if (soundDef.sustain)
						{
							Log.Error("Tried to play sustainer SoundDef " + soundDef + " as a one-shot sound.");
						}
						else if (SoundSlotManager.CanPlayNow(soundDef.slot))
						{
							for (int i = 0; i < soundDef.subSounds.Count; i++)
							{
								soundDef.subSounds[i].TryPlay(info);
							}
						}
					}
				}
			}
		}

		public static Sustainer TrySpawnSustainer(this SoundDef soundDef, SoundInfo info)
		{
			DebugSoundEventsLog.Notify_SoundEvent(soundDef, info);
			Sustainer result;
			if (soundDef == null)
			{
				result = null;
			}
			else if (soundDef.isUndefined)
			{
				if (Prefs.DevMode && Find.WindowStack.IsOpen(typeof(EditWindow_DefEditor)))
				{
					DefDatabase<SoundDef>.Clear();
					DefDatabase<SoundDef>.AddAllInMods();
					SoundDef soundDef2 = SoundDef.Named(soundDef.defName);
					if (!soundDef2.isUndefined)
					{
						result = soundDef2.TrySpawnSustainer(info);
						goto IL_0112;
					}
				}
				result = null;
			}
			else if (!soundDef.sustain)
			{
				Log.Error("Tried to spawn a sustainer from non-sustainer sound " + soundDef + ".");
				result = null;
			}
			else if (!info.IsOnCamera && info.Maker.Thing != null && info.Maker.Thing.Destroyed)
			{
				result = null;
			}
			else
			{
				if (!soundDef.sustainStartSound.NullOrEmpty())
				{
					SoundDef.Named(soundDef.sustainStartSound).PlayOneShot(info);
				}
				result = new Sustainer(soundDef, info);
			}
			goto IL_0112;
			IL_0112:
			return result;
		}
	}
}
