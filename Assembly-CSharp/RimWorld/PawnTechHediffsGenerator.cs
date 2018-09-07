using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public static class PawnTechHediffsGenerator
	{
		private static List<Thing> emptyIngredientsList = new List<Thing>();

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache0;

		public static void GenerateTechHediffsFor(Pawn pawn)
		{
			if (pawn.kindDef.techHediffsTags == null)
			{
				return;
			}
			if (Rand.Value > pawn.kindDef.techHediffsChance)
			{
				return;
			}
			float partsMoney = pawn.kindDef.techHediffsMoney.RandomInRange;
			IEnumerable<ThingDef> source = from x in DefDatabase<ThingDef>.AllDefs
			where x.isTechHediff && x.BaseMarketValue <= partsMoney && x.techHediffsTags != null && pawn.kindDef.techHediffsTags.Any((string tag) => x.techHediffsTags.Contains(tag))
			select x;
			if (source.Any<ThingDef>())
			{
				ThingDef partDef = source.RandomElementByWeight((ThingDef w) => w.BaseMarketValue);
				IEnumerable<RecipeDef> source2 = from x in DefDatabase<RecipeDef>.AllDefs
				where x.IsIngredient(partDef) && pawn.def.AllRecipes.Contains(x)
				select x;
				if (source2.Any<RecipeDef>())
				{
					RecipeDef recipeDef = source2.RandomElement<RecipeDef>();
					if (recipeDef.Worker.GetPartsToApplyOn(pawn, recipeDef).Any<BodyPartRecord>())
					{
						recipeDef.Worker.ApplyOnPawn(pawn, recipeDef.Worker.GetPartsToApplyOn(pawn, recipeDef).RandomElement<BodyPartRecord>(), null, PawnTechHediffsGenerator.emptyIngredientsList, null);
					}
				}
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static PawnTechHediffsGenerator()
		{
		}

		[CompilerGenerated]
		private static float <GenerateTechHediffsFor>m__0(ThingDef w)
		{
			return w.BaseMarketValue;
		}

		[CompilerGenerated]
		private sealed class <GenerateTechHediffsFor>c__AnonStorey0
		{
			internal float partsMoney;

			internal Pawn pawn;

			public <GenerateTechHediffsFor>c__AnonStorey0()
			{
			}

			internal bool <>m__0(ThingDef x)
			{
				return x.isTechHediff && x.BaseMarketValue <= this.partsMoney && x.techHediffsTags != null && this.pawn.kindDef.techHediffsTags.Any((string tag) => x.techHediffsTags.Contains(tag));
			}

			private sealed class <GenerateTechHediffsFor>c__AnonStorey1
			{
				internal ThingDef x;

				internal PawnTechHediffsGenerator.<GenerateTechHediffsFor>c__AnonStorey0 <>f__ref$0;

				public <GenerateTechHediffsFor>c__AnonStorey1()
				{
				}

				internal bool <>m__0(string tag)
				{
					return this.x.techHediffsTags.Contains(tag);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <GenerateTechHediffsFor>c__AnonStorey2
		{
			internal ThingDef partDef;

			internal PawnTechHediffsGenerator.<GenerateTechHediffsFor>c__AnonStorey0 <>f__ref$0;

			public <GenerateTechHediffsFor>c__AnonStorey2()
			{
			}

			internal bool <>m__0(RecipeDef x)
			{
				return x.IsIngredient(this.partDef) && this.<>f__ref$0.pawn.def.AllRecipes.Contains(x);
			}
		}
	}
}
