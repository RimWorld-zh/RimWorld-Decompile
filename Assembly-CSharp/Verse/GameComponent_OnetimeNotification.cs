using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000BC7 RID: 3015
	public class GameComponent_OnetimeNotification : GameComponent
	{
		// Token: 0x04002CD5 RID: 11477
		public bool sendAICoreRequestReminder = true;

		// Token: 0x060041AA RID: 16810 RVA: 0x0022A43F File Offset: 0x0022883F
		public GameComponent_OnetimeNotification(Game game)
		{
		}

		// Token: 0x060041AB RID: 16811 RVA: 0x0022A450 File Offset: 0x00228850
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

		// Token: 0x060041AC RID: 16812 RVA: 0x0022A54A File Offset: 0x0022894A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.sendAICoreRequestReminder, "sendAICoreRequestReminder", false, false);
		}
	}
}
