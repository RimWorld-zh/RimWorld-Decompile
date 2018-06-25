using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000DBA RID: 3514
	public static class SoundSlotManager
	{
		// Token: 0x0400344A RID: 13386
		private static Dictionary<string, float> allowedPlayTimes = new Dictionary<string, float>();

		// Token: 0x06004E88 RID: 20104 RVA: 0x00290CBC File Offset: 0x0028F0BC
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

		// Token: 0x06004E89 RID: 20105 RVA: 0x00290D20 File Offset: 0x0028F120
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
	}
}
