using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DBB RID: 3515
	public static class SoundSlotManager
	{
		// Token: 0x06004E71 RID: 20081 RVA: 0x0028F320 File Offset: 0x0028D720
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

		// Token: 0x06004E72 RID: 20082 RVA: 0x0028F384 File Offset: 0x0028D784
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

		// Token: 0x0400343A RID: 13370
		private static Dictionary<string, float> allowedPlayTimes = new Dictionary<string, float>();
	}
}
