using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200062F RID: 1583
	public class ScenPart_PermaGameCondition : ScenPart
	{
		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x0600209D RID: 8349 RVA: 0x00117634 File Offset: 0x00115A34
		public override string Label
		{
			get
			{
				return "Permanent".Translate().CapitalizeFirst() + ": " + this.gameCondition.label.CapitalizeFirst();
			}
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x00117674 File Offset: 0x00115A74
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			if (Widgets.ButtonText(scenPartRect, this.gameCondition.LabelCap, true, false, true))
			{
				FloatMenuUtility.MakeMenu<GameConditionDef>(this.AllowedGameConditions(), (GameConditionDef d) => d.LabelCap, (GameConditionDef d) => delegate()
				{
					this.gameCondition = d;
				});
			}
		}

		// Token: 0x0600209F RID: 8351 RVA: 0x001176DE File Offset: 0x00115ADE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<GameConditionDef>(ref this.gameCondition, "gameCondition");
		}

		// Token: 0x060020A0 RID: 8352 RVA: 0x001176F7 File Offset: 0x00115AF7
		public override void Randomize()
		{
			this.gameCondition = this.AllowedGameConditions().RandomElement<GameConditionDef>();
		}

		// Token: 0x060020A1 RID: 8353 RVA: 0x0011770C File Offset: 0x00115B0C
		private IEnumerable<GameConditionDef> AllowedGameConditions()
		{
			return from d in DefDatabase<GameConditionDef>.AllDefs
			where d.canBePermanent
			select d;
		}

		// Token: 0x060020A2 RID: 8354 RVA: 0x00117748 File Offset: 0x00115B48
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PermaGameCondition", "ScenPart_PermaGameCondition".Translate());
		}

		// Token: 0x060020A3 RID: 8355 RVA: 0x00117774 File Offset: 0x00115B74
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PermaGameCondition")
			{
				yield return this.gameCondition.LabelCap + ": " + this.gameCondition.description.CapitalizeFirst();
			}
			yield break;
		}

		// Token: 0x060020A4 RID: 8356 RVA: 0x001177A8 File Offset: 0x00115BA8
		public override void GenerateIntoMap(Map map)
		{
			GameCondition cond = GameConditionMaker.MakeConditionPermanent(this.gameCondition);
			map.gameConditionManager.RegisterCondition(cond);
		}

		// Token: 0x060020A5 RID: 8357 RVA: 0x001177D0 File Offset: 0x00115BD0
		public override bool CanCoexistWith(ScenPart other)
		{
			bool result;
			if (this.gameCondition == null)
			{
				result = true;
			}
			else
			{
				ScenPart_PermaGameCondition scenPart_PermaGameCondition = other as ScenPart_PermaGameCondition;
				if (scenPart_PermaGameCondition != null)
				{
					if (!this.gameCondition.CanCoexistWith(scenPart_PermaGameCondition.gameCondition))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x040012B9 RID: 4793
		private GameConditionDef gameCondition;

		// Token: 0x040012BA RID: 4794
		public const string PermaGameConditionTag = "PermaGameCondition";
	}
}
