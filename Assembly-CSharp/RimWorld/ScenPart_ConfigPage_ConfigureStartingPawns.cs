using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200064D RID: 1613
	public class ScenPart_ConfigPage_ConfigureStartingPawns : ScenPart_ConfigPage
	{
		// Token: 0x06002179 RID: 8569 RVA: 0x0011BC50 File Offset: 0x0011A050
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			base.DoEditInterface(listing);
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			scenPartRect.height = ScenPart.RowHeight;
			Text.Anchor = TextAnchor.UpperRight;
			Rect rect = new Rect(scenPartRect.x - 200f, scenPartRect.y + ScenPart.RowHeight, 200f, ScenPart.RowHeight);
			rect.xMax -= 4f;
			Widgets.Label(rect, "ScenPart_StartWithPawns_OutOf".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.TextFieldNumeric<int>(scenPartRect, ref this.pawnCount, ref this.pawnCountBuffer, 1f, 10f);
			scenPartRect.y += ScenPart.RowHeight;
			Widgets.TextFieldNumeric<int>(scenPartRect, ref this.pawnChoiceCount, ref this.pawnCountChoiceBuffer, (float)this.pawnCount, 10f);
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x0011BD2D File Offset: 0x0011A12D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.pawnCount, "pawnCount", 0, false);
			Scribe_Values.Look<int>(ref this.pawnChoiceCount, "pawnChoiceCount", 0, false);
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x0011BD5C File Offset: 0x0011A15C
		public override string Summary(Scenario scen)
		{
			return "ScenPart_StartWithPawns".Translate(new object[]
			{
				this.pawnCount,
				this.pawnChoiceCount
			});
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x0011BD9D File Offset: 0x0011A19D
		public override void Randomize()
		{
			this.pawnCount = Rand.RangeInclusive(1, 6);
			this.pawnChoiceCount = 10;
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x0011BDB8 File Offset: 0x0011A1B8
		public override void PostWorldGenerate()
		{
			Find.GameInitData.startingPawnCount = this.pawnCount;
			int num = 0;
			do
			{
				StartingPawnUtility.ClearAllStartingPawns();
				for (int i = 0; i < this.pawnCount; i++)
				{
					Find.GameInitData.startingAndOptionalPawns.Add(StartingPawnUtility.NewGeneratedStartingPawn());
				}
				num++;
				if (num > 20)
				{
					break;
				}
			}
			while (!StartingPawnUtility.WorkTypeRequirementsSatisfied());
			while (Find.GameInitData.startingAndOptionalPawns.Count < this.pawnChoiceCount)
			{
				Find.GameInitData.startingAndOptionalPawns.Add(StartingPawnUtility.NewGeneratedStartingPawn());
			}
		}

		// Token: 0x04001308 RID: 4872
		public int pawnCount = 3;

		// Token: 0x04001309 RID: 4873
		public int pawnChoiceCount = 10;

		// Token: 0x0400130A RID: 4874
		private string pawnCountBuffer;

		// Token: 0x0400130B RID: 4875
		private string pawnCountChoiceBuffer;

		// Token: 0x0400130C RID: 4876
		private const int MaxPawnCount = 10;

		// Token: 0x0400130D RID: 4877
		private const int MaxPawnChoiceCount = 10;
	}
}
