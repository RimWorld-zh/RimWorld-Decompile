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
		// (get) Token: 0x060020A3 RID: 8355 RVA: 0x00117510 File Offset: 0x00115910
		public override string Label
		{
			get
			{
				return "Permanent".Translate().CapitalizeFirst() + ": " + this.gameCondition.label.CapitalizeFirst();
			}
		}

		// Token: 0x060020A4 RID: 8356 RVA: 0x00117550 File Offset: 0x00115950
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

		// Token: 0x060020A5 RID: 8357 RVA: 0x001175BA File Offset: 0x001159BA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<GameConditionDef>(ref this.gameCondition, "gameCondition");
		}

		// Token: 0x060020A6 RID: 8358 RVA: 0x001175D3 File Offset: 0x001159D3
		public override void Randomize()
		{
			this.gameCondition = this.AllowedGameConditions().RandomElement<GameConditionDef>();
		}

		// Token: 0x060020A7 RID: 8359 RVA: 0x001175E8 File Offset: 0x001159E8
		private IEnumerable<GameConditionDef> AllowedGameConditions()
		{
			return from d in DefDatabase<GameConditionDef>.AllDefs
			where d.canBePermanent
			select d;
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x00117624 File Offset: 0x00115A24
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PermaGameCondition", "ScenPart_PermaGameCondition".Translate());
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x00117650 File Offset: 0x00115A50
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PermaGameCondition")
			{
				yield return this.gameCondition.LabelCap + ": " + this.gameCondition.description.CapitalizeFirst();
			}
			yield break;
		}

		// Token: 0x060020AA RID: 8362 RVA: 0x00117684 File Offset: 0x00115A84
		public override void GenerateIntoMap(Map map)
		{
			GameCondition cond = GameConditionMaker.MakeConditionPermanent(this.gameCondition);
			map.gameConditionManager.RegisterCondition(cond);
		}

		// Token: 0x060020AB RID: 8363 RVA: 0x001176AC File Offset: 0x00115AAC
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
