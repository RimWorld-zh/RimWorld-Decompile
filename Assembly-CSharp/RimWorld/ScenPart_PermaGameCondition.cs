using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000631 RID: 1585
	public class ScenPart_PermaGameCondition : ScenPart
	{
		// Token: 0x040012BD RID: 4797
		private GameConditionDef gameCondition;

		// Token: 0x040012BE RID: 4798
		public const string PermaGameConditionTag = "PermaGameCondition";

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x060020A0 RID: 8352 RVA: 0x001179EC File Offset: 0x00115DEC
		public override string Label
		{
			get
			{
				return "Permanent".Translate().CapitalizeFirst() + ": " + this.gameCondition.label.CapitalizeFirst();
			}
		}

		// Token: 0x060020A1 RID: 8353 RVA: 0x00117A2C File Offset: 0x00115E2C
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

		// Token: 0x060020A2 RID: 8354 RVA: 0x00117A96 File Offset: 0x00115E96
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<GameConditionDef>(ref this.gameCondition, "gameCondition");
		}

		// Token: 0x060020A3 RID: 8355 RVA: 0x00117AAF File Offset: 0x00115EAF
		public override void Randomize()
		{
			this.gameCondition = this.AllowedGameConditions().RandomElement<GameConditionDef>();
		}

		// Token: 0x060020A4 RID: 8356 RVA: 0x00117AC4 File Offset: 0x00115EC4
		private IEnumerable<GameConditionDef> AllowedGameConditions()
		{
			return from d in DefDatabase<GameConditionDef>.AllDefs
			where d.canBePermanent
			select d;
		}

		// Token: 0x060020A5 RID: 8357 RVA: 0x00117B00 File Offset: 0x00115F00
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PermaGameCondition", "ScenPart_PermaGameCondition".Translate());
		}

		// Token: 0x060020A6 RID: 8358 RVA: 0x00117B2C File Offset: 0x00115F2C
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PermaGameCondition")
			{
				yield return this.gameCondition.LabelCap + ": " + this.gameCondition.description.CapitalizeFirst();
			}
			yield break;
		}

		// Token: 0x060020A7 RID: 8359 RVA: 0x00117B60 File Offset: 0x00115F60
		public override void GenerateIntoMap(Map map)
		{
			GameCondition cond = GameConditionMaker.MakeConditionPermanent(this.gameCondition);
			map.gameConditionManager.RegisterCondition(cond);
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x00117B88 File Offset: 0x00115F88
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
	}
}
