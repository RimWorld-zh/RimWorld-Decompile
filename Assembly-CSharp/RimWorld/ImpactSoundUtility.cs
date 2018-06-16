using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000989 RID: 2441
	public static class ImpactSoundUtility
	{
		// Token: 0x060036F0 RID: 14064 RVA: 0x001D5AB8 File Offset: 0x001D3EB8
		public static void PlayImpactSound(Thing hitThing, ImpactSoundTypeDef ist, Map map)
		{
			if (ist != null)
			{
				if (!ist.playOnlyIfHitPawn || hitThing is Pawn)
				{
					if (map == null)
					{
						Log.Warning("Can't play impact sound because map is null.", false);
					}
					else
					{
						SoundDef soundDef;
						if (hitThing.Stuff != null)
						{
							soundDef = hitThing.Stuff.stuffProps.soundImpactStuff;
						}
						else
						{
							soundDef = hitThing.def.soundImpactDefault;
						}
						if (soundDef.NullOrUndefined())
						{
							soundDef = SoundDefOf.BulletImpact_Ground;
						}
						soundDef.PlayOneShot(new TargetInfo(hitThing.PositionHeld, map, false));
					}
				}
			}
		}
	}
}
