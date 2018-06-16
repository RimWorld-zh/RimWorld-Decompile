using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000632 RID: 1586
	public class ScenPart_GameCondition : ScenPart
	{
		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06002099 RID: 8345 RVA: 0x0011731C File Offset: 0x0011571C
		public override string Label
		{
			get
			{
				return this.def.gameCondition.LabelCap;
			}
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x00117341 File Offset: 0x00115741
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.durationDays, "durationDayS", 0f, false);
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x00117360 File Offset: 0x00115760
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

		// Token: 0x0600209C RID: 8348 RVA: 0x001173D8 File Offset: 0x001157D8
		public override void Randomize()
		{
			this.durationDays = Mathf.Round(this.def.durationRandomRange.RandomInRange);
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x001173F8 File Offset: 0x001157F8
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			Widgets.TextFieldNumericLabeled<float>(scenPartRect, "durationDays".Translate(), ref this.durationDays, ref this.durationDaysBuf, 0f, 1E+09f);
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x00117439 File Offset: 0x00115839
		public override void GenerateIntoMap(Map map)
		{
			if (!this.def.gameConditionTargetsWorld)
			{
				map.gameConditionManager.RegisterCondition(this.MakeCondition());
			}
		}

		// Token: 0x0600209F RID: 8351 RVA: 0x0011745D File Offset: 0x0011585D
		public override void PostWorldGenerate()
		{
			if (this.def.gameConditionTargetsWorld)
			{
				Find.World.gameConditionManager.RegisterCondition(this.MakeCondition());
			}
		}

		// Token: 0x060020A0 RID: 8352 RVA: 0x00117488 File Offset: 0x00115888
		private GameCondition MakeCondition()
		{
			return GameConditionMaker.MakeCondition(this.def.gameCondition, (int)(this.durationDays * 60000f), 0);
		}

		// Token: 0x060020A1 RID: 8353 RVA: 0x001174BC File Offset: 0x001158BC
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
