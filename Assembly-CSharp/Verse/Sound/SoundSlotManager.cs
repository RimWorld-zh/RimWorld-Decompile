using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DB7 RID: 3511
	public static class SoundSlotManager
	{
		// Token: 0x06004E84 RID: 20100 RVA: 0x002908B0 File Offset: 0x0028ECB0
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

		// Token: 0x06004E85 RID: 20101 RVA: 0x00290914 File Offset: 0x0028ED14
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

		// Token: 0x04003443 RID: 13379
		private static Dictionary<string, float> allowedPlayTimes = new Dictionary<string, float>();
	}
}
