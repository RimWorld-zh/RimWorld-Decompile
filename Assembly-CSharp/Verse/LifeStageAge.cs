using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B23 RID: 2851
	[StaticConstructorOnStartup]
	public class LifeStageAge
	{
		// Token: 0x04002875 RID: 10357
		public LifeStageDef def;

		// Token: 0x04002876 RID: 10358
		public float minAge = 0f;

		// Token: 0x04002877 RID: 10359
		public SoundDef soundCall = null;

		// Token: 0x04002878 RID: 10360
		public SoundDef soundAngry = null;

		// Token: 0x04002879 RID: 10361
		public SoundDef soundWounded = null;

		// Token: 0x0400287A RID: 10362
		public SoundDef soundDeath = null;

		// Token: 0x0400287B RID: 10363
		private static readonly Texture2D VeryYoungIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/VeryYoung", true);

		// Token: 0x0400287C RID: 10364
		private static readonly Texture2D YoungIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/Young", true);

		// Token: 0x0400287D RID: 10365
		private static readonly Texture2D AdultIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/Adult", true);

		// Token: 0x06003ED0 RID: 16080 RVA: 0x00211424 File Offset: 0x0020F824
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
