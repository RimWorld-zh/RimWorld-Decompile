using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B24 RID: 2852
	[StaticConstructorOnStartup]
	public class LifeStageAge
	{
		// Token: 0x0400287C RID: 10364
		public LifeStageDef def;

		// Token: 0x0400287D RID: 10365
		public float minAge = 0f;

		// Token: 0x0400287E RID: 10366
		public SoundDef soundCall = null;

		// Token: 0x0400287F RID: 10367
		public SoundDef soundAngry = null;

		// Token: 0x04002880 RID: 10368
		public SoundDef soundWounded = null;

		// Token: 0x04002881 RID: 10369
		public SoundDef soundDeath = null;

		// Token: 0x04002882 RID: 10370
		private static readonly Texture2D VeryYoungIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/VeryYoung", true);

		// Token: 0x04002883 RID: 10371
		private static readonly Texture2D YoungIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/Young", true);

		// Token: 0x04002884 RID: 10372
		private static readonly Texture2D AdultIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/Adult", true);

		// Token: 0x06003ED0 RID: 16080 RVA: 0x00211704 File Offset: 0x0020FB04
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
	}
}
