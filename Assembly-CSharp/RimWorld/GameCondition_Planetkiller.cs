using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class GameCondition_Planetkiller : GameCondition
	{
		private const int SoundDuration = 179;

		private const int FadeDuration = 90;

		private static readonly Color FadeColor = Color.white;

		public override string TooltipString
		{
			get
			{
				string labelCap = base.def.LabelCap;
				labelCap += "\n";
				string text;
				labelCap = (text = labelCap + "\n" + base.def.description);
				labelCap = (text = text + "\n" + "ImpactDate".Translate().CapitalizeFirst() + ": " + GenDate.DateFullStringAt(GenDate.TickGameToAbs(base.startTick + base.Duration), Find.WorldGrid.LongLatOf(base.Map.Tile)));
				return text + "\n" + "TimeLeft".Translate().CapitalizeFirst() + ": " + base.TicksLeft.ToStringTicksToPeriod(true, false, true);
			}
		}

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

		public override void End()
		{
			base.End();
			this.Impact();
		}

		private void Impact()
		{
			ScreenFader.SetColor(Color.clear);
			GenGameEnd.EndGameDialogMessage("GameOverPlanetkillerImpact".Translate(Find.World.info.name), false, GameCondition_Planetkiller.FadeColor);
		}
	}
}
