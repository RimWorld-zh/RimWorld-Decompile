using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	[StaticConstructorOnStartup]
	public class LifeStageAge
	{
		public LifeStageDef def;

		public float minAge = 0f;

		public SoundDef soundCall = null;

		public SoundDef soundAngry = null;

		public SoundDef soundWounded = null;

		public SoundDef soundDeath = null;

		private static readonly Texture2D VeryYoungIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/VeryYoung", true);

		private static readonly Texture2D YoungIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/Young", true);

		private static readonly Texture2D AdultIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/Adult", true);

		public LifeStageAge()
		{
		}

		public Texture2D GetIcon(Pawn forPawn)
		{
			Texture2D result;
			if (this.def.iconTex != null)
			{
				result = this.def.iconTex;
			}
			else
			{
				int count = forPawn.RaceProps.lifeStageAges.Count;
				int num = forPawn.RaceProps.lifeStageAges.IndexOf(this);
				if (num == count - 1)
				{
					result = LifeStageAge.AdultIcon;
				}
				else if (num == 0)
				{
					result = LifeStageAge.VeryYoungIcon;
				}
				else
				{
					result = LifeStageAge.YoungIcon;
				}
			}
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static LifeStageAge()
		{
		}
	}
}
