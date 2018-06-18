using System;
using RimWorld.Planet;

namespace Verse.Sound
{
	// Token: 0x02000DBB RID: 3515
	public static class SoundStarter
	{
		// Token: 0x06004E72 RID: 20082 RVA: 0x0028F3D4 File Offset: 0x0028D7D4
		public static void PlayOneShotOnCamera(this SoundDef soundDef, Map onlyThisMap = null)
		{
			if (UnityData.IsInMainThread)
			{
				if (onlyThisMap == null || (Find.CurrentMap == onlyThisMap && !WorldRendererUtility.WorldRenderedNow))
				{
					if (soundDef != null)
					{
						if (soundDef.subSounds.Count > 0)
						{
							bool flag = false;
							for (int i = 0; i < soundDef.subSounds.Count; i++)
							{
								if (soundDef.subSounds[i].onCamera)
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								Log.Error("Tried to play " + soundDef + " on camera but it has no on-camera subSounds.", false);
							}
						}
						soundDef.PlayOneShot(SoundInfo.OnCamera(MaintenanceType.None));
					}
				}
			}
		}

		// Token: 0x06004E73 RID: 20083 RVA: 0x0028F498 File Offset: 0x0028D898
		public static void PlayOneShot(this SoundDef soundDef, SoundInfo info)
		{
			if (UnityData.IsInMainThread)
			{
				if (soundDef == null)
				{
					Log.Error("Tried to PlayOneShot with null SoundDef. Info=" + info, false);
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
							Log.Error("Tried to play sustainer SoundDef " + soundDef + " as a one-shot sound.", false);
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

		// Token: 0x06004E74 RID: 20084 RVA: 0x0028F5B4 File Offset: 0x0028D9B4
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
						return soundDef2.TrySpawnSustainer(info);
					}
				}
				result = null;
			}
			else if (!soundDef.sustain)
			{
				Log.Error("Tried to spawn a sustainer from non-sustainer sound " + soundDef + ".", false);
				result = null;
			}
			else if (!info.IsOnCamera && info.Maker.Thing != null && info.Maker.Thing.Destroyed)
			{
				result = null;
			}
			else
			{
				if (soundDef.sustainStartSound != null)
				{
					soundDef.sustainStartSound.PlayOneShot(info);
				}
				result = new Sustainer(soundDef, info);
			}
			return result;
		}
	}
}
