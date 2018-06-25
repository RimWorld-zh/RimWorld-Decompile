using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000630 RID: 1584
	public class ScenPart_GameCondition : ScenPart
	{
		// Token: 0x040012BB RID: 4795
		private float durationDays;

		// Token: 0x040012BC RID: 4796
		private string durationDaysBuf;

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06002096 RID: 8342 RVA: 0x001177F8 File Offset: 0x00115BF8
		public override string Label
		{
			get
			{
				return this.def.gameCondition.LabelCap;
			}
		}

		// Token: 0x06002097 RID: 8343 RVA: 0x0011781D File Offset: 0x00115C1D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.durationDays, "durationDayS", 0f, false);
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x0011783C File Offset: 0x00115C3C
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

		// Token: 0x06002099 RID: 8345 RVA: 0x001178B4 File Offset: 0x00115CB4
		public override void Randomize()
		{
			this.durationDays = Mathf.Round(this.def.durationRandomRange.RandomInRange);
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x001178D4 File Offset: 0x00115CD4
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			Widgets.TextFieldNumericLabeled<float>(scenPartRect, "durationDays".Translate(), ref this.durationDays, ref this.durationDaysBuf, 0f, 1E+09f);
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x00117915 File Offset: 0x00115D15
		public override void GenerateIntoMap(Map map)
		{
			if (!this.def.gameConditionTargetsWorld)
			{
				map.gameConditionManager.RegisterCondition(this.MakeCondition());
			}
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x00117939 File Offset: 0x00115D39
		public override void PostWorldGenerate()
		{
			if (this.def.gameConditionTargetsWorld)
			{
				Find.World.gameConditionManager.RegisterCondition(this.MakeCondition());
			}
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x00117964 File Offset: 0x00115D64
		private GameCondition MakeCondition()
		{
			return GameConditionMaker.MakeCondition(this.def.gameCondition, (int)(this.durationDays * 60000f), 0);
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x00117998 File Offset: 0x00115D98
		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_GameCondition scenPart_GameCondition = other as ScenPart_GameCondition;
			return scenPart_GameCondition == null || scenPart_GameCondition.def.gameCondition.CanCoexistWith(this.def.gameCondition);
		}
	}
}
