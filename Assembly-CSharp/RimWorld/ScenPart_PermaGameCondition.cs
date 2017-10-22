using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ScenPart_PermaGameCondition : ScenPart
	{
		private GameConditionDef gameCondition;

		public const string PermaGameConditionTag = "PermaGameCondition";

		public override string Label
		{
			get
			{
				return "Permanent".Translate().CapitalizeFirst() + ": " + this.gameCondition.label;
			}
		}

		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			if (Widgets.ButtonText(scenPartRect, this.gameCondition.LabelCap, true, false, true))
			{
				FloatMenuUtility.MakeMenu(this.AllowedGameConditions(), (Func<GameConditionDef, string>)((GameConditionDef d) => d.LabelCap), (Func<GameConditionDef, Action>)((GameConditionDef d) => (Action)delegate()
				{
					this.gameCondition = d;
				}));
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<GameConditionDef>(ref this.gameCondition, "gameCondition");
		}

		public override void Randomize()
		{
			this.gameCondition = this.AllowedGameConditions().RandomElement();
		}

		private IEnumerable<GameConditionDef> AllowedGameConditions()
		{
			return from d in DefDatabase<GameConditionDef>.AllDefs
			where d.canBePermanent
			select d;
		}

		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PermaGameCondition", "ScenPart_PermaGameCondition".Translate());
		}

		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (!(tag == "PermaGameCondition"))
				yield break;
			yield return this.gameCondition.LabelCap + ": " + this.gameCondition.description;
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public override void GenerateIntoMap(Map map)
		{
			GameCondition cond = GameConditionMaker.MakeConditionPermanent(this.gameCondition);
			map.gameConditionManager.RegisterCondition(cond);
		}

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
				result = ((byte)((scenPart_PermaGameCondition == null || this.gameCondition.CanCoexistWith(scenPart_PermaGameCondition.gameCondition)) ? 1 : 0) != 0);
			}
			return result;
		}
	}
}
