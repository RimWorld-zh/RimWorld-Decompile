using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	public static class SoundSlotManager
	{
		private static Dictionary<string, float> allowedPlayTimes = new Dictionary<string, float>();

		public static bool CanPlayNow(string slotName)
		{
			bool result;
			if (slotName == "")
			{
				result = true;
			}
			else
			{
				float num = 0f;
				if (SoundSlotManager.allowedPlayTimes.TryGetValue(slotName, out num))
				{
					if (Time.realtimeSinceStartup < SoundSlotManager.allowedPlayTimes[slotName])
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		public static void Notify_Played(string slot, float duration)
		{
			if (!(slot == ""))
			{
				float a;
				if (SoundSlotManager.allowedPlayTimes.TryGetValue(slot, out a))
				{
					SoundSlotManager.allowedPlayTimes[slot] = Mathf.Max(a, Time.realtimeSinceStartup + duration);
				}
				else
				{
					SoundSlotManager.allowedPlayTimes[slot] = Time.realtimeSinceStartup + duration;
				}
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static SoundSlotManager()
		{
		}
	}
}
