using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000641 RID: 1601
	public class ScenPart_PlayerFaction : ScenPart
	{
		// Token: 0x0600213A RID: 8506 RVA: 0x0011A5CA File Offset: 0x001189CA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<FactionDef>(ref this.factionDef, "factionDef");
		}

		// Token: 0x0600213B RID: 8507 RVA: 0x0011A5E4 File Offset: 0x001189E4
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

		// Token: 0x0600213C RID: 8508 RVA: 0x0011A6E0 File Offset: 0x00118AE0
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PlayerFaction".Translate(new object[]
			{
				this.factionDef.label
			});
		}

		// Token: 0x0600213D RID: 8509 RVA: 0x0011A713 File Offset: 0x00118B13
		public override void Randomize()
		{
			this.factionDef = (from fd in DefDatabase<FactionDef>.AllDefs
			where fd.isPlayer
			select fd).RandomElement<FactionDef>();
		}

		// Token: 0x0600213E RID: 8510 RVA: 0x0011A748 File Offset: 0x00118B48
		public override void PostWorldGenerate()
		{
			Find.GameInitData.playerFaction = FactionGenerator.NewGeneratedFaction(this.factionDef);
			Find.FactionManager.Add(Find.GameInitData.playerFaction);
			FactionGenerator.EnsureRequiredEnemies(Find.GameInitData.playerFaction);
		}

		// Token: 0x0600213F RID: 8511 RVA: 0x0011A784 File Offset: 0x00118B84
		public override void PreMapGenerate()
		{
			FactionBase factionBase = (FactionBase)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.FactionBase);
			factionBase.SetFaction(Find.GameInitData.playerFaction);
			factionBase.Tile = Find.GameInitData.startingTile;
			factionBase.Name = FactionBaseNameGenerator.GenerateFactionBaseName(factionBase, Find.GameInitData.playerFaction.def.playerInitialSettlementNameMaker);
			Find.WorldObjects.Add(factionBase);
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x0011A7ED File Offset: 0x00118BED
		public override void PostGameStart()
		{
			Find.GameInitData.playerFaction = null;
		}

		// Token: 0x06002141 RID: 8513 RVA: 0x0011A7FC File Offset: 0x00118BFC
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.factionDef == null)
			{
				yield return "factionDef is null";
			}
			yield break;
		}

		// Token: 0x040012EE RID: 4846
		internal FactionDef factionDef;
	}
}
