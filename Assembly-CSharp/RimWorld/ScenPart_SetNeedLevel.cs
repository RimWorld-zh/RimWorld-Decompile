using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000639 RID: 1593
	public class ScenPart_SetNeedLevel : ScenPart_PawnModifier
	{
		// Token: 0x040012D4 RID: 4820
		private NeedDef need;

		// Token: 0x040012D5 RID: 4821
		private FloatRange levelRange;

		// Token: 0x060020E0 RID: 8416 RVA: 0x00119184 File Offset: 0x00117584
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 3f + 31f);
			if (Widgets.ButtonText(scenPartRect.TopPartPixels(ScenPart.RowHeight), this.need.LabelCap, true, false, true))
			{
				FloatMenuUtility.MakeMenu<NeedDef>(this.PossibleNeeds(), (NeedDef hd) => hd.LabelCap, (NeedDef n) => delegate()
				{
					this.need = n;
				});
			}
			Widgets.FloatRange(new Rect(scenPartRect.x, scenPartRect.y + ScenPart.RowHeight, scenPartRect.width, 31f), listing.CurHeight.GetHashCode(), ref this.levelRange, 0f, 1f, "ConfigurableLevel", ToStringStyle.FloatTwo);
			base.DoPawnModifierEditInterface(scenPartRect.BottomPartPixels(ScenPart.RowHeight * 2f));
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x00119270 File Offset: 0x00117670
		private IEnumerable<NeedDef> PossibleNeeds()
		{
			return from x in DefDatabase<NeedDef>.AllDefsListForReading
			where x.major
			select x;
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x001192AC File Offset: 0x001176AC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<NeedDef>(ref this.need, "need");
			Scribe_Values.Look<FloatRange>(ref this.levelRange, "levelRange", default(FloatRange), false);
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x001192EC File Offset: 0x001176EC
		public override string Summary(Scenario scen)
		{
			return "ScenPart_SetNeed".Translate(new object[]
			{
				this.context.ToStringHuman(),
				this.chance.ToStringPercent(),
				this.need.label,
				this.levelRange.min.ToStringPercent(),
				this.levelRange.max.ToStringPercent()
			}).CapitalizeFirst();
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x00119368 File Offset: 0x00117768
		public override void Randomize()
		{
			base.Randomize();
			this.need = this.PossibleNeeds().RandomElement<NeedDef>();
			this.levelRange.max = Rand.Range(0f, 1f);
			this.levelRange.min = this.levelRange.max * Rand.Range(0f, 0.95f);
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x001193D0 File Offset: 0x001177D0
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_SetNeedLevel scenPart_SetNeedLevel = other as ScenPart_SetNeedLevel;
			bool result;
			if (scenPart_SetNeedLevel != null && this.need == scenPart_SetNeedLevel.need)
			{
				this.chance = GenMath.ChanceEitherHappens(this.chance, scenPart_SetNeedLevel.chance);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x00119424 File Offset: 0x00117824
		protected override void ModifyPawnPostGenerate(Pawn p, bool redressed)
		{
			if (p.needs != null)
			{
				Need need = p.needs.TryGetNeed(this.need);
				if (need != null)
				{
					need.ForceSetLevel(this.levelRange.RandomInRange);
				}
			}
		}
	}
}
