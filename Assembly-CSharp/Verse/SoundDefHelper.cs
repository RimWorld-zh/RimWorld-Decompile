using System;
using RimWorld.Planet;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000B7F RID: 2943
	public static class SoundDefHelper
	{
		// Token: 0x06004012 RID: 16402 RVA: 0x0021BCB0 File Offset: 0x0021A0B0
		public static bool NullOrUndefined(this SoundDef def)
		{
			return def == null || def.isUndefined;
		}

		// Token: 0x06004013 RID: 16403 RVA: 0x0021BCD4 File Offset: 0x0021A0D4
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
