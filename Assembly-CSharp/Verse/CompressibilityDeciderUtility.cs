using System.Collections.Generic;

namespace Verse
{
	public static class CompressibilityDeciderUtility
	{
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
							goto IL_00a8;
					}
				}
				if (!flag)
				{
					Log.ErrorOnce("Called IsSaveCompressible but there are no maps with compressor != null. This should never happen. It probably means that we're not saving any map at the moment?", 1935111328);
				}
				result = true;
			}
			goto IL_00df;
			IL_00a8:
			result = false;
			goto IL_00df;
			IL_00df:
			return result;
		}
	}
}
