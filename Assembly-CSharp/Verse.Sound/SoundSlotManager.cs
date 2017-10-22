using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	public static class SoundSlotManager
	{
		private static Dictionary<string, float> allowedPlayTimes = new Dictionary<string, float>();

		public static bool CanPlayNow(string slotName)
		{
			if (slotName == string.Empty)
			{
				return true;
			}
			float num = 0f;
			if (SoundSlotManager.allowedPlayTimes.TryGetValue(slotName, out num) && Time.realtimeSinceStartup < SoundSlotManager.allowedPlayTimes[slotName])
			{
				return false;
			}
			return true;
		}

		public static void Notify_Played(string slot, float duration)
		{
			if (!(slot == string.Empty))
			{
				float a = default(float);
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
	}
}
