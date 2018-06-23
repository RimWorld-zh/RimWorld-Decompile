using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200062E RID: 1582
	public class ScenPart_GameCondition : ScenPart
	{
		// Token: 0x040012B7 RID: 4791
		private float durationDays;

		// Token: 0x040012B8 RID: 4792
		private string durationDaysBuf;

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06002093 RID: 8339 RVA: 0x00117440 File Offset: 0x00115840
		public override string Label
		{
			get
			{
				return this.def.gameCondition.LabelCap;
			}
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x00117465 File Offset: 0x00115865
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.durationDays, "durationDayS", 0f, false);
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x00117484 File Offset: 0x00115884
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

		// Token: 0x06002096 RID: 8342 RVA: 0x001174FC File Offset: 0x001158FC
		public override void Randomize()
		{
			this.durationDays = Mathf.Round(this.def.durationRandomRange.RandomInRange);
		}

		// Token: 0x06002097 RID: 8343 RVA: 0x0011751C File Offset: 0x0011591C
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			Widgets.TextFieldNumericLabeled<float>(scenPartRect, "durationDays".Translate(), ref this.durationDays, ref this.durationDaysBuf, 0f, 1E+09f);
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x0011755D File Offset: 0x0011595D
		public override void GenerateIntoMap(Map map)
		{
			if (!this.def.gameConditionTargetsWorld)
			{
				map.gameConditionManager.RegisterCondition(this.MakeCondition());
			}
		}

		// Token: 0x06002099 RID: 8345 RVA: 0x00117581 File Offset: 0x00115981
		public override void PostWorldGenerate()
		{
			if (this.def.gameConditionTargetsWorld)
			{
				Find.World.gameConditionManager.RegisterCondition(this.MakeCondition());
			}
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x001175AC File Offset: 0x001159AC
		private GameCondition MakeCondition()
		{
			return GameConditionMaker.MakeCondition(this.def.gameCondition, (int)(this.durationDays * 60000f), 0);
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x001175E0 File Offset: 0x001159E0
		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_GameCondition scenPart_GameCondition = other as ScenPart_GameCondition;
			return scenPart_GameCondition == null || scenPart_GameCondition.def.gameCondition.CanCoexistWith(this.def.gameCondition);
		}
	}
}
