using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000633 RID: 1587
	public class ScenPart_ForcedTrait : ScenPart_PawnModifier
	{
		// Token: 0x040012C1 RID: 4801
		private TraitDef trait;

		// Token: 0x040012C2 RID: 4802
		private int degree = 0;

		// Token: 0x060020BD RID: 8381 RVA: 0x00118408 File Offset: 0x00116808
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<TraitDef>(ref this.trait, "trait");
			Scribe_Values.Look<int>(ref this.degree, "degree", 0, false);
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x00118434 File Offset: 0x00116834
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 3f);
			if (Widgets.ButtonText(scenPartRect.TopPart(0.333f), this.trait.DataAtDegree(this.degree).label.CapitalizeFirst(), true, false, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (TraitDef traitDef in from td in DefDatabase<TraitDef>.AllDefs
				orderby td.label
				select td)
				{
					foreach (TraitDegreeData localDeg2 in traitDef.degreeDatas)
					{
						TraitDef localDef = traitDef;
						TraitDegreeData localDeg = localDeg2;
						list.Add(new FloatMenuOption(localDeg.label.CapitalizeFirst(), delegate()
						{
							this.trait = localDef;
							this.degree = localDeg.degree;
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			base.DoPawnModifierEditInterface(scenPartRect.BottomPart(0.666f));
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x001185B8 File Offset: 0x001169B8
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PawnsHaveTrait".Translate(new object[]
			{
				this.context.ToStringHuman(),
				this.chance.ToStringPercent(),
				this.trait.DataAtDegree(this.degree).label.CapitalizeFirst()
			}).CapitalizeFirst();
		}

		// Token: 0x060020C0 RID: 8384 RVA: 0x0011861C File Offset: 0x00116A1C
		public override void Randomize()
		{
			base.Randomize();
			this.trait = DefDatabase<TraitDef>.GetRandom();
			this.degree = this.trait.degreeDatas.RandomElement<TraitDegreeData>().degree;
		}

		// Token: 0x060020C1 RID: 8385 RVA: 0x0011864C File Offset: 0x00116A4C
		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_ForcedTrait scenPart_ForcedTrait = other as ScenPart_ForcedTrait;
			return scenPart_ForcedTrait == null || this.trait != scenPart_ForcedTrait.trait || !this.context.OverlapsWith(scenPart_ForcedTrait.context);
		}

		// Token: 0x060020C2 RID: 8386 RVA: 0x001186A0 File Offset: 0x00116AA0
		protected override void ModifyPawnPostGenerate(Pawn pawn, bool redressed)
		{
			if (pawn.story != null && pawn.story.traits != null)
			{
				if (!pawn.story.traits.HasTrait(this.trait) || pawn.story.traits.DegreeOfTrait(this.trait) != this.degree)
				{
					if (pawn.story.traits.HasTrait(this.trait))
					{
						pawn.story.traits.allTraits.RemoveAll((Trait tr) => tr.def == this.trait);
					}
					else
					{
						IEnumerable<Trait> source = from tr in pawn.story.traits.allTraits
						where !tr.ScenForced && !ScenPart_ForcedTrait.PawnHasTraitForcedByBackstory(pawn, tr.def)
						select tr;
						if (source.Any<Trait>())
						{
							Trait trait = (from tr in source
							where tr.def.conflictingTraits.Contains(this.trait)
							select tr).FirstOrDefault<Trait>();
							if (trait != null)
							{
								pawn.story.traits.allTraits.Remove(trait);
							}
							else
							{
								pawn.story.traits.allTraits.Remove(source.RandomElement<Trait>());
							}
						}
					}
					pawn.story.traits.GainTrait(new Trait(this.trait, this.degree, true));
				}
			}
		}

		// Token: 0x060020C3 RID: 8387 RVA: 0x00118844 File Offset: 0x00116C44
		private static bool PawnHasTraitForcedByBackstory(Pawn pawn, TraitDef trait)
		{
			return (pawn.story.childhood != null && pawn.story.childhood.forcedTraits != null && pawn.story.childhood.forcedTraits.Any((TraitEntry te) => te.def == trait)) || (pawn.story.adulthood != null && pawn.story.adulthood.forcedTraits != null && pawn.story.adulthood.forcedTraits.Any((TraitEntry te) => te.def == trait));
		}
	}
}
