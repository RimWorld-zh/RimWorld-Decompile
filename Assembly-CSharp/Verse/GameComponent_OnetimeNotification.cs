using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000BC9 RID: 3017
	public class GameComponent_OnetimeNotification : GameComponent
	{
		// Token: 0x060041A5 RID: 16805 RVA: 0x00229C8F File Offset: 0x0022808F
		public GameComponent_OnetimeNotification(Game game)
		{
		}

		// Token: 0x060041A6 RID: 16806 RVA: 0x00229CA0 File Offset: 0x002280A0
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

		// Token: 0x060041A7 RID: 16807 RVA: 0x00229D9A File Offset: 0x0022819A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.sendAICoreRequestReminder, "sendAICoreRequestReminder", false, false);
		}

		// Token: 0x04002CD0 RID: 11472
		public bool sendAICoreRequestReminder = true;
	}
}
