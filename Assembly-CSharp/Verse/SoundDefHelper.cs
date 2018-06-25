using System;
using RimWorld.Planet;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000B7D RID: 2941
	public static class SoundDefHelper
	{
		// Token: 0x06004019 RID: 16409 RVA: 0x0021C4FC File Offset: 0x0021A8FC
		public static bool NullOrUndefined(this SoundDef def)
		{
			return def == null || def.isUndefined;
		}

		// Token: 0x0600401A RID: 16410 RVA: 0x0021C520 File Offset: 0x0021A920
		public static bool CorrectContextNow(SoundDef def, Map sourceMap)
		{
			bool result;
			if (sourceMap != null && (Find.CurrentMap != sourceMap || WorldRendererUtility.WorldRenderedNow))
			{
				result = false;
			}
			else
			{
				switch (def.context)
				{
				case SoundContext.Any:
					result = true;
					break;
				case SoundContext.MapOnly:
					result = (Current.ProgramState == ProgramState.Playing && !WorldRendererUtility.WorldRenderedNow);
					break;
				case SoundContext.WorldOnly:
					result = WorldRendererUtility.WorldRenderedNow;
					break;
				default:
					throw new NotImplementedException();
				}
			}
			return result;
		}
	}
}
