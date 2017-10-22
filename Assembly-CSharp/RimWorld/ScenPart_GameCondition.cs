using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ScenPart_GameCondition : ScenPart
	{
		private float durationDays;

		private string durationDaysBuf;

		public override string Label
		{
			get
			{
				return base.def.gameCondition.LabelCap;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.durationDays, "durationDayS", 0f, false);
		}

		public override string Summary(Scenario scen)
		{
			return base.def.gameCondition.LabelCap + ": " + base.def.gameCondition.description + " (" + ((int)(this.durationDays * 60000.0)).ToStringTicksToDays("F1") + ")";
		}

		public override void Randomize()
		{
			this.durationDays = Mathf.Round(base.def.durationRandomRange.RandomInRange);
		}

		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			Widgets.TextFieldNumericLabeled<float>(scenPartRect, "durationDays".Translate(), ref this.durationDays, ref this.durationDaysBuf, 0f, 1E+09f);
		}

		public override void GenerateIntoMap(Map map)
		{
			if (Find.GameInitData != null)
			{
				GameCondition cond = GameConditionMaker.MakeCondition(base.def.gameCondition, (int)(this.durationDays * 60000.0), 0);
				map.gameConditionManager.RegisterCondition(cond);
			}
		}

		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_GameCondition scenPart_GameCondition = other as ScenPart_GameCondition;
			if (scenPart_GameCondition != null && !scenPart_GameCondition.def.gameCondition.CanCoexistWith(base.def.gameCondition))
			{
				return false;
			}
			return true;
		}
	}
}
