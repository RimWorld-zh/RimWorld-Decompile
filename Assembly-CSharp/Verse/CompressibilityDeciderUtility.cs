using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C04 RID: 3076
	public static class CompressibilityDeciderUtility
	{
		// Token: 0x06004345 RID: 17221 RVA: 0x00239308 File Offset: 0x00237708
		public static bool IsSaveCompressible(this Thing t)
		{
			bool result;
			if (Scribe.saver.savingForDebug)
			{
				result = false;
			}
			else if (!t.def.saveCompressible)
			{
				result = false;
			}
			else if (t.def.useHitPoints && t.HitPoints != t.MaxHitPoints)
			{
				result = false;
			}
			else if (!t.Spawned)
			{
				result = false;
			}
			else
			{
				bool flag = false;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].compressor != null)
					{
						flag = true;
						if (maps[i].compressor.compressibilityDecider.IsReferenced(t))
						{
							return false;
						}
					}
				}
				if (!flag)
				{
					Log.ErrorOnce("Called IsSaveCompressible but there are no maps with compressor != null. This should never happen. It probably means that we're not saving any map at the moment?", 1935111328, false);
				}
				result = true;
			}
			return result;
		}
	}
}
