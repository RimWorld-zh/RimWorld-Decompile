using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000633 RID: 1587
	public class ScenPart_PermaGameCondition : ScenPart
	{
		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x060020A5 RID: 8357 RVA: 0x00117588 File Offset: 0x00115988
		public override string Label
		{
			get
			{
				return "Permanent".Translate().CapitalizeFirst() + ": " + this.gameCondition.label.CapitalizeFirst();
			}
		}

		// Token: 0x060020A6 RID: 8358 RVA: 0x001175C8 File Offset: 0x001159C8
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

		// Token: 0x060020A7 RID: 8359 RVA: 0x00117632 File Offset: 0x00115A32
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<GameConditionDef>(ref this.gameCondition, "gameCondition");
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x0011764B File Offset: 0x00115A4B
		public override void Randomize()
		{
			this.gameCondition = this.AllowedGameConditions().RandomElement<GameConditionDef>();
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x00117660 File Offset: 0x00115A60
		private IEnumerable<GameConditionDef> AllowedGameConditions()
		{
			return from d in DefDatabase<GameConditionDef>.AllDefs
			where d.canBePermanent
			select d;
		}

		// Token: 0x060020AA RID: 8362 RVA: 0x0011769C File Offset: 0x00115A9C
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PermaGameCondition", "ScenPart_PermaGameCondition".Translate());
		}

		// Token: 0x060020AB RID: 8363 RVA: 0x001176C8 File Offset: 0x00115AC8
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PermaGameCondition")
			{
				yield return this.gameCondition.LabelCap + ": " + this.gameCondition.description.CapitalizeFirst();
			}
			yield break;
		}

		// Token: 0x060020AC RID: 8364 RVA: 0x001176FC File Offset: 0x00115AFC
		public override void GenerateIntoMap(Map map)
		{
			GameCondition cond = GameConditionMaker.MakeConditionPermanent(this.gameCondition);
			map.gameConditionManager.RegisterCondition(cond);
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x00117724 File Offset: 0x00115B24
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

		// Token: 0x040012BC RID: 4796
		private GameConditionDef gameCondition;

		// Token: 0x040012BD RID: 4797
		public const string PermaGameConditionTag = "PermaGameCondition";
	}
}
