using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000BC5 RID: 3013
	public class GameComponent_OnetimeNotification : GameComponent
	{
		// Token: 0x04002CD5 RID: 11477
		public bool sendAICoreRequestReminder = true;

		// Token: 0x060041A7 RID: 16807 RVA: 0x0022A363 File Offset: 0x00228763
		public GameComponent_OnetimeNotification(Game game)
		{
		}

		// Token: 0x060041A8 RID: 16808 RVA: 0x0022A374 File Offset: 0x00228774
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

		// Token: 0x060041A9 RID: 16809 RVA: 0x0022A46E File Offset: 0x0022886E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.sendAICoreRequestReminder, "sendAICoreRequestReminder", false, false);
		}
	}
}
