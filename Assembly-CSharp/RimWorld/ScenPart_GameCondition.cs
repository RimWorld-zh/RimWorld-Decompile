using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000630 RID: 1584
	public class ScenPart_GameCondition : ScenPart
	{
		// Token: 0x040012B7 RID: 4791
		private float durationDays;

		// Token: 0x040012B8 RID: 4792
		private string durationDaysBuf;

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06002097 RID: 8343 RVA: 0x00117590 File Offset: 0x00115990
		public override string Label
		{
			get
			{
				return this.def.gameCondition.LabelCap;
			}
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x001175B5 File Offset: 0x001159B5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.durationDays, "durationDayS", 0f, false);
		}

		// Token: 0x06002099 RID: 8345 RVA: 0x001175D4 File Offset: 0x001159D4
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

		// Token: 0x0600209A RID: 8346 RVA: 0x0011764C File Offset: 0x00115A4C
		public override void Randomize()
		{
			this.durationDays = Mathf.Round(this.def.durationRandomRange.RandomInRange);
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x0011766C File Offset: 0x00115A6C
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			Widgets.TextFieldNumericLabeled<float>(scenPartRect, "durationDays".Translate(), ref this.durationDays, ref this.durationDaysBuf, 0f, 1E+09f);
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x001176AD File Offset: 0x00115AAD
		public override void GenerateIntoMap(Map map)
		{
			if (!this.def.gameConditionTargetsWorld)
			{
				map.gameConditionManager.RegisterCondition(this.MakeCondition());
			}
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x001176D1 File Offset: 0x00115AD1
		public override void PostWorldGenerate()
		{
			if (this.def.gameConditionTargetsWorld)
			{
				Find.World.gameConditionManager.RegisterCondition(this.MakeCondition());
			}
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x001176FC File Offset: 0x00115AFC
		private GameCondition MakeCondition()
		{
			return GameConditionMaker.MakeCondition(this.def.gameCondition, (int)(this.durationDays * 60000f), 0);
		}

		// Token: 0x0600209F RID: 8351 RVA: 0x00117730 File Offset: 0x00115B30
		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_GameCondition scenPart_GameCondition = other as ScenPart_GameCondition;
			return scenPart_GameCondition == null || scenPart_GameCondition.def.gameCondition.CanCoexistWith(this.def.gameCondition);
		}
	}
}
