using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200030E RID: 782
	public class GameCondition_Planetkiller : GameCondition
	{
		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000D3D RID: 3389 RVA: 0x00072B7C File Offset: 0x00070F7C
		public override string TooltipString
		{
			get
			{
				Vector2 location;
				if (Find.CurrentMap != null)
				{
					location = Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile);
				}
				else
				{
					location = default(Vector2);
				}
				string text = this.def.LabelCap;
				text += "\n";
				text = text + "\n" + this.def.description;
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					"\n",
					"ImpactDate".Translate().CapitalizeFirst(),
					": ",
					GenDate.DateFullStringAt((long)GenDate.TickGameToAbs(this.startTick + base.Duration), location)
				});
				text2 = text;
				return string.Concat(new string[]
				{
					text2,
					"\n",
					"TimeLeft".Translate().CapitalizeFirst(),
					": ",
					base.TicksLeft.ToStringTicksToPeriod()
				});
			}
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x00072C80 File Offset: 0x00071080
		public override void GameConditionTick()
		{
			base.GameConditionTick();
			if (base.TicksLeft <= 179)
			{
				Find.ActiveLesson.Deactivate();
				if (base.TicksLeft == 179)
				{
					SoundDefOf.PlanetkillerImpact.PlayOneShotOnCamera(null);
				}
				if (base.TicksLeft == 90)
				{
					ScreenFader.StartFade(GameCondition_Planetkiller.FadeColor, 1f);
				}
			}
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x00072CE7 File Offset: 0x000710E7
		public override void End()
		{
			base.End();
			this.Impact();
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x00072CF6 File Offset: 0x000710F6
		private void Impact()
		{
			ScreenFader.SetColor(Color.clear);
			GenGameEnd.EndGameDialogMessage("GameOverPlanetkillerImpact".Translate(new object[]
			{
				Find.World.info.name
			}), false, GameCondition_Planetkiller.FadeColor);
		}

		// Token: 0x0400087B RID: 2171
		private const int SoundDuration = 179;

		// Token: 0x0400087C RID: 2172
		private const int FadeDuration = 90;

		// Token: 0x0400087D RID: 2173
		private static readonly Color FadeColor = Color.white;
	}
}
