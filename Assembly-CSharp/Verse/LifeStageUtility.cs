using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000F5D RID: 3933
	public static class LifeStageUtility
	{
		// Token: 0x06005F1B RID: 24347 RVA: 0x0030748C File Offset: 0x0030588C
		public static void PlayNearestLifestageSound(Pawn pawn, Func<LifeStageAge, SoundDef> getter, float volumeFactor = 1f)
		{
			SoundDef soundDef;
			float pitchFactor;
			float num;
			LifeStageUtility.GetNearestLifestageSound(pawn, getter, out soundDef, out pitchFactor, out num);
			if (soundDef != null)
			{
				if (pawn.SpawnedOrAnyParentSpawned)
				{
					SoundInfo info = SoundInfo.InMap(new TargetInfo(pawn.PositionHeld, pawn.MapHeld, false), MaintenanceType.None);
					info.pitchFactor = pitchFactor;
					info.volumeFactor = num * volumeFactor;
					soundDef.PlayOneShot(info);
				}
			}
		}

		// Token: 0x06005F1C RID: 24348 RVA: 0x003074F4 File Offset: 0x003058F4
		private static void GetNearestLifestageSound(Pawn pawn, Func<LifeStageAge, SoundDef> getter, out SoundDef def, out float pitch, out float volume)
		{
			int num = pawn.ageTracker.CurLifeStageIndex;
			LifeStageAge lifeStageAge;
			for (;;)
			{
				lifeStageAge = pawn.RaceProps.lifeStageAges[num];
				def = getter(lifeStageAge);
				if (def != null)
				{
					break;
				}
				num++;
				if (num < 0 || num >= pawn.RaceProps.lifeStageAges.Count)
				{
					goto IL_95;
				}
			}
			pitch = pawn.ageTracker.CurLifeStage.voxPitch / lifeStageAge.def.voxPitch;
			volume = pawn.ageTracker.CurLifeStage.voxVolume / lifeStageAge.def.voxVolume;
			return;
			IL_95:
			def = null;
			pitch = (volume = 1f);
		}
	}
}
