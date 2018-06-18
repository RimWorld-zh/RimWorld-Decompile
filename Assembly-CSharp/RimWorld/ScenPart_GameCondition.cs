using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000632 RID: 1586
	public class ScenPart_GameCondition : ScenPart
	{
		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x0600209B RID: 8347 RVA: 0x00117394 File Offset: 0x00115794
		public override string Label
		{
			get
			{
				return this.def.gameCondition.LabelCap;
			}
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x001173B9 File Offset: 0x001157B9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.durationDays, "durationDayS", 0f, false);
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x001173D8 File Offset: 0x001157D8
		public override string Summary(Scenario scen)
		{
			return string.Concat(new string[]
			{
				this.def.gameCondition.LabelCap,
				": ",
				this.def.gameCondition.description,
				" (",
				((int)(this.durationDays * 60000f)).ToStringTicksToDays("F1"),
				")"
			});
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x00117450 File Offset: 0x00115850
		public override void Randomize()
		{
			this.durationDays = Mathf.Round(this.def.durationRandomRange.RandomInRange);
		}

		// Token: 0x0600209F RID: 8351 RVA: 0x00117470 File Offset: 0x00115870
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			Widgets.TextFieldNumericLabeled<float>(scenPartRect, "durationDays".Translate(), ref this.durationDays, ref this.durationDaysBuf, 0f, 1E+09f);
		}

		// Token: 0x060020A0 RID: 8352 RVA: 0x001174B1 File Offset: 0x001158B1
		public override void GenerateIntoMap(Map map)
		{
			if (!this.def.gameConditionTargetsWorld)
			{
				map.gameConditionManager.RegisterCondition(this.MakeCondition());
			}
		}

		// Token: 0x060020A1 RID: 8353 RVA: 0x001174D5 File Offset: 0x001158D5
		public override void PostWorldGenerate()
		{
			if (this.def.gameConditionTargetsWorld)
			{
				Find.World.gameConditionManager.RegisterCondition(this.MakeCondition());
			}
		}

		// Token: 0x060020A2 RID: 8354 RVA: 0x00117500 File Offset: 0x00115900
		private GameCondition MakeCondition()
		{
			return GameConditionMaker.MakeCondition(this.def.gameCondition, (int)(this.durationDays * 60000f), 0);
		}

		// Token: 0x060020A3 RID: 8355 RVA: 0x00117534 File Offset: 0x00115934
		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_GameCondition scenPart_GameCondition = other as ScenPart_GameCondition;
			return scenPart_GameCondition == null || scenPart_GameCondition.def.gameCondition.CanCoexistWith(this.def.gameCondition);
		}

		// Token: 0x040012BA RID: 4794
		private float durationDays;

		// Token: 0x040012BB RID: 4795
		private string durationDaysBuf;
	}
}
