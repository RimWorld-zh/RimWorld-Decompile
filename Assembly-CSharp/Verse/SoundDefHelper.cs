using System;
using RimWorld.Planet;
using Verse.Sound;

namespace Verse
{
	public static class SoundDefHelper
	{
		public static bool NullOrUndefined(this SoundDef def)
		{
			return def == null || def.isUndefined;
		}

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
