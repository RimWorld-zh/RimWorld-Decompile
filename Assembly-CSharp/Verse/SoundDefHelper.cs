using System;
using RimWorld.Planet;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000B7B RID: 2939
	public static class SoundDefHelper
	{
		// Token: 0x06004016 RID: 16406 RVA: 0x0021C420 File Offset: 0x0021A820
		public static bool NullOrUndefined(this SoundDef def)
		{
			return def == null || def.isUndefined;
		}

		// Token: 0x06004017 RID: 16407 RVA: 0x0021C444 File Offset: 0x0021A844
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
