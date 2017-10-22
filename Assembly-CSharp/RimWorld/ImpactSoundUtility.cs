using Verse;
using Verse.Sound;

namespace RimWorld
{
	public static class ImpactSoundUtility
	{
		public static void PlayImpactSound(Thing hitThing, ImpactSoundTypeDef ist, Map map)
		{
			if (ist != null)
			{
				if (map == null)
				{
					Log.Warning("Can't play impact sound because map is null.");
				}
				else
				{
					SoundDef soundDef = null;
					soundDef = ((hitThing.Stuff == null) ? hitThing.def.soundImpactDefault : hitThing.Stuff.stuffProps.soundImpactStuff);
					if (soundDef.NullOrUndefined())
					{
						soundDef = SoundDefOf.BulletImpactGround;
					}
					soundDef.PlayOneShot(new TargetInfo(hitThing.PositionHeld, map, false));
				}
			}
		}
	}
}
