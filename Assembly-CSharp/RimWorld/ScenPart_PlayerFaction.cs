using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000643 RID: 1603
	public class ScenPart_PlayerFaction : ScenPart
	{
		// Token: 0x040012F2 RID: 4850
		internal FactionDef factionDef;

		// Token: 0x0600213D RID: 8509 RVA: 0x0011A982 File Offset: 0x00118D82
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<FactionDef>(ref this.factionDef, "factionDef");
		}

		// Token: 0x0600213E RID: 8510 RVA: 0x0011A99C File Offset: 0x00118D9C
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			if (Widgets.ButtonText(scenPartRect, this.factionDef.LabelCap, true, false, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (FactionDef localFd2 in from d in DefDatabase<FactionDef>.AllDefs
				where d.isPlayer
				select d)
				{
					FactionDef localFd = localFd2;
					list.Add(new FloatMenuOption(localFd.LabelCap, delegate()
					{
						this.factionDef = localFd;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x0600213F RID: 8511 RVA: 0x0011AA98 File Offset: 0x00118E98
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PlayerFaction".Translate(new object[]
			{
				this.factionDef.label
			});
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x0011AACB File Offset: 0x00118ECB
		public override void Randomize()
		{
			this.factionDef = (from fd in DefDatabase<FactionDef>.AllDefs
			where fd.isPlayer
			select fd).RandomElement<FactionDef>();
		}

		// Token: 0x06002141 RID: 8513 RVA: 0x0011AB00 File Offset: 0x00118F00
		public override void PostWorldGenerate()
		{
			Find.GameInitData.playerFaction = FactionGenerator.NewGeneratedFaction(this.factionDef);
			Find.FactionManager.Add(Find.GameInitData.playerFaction);
			FactionGenerator.EnsureRequiredEnemies(Find.GameInitData.playerFaction);
		}

		// Token: 0x06002142 RID: 8514 RVA: 0x0011AB3C File Offset: 0x00118F3C
		public override void PreMapGenerate()
		{
			FactionBase factionBase = (FactionBase)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.FactionBase);
			factionBase.SetFaction(Find.GameInitData.playerFaction);
			factionBase.Tile = Find.GameInitData.startingTile;
			factionBase.Name = FactionBaseNameGenerator.GenerateFactionBaseName(factionBase, Find.GameInitData.playerFaction.def.playerInitialSettlementNameMaker);
			Find.WorldObjects.Add(factionBase);
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x0011ABA5 File Offset: 0x00118FA5
		public override void PostGameStart()
		{
			Find.GameInitData.playerFaction = null;
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x0011ABB4 File Offset: 0x00118FB4
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.factionDef == null)
			{
				yield return "factionDef is null";
			}
			yield break;
		}
	}
}
