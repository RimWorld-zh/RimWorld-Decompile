using System;
using Verse.Sound;

namespace Verse
{
	public static class LifeStageUtility
	{
		public static void PlayNearestLifestageSound(Pawn pawn, Func<LifeStageAge, SoundDef> getter, float volumeFactor = 1f)
		{
			SoundDef soundDef = default(SoundDef);
			float pitchFactor = default(float);
			float num = default(float);
			LifeStageUtility.GetNearestLifestageSound(pawn, getter, out soundDef, out pitchFactor, out num);
			if (soundDef != null && pawn.SpawnedOrAnyParentSpawned)
			{
				SoundInfo info = SoundInfo.InMap(new TargetInfo(pawn.PositionHeld, pawn.MapHeld, false), MaintenanceType.None);
				info.pitchFactor = pitchFactor;
				info.volumeFactor = num * volumeFactor;
				soundDef.PlayOneShot(info);
			}
		}

		private static void GetNearestLifestageSound(Pawn pawn, Func<LifeStageAge, SoundDef> getter, out SoundDef def, out float pitch, out float volume)
		{
			int num = pawn.ageTracker.CurLifeStageIndex;
			while (true)
			{
				LifeStageAge lifeStageAge = pawn.RaceProps.lifeStageAges[num];
				def = getter(lifeStageAge);
				if (def != null)
				{
					pitch = pawn.ageTracker.CurLifeStage.voxPitch / lifeStageAge.def.voxPitch;
					volume = pawn.ageTracker.CurLifeStage.voxVolume / lifeStageAge.def.voxVolume;
					return;
				}
				num++;
				if (num < 0)
					break;
				if (num >= pawn.RaceProps.lifeStageAges.Count)
					break;
			}
			def = null;
			pitch = (volume = 1f);
		}
	}
}
