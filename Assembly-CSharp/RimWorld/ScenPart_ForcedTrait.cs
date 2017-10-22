using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ScenPart_ForcedTrait : ScenPart_PawnModifier
	{
		private TraitDef trait;

		private int degree;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<TraitDef>(ref this.trait, "trait");
			Scribe_Values.Look<int>(ref this.degree, "degree", 0, false);
		}

		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, (float)(ScenPart.RowHeight * 3.0));
			if (Widgets.ButtonText(scenPartRect.TopPart(0.333f), this.trait.DataAtDegree(this.degree).label.CapitalizeFirst(), true, false, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (TraitDef item in from td in DefDatabase<TraitDef>.AllDefs
				orderby td.label
				select td)
				{
					List<TraitDegreeData>.Enumerator enumerator2 = item.degreeDatas.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							TraitDegreeData current2 = enumerator2.Current;
							TraitDef localDef = item;
							TraitDegreeData localDeg = current2;
							list.Add(new FloatMenuOption(localDeg.label.CapitalizeFirst(), (Action)delegate
							{
								this.trait = localDef;
								this.degree = localDeg.degree;
							}, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
					}
					finally
					{
						((IDisposable)(object)enumerator2).Dispose();
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			base.DoPawnModifierEditInterface(scenPartRect.BottomPart(0.666f));
		}

		public override string Summary(Scenario scen)
		{
			return "ScenPart_PawnsHaveTrait".Translate(base.context.ToStringHuman(), base.chance.ToStringPercent(), this.trait.DataAtDegree(this.degree).label.CapitalizeFirst()).CapitalizeFirst();
		}

		public override void Randomize()
		{
			base.Randomize();
			this.trait = DefDatabase<TraitDef>.GetRandom();
			this.degree = this.trait.degreeDatas.RandomElement().degree;
		}

		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_ForcedTrait scenPart_ForcedTrait = other as ScenPart_ForcedTrait;
			if (scenPart_ForcedTrait != null && this.trait == scenPart_ForcedTrait.trait && base.context.OverlapsWith(scenPart_ForcedTrait.context))
			{
				return false;
			}
			return true;
		}

		protected override void ModifyPawn(Pawn pawn)
		{
			if (!(Rand.Value > base.chance) && pawn.story != null && pawn.story.traits != null && (!pawn.story.traits.HasTrait(this.trait) || pawn.story.traits.DegreeOfTrait(this.trait) != this.degree))
			{
				if (pawn.story.traits.HasTrait(this.trait))
				{
					pawn.story.traits.allTraits.RemoveAll((Predicate<Trait>)((Trait tr) => tr.def == this.trait));
				}
				else
				{
					IEnumerable<Trait> source = from tr in pawn.story.traits.allTraits
					where !tr.ScenForced && !ScenPart_ForcedTrait.PawnHasTraitForcedByBackstory(pawn, tr.def)
					select tr;
					if (source.Any())
					{
						Trait trait = (from tr in source
						where tr.def.conflictingTraits.Contains(this.trait)
						select tr).FirstOrDefault();
						if (trait != null)
						{
							pawn.story.traits.allTraits.Remove(trait);
						}
						else
						{
							pawn.story.traits.allTraits.Remove(source.RandomElement());
						}
					}
				}
				pawn.story.traits.GainTrait(new Trait(this.trait, this.degree, true));
			}
		}

		private static bool PawnHasTraitForcedByBackstory(Pawn pawn, TraitDef trait)
		{
			if (pawn.story.childhood != null && pawn.story.childhood.forcedTraits != null && pawn.story.childhood.forcedTraits.Any((Predicate<TraitEntry>)((TraitEntry te) => te.def == trait)))
			{
				return true;
			}
			if (pawn.story.adulthood != null && pawn.story.adulthood.forcedTraits != null && pawn.story.adulthood.forcedTraits.Any((Predicate<TraitEntry>)((TraitEntry te) => te.def == trait)))
			{
				return true;
			}
			return false;
		}
	}
}
