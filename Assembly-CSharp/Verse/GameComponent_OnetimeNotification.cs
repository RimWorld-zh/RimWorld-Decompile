using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	public class GameComponent_OnetimeNotification : GameComponent
	{
		public bool sendAICoreRequestReminder = true;

		public GameComponent_OnetimeNotification(Game game)
		{
		}

		public override void GameComponentTick()
		{
			if (Find.TickManager.TicksGame % 2000 == 0 && Rand.Chance(0.05f))
			{
				if (this.sendAICoreRequestReminder)
				{
					if (ResearchProjectTagDefOf.ShipRelated.CompletedProjects() >= 2)
					{
						if (!PlayerItemAccessibilityUtility.PlayerOrItemStashHas(ThingDefOf.AIPersonaCore) && !PlayerItemAccessibilityUtility.PlayerOrItemStashHas(ThingDefOf.Ship_ComputerCore))
						{
							Faction faction = Find.FactionManager.RandomNonHostileFaction(false, false, true, TechLevel.Undefined);
							if (faction != null && faction.leader != null)
							{
								Find.LetterStack.ReceiveLetter("LetterLabelAICoreOffer".Translate(), "LetterAICoreOffer".Translate(new object[]
								{
									faction.leader.LabelDefinite(),
									faction.Name
								}).CapitalizeFirst(), LetterDefOf.NeutralEvent, GlobalTargetInfo.Invalid, faction, null);
								this.sendAICoreRequestReminder = false;
							}
						}
					}
				}
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.sendAICoreRequestReminder, "sendAICoreRequestReminder", false, false);
		}
	}
}
