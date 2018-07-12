using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	[HasDebugOutput]
	public static class DebugOutputsMisc
	{
		[CompilerGenerated]
		private static Func<ThingDef, ThingDef> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<ThingDef, int> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<ThingDef, object> <>f__am$cache5;

		[CompilerGenerated]
		private static Func<ThingDef, int> <>f__am$cache6;

		[CompilerGenerated]
		private static Func<ThingDef, int> <>f__am$cache7;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache8;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache9;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cacheA;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cacheB;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cacheC;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cacheD;

		[CompilerGenerated]
		private static Func<BuildableDef, bool> <>f__am$cacheE;

		[CompilerGenerated]
		private static Func<BuildableDef, int> <>f__am$cacheF;

		[CompilerGenerated]
		private static Func<BuildableDef, string> <>f__am$cache10;

		[CompilerGenerated]
		private static Func<BuildableDef, string> <>f__am$cache11;

		[CompilerGenerated]
		private static Func<BuildableDef, string> <>f__am$cache12;

		[CompilerGenerated]
		private static Func<BuildableDef, string> <>f__am$cache13;

		[CompilerGenerated]
		private static Func<BuildableDef, string> <>f__am$cache14;

		[CompilerGenerated]
		private static Func<BuildableDef, string> <>f__am$cache15;

		[CompilerGenerated]
		private static Func<ThingDef, CompProperties_HeatPusher> <>f__am$cache16;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache17;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache18;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache19;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache1A;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache1B;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache1C;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache1D;

		[CompilerGenerated]
		private static Func<ThingDef, object> <>f__am$cache1E;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache1F;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache20;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache21;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache22;

		[CompilerGenerated]
		private static Func<ThingDef, int> <>f__am$cache23;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache24;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache25;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache26;

		[CompilerGenerated]
		private static Func<ThingDef, StatDef, string> <>f__am$cache27;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache28;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache29;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache2A;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache2B;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache2C;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache2D;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache2E;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache2F;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache30;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache31;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache32;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache33;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache34;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache35;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache36;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache37;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache38;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache39;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache3A;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache3B;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache3C;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache3D;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache3E;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache3F;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache40;

		[CompilerGenerated]
		private static Func<float, string> <>f__am$cache41;

		[CompilerGenerated]
		private static Func<int, string> <>f__am$cache42;

		[CompilerGenerated]
		private static Func<int, string> <>f__am$cache43;

		[CompilerGenerated]
		private static Func<int, string> <>f__am$cache44;

		[CompilerGenerated]
		private static Func<Hediff_MissingPart, BodyPartDef> <>f__am$cache45;

		[CompilerGenerated]
		private static Func<Hediff_MissingPart, BodyPartDef> <>f__am$cache46;

		[CompilerGenerated]
		private static Func<PawnBio, IEnumerable<string>> <>f__am$cache47;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache48;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache49;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache4A;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache4B;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache4C;

		[CompilerGenerated]
		private static Func<PawnKindDef, bool> <>f__am$cache4D;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache4E;

		[CompilerGenerated]
		private static Func<ThingDef, FoodPreferability> <>f__am$cache4F;

		[CompilerGenerated]
		private static Func<Type, string> <>f__am$cache50;

		[CompilerGenerated]
		private static Func<Type, string> <>f__am$cache51;

		[CompilerGenerated]
		private static Func<Def, string> <>f__am$cache52;

		[CompilerGenerated]
		private static Func<Type, string> <>f__am$cache53;

		[CompilerGenerated]
		private static Func<BodyPartDef, string> <>f__am$cache54;

		[CompilerGenerated]
		private static Func<BodyPartDef, int> <>f__am$cache55;

		[CompilerGenerated]
		private static Func<BodyPartDef, string> <>f__am$cache56;

		[CompilerGenerated]
		private static Func<BodyPartDef, string> <>f__am$cache57;

		[CompilerGenerated]
		private static Func<BodyPartDef, float> <>f__am$cache58;

		[CompilerGenerated]
		private static Func<BodyPartDef, string> <>f__am$cache59;

		[CompilerGenerated]
		private static Func<BodyPartDef, string> <>f__am$cache5A;

		[CompilerGenerated]
		private static Func<BodyPartDef, string> <>f__am$cache5B;

		[CompilerGenerated]
		private static Func<BodyPartDef, string> <>f__am$cache5C;

		[CompilerGenerated]
		private static Func<BodyPartDef, string> <>f__am$cache5D;

		[CompilerGenerated]
		private static Func<BodyPartDef, string> <>f__am$cache5E;

		[CompilerGenerated]
		private static Func<BodyPartDef, string> <>f__am$cache5F;

		[CompilerGenerated]
		private static Func<BodyPartDef, string> <>f__am$cache60;

		[CompilerGenerated]
		private static Func<BodyPartDef, ThingDef> <>f__am$cache61;

		[CompilerGenerated]
		private static Func<BodyPartDef, string> <>f__am$cache62;

		[CompilerGenerated]
		private static Func<BodyPartDef, string> <>f__am$cache63;

		[CompilerGenerated]
		private static Func<TraderKindDef, string> <>f__am$cache64;

		[CompilerGenerated]
		private static Func<TraderKindDef, string> <>f__am$cache65;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache66;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache67;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache68;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache69;

		[CompilerGenerated]
		private static Func<RecipeDef, bool> <>f__am$cache6A;

		[CompilerGenerated]
		private static Func<RecipeDef, float> <>f__am$cache6B;

		[CompilerGenerated]
		private static Func<RecipeDef, string> <>f__am$cache6C;

		[CompilerGenerated]
		private static Func<RecipeDef, string> <>f__am$cache6D;

		[CompilerGenerated]
		private static Func<RecipeDef, string> <>f__am$cache6E;

		[CompilerGenerated]
		private static Func<RecipeDef, string> <>f__am$cache6F;

		[CompilerGenerated]
		private static Func<RecipeDef, string> <>f__am$cache70;

		[CompilerGenerated]
		private static Func<RecipeDef, string> <>f__am$cache71;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache72;

		[CompilerGenerated]
		private static Func<ThingDef, <>__AnonType1<ThingDef, float, int>> <>f__am$cache73;

		[CompilerGenerated]
		private static Func<<>__AnonType1<ThingDef, float, int>, ThingDef> <>f__am$cache74;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache75;

		[CompilerGenerated]
		private static Func<ThingDef, float> <>f__am$cache76;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache77;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache78;

		[CompilerGenerated]
		private static Func<TerrainDef, string> <>f__am$cache79;

		[CompilerGenerated]
		private static Func<TerrainDef, string> <>f__am$cache7A;

		[CompilerGenerated]
		private static Func<TerrainDef, string> <>f__am$cache7B;

		[CompilerGenerated]
		private static Func<TerrainDef, string> <>f__am$cache7C;

		[CompilerGenerated]
		private static Func<TerrainDef, string> <>f__am$cache7D;

		[CompilerGenerated]
		private static Func<TerrainDef, string> <>f__am$cache7E;

		[CompilerGenerated]
		private static Func<TerrainDef, string> <>f__am$cache7F;

		[CompilerGenerated]
		private static Func<TerrainDef, string> <>f__am$cache80;

		[CompilerGenerated]
		private static Func<TerrainDef, string> <>f__am$cache81;

		[CompilerGenerated]
		private static Func<TerrainDef, string> <>f__am$cache82;

		[CompilerGenerated]
		private static Func<TerrainDef, string> <>f__am$cache83;

		[CompilerGenerated]
		private static Func<TerrainDef, string> <>f__am$cache84;

		[CompilerGenerated]
		private static Func<TerrainDef, string> <>f__am$cache85;

		[CompilerGenerated]
		private static Func<TerrainDef, string> <>f__am$cache86;

		[CompilerGenerated]
		private static Func<TerrainDef, string> <>f__am$cache87;

		[CompilerGenerated]
		private static Func<TerrainDef, string> <>f__am$cache88;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache89;

		[CompilerGenerated]
		private static Func<BuildableDef, string> <>f__am$cache8A;

		[CompilerGenerated]
		private static Func<BuildableDef, string> <>f__am$cache8B;

		[CompilerGenerated]
		private static Func<BuildableDef, string> <>f__am$cache8C;

		[CompilerGenerated]
		private static Func<MentalBreakDef, MentalBreakIntensity> <>f__am$cache8D;

		[CompilerGenerated]
		private static Func<MentalBreakDef, string> <>f__am$cache8E;

		[CompilerGenerated]
		private static Func<MentalBreakDef, string> <>f__am$cache8F;

		[CompilerGenerated]
		private static Func<MentalBreakDef, string> <>f__am$cache90;

		[CompilerGenerated]
		private static Func<MentalBreakDef, string> <>f__am$cache91;

		[CompilerGenerated]
		private static Func<MentalBreakDef, string> <>f__am$cache92;

		[CompilerGenerated]
		private static Func<MentalBreakDef, string> <>f__am$cache93;

		[CompilerGenerated]
		private static Func<MentalBreakDef, string> <>f__am$cache94;

		[CompilerGenerated]
		private static Func<MentalBreakDef, string> <>f__am$cache95;

		[CompilerGenerated]
		private static Func<MentalBreakDef, string> <>f__am$cache96;

		[CompilerGenerated]
		private static Func<MentalBreakDef, string> <>f__am$cache97;

		[CompilerGenerated]
		private static Func<MentalBreakDef, string> <>f__am$cache98;

		[CompilerGenerated]
		private static Func<MentalBreakDef, string> <>f__am$cache99;

		[CompilerGenerated]
		private static Func<TraitDegreeData, TraitDef> <>f__am$cache9A;

		[CompilerGenerated]
		private static Func<TraitDef, IEnumerable<TraitDegreeData>> <>f__am$cache9B;

		[CompilerGenerated]
		private static Func<TraitDegreeData, string> <>f__am$cache9C;

		[CompilerGenerated]
		private static Func<TraitDegreeData, string> <>f__am$cache9D;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache9E;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cache9F;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cacheA0;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cacheA1;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cacheA2;

		[CompilerGenerated]
		private static Predicate<Pawn> <>f__am$cacheA3;

		[CompilerGenerated]
		private static Func<RecipeDef, ThingDef> <>f__am$cacheA4;

		[CompilerGenerated]
		private static Func<RecipeDef, string> <>f__am$cacheA5;

		[CompilerGenerated]
		private static Func<RecipeDef, float> <>f__am$cacheA6;

		[CompilerGenerated]
		private static Func<RecipeDef, float> <>f__am$cacheA7;

		[CompilerGenerated]
		private static Func<RecipeDef, int> <>f__am$cacheA8;

		[CompilerGenerated]
		private static Func<PawnCapacityDef, int> <>f__am$cacheA9;

		[CompilerGenerated]
		private static Func<RecipeDef, bool> <>f__am$cacheAA;

		[CompilerGenerated]
		private static Func<JoyGiverDef, string> <>f__am$cacheAB;

		[CompilerGenerated]
		private static Func<JoyGiverDef, string> <>f__am$cacheAC;

		[CompilerGenerated]
		private static Func<JoyGiverDef, string> <>f__am$cacheAD;

		[CompilerGenerated]
		private static Func<JoyGiverDef, string> <>f__am$cacheAE;

		[CompilerGenerated]
		private static Func<JoyGiverDef, string> <>f__am$cacheAF;

		[CompilerGenerated]
		private static Func<JoyGiverDef, string> <>f__am$cacheB0;

		[CompilerGenerated]
		private static Func<JoyGiverDef, string> <>f__am$cacheB1;

		[CompilerGenerated]
		private static Func<JoyGiverDef, string> <>f__am$cacheB2;

		[CompilerGenerated]
		private static Func<JoyGiverDef, string> <>f__am$cacheB3;

		[CompilerGenerated]
		private static Func<JoyGiverDef, string> <>f__am$cacheB4;

		[CompilerGenerated]
		private static Func<JoyGiverDef, string> <>f__am$cacheB5;

		[CompilerGenerated]
		private static Func<JobDef, bool> <>f__am$cacheB6;

		[CompilerGenerated]
		private static Func<JobDef, string> <>f__am$cacheB7;

		[CompilerGenerated]
		private static Func<JobDef, string> <>f__am$cacheB8;

		[CompilerGenerated]
		private static Func<JobDef, string> <>f__am$cacheB9;

		[CompilerGenerated]
		private static Func<JobDef, string> <>f__am$cacheBA;

		[CompilerGenerated]
		private static Func<JobDef, string> <>f__am$cacheBB;

		[CompilerGenerated]
		private static Func<JobDef, string> <>f__am$cacheBC;

		[CompilerGenerated]
		private static Func<JobDef, string> <>f__am$cacheBD;

		[CompilerGenerated]
		private static Func<ThoughtDef, string> <>f__am$cacheBE;

		[CompilerGenerated]
		private static Func<ThoughtDef, string> <>f__am$cacheBF;

		[CompilerGenerated]
		private static Func<ThoughtDef, string> <>f__am$cacheC0;

		[CompilerGenerated]
		private static Func<ThoughtDef, string> <>f__am$cacheC1;

		[CompilerGenerated]
		private static Func<ThoughtDef, float> <>f__am$cacheC2;

		[CompilerGenerated]
		private static Func<ThoughtDef, float> <>f__am$cacheC3;

		[CompilerGenerated]
		private static Func<ThoughtDef, string> <>f__am$cacheC4;

		[CompilerGenerated]
		private static Func<ThoughtDef, string> <>f__am$cacheC5;

		[CompilerGenerated]
		private static Func<ThoughtDef, string> <>f__am$cacheC6;

		[CompilerGenerated]
		private static Func<ThoughtDef, string> <>f__am$cacheC7;

		[CompilerGenerated]
		private static Func<ThoughtDef, string> <>f__am$cacheC8;

		[CompilerGenerated]
		private static Func<ThoughtDef, string> <>f__am$cacheC9;

		[CompilerGenerated]
		private static Func<ThoughtDef, string> <>f__am$cacheCA;

		[CompilerGenerated]
		private static Func<ThoughtDef, string> <>f__am$cacheCB;

		[CompilerGenerated]
		private static Func<ThoughtDef, string> <>f__am$cacheCC;

		[CompilerGenerated]
		private static Func<ThoughtDef, string> <>f__am$cacheCD;

		[CompilerGenerated]
		private static Func<ThoughtDef, string> <>f__am$cacheCE;

		[CompilerGenerated]
		private static Func<ThoughtDef, string> <>f__am$cacheCF;

		[CompilerGenerated]
		private static Func<GenStepDef, float> <>f__am$cacheD0;

		[CompilerGenerated]
		private static Func<GenStepDef, ushort> <>f__am$cacheD1;

		[CompilerGenerated]
		private static Func<GenStepDef, string> <>f__am$cacheD2;

		[CompilerGenerated]
		private static Func<GenStepDef, string> <>f__am$cacheD3;

		[CompilerGenerated]
		private static Func<GenStepDef, string> <>f__am$cacheD4;

		[CompilerGenerated]
		private static Func<GenStepDef, string> <>f__am$cacheD5;

		[CompilerGenerated]
		private static Func<WorldGenStepDef, float> <>f__am$cacheD6;

		[CompilerGenerated]
		private static Func<WorldGenStepDef, ushort> <>f__am$cacheD7;

		[CompilerGenerated]
		private static Func<WorldGenStepDef, string> <>f__am$cacheD8;

		[CompilerGenerated]
		private static Func<WorldGenStepDef, string> <>f__am$cacheD9;

		[CompilerGenerated]
		private static Func<WorldGenStepDef, string> <>f__am$cacheDA;

		[CompilerGenerated]
		private static Func<StuffCategoryDef, string> <>f__am$cacheDB;

		[CompilerGenerated]
		private static Func<KeyValuePair<DamageDef, float>, string> <>f__am$cacheDC;

		[CompilerGenerated]
		private static Func<BodyPartTagDef, string> <>f__am$cacheDD;

		[CompilerGenerated]
		private static Func<IngredientCount, string> <>f__am$cacheDE;

		[CompilerGenerated]
		private static Func<SkillRequirement, string> <>f__am$cacheDF;

		[CompilerGenerated]
		private static Func<ResearchProjectDef, string> <>f__am$cacheE0;

		[CompilerGenerated]
		private static Func<TerrainAffordanceDef, string> <>f__am$cacheE1;

		[CompilerGenerated]
		private static Func<MentalBreakDef, float> <>f__am$cacheE2;

		[CompilerGenerated]
		private static Func<ThingRequestGroup, int> <>f__am$cacheE3;

		[CompilerGenerated]
		private static Func<ThingRequestGroup, int> <>f__am$cacheE4;

		[CompilerGenerated]
		private static Func<IngredientCount, ThingDef> <>f__am$cacheE5;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cacheE6;

		[CompilerGenerated]
		private static Func<IngredientCount, float> <>f__am$cacheE7;

		[CompilerGenerated]
		private static Func<SkillRequirement, int> <>f__am$cacheE8;

		[CompilerGenerated]
		private static Func<PawnCapacityDef, string> <>f__am$cacheE9;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cacheEA;

		[CompilerGenerated]
		private static Func<ThingDef, string> <>f__am$cacheEB;

		[CompilerGenerated]
		private static Func<ThoughtStage, bool> <>f__am$cacheEC;

		[CompilerGenerated]
		private static Func<ThoughtStage, float> <>f__am$cacheED;

		[CompilerGenerated]
		private static Func<ThoughtStage, bool> <>f__am$cacheEE;

		[CompilerGenerated]
		private static Func<ThoughtStage, float> <>f__am$cacheEF;

		[DebugOutput]
		public static void MiningResourceGeneration()
		{
			Func<ThingDef, ThingDef> mineable = delegate(ThingDef d)
			{
				List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].building != null && allDefsListForReading[i].building.mineableThing == d)
					{
						return allDefsListForReading[i];
					}
				}
				return null;
			};
			Func<ThingDef, float> mineableCommonality = delegate(ThingDef d)
			{
				float result;
				if (mineable(d) != null)
				{
					result = mineable(d).building.mineableScatterCommonality;
				}
				else
				{
					result = 0f;
				}
				return result;
			};
			Func<ThingDef, IntRange> mineableLumpSizeRange = delegate(ThingDef d)
			{
				IntRange result;
				if (mineable(d) != null)
				{
					result = mineable(d).building.mineableScatterLumpSizeRange;
				}
				else
				{
					result = IntRange.zero;
				}
				return result;
			};
			Func<ThingDef, float> mineableYield = delegate(ThingDef d)
			{
				float result;
				if (mineable(d) != null)
				{
					result = (float)mineable(d).building.mineableYield;
				}
				else
				{
					result = 0f;
				}
				return result;
			};
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.deepCommonality > 0f || mineableCommonality(d) > 0f
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[11];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("market value", (ThingDef d) => d.BaseMarketValue.ToString("F2"));
			array[2] = new TableDataGetter<ThingDef>("stackLimit", (ThingDef d) => d.stackLimit);
			array[3] = new TableDataGetter<ThingDef>("deep\ncommonality", (ThingDef d) => d.deepCommonality.ToString("F2"));
			array[4] = new TableDataGetter<ThingDef>("deep\nlump size", (ThingDef d) => d.deepLumpSizeRange);
			array[5] = new TableDataGetter<ThingDef>("deep count\nper cell", (ThingDef d) => d.deepCountPerCell);
			array[6] = new TableDataGetter<ThingDef>("deep count\nper portion", (ThingDef d) => d.deepCountPerPortion);
			array[7] = new TableDataGetter<ThingDef>("deep portion value", (ThingDef d) => ((float)d.deepCountPerPortion * d.BaseMarketValue).ToStringMoney(null));
			array[8] = new TableDataGetter<ThingDef>("mineable\ncommonality", (ThingDef d) => mineableCommonality(d).ToString("F2"));
			array[9] = new TableDataGetter<ThingDef>("mineable\nlump size", (ThingDef d) => mineableLumpSizeRange(d));
			array[10] = new TableDataGetter<ThingDef>("mineable yield\nper cell", (ThingDef d) => mineableYield(d));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		[DebugOutput]
		public static void DefaultStuffs()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.MadeFromStuff && !d.IsFrame
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
			array[0] = new TableDataGetter<ThingDef>("category", (ThingDef d) => d.category.ToString());
			array[1] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[2] = new TableDataGetter<ThingDef>("default stuff", (ThingDef d) => GenStuff.DefaultStuffFor(d).defName);
			array[3] = new TableDataGetter<ThingDef>("stuff categories", (ThingDef d) => (from c in d.stuffCategories
			select c.defName).ToCommaList(false));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		[DebugOutput]
		public static void Beauties()
		{
			IEnumerable<BuildableDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs.Cast<BuildableDef>().Concat(DefDatabase<TerrainDef>.AllDefs.Cast<BuildableDef>()).Where(delegate(BuildableDef d)
			{
				ThingDef thingDef = d as ThingDef;
				bool result;
				if (thingDef != null)
				{
					result = BeautyUtility.BeautyRelevant(thingDef.category);
				}
				else
				{
					result = (d is TerrainDef);
				}
				return result;
			})
			orderby (int)d.GetStatValueAbstract(StatDefOf.Beauty, null) descending
			select d;
			TableDataGetter<BuildableDef>[] array = new TableDataGetter<BuildableDef>[6];
			array[0] = new TableDataGetter<BuildableDef>("category", (BuildableDef d) => (!(d is ThingDef)) ? "Terrain" : ((ThingDef)d).category.ToString());
			array[1] = new TableDataGetter<BuildableDef>("defName", (BuildableDef d) => d.defName);
			array[2] = new TableDataGetter<BuildableDef>("beauty", (BuildableDef d) => d.GetStatValueAbstract(StatDefOf.Beauty, null).ToString());
			array[3] = new TableDataGetter<BuildableDef>("market value", (BuildableDef d) => d.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F1"));
			array[4] = new TableDataGetter<BuildableDef>("work to produce", (BuildableDef d) => DebugOutputsEconomy.WorkToProduceBest(d).ToString());
			array[5] = new TableDataGetter<BuildableDef>("beauty per market value", (BuildableDef d) => (d.GetStatValueAbstract(StatDefOf.Beauty, null) <= 0f) ? "" : (d.GetStatValueAbstract(StatDefOf.Beauty, null) / d.GetStatValueAbstract(StatDefOf.MarketValue, null)).ToString("F5"));
			DebugTables.MakeTablesDialog<BuildableDef>(dataSources, array);
		}

		[DebugOutput]
		public static void ThingsPowerAndHeat()
		{
			Func<ThingDef, CompProperties_HeatPusher> heatPusher = delegate(ThingDef d)
			{
				CompProperties_HeatPusher result;
				if (d.comps == null)
				{
					result = null;
				}
				else
				{
					for (int i = 0; i < d.comps.Count; i++)
					{
						CompProperties_HeatPusher compProperties_HeatPusher = d.comps[i] as CompProperties_HeatPusher;
						if (compProperties_HeatPusher != null)
						{
							return compProperties_HeatPusher;
						}
					}
					result = null;
				}
				return result;
			};
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where (d.category == ThingCategory.Building || d.GetCompProperties<CompProperties_Power>() != null || heatPusher(d) != null) && !d.IsFrame
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[10];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("base\npower consumption", (ThingDef d) => (d.GetCompProperties<CompProperties_Power>() != null) ? d.GetCompProperties<CompProperties_Power>().basePowerConsumption.ToString() : "");
			array[2] = new TableDataGetter<ThingDef>("short circuit\nin rain", (ThingDef d) => (d.GetCompProperties<CompProperties_Power>() != null) ? ((!d.GetCompProperties<CompProperties_Power>().shortCircuitInRain) ? "" : "rainfire") : "");
			array[3] = new TableDataGetter<ThingDef>("transmits\npower", (ThingDef d) => (d.GetCompProperties<CompProperties_Power>() != null) ? ((!d.GetCompProperties<CompProperties_Power>().transmitsPower) ? "" : "transmit") : "");
			array[4] = new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.BaseMarketValue);
			array[5] = new TableDataGetter<ThingDef>("cost list", (ThingDef d) => DebugOutputsEconomy.CostListString(d, true, false));
			array[6] = new TableDataGetter<ThingDef>("heat pusher\ncompClass", (ThingDef d) => (heatPusher(d) != null) ? heatPusher(d).compClass.ToString() : "");
			array[7] = new TableDataGetter<ThingDef>("heat pusher\nheat per sec", (ThingDef d) => (heatPusher(d) != null) ? heatPusher(d).heatPerSecond.ToString() : "");
			array[8] = new TableDataGetter<ThingDef>("heat pusher\nmin temp", (ThingDef d) => (heatPusher(d) != null) ? heatPusher(d).heatPushMinTemperature.ToStringTemperature("F1") : "");
			array[9] = new TableDataGetter<ThingDef>("heat pusher\nmax temp", (ThingDef d) => (heatPusher(d) != null) ? heatPusher(d).heatPushMaxTemperature.ToStringTemperature("F1") : "");
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		[DebugOutput]
		public static void FoodPoisonChances()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.IsIngestible
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[3];
			array[0] = new TableDataGetter<ThingDef>("category", (ThingDef d) => d.category);
			array[1] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[2] = new TableDataGetter<ThingDef>("food poison chance", delegate(ThingDef d)
			{
				CompProperties_FoodPoisonable compProperties = d.GetCompProperties<CompProperties_FoodPoisonable>();
				string result;
				if (compProperties != null)
				{
					result = "poisonable by cook";
				}
				else
				{
					float statValueAbstract = d.GetStatValueAbstract(StatDefOf.FoodPoisonChanceFixedHuman, null);
					if (statValueAbstract != 0f)
					{
						result = statValueAbstract.ToStringPercent();
					}
					else
					{
						result = "";
					}
				}
				return result;
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		[DebugOutput]
		public static void TechLevels()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Building || d.category == ThingCategory.Item
			where !d.IsFrame && (d.building == null || !d.building.isNaturalRock)
			orderby (int)d.techLevel descending
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[3];
			array[0] = new TableDataGetter<ThingDef>("category", (ThingDef d) => d.category.ToString());
			array[1] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[2] = new TableDataGetter<ThingDef>("tech level", (ThingDef d) => d.techLevel.ToString());
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		[DebugOutput]
		public static void Stuffs()
		{
			Func<ThingDef, StatDef, string> getStatFactorString = delegate(ThingDef d, StatDef stat)
			{
				string result;
				if (d.stuffProps.statFactors == null)
				{
					result = "";
				}
				else
				{
					StatModifier statModifier = d.stuffProps.statFactors.FirstOrDefault((StatModifier fa) => fa.stat == stat);
					if (statModifier == null)
					{
						result = "";
					}
					else
					{
						result = stat.ValueToString(statModifier.value, ToStringNumberSense.Absolute);
					}
				}
				return result;
			};
			Func<ThingDef, float> meleeDpsSharpFactorOverall = delegate(ThingDef d)
			{
				float statValueAbstract = d.GetStatValueAbstract(StatDefOf.SharpDamageMultiplier, null);
				float statFactorFromList = d.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.MeleeWeapon_CooldownMultiplier);
				return statValueAbstract / statFactorFromList;
			};
			Func<ThingDef, float> meleeDpsBluntFactorOverall = delegate(ThingDef d)
			{
				float statValueAbstract = d.GetStatValueAbstract(StatDefOf.BluntDamageMultiplier, null);
				float statFactorFromList = d.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.MeleeWeapon_CooldownMultiplier);
				return statValueAbstract / statFactorFromList;
			};
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.IsStuff
			orderby d.BaseMarketValue
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[24];
			array[0] = new TableDataGetter<ThingDef>("fabric", (ThingDef d) => d.stuffProps.categories.Contains(StuffCategoryDefOf.Fabric).ToStringCheckBlank());
			array[1] = new TableDataGetter<ThingDef>("leather", (ThingDef d) => d.stuffProps.categories.Contains(StuffCategoryDefOf.Leathery).ToStringCheckBlank());
			array[2] = new TableDataGetter<ThingDef>("metal", (ThingDef d) => d.stuffProps.categories.Contains(StuffCategoryDefOf.Metallic).ToStringCheckBlank());
			array[3] = new TableDataGetter<ThingDef>("stony", (ThingDef d) => d.stuffProps.categories.Contains(StuffCategoryDefOf.Stony).ToStringCheckBlank());
			array[4] = new TableDataGetter<ThingDef>("woody", (ThingDef d) => d.stuffProps.categories.Contains(StuffCategoryDefOf.Woody).ToStringCheckBlank());
			array[5] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[6] = new TableDataGetter<ThingDef>("base\nmarket\nvalue", (ThingDef d) => d.BaseMarketValue.ToStringMoney(null));
			array[7] = new TableDataGetter<ThingDef>("melee\ncooldown\nmultiplier", (ThingDef d) => getStatFactorString(d, StatDefOf.MeleeWeapon_CooldownMultiplier));
			array[8] = new TableDataGetter<ThingDef>("melee\nsharp\ndamage\nmultiplier", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.SharpDamageMultiplier, null).ToString("F2"));
			array[9] = new TableDataGetter<ThingDef>("melee\nsharp\ndps factor\noverall", (ThingDef d) => meleeDpsSharpFactorOverall(d).ToString("F2"));
			array[10] = new TableDataGetter<ThingDef>("melee\nblunt\ndamage\nmultiplier", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.BluntDamageMultiplier, null).ToString("F2"));
			array[11] = new TableDataGetter<ThingDef>("melee\nblunt\ndps factor\noverall", (ThingDef d) => meleeDpsBluntFactorOverall(d).ToString("F2"));
			array[12] = new TableDataGetter<ThingDef>("armor power\nsharp", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Sharp, null).ToString("F2"));
			array[13] = new TableDataGetter<ThingDef>("armor power\nblunt", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Blunt, null).ToString("F2"));
			array[14] = new TableDataGetter<ThingDef>("armor power\nheat", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Heat, null).ToString("F2"));
			array[15] = new TableDataGetter<ThingDef>("insulation\ncold", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffPower_Insulation_Cold, null).ToString("F2"));
			array[16] = new TableDataGetter<ThingDef>("insulation\nheat", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.StuffPower_Insulation_Heat, null).ToString("F2"));
			array[17] = new TableDataGetter<ThingDef>("flammability", (ThingDef d) => d.GetStatValueAbstract(StatDefOf.Flammability, null).ToString("F2"));
			array[18] = new TableDataGetter<ThingDef>("factor\nFlammability", (ThingDef d) => getStatFactorString(d, StatDefOf.Flammability));
			array[19] = new TableDataGetter<ThingDef>("factor\nWorkToMake", (ThingDef d) => getStatFactorString(d, StatDefOf.WorkToMake));
			array[20] = new TableDataGetter<ThingDef>("factor\nWorkToBuild", (ThingDef d) => getStatFactorString(d, StatDefOf.WorkToBuild));
			array[21] = new TableDataGetter<ThingDef>("factor\nMaxHp", (ThingDef d) => getStatFactorString(d, StatDefOf.MaxHitPoints));
			array[22] = new TableDataGetter<ThingDef>("factor\nBeauty", (ThingDef d) => getStatFactorString(d, StatDefOf.Beauty));
			array[23] = new TableDataGetter<ThingDef>("factor\nDoorspeed", (ThingDef d) => getStatFactorString(d, StatDefOf.DoorOpenSpeed));
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		[DebugOutput]
		public static void Drugs()
		{
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.IsDrug
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[3];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("pleasure", (ThingDef d) => (!d.IsPleasureDrug) ? "" : "pleasure");
			array[2] = new TableDataGetter<ThingDef>("non-medical", (ThingDef d) => (!d.IsNonMedicalDrug) ? "" : "non-medical");
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		[DebugOutput]
		public static void Medicines()
		{
			List<float> list = new List<float>();
			list.Add(0.3f);
			list.AddRange(from d in DefDatabase<ThingDef>.AllDefs
			where typeof(Medicine).IsAssignableFrom(d.thingClass)
			select d.GetStatValueAbstract(StatDefOf.MedicalPotency, null));
			SkillNeed_Direct skillNeed_Direct = (SkillNeed_Direct)StatDefOf.MedicalTendQuality.skillNeedFactors[0];
			TableDataGetter<float>[] array = new TableDataGetter<float>[21];
			array[0] = new TableDataGetter<float>("potency", (float p) => p.ToStringPercent());
			for (int i = 0; i < 20; i++)
			{
				float factor = skillNeed_Direct.valuesPerLevel[i];
				array[i + 1] = new TableDataGetter<float>((i + 1).ToString(), (float p) => (p * factor).ToStringPercent());
			}
			DebugTables.MakeTablesDialog<float>(list, array);
		}

		[DebugOutput]
		public static void ShootingAccuracy()
		{
			StatDef stat = StatDefOf.ShootingAccuracyPawn;
			Func<int, float, int, float> accAtDistance = delegate(int level, float dist, int traitDegree)
			{
				float num = 1f;
				if (traitDegree != 0)
				{
					float value = TraitDef.Named("ShootingAccuracy").DataAtDegree(traitDegree).statOffsets.First((StatModifier so) => so.stat == stat).value;
					num += value;
				}
				foreach (SkillNeed skillNeed in stat.skillNeedFactors)
				{
					SkillNeed_Direct skillNeed_Direct = skillNeed as SkillNeed_Direct;
					num *= skillNeed_Direct.valuesPerLevel[level];
				}
				num = stat.postProcessCurve.Evaluate(num);
				return Mathf.Pow(num, dist);
			};
			List<int> list = new List<int>();
			for (int i = 0; i <= 20; i++)
			{
				list.Add(i);
			}
			IEnumerable<int> dataSources = list;
			TableDataGetter<int>[] array = new TableDataGetter<int>[18];
			array[0] = new TableDataGetter<int>("No trait skill", (int lev) => lev.ToString());
			array[1] = new TableDataGetter<int>("acc at 1", (int lev) => accAtDistance(lev, 1f, 0).ToStringPercent("F2"));
			array[2] = new TableDataGetter<int>("acc at 10", (int lev) => accAtDistance(lev, 10f, 0).ToStringPercent("F2"));
			array[3] = new TableDataGetter<int>("acc at 20", (int lev) => accAtDistance(lev, 20f, 0).ToStringPercent("F2"));
			array[4] = new TableDataGetter<int>("acc at 30", (int lev) => accAtDistance(lev, 30f, 0).ToStringPercent("F2"));
			array[5] = new TableDataGetter<int>("acc at 50", (int lev) => accAtDistance(lev, 50f, 0).ToStringPercent("F2"));
			array[6] = new TableDataGetter<int>("Careful shooter skill", (int lev) => lev.ToString());
			array[7] = new TableDataGetter<int>("acc at 1", (int lev) => accAtDistance(lev, 1f, 1).ToStringPercent("F2"));
			array[8] = new TableDataGetter<int>("acc at 10", (int lev) => accAtDistance(lev, 10f, 1).ToStringPercent("F2"));
			array[9] = new TableDataGetter<int>("acc at 20", (int lev) => accAtDistance(lev, 20f, 1).ToStringPercent("F2"));
			array[10] = new TableDataGetter<int>("acc at 30", (int lev) => accAtDistance(lev, 30f, 1).ToStringPercent("F2"));
			array[11] = new TableDataGetter<int>("acc at 50", (int lev) => accAtDistance(lev, 50f, 1).ToStringPercent("F2"));
			array[12] = new TableDataGetter<int>("Trigger-happy skill", (int lev) => lev.ToString());
			array[13] = new TableDataGetter<int>("acc at 1", (int lev) => accAtDistance(lev, 1f, -1).ToStringPercent("F2"));
			array[14] = new TableDataGetter<int>("acc at 10", (int lev) => accAtDistance(lev, 10f, -1).ToStringPercent("F2"));
			array[15] = new TableDataGetter<int>("acc at 20", (int lev) => accAtDistance(lev, 20f, -1).ToStringPercent("F2"));
			array[16] = new TableDataGetter<int>("acc at 30", (int lev) => accAtDistance(lev, 30f, -1).ToStringPercent("F2"));
			array[17] = new TableDataGetter<int>("acc at 50", (int lev) => accAtDistance(lev, 50f, -1).ToStringPercent("F2"));
			DebugTables.MakeTablesDialog<int>(dataSources, array);
		}

		[DebugOutput]
		[ModeRestrictionPlay]
		public static void TemperatureData()
		{
			Find.CurrentMap.mapTemperature.DebugLogTemps();
		}

		[DebugOutput]
		[ModeRestrictionPlay]
		public static void WeatherChances()
		{
			Find.CurrentMap.weatherDecider.LogWeatherChances();
		}

		[DebugOutput]
		[ModeRestrictionPlay]
		public static void CelestialGlow()
		{
			GenCelestial.LogSunGlowForYear();
		}

		[DebugOutput]
		[ModeRestrictionPlay]
		public static void FallColor()
		{
			PlantUtility.LogFallColorForYear();
		}

		[DebugOutput]
		[ModeRestrictionPlay]
		public static void PawnsListAllOnMap()
		{
			Find.CurrentMap.mapPawns.LogListedPawns();
		}

		[DebugOutput]
		[ModeRestrictionPlay]
		public static void WindSpeeds()
		{
			Find.CurrentMap.windManager.LogWindSpeeds();
		}

		[DebugOutput]
		[ModeRestrictionPlay]
		public static void MapPawnsList()
		{
			Find.CurrentMap.mapPawns.LogListedPawns();
		}

		[DebugOutput]
		public static void Lords()
		{
			Find.CurrentMap.lordManager.LogLords();
		}

		[DebugOutput]
		public static void DamageTest()
		{
			ThingDef thingDef = ThingDef.Named("Bullet_BoltActionRifle");
			PawnKindDef slave = PawnKindDefOf.Slave;
			Faction faction = FactionUtility.DefaultFactionFrom(slave.defaultFactionType);
			DamageInfo dinfo = new DamageInfo(thingDef.projectile.damageDef, (float)thingDef.projectile.GetDamageAmount(null, null), thingDef.projectile.GetArmorPenetration(null, null), -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
			int num = 0;
			int num2 = 0;
			DefMap<BodyPartDef, int> defMap = new DefMap<BodyPartDef, int>();
			for (int i = 0; i < 500; i++)
			{
				PawnGenerationRequest request = new PawnGenerationRequest(slave, faction, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, false, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
				Pawn pawn = PawnGenerator.GeneratePawn(request);
				List<BodyPartDef> list = (from hd in pawn.health.hediffSet.GetMissingPartsCommonAncestors()
				select hd.Part.def).ToList<BodyPartDef>();
				for (int j = 0; j < 2; j++)
				{
					pawn.TakeDamage(dinfo);
					if (pawn.Dead)
					{
						num++;
						break;
					}
				}
				List<BodyPartDef> list2 = (from hd in pawn.health.hediffSet.GetMissingPartsCommonAncestors()
				select hd.Part.def).ToList<BodyPartDef>();
				if (list2.Count > list.Count)
				{
					num2++;
					foreach (BodyPartDef bodyPartDef in list2)
					{
						DefMap<BodyPartDef, int> defMap2;
						BodyPartDef def;
						(defMap2 = defMap)[def = bodyPartDef] = defMap2[def] + 1;
					}
					foreach (BodyPartDef bodyPartDef2 in list)
					{
						DefMap<BodyPartDef, int> defMap2;
						BodyPartDef def2;
						(defMap2 = defMap)[def2 = bodyPartDef2] = defMap2[def2] - 1;
					}
				}
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Damage test");
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Hit ",
				500,
				" ",
				slave.label,
				"s with ",
				2,
				"x ",
				thingDef.label,
				" (",
				thingDef.projectile.GetDamageAmount(null, null),
				" damage) each. Results:"
			}));
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Killed: ",
				num,
				" / ",
				500,
				" (",
				((float)num / 500f).ToStringPercent(),
				")"
			}));
			stringBuilder.AppendLine(string.Concat(new object[]
			{
				"Part losers: ",
				num2,
				" / ",
				500,
				" (",
				((float)num2 / 500f).ToStringPercent(),
				")"
			}));
			stringBuilder.AppendLine("Parts lost:");
			foreach (BodyPartDef bodyPartDef3 in DefDatabase<BodyPartDef>.AllDefs)
			{
				if (defMap[bodyPartDef3] > 0)
				{
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"   ",
						bodyPartDef3.label,
						": ",
						defMap[bodyPartDef3]
					}));
				}
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		[DebugOutput]
		public static void BodyPartTagGroups()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (BodyDef localBd2 in DefDatabase<BodyDef>.AllDefs)
			{
				BodyDef localBd = localBd2;
				FloatMenuOption item = new FloatMenuOption(localBd.defName, delegate()
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine(localBd.defName + "\n----------------");
					using (IEnumerator<BodyPartTagDef> enumerator2 = (from elem in localBd.AllParts.SelectMany((BodyPartRecord part) => part.def.tags)
					orderby elem
					select elem).Distinct<BodyPartTagDef>().GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							BodyPartTagDef tag = enumerator2.Current;
							stringBuilder.AppendLine(tag.defName);
							foreach (BodyPartRecord bodyPartRecord in from part in localBd.AllParts
							where part.def.tags.Contains(tag)
							orderby part.def.defName
							select part)
							{
								stringBuilder.AppendLine("  " + bodyPartRecord.def.defName);
							}
						}
					}
					Log.Message(stringBuilder.ToString(), false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		[DebugOutput]
		public static void MinifiableTags()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.Minifiable)
				{
					stringBuilder.Append(thingDef.defName);
					if (!thingDef.tradeTags.NullOrEmpty<string>())
					{
						stringBuilder.Append(" - ");
						stringBuilder.Append(thingDef.tradeTags.ToCommaList(false));
					}
					stringBuilder.AppendLine();
				}
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		[DebugOutput]
		public static void ListSolidBackstories()
		{
			IEnumerable<string> enumerable = SolidBioDatabase.allBios.SelectMany((PawnBio bio) => bio.adulthood.spawnCategories).Distinct<string>();
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (string catInner2 in enumerable)
			{
				string catInner = catInner2;
				FloatMenuOption item = new FloatMenuOption(catInner, delegate()
				{
					IEnumerable<PawnBio> enumerable2 = from b in SolidBioDatabase.allBios
					where b.adulthood.spawnCategories.Contains(catInner)
					select b;
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"Backstories with category: ",
						catInner,
						" (",
						enumerable2.Count<PawnBio>(),
						")"
					}));
					foreach (PawnBio pawnBio in enumerable2)
					{
						stringBuilder.AppendLine(pawnBio.ToString());
					}
					Log.Message(stringBuilder.ToString(), false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		[DebugOutput]
		public static void ThingSetMakerTest()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ThingSetMakerDef localDef2 in DefDatabase<ThingSetMakerDef>.AllDefs)
			{
				ThingSetMakerDef localDef = localDef2;
				DebugMenuOption item = new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Action, delegate()
				{
					Action<ThingSetMakerParams> generate = delegate(ThingSetMakerParams parms)
					{
						StringBuilder stringBuilder = new StringBuilder();
						float num = 0f;
						float num2 = 0f;
						for (int i = 0; i < 50; i++)
						{
							List<Thing> list3 = localDef.root.Generate(parms);
							if (stringBuilder.Length > 0)
							{
								stringBuilder.AppendLine();
							}
							float num3 = 0f;
							float num4 = 0f;
							for (int j = 0; j < list3.Count; j++)
							{
								stringBuilder.AppendLine("-" + list3[j].LabelCap + " - $" + (list3[j].MarketValue * (float)list3[j].stackCount).ToString("F0"));
								num3 += list3[j].MarketValue * (float)list3[j].stackCount;
								if (!(list3[j] is Pawn))
								{
									num4 += list3[j].GetStatValue(StatDefOf.Mass, true) * (float)list3[j].stackCount;
								}
								list3[j].Destroy(DestroyMode.Vanish);
							}
							num += num3;
							num2 += num4;
							stringBuilder.AppendLine("   Total market value: $" + num3.ToString("F0"));
							stringBuilder.AppendLine("   Total mass: " + num4.ToStringMass());
						}
						StringBuilder stringBuilder2 = new StringBuilder();
						stringBuilder2.AppendLine("Default thing sets generated by: " + localDef.defName);
						string nonNullFieldsDebugInfo = Gen.GetNonNullFieldsDebugInfo(localDef.root.fixedParams);
						stringBuilder2.AppendLine("root fixedParams: " + ((!nonNullFieldsDebugInfo.NullOrEmpty()) ? nonNullFieldsDebugInfo : "none"));
						string nonNullFieldsDebugInfo2 = Gen.GetNonNullFieldsDebugInfo(parms);
						if (!nonNullFieldsDebugInfo2.NullOrEmpty())
						{
							stringBuilder2.AppendLine("(used custom debug params: " + nonNullFieldsDebugInfo2 + ")");
						}
						stringBuilder2.AppendLine("Average market value: $" + (num / 50f).ToString("F1"));
						stringBuilder2.AppendLine("Average mass: " + (num2 / 50f).ToStringMass());
						stringBuilder2.AppendLine();
						stringBuilder2.Append(stringBuilder.ToString());
						Log.Message(stringBuilder2.ToString(), false);
					};
					if (localDef == ThingSetMakerDefOf.TraderStock)
					{
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (Faction faction in Find.FactionManager.AllFactions)
						{
							if (faction != Faction.OfPlayer)
							{
								Faction localF = faction;
								list2.Add(new DebugMenuOption(localF.Name + " (" + localF.def.defName + ")", DebugMenuOptionMode.Action, delegate()
								{
									List<DebugMenuOption> list3 = new List<DebugMenuOption>();
									foreach (TraderKindDef localKind2 in (from x in DefDatabase<TraderKindDef>.AllDefs
									where x.orbital
									select x).Concat(localF.def.caravanTraderKinds).Concat(localF.def.visitorTraderKinds).Concat(localF.def.baseTraderKinds))
									{
										TraderKindDef localKind = localKind2;
										list3.Add(new DebugMenuOption(localKind.defName, DebugMenuOptionMode.Action, delegate()
										{
											ThingSetMakerParams obj = default(ThingSetMakerParams);
											obj.traderFaction = localF;
											obj.traderDef = localKind;
											generate(obj);
										}));
									}
									Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3));
								}));
							}
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
					}
					else
					{
						generate(localDef.debugParams);
					}
				});
				list.Add(item);
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		[DebugOutput]
		public static void ThingSetMakerPossibleDefs()
		{
			Dictionary<ThingSetMakerDef, List<ThingDef>> generatableThings = new Dictionary<ThingSetMakerDef, List<ThingDef>>();
			foreach (ThingSetMakerDef thingSetMakerDef in DefDatabase<ThingSetMakerDef>.AllDefs)
			{
				ThingSetMakerDef thingSetMakerDef2 = thingSetMakerDef;
				generatableThings[thingSetMakerDef] = thingSetMakerDef2.root.AllGeneratableThingsDebug(thingSetMakerDef2.debugParams).ToList<ThingDef>();
			}
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName));
			list.Add(new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.BaseMarketValue.ToStringMoney(null)));
			list.Add(new TableDataGetter<ThingDef>("mass", (ThingDef d) => d.BaseMass.ToStringMass()));
			foreach (ThingSetMakerDef localDef2 in DefDatabase<ThingSetMakerDef>.AllDefs)
			{
				ThingSetMakerDef localDef = localDef2;
				list.Add(new TableDataGetter<ThingDef>(localDef.defName.Shorten(), (ThingDef d) => generatableThings[localDef].Contains(d).ToStringCheckBlank()));
			}
			DebugTables.MakeTablesDialog<ThingDef>(from d in DefDatabase<ThingDef>.AllDefs
			where (d.category == ThingCategory.Item && !d.IsCorpse && !d.isUnfinishedThing) || (d.category == ThingCategory.Building && d.Minifiable) || d.category == ThingCategory.Pawn
			orderby d.BaseMarketValue descending
			select d, list.ToArray());
		}

		[DebugOutput]
		public static void ThingSetMakerSampled()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (ThingSetMakerDef localDef2 in DefDatabase<ThingSetMakerDef>.AllDefs)
			{
				ThingSetMakerDef localDef = localDef2;
				DebugMenuOption item = new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Action, delegate()
				{
					Action<ThingSetMakerParams> generate = delegate(ThingSetMakerParams parms)
					{
						Dictionary<ThingDef, int> counts = new Dictionary<ThingDef, int>();
						for (int i = 0; i < 500; i++)
						{
							List<Thing> list3 = localDef.root.Generate(parms);
							foreach (ThingDef thingDef in (from th in list3
							select th.GetInnerIfMinified().def).Distinct<ThingDef>())
							{
								if (!counts.ContainsKey(thingDef))
								{
									counts.Add(thingDef, 0);
								}
								Dictionary<ThingDef, int> counts2;
								ThingDef key;
								(counts2 = counts)[key = thingDef] = counts2[key] + 1;
							}
							for (int j = 0; j < list3.Count; j++)
							{
								list3[j].Destroy(DestroyMode.Vanish);
							}
						}
						IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
						where counts.ContainsKey(d)
						orderby counts[d] descending
						select d;
						TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
						array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
						array[1] = new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.BaseMarketValue.ToStringMoney(null));
						array[2] = new TableDataGetter<ThingDef>("mass", (ThingDef d) => d.BaseMass.ToStringMass());
						array[3] = new TableDataGetter<ThingDef>("appearance rate in " + localDef.defName, (ThingDef d) => ((float)counts[d] / 500f).ToStringPercent());
						DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
					};
					if (localDef == ThingSetMakerDefOf.TraderStock)
					{
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (Faction faction in Find.FactionManager.AllFactions)
						{
							if (faction != Faction.OfPlayer)
							{
								Faction localF = faction;
								list2.Add(new DebugMenuOption(localF.Name + " (" + localF.def.defName + ")", DebugMenuOptionMode.Action, delegate()
								{
									List<DebugMenuOption> list3 = new List<DebugMenuOption>();
									foreach (TraderKindDef localKind2 in (from x in DefDatabase<TraderKindDef>.AllDefs
									where x.orbital
									select x).Concat(localF.def.caravanTraderKinds).Concat(localF.def.visitorTraderKinds).Concat(localF.def.baseTraderKinds))
									{
										TraderKindDef localKind = localKind2;
										list3.Add(new DebugMenuOption(localKind.defName, DebugMenuOptionMode.Action, delegate()
										{
											ThingSetMakerParams obj = default(ThingSetMakerParams);
											obj.traderFaction = localF;
											obj.traderDef = localKind;
											generate(obj);
										}));
									}
									Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3));
								}));
							}
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
					}
					else
					{
						generate(localDef.debugParams);
					}
				});
				list.Add(item);
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		[DebugOutput]
		public static void WorkDisables()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (PawnKindDef pkInner2 in from ki in DefDatabase<PawnKindDef>.AllDefs
			where ki.RaceProps.Humanlike
			select ki)
			{
				PawnKindDef pkInner = pkInner2;
				Faction faction = FactionUtility.DefaultFactionFrom(pkInner.defaultFactionType);
				FloatMenuOption item = new FloatMenuOption(pkInner.defName, delegate()
				{
					int num = 500;
					DefMap<WorkTypeDef, int> defMap = new DefMap<WorkTypeDef, int>();
					for (int i = 0; i < num; i++)
					{
						Pawn pawn = PawnGenerator.GeneratePawn(pkInner, faction);
						foreach (WorkTypeDef workTypeDef in pawn.story.DisabledWorkTypes)
						{
							DefMap<WorkTypeDef, int> defMap2;
							WorkTypeDef def;
							(defMap2 = defMap)[def = workTypeDef] = defMap2[def] + 1;
						}
					}
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"Generated ",
						num,
						" pawns of kind ",
						pkInner.defName,
						" on faction ",
						faction.ToStringSafe<Faction>()
					}));
					stringBuilder.AppendLine("Work types disabled:");
					foreach (WorkTypeDef workTypeDef2 in DefDatabase<WorkTypeDef>.AllDefs)
					{
						if (workTypeDef2.workTags != WorkTags.None)
						{
							stringBuilder.AppendLine(string.Concat(new object[]
							{
								"   ",
								workTypeDef2.defName,
								": ",
								defMap[workTypeDef2],
								"        ",
								((float)defMap[workTypeDef2] / (float)num).ToStringPercent()
							}));
						}
					}
					IEnumerable<Backstory> enumerable = BackstoryDatabase.allBackstories.Select((KeyValuePair<string, Backstory> kvp) => kvp.Value);
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("Backstories WorkTypeDef disable rates (there are " + enumerable.Count<Backstory>() + " backstories):");
					using (IEnumerator<WorkTypeDef> enumerator4 = DefDatabase<WorkTypeDef>.AllDefs.GetEnumerator())
					{
						while (enumerator4.MoveNext())
						{
							WorkTypeDef wt = enumerator4.Current;
							int num2 = 0;
							foreach (Backstory backstory in enumerable)
							{
								if (backstory.DisabledWorkTypes.Any((WorkTypeDef wd) => wt == wd))
								{
									num2++;
								}
							}
							stringBuilder.AppendLine(string.Concat(new object[]
							{
								"   ",
								wt.defName,
								": ",
								num2,
								"     ",
								((float)num2 / (float)BackstoryDatabase.allBackstories.Count).ToStringPercent()
							}));
						}
					}
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("Backstories WorkTag disable rates (there are " + enumerable.Count<Backstory>() + " backstories):");
					IEnumerator enumerator6 = Enum.GetValues(typeof(WorkTags)).GetEnumerator();
					try
					{
						while (enumerator6.MoveNext())
						{
							object obj = enumerator6.Current;
							WorkTags workTags = (WorkTags)obj;
							int num3 = 0;
							foreach (Backstory backstory2 in enumerable)
							{
								if ((workTags & backstory2.workDisables) != WorkTags.None)
								{
									num3++;
								}
							}
							stringBuilder.AppendLine(string.Concat(new object[]
							{
								"   ",
								workTags,
								": ",
								num3,
								"     ",
								((float)num3 / (float)BackstoryDatabase.allBackstories.Count).ToStringPercent()
							}));
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator6 as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					Log.Message(stringBuilder.ToString(), false);
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				list.Add(item);
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		[DebugOutput]
		public static void KeyStrings()
		{
			StringBuilder stringBuilder = new StringBuilder();
			IEnumerator enumerator = Enum.GetValues(typeof(KeyCode)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					KeyCode k = (KeyCode)obj;
					stringBuilder.AppendLine(k.ToString() + " - " + k.ToStringReadable());
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		[DebugOutput]
		public static void SocialPropernessMatters()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Social-properness-matters things:");
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				if (thingDef.socialPropernessMatters)
				{
					stringBuilder.AppendLine(string.Format("  {0}", thingDef.defName));
				}
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		[DebugOutput]
		public static void FoodPreferability()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Food, ordered by preferability:");
			foreach (ThingDef thingDef in from td in DefDatabase<ThingDef>.AllDefs
			where td.ingestible != null
			orderby td.ingestible.preferability
			select td)
			{
				stringBuilder.AppendLine(string.Format("  {0}: {1}", thingDef.ingestible.preferability, thingDef.defName));
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		[DebugOutput]
		public static void MapDanger()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Map danger status:");
			foreach (Map map in Find.Maps)
			{
				stringBuilder.AppendLine(string.Format("{0}: {1}", map, map.dangerWatcher.DangerRating));
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		[DebugOutput]
		public static void DefNames()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<Type> enumerator = (from def in GenDefDatabase.AllDefTypesWithDatabases()
			orderby def.Name
			select def).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Type type = enumerator.Current;
					DebugMenuOption item = new DebugMenuOption(type.Name, DebugMenuOptionMode.Action, delegate()
					{
						IEnumerable source = (IEnumerable)GenGeneric.GetStaticPropertyOnGenericType(typeof(DefDatabase<>), type, "AllDefs");
						int num = 0;
						StringBuilder stringBuilder = new StringBuilder();
						foreach (Def def in source.Cast<Def>())
						{
							stringBuilder.AppendLine(def.defName);
							num++;
							if (num >= 500)
							{
								Log.Message(stringBuilder.ToString(), false);
								stringBuilder = new StringBuilder();
								num = 0;
							}
						}
						Log.Message(stringBuilder.ToString(), false);
					});
					list.Add(item);
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		[DebugOutput]
		public static void DefNamesAll()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (Type type in from def in GenDefDatabase.AllDefTypesWithDatabases()
			orderby def.Name
			select def)
			{
				IEnumerable source = (IEnumerable)GenGeneric.GetStaticPropertyOnGenericType(typeof(DefDatabase<>), type, "AllDefs");
				stringBuilder.AppendLine("--    " + type.ToString());
				foreach (Def def2 in source.Cast<Def>().OrderBy((Def def) => def.defName))
				{
					stringBuilder.AppendLine(def2.defName);
					num++;
					if (num >= 500)
					{
						Log.Message(stringBuilder.ToString(), false);
						stringBuilder = new StringBuilder();
						num = 0;
					}
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		[DebugOutput]
		public static void DefLabels()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<Type> enumerator = (from def in GenDefDatabase.AllDefTypesWithDatabases()
			orderby def.Name
			select def).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Type type = enumerator.Current;
					DebugMenuOption item = new DebugMenuOption(type.Name, DebugMenuOptionMode.Action, delegate()
					{
						IEnumerable source = (IEnumerable)GenGeneric.GetStaticPropertyOnGenericType(typeof(DefDatabase<>), type, "AllDefs");
						int num = 0;
						StringBuilder stringBuilder = new StringBuilder();
						foreach (Def def in source.Cast<Def>())
						{
							stringBuilder.AppendLine(def.label);
							num++;
							if (num >= 500)
							{
								Log.Message(stringBuilder.ToString(), false);
								stringBuilder = new StringBuilder();
								num = 0;
							}
						}
						Log.Message(stringBuilder.ToString(), false);
					});
					list.Add(item);
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		[DebugOutput]
		public static void Bodies()
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (BodyDef localBd2 in DefDatabase<BodyDef>.AllDefs)
			{
				BodyDef localBd = localBd2;
				list.Add(new FloatMenuOption(localBd.defName, delegate()
				{
					IEnumerable<BodyPartRecord> dataSources = from d in localBd.AllParts
					orderby d.height descending
					select d;
					TableDataGetter<BodyPartRecord>[] array = new TableDataGetter<BodyPartRecord>[7];
					array[0] = new TableDataGetter<BodyPartRecord>("defName", (BodyPartRecord d) => d.def.defName);
					array[1] = new TableDataGetter<BodyPartRecord>("hitPoints\n(non-adjusted)", (BodyPartRecord d) => d.def.hitPoints);
					array[2] = new TableDataGetter<BodyPartRecord>("coverage", (BodyPartRecord d) => d.coverage.ToStringPercent());
					array[3] = new TableDataGetter<BodyPartRecord>("coverageAbsWithChildren", (BodyPartRecord d) => d.coverageAbsWithChildren.ToStringPercent());
					array[4] = new TableDataGetter<BodyPartRecord>("coverageAbs", (BodyPartRecord d) => d.coverageAbs.ToStringPercent());
					array[5] = new TableDataGetter<BodyPartRecord>("depth", (BodyPartRecord d) => d.depth.ToString());
					array[6] = new TableDataGetter<BodyPartRecord>("height", (BodyPartRecord d) => d.height.ToString());
					DebugTables.MakeTablesDialog<BodyPartRecord>(dataSources, array);
				}, MenuOptionPriority.Default, null, null, 0f, null, null));
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		[DebugOutput]
		public static void BodyParts()
		{
			IEnumerable<BodyPartDef> allDefs = DefDatabase<BodyPartDef>.AllDefs;
			TableDataGetter<BodyPartDef>[] array = new TableDataGetter<BodyPartDef>[16];
			array[0] = new TableDataGetter<BodyPartDef>("defName", (BodyPartDef d) => d.defName);
			array[1] = new TableDataGetter<BodyPartDef>("hit\npoints", (BodyPartDef d) => d.hitPoints);
			array[2] = new TableDataGetter<BodyPartDef>("bleeding\nate\nmultiplier", (BodyPartDef d) => d.bleedRate.ToStringPercent());
			array[3] = new TableDataGetter<BodyPartDef>("perm injury\nchance factor", (BodyPartDef d) => d.permanentInjuryChanceFactor.ToStringPercent());
			array[4] = new TableDataGetter<BodyPartDef>("frostbite\nvulnerability", (BodyPartDef d) => d.frostbiteVulnerability);
			array[5] = new TableDataGetter<BodyPartDef>("solid", (BodyPartDef d) => (!d.IsSolidInDefinition_Debug) ? "" : "S");
			array[6] = new TableDataGetter<BodyPartDef>("beauty\nrelated", (BodyPartDef d) => (!d.beautyRelated) ? "" : "B");
			array[7] = new TableDataGetter<BodyPartDef>("alive", (BodyPartDef d) => (!d.alive) ? "" : "A");
			array[8] = new TableDataGetter<BodyPartDef>("conceptual", (BodyPartDef d) => (!d.conceptual) ? "" : "C");
			array[9] = new TableDataGetter<BodyPartDef>("can\nsuggest\namputation", (BodyPartDef d) => (!d.canSuggestAmputation) ? "no A" : "");
			array[10] = new TableDataGetter<BodyPartDef>("socketed", (BodyPartDef d) => (!d.socketed) ? "" : "DoL");
			array[11] = new TableDataGetter<BodyPartDef>("skin covered", (BodyPartDef d) => (!d.IsSkinCoveredInDefinition_Debug) ? "" : "skin");
			array[12] = new TableDataGetter<BodyPartDef>("pawn generator\ncan amputate", (BodyPartDef d) => (!d.pawnGeneratorCanAmputate) ? "" : "amp");
			array[13] = new TableDataGetter<BodyPartDef>("spawn thing\non removed", (BodyPartDef d) => d.spawnThingOnRemoved);
			array[14] = new TableDataGetter<BodyPartDef>("hitChanceFactors", delegate(BodyPartDef d)
			{
				string result;
				if (d.hitChanceFactors == null)
				{
					result = "";
				}
				else
				{
					result = (from kvp in d.hitChanceFactors
					select kvp.ToString()).ToCommaList(false);
				}
				return result;
			});
			array[15] = new TableDataGetter<BodyPartDef>("tags", delegate(BodyPartDef d)
			{
				string result;
				if (d.tags == null)
				{
					result = "";
				}
				else
				{
					result = (from t in d.tags
					select t.defName).ToCommaList(false);
				}
				return result;
			});
			DebugTables.MakeTablesDialog<BodyPartDef>(allDefs, array);
		}

		[DebugOutput]
		public static void TraderKinds()
		{
			IEnumerable<TraderKindDef> allDefs = DefDatabase<TraderKindDef>.AllDefs;
			TableDataGetter<TraderKindDef>[] array = new TableDataGetter<TraderKindDef>[2];
			array[0] = new TableDataGetter<TraderKindDef>("defName", (TraderKindDef d) => d.defName);
			array[1] = new TableDataGetter<TraderKindDef>("commonality", (TraderKindDef d) => d.CalculatedCommonality.ToString("F2"));
			DebugTables.MakeTablesDialog<TraderKindDef>(allDefs, array);
		}

		[DebugOutput]
		public static void TraderKindThings()
		{
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName));
			foreach (TraderKindDef localTk2 in DefDatabase<TraderKindDef>.AllDefs)
			{
				TraderKindDef localTk = localTk2;
				string text = localTk.defName;
				text = text.Replace("Caravan", "Car");
				text = text.Replace("Visitor", "Vis");
				text = text.Replace("Orbital", "Orb");
				text = text.Replace("Neolithic", "Ne");
				text = text.Replace("Outlander", "Out");
				text = text.Replace("_", " ");
				text = text.Shorten();
				list.Add(new TableDataGetter<ThingDef>(text, (ThingDef td) => localTk.WillTrade(td).ToStringCheckBlank()));
			}
			DebugTables.MakeTablesDialog<ThingDef>(from d in DefDatabase<ThingDef>.AllDefs
			where (d.category == ThingCategory.Item && d.BaseMarketValue > 0.001f && !d.isUnfinishedThing && !d.IsCorpse && !d.destroyOnDrop && d != ThingDefOf.Silver && !d.thingCategories.NullOrEmpty<ThingCategoryDef>()) || (d.category == ThingCategory.Building && d.Minifiable) || d.category == ThingCategory.Pawn
			orderby d.thingCategories.NullOrEmpty<ThingCategoryDef>() ? "zzzzzzz" : d.thingCategories[0].defName, d.BaseMarketValue
			select d, list.ToArray());
		}

		[DebugOutput]
		public static void Surgeries()
		{
			IEnumerable<RecipeDef> dataSources = from d in DefDatabase<RecipeDef>.AllDefs
			where d.IsSurgery
			orderby d.WorkAmountTotal(null) descending
			select d;
			TableDataGetter<RecipeDef>[] array = new TableDataGetter<RecipeDef>[6];
			array[0] = new TableDataGetter<RecipeDef>("defName", (RecipeDef d) => d.defName);
			array[1] = new TableDataGetter<RecipeDef>("work", (RecipeDef d) => d.WorkAmountTotal(null).ToString("F0"));
			array[2] = new TableDataGetter<RecipeDef>("ingredients", (RecipeDef d) => (from ing in d.ingredients
			select ing.ToString()).ToCommaList(false));
			array[3] = new TableDataGetter<RecipeDef>("skillRequirements", delegate(RecipeDef d)
			{
				string result;
				if (d.skillRequirements == null)
				{
					result = "-";
				}
				else
				{
					result = (from ing in d.skillRequirements
					select ing.ToString()).ToCommaList(false);
				}
				return result;
			});
			array[4] = new TableDataGetter<RecipeDef>("surgerySuccessChanceFactor", (RecipeDef d) => d.surgerySuccessChanceFactor.ToStringPercent());
			array[5] = new TableDataGetter<RecipeDef>("deathOnFailChance", (RecipeDef d) => d.deathOnFailedSurgeryChance.ToStringPercent());
			DebugTables.MakeTablesDialog<RecipeDef>(dataSources, array);
		}

		[DebugOutput]
		public static void HitsToKill()
		{
			Dictionary<ThingDef, <>__AnonType1<ThingDef, float, int>> data = (from d in DefDatabase<ThingDef>.AllDefs
			where d.race != null
			select d).Select(delegate(ThingDef x)
			{
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < 15; i++)
				{
					PawnGenerationRequest request = new PawnGenerationRequest(x.race.AnyPawnKind, null, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, false, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
					Pawn pawn = PawnGenerator.GeneratePawn(request);
					for (int j = 0; j < 1000; j++)
					{
						pawn.TakeDamage(new DamageInfo(DamageDefOf.Crush, 10f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
						if (pawn.Destroyed)
						{
							num += j + 1;
							break;
						}
					}
					if (!pawn.Destroyed)
					{
						Log.Error("Could not kill pawn " + pawn.ToStringSafe<Pawn>(), false);
					}
					if (pawn.health.ShouldBeDeadFromLethalDamageThreshold())
					{
						num2++;
					}
					if (Find.WorldPawns.Contains(pawn))
					{
						Find.WorldPawns.RemovePawn(pawn);
					}
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
				}
				float hits = (float)num / 15f;
				return new
				{
					Race = x,
					Hits = hits,
					DiedDueToDamageThreshold = num2
				};
			}).ToDictionary(x => x.Race);
			IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
			where d.race != null
			orderby d.race.baseHealthScale descending
			select d;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("10 damage hits", (ThingDef d) => data[d].Hits.ToString("F0"));
			array[2] = new TableDataGetter<ThingDef>("died due to\ndam. thresh.", (ThingDef d) => data[d].DiedDueToDamageThreshold + "/" + 15);
			array[3] = new TableDataGetter<ThingDef>("mech", (ThingDef d) => (!d.race.IsMechanoid) ? "" : "mech");
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		[DebugOutput]
		public static void Terrains()
		{
			IEnumerable<TerrainDef> allDefs = DefDatabase<TerrainDef>.AllDefs;
			TableDataGetter<TerrainDef>[] array = new TableDataGetter<TerrainDef>[16];
			array[0] = new TableDataGetter<TerrainDef>("defName", (TerrainDef d) => d.defName);
			array[1] = new TableDataGetter<TerrainDef>("work", (TerrainDef d) => d.GetStatValueAbstract(StatDefOf.WorkToBuild, null).ToString());
			array[2] = new TableDataGetter<TerrainDef>("beauty", (TerrainDef d) => d.GetStatValueAbstract(StatDefOf.Beauty, null).ToString());
			array[3] = new TableDataGetter<TerrainDef>("cleanliness", (TerrainDef d) => d.GetStatValueAbstract(StatDefOf.Cleanliness, null).ToString());
			array[4] = new TableDataGetter<TerrainDef>("path\ncost", (TerrainDef d) => d.pathCost.ToString());
			array[5] = new TableDataGetter<TerrainDef>("fertility", (TerrainDef d) => d.fertility.ToStringPercentEmptyZero("F0"));
			array[6] = new TableDataGetter<TerrainDef>("accept\nfilth", (TerrainDef d) => d.acceptFilth.ToStringCheckBlank());
			array[7] = new TableDataGetter<TerrainDef>("accept terrain\nsource filth", (TerrainDef d) => d.acceptTerrainSourceFilth.ToStringCheckBlank());
			array[8] = new TableDataGetter<TerrainDef>("generated\nfilth", (TerrainDef d) => (d.generatedFilth == null) ? "" : d.generatedFilth.defName);
			array[9] = new TableDataGetter<TerrainDef>("hold\nsnow", (TerrainDef d) => d.holdSnow.ToStringCheckBlank());
			array[10] = new TableDataGetter<TerrainDef>("take\nfootprints", (TerrainDef d) => d.takeFootprints.ToStringCheckBlank());
			array[11] = new TableDataGetter<TerrainDef>("avoid\nwander", (TerrainDef d) => d.avoidWander.ToStringCheckBlank());
			array[12] = new TableDataGetter<TerrainDef>("buildable", (TerrainDef d) => d.BuildableByPlayer.ToStringCheckBlank());
			array[13] = new TableDataGetter<TerrainDef>("cost\nlist", (TerrainDef d) => DebugOutputsEconomy.CostListString(d, false, false));
			array[14] = new TableDataGetter<TerrainDef>("research", delegate(TerrainDef d)
			{
				string result;
				if (d.researchPrerequisites != null)
				{
					result = (from pr in d.researchPrerequisites
					select pr.defName).ToCommaList(false);
				}
				else
				{
					result = "";
				}
				return result;
			});
			array[15] = new TableDataGetter<TerrainDef>("affordances", (TerrainDef d) => (from af in d.affordances
			select af.defName).ToCommaList(false));
			DebugTables.MakeTablesDialog<TerrainDef>(allDefs, array);
		}

		[DebugOutput]
		public static void TerrainAffordances()
		{
			IEnumerable<BuildableDef> dataSources = (from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Building && !d.IsFrame
			select d).Cast<BuildableDef>().Concat(DefDatabase<TerrainDef>.AllDefs.Cast<BuildableDef>());
			TableDataGetter<BuildableDef>[] array = new TableDataGetter<BuildableDef>[3];
			array[0] = new TableDataGetter<BuildableDef>("type", (BuildableDef d) => (!(d is TerrainDef)) ? "building" : "terrain");
			array[1] = new TableDataGetter<BuildableDef>("defName", (BuildableDef d) => d.defName);
			array[2] = new TableDataGetter<BuildableDef>("terrain\naffordance\nneeded", (BuildableDef d) => (d.terrainAffordanceNeeded == null) ? "" : d.terrainAffordanceNeeded.defName);
			DebugTables.MakeTablesDialog<BuildableDef>(dataSources, array);
		}

		[DebugOutput]
		public static void MentalBreaks()
		{
			IEnumerable<MentalBreakDef> dataSources = from d in DefDatabase<MentalBreakDef>.AllDefs
			orderby d.intensity, d.defName
			select d;
			TableDataGetter<MentalBreakDef>[] array = new TableDataGetter<MentalBreakDef>[11];
			array[0] = new TableDataGetter<MentalBreakDef>("defName", (MentalBreakDef d) => d.defName);
			array[1] = new TableDataGetter<MentalBreakDef>("intensity", (MentalBreakDef d) => d.intensity.ToString());
			array[2] = new TableDataGetter<MentalBreakDef>("chance in intensity", (MentalBreakDef d) => (d.baseCommonality / (from x in DefDatabase<MentalBreakDef>.AllDefs
			where x.intensity == d.intensity
			select x).Sum((MentalBreakDef x) => x.baseCommonality)).ToStringPercent());
			array[3] = new TableDataGetter<MentalBreakDef>("min duration (days)", (MentalBreakDef d) => (d.mentalState != null) ? ((float)d.mentalState.minTicksBeforeRecovery / 60000f).ToString("0.##") : "");
			array[4] = new TableDataGetter<MentalBreakDef>("avg duration (days)", (MentalBreakDef d) => (d.mentalState != null) ? (Mathf.Min((float)d.mentalState.minTicksBeforeRecovery + d.mentalState.recoveryMtbDays * 60000f, (float)d.mentalState.maxTicksBeforeRecovery) / 60000f).ToString("0.##") : "");
			array[5] = new TableDataGetter<MentalBreakDef>("max duration (days)", (MentalBreakDef d) => (d.mentalState != null) ? ((float)d.mentalState.maxTicksBeforeRecovery / 60000f).ToString("0.##") : "");
			array[6] = new TableDataGetter<MentalBreakDef>("recoverFromSleep", (MentalBreakDef d) => (d.mentalState == null || !d.mentalState.recoverFromSleep) ? "" : "recoverFromSleep");
			array[7] = new TableDataGetter<MentalBreakDef>("recoveryThought", (MentalBreakDef d) => (d.mentalState != null) ? d.mentalState.moodRecoveryThought.ToStringSafe<ThoughtDef>() : "");
			array[8] = new TableDataGetter<MentalBreakDef>("category", (MentalBreakDef d) => (d.mentalState == null) ? "" : d.mentalState.category.ToString());
			array[9] = new TableDataGetter<MentalBreakDef>("blockNormalThoughts", (MentalBreakDef d) => (d.mentalState == null || !d.mentalState.blockNormalThoughts) ? "" : "blockNormalThoughts");
			array[10] = new TableDataGetter<MentalBreakDef>("allowBeatfire", (MentalBreakDef d) => (d.mentalState == null || !d.mentalState.allowBeatfire) ? "" : "allowBeatfire");
			DebugTables.MakeTablesDialog<MentalBreakDef>(dataSources, array);
		}

		[DebugOutput]
		public static void TraitsSampled()
		{
			List<Pawn> testColonists = new List<Pawn>();
			for (int i = 0; i < 4000; i++)
			{
				testColonists.Add(PawnGenerator.GeneratePawn(PawnKindDefOf.Colonist, Faction.OfPlayer));
			}
			Func<TraitDegreeData, TraitDef> getTrait = (TraitDegreeData d) => DefDatabase<TraitDef>.AllDefs.First((TraitDef td) => td.degreeDatas.Contains(d));
			Func<TraitDegreeData, float> getPrevalence = delegate(TraitDegreeData d)
			{
				float num = 0f;
				foreach (Pawn pawn in testColonists)
				{
					Trait trait = pawn.story.traits.GetTrait(getTrait(d));
					if (trait != null && trait.Degree == d.degree)
					{
						num += 1f;
					}
				}
				return num / 4000f;
			};
			IEnumerable<TraitDegreeData> dataSources = DefDatabase<TraitDef>.AllDefs.SelectMany((TraitDef tr) => tr.degreeDatas);
			TableDataGetter<TraitDegreeData>[] array = new TableDataGetter<TraitDegreeData>[8];
			array[0] = new TableDataGetter<TraitDegreeData>("trait", (TraitDegreeData d) => getTrait(d).defName);
			array[1] = new TableDataGetter<TraitDegreeData>("trait commonality", (TraitDegreeData d) => getTrait(d).GetGenderSpecificCommonality(Gender.None).ToString("F2"));
			array[2] = new TableDataGetter<TraitDegreeData>("trait commonalityFemale", (TraitDegreeData d) => getTrait(d).GetGenderSpecificCommonality(Gender.Female).ToString("F2"));
			array[3] = new TableDataGetter<TraitDegreeData>("degree", (TraitDegreeData d) => d.label);
			array[4] = new TableDataGetter<TraitDegreeData>("degree num", (TraitDegreeData d) => (getTrait(d).degreeDatas.Count <= 0) ? "" : d.degree.ToString());
			array[5] = new TableDataGetter<TraitDegreeData>("degree commonality", (TraitDegreeData d) => (getTrait(d).degreeDatas.Count <= 0) ? "" : d.commonality.ToString("F2"));
			array[6] = new TableDataGetter<TraitDegreeData>("marketValueFactorOffset", (TraitDegreeData d) => d.marketValueFactorOffset.ToString("F0"));
			array[7] = new TableDataGetter<TraitDegreeData>("prevalence among " + 4000 + "\ngenerated Colonists", (TraitDegreeData d) => getPrevalence(d).ToStringPercent());
			DebugTables.MakeTablesDialog<TraitDegreeData>(dataSources, array);
		}

		[DebugOutput]
		public static void BestThingRequestGroup()
		{
			IEnumerable<ThingDef> dataSources = from x in DefDatabase<ThingDef>.AllDefs
			where ListerThings.EverListable(x, ListerThingsUse.Global) || ListerThings.EverListable(x, ListerThingsUse.Region)
			orderby x.label
			select x;
			TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[3];
			array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
			array[1] = new TableDataGetter<ThingDef>("best local", delegate(ThingDef d)
			{
				IEnumerable<ThingRequestGroup> source;
				if (!ListerThings.EverListable(d, ListerThingsUse.Region))
				{
					source = Enumerable.Empty<ThingRequestGroup>();
				}
				else
				{
					source = from x in (ThingRequestGroup[])Enum.GetValues(typeof(ThingRequestGroup))
					where x.StoreInRegion() && x.Includes(d)
					select x;
				}
				string result;
				if (!source.Any<ThingRequestGroup>())
				{
					result = "-";
				}
				else
				{
					ThingRequestGroup best = source.MinBy((ThingRequestGroup x) => DefDatabase<ThingDef>.AllDefs.Count((ThingDef y) => ListerThings.EverListable(y, ListerThingsUse.Region) && x.Includes(y)));
					result = string.Concat(new object[]
					{
						best,
						" (defs: ",
						DefDatabase<ThingDef>.AllDefs.Count((ThingDef x) => ListerThings.EverListable(x, ListerThingsUse.Region) && best.Includes(x)),
						")"
					});
				}
				return result;
			});
			array[2] = new TableDataGetter<ThingDef>("best global", delegate(ThingDef d)
			{
				IEnumerable<ThingRequestGroup> source;
				if (!ListerThings.EverListable(d, ListerThingsUse.Global))
				{
					source = Enumerable.Empty<ThingRequestGroup>();
				}
				else
				{
					source = from x in (ThingRequestGroup[])Enum.GetValues(typeof(ThingRequestGroup))
					where x.Includes(d)
					select x;
				}
				string result;
				if (!source.Any<ThingRequestGroup>())
				{
					result = "-";
				}
				else
				{
					ThingRequestGroup best = source.MinBy((ThingRequestGroup x) => DefDatabase<ThingDef>.AllDefs.Count((ThingDef y) => ListerThings.EverListable(y, ListerThingsUse.Global) && x.Includes(y)));
					result = string.Concat(new object[]
					{
						best,
						" (defs: ",
						DefDatabase<ThingDef>.AllDefs.Count((ThingDef x) => ListerThings.EverListable(x, ListerThingsUse.Global) && best.Includes(x)),
						")"
					});
				}
				return result;
			});
			DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
		}

		[DebugOutput]
		public static void Prosthetics()
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.Colonist, Faction.OfPlayer, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, false, false, false, false, (Pawn p) => p.health.hediffSet.hediffs.Count == 0, null, null, null, null, null, null, null);
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			Action refreshPawn = delegate()
			{
				while (pawn.health.hediffSet.hediffs.Count > 0)
				{
					pawn.health.RemoveHediff(pawn.health.hediffSet.hediffs[0]);
				}
			};
			Func<RecipeDef, BodyPartRecord> getApplicationPoint = (RecipeDef recipe) => recipe.appliedOnFixedBodyParts.SelectMany((BodyPartDef bpd) => pawn.def.race.body.GetPartsWithDef(bpd)).FirstOrDefault<BodyPartRecord>();
			Func<RecipeDef, ThingDef> getProstheticItem = (RecipeDef recipe) => (from ic in recipe.ingredients
			select ic.filter.AnyAllowedDef).FirstOrDefault((ThingDef td) => !td.IsMedicine);
			List<TableDataGetter<RecipeDef>> list = new List<TableDataGetter<RecipeDef>>();
			list.Add(new TableDataGetter<RecipeDef>("defName", (RecipeDef r) => r.defName));
			list.Add(new TableDataGetter<RecipeDef>("price", delegate(RecipeDef r)
			{
				ThingDef thingDef = getProstheticItem(r);
				return (thingDef == null) ? 0f : thingDef.BaseMarketValue;
			}));
			list.Add(new TableDataGetter<RecipeDef>("install time", (RecipeDef r) => r.workAmount));
			list.Add(new TableDataGetter<RecipeDef>("install total cost", delegate(RecipeDef r)
			{
				float num = r.ingredients.Sum((IngredientCount ic) => ic.filter.AnyAllowedDef.BaseMarketValue * ic.GetBaseCount());
				float num2 = r.workAmount * 0.0036f;
				return num + num2;
			}));
			list.Add(new TableDataGetter<RecipeDef>("install skill", (RecipeDef r) => (from sr in r.skillRequirements
			select sr.minLevel).Max()));
			using (IEnumerator<PawnCapacityDef> enumerator = (from pc in DefDatabase<PawnCapacityDef>.AllDefs
			orderby pc.listOrder
			select pc).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PawnCapacityDef cap = enumerator.Current;
					list.Add(new TableDataGetter<RecipeDef>(cap.defName, delegate(RecipeDef r)
					{
						refreshPawn();
						Thing pawn;
						r.Worker.ApplyOnPawn(pawn, getApplicationPoint(r), null, null, null);
						float num = pawn.health.capacities.GetLevel(cap) - 1f;
						string result;
						if ((double)Math.Abs(num) > 0.001)
						{
							result = num.ToStringPercent();
						}
						else
						{
							refreshPawn();
							BodyPartRecord bodyPartRecord = getApplicationPoint(r);
							pawn = pawn;
							DamageDef executionCut = DamageDefOf.ExecutionCut;
							float amount = pawn.health.hediffSet.GetPartHealth(bodyPartRecord) / 2f;
							float armorPenetration = 999f;
							BodyPartRecord hitPart = bodyPartRecord;
							pawn.TakeDamage(new DamageInfo(executionCut, amount, armorPenetration, -1f, null, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
							List<PawnCapacityUtility.CapacityImpactor> list2 = new List<PawnCapacityUtility.CapacityImpactor>();
							PawnCapacityUtility.CalculateCapacityLevel(pawn.health.hediffSet, cap, list2);
							if (list2.Any((PawnCapacityUtility.CapacityImpactor imp) => imp.IsDirect))
							{
								result = 0f.ToStringPercent();
							}
							else
							{
								result = "";
							}
						}
						return result;
					}));
				}
			}
			list.Add(new TableDataGetter<RecipeDef>("tech level", (RecipeDef r) => (getProstheticItem(r) != null) ? getProstheticItem(r).techLevel.ToStringHuman() : ""));
			list.Add(new TableDataGetter<RecipeDef>("thingSetMakerTags", (RecipeDef r) => (getProstheticItem(r) != null) ? getProstheticItem(r).thingSetMakerTags.ToCommaList(false) : ""));
			list.Add(new TableDataGetter<RecipeDef>("techHediffsTags", (RecipeDef r) => (getProstheticItem(r) != null) ? getProstheticItem(r).techHediffsTags.ToCommaList(false) : ""));
			DebugTables.MakeTablesDialog<RecipeDef>(from r in ThingDefOf.Human.AllRecipes
			where r.workerClass == typeof(Recipe_InstallArtificialBodyPart) || r.workerClass == typeof(Recipe_InstallNaturalBodyPart)
			select r, list.ToArray());
			Messages.Clear();
		}

		[DebugOutput]
		public static void JoyGivers()
		{
			IEnumerable<JoyGiverDef> allDefs = DefDatabase<JoyGiverDef>.AllDefs;
			TableDataGetter<JoyGiverDef>[] array = new TableDataGetter<JoyGiverDef>[11];
			array[0] = new TableDataGetter<JoyGiverDef>("defName", (JoyGiverDef d) => d.defName);
			array[1] = new TableDataGetter<JoyGiverDef>("joyKind", (JoyGiverDef d) => (d.joyKind != null) ? d.joyKind.defName : "null");
			array[2] = new TableDataGetter<JoyGiverDef>("baseChance", (JoyGiverDef d) => d.baseChance.ToString());
			array[3] = new TableDataGetter<JoyGiverDef>("canDoWhileInBed", (JoyGiverDef d) => d.canDoWhileInBed.ToStringCheckBlank());
			array[4] = new TableDataGetter<JoyGiverDef>("desireSit", (JoyGiverDef d) => d.desireSit.ToStringCheckBlank());
			array[5] = new TableDataGetter<JoyGiverDef>("unroofedOnly", (JoyGiverDef d) => d.unroofedOnly.ToStringCheckBlank());
			array[6] = new TableDataGetter<JoyGiverDef>("jobDef", (JoyGiverDef d) => (d.jobDef != null) ? d.jobDef.defName : "null");
			array[7] = new TableDataGetter<JoyGiverDef>("pctPawnsEverDo", (JoyGiverDef d) => d.pctPawnsEverDo.ToStringPercent());
			array[8] = new TableDataGetter<JoyGiverDef>("requiredCapacities", delegate(JoyGiverDef d)
			{
				string result;
				if (d.requiredCapacities == null)
				{
					result = "";
				}
				else
				{
					result = (from c in d.requiredCapacities
					select c.defName).ToCommaList(false);
				}
				return result;
			});
			array[9] = new TableDataGetter<JoyGiverDef>("thingDefs", delegate(JoyGiverDef d)
			{
				string result;
				if (d.thingDefs == null)
				{
					result = "";
				}
				else
				{
					result = (from c in d.thingDefs
					select c.defName).ToCommaList(false);
				}
				return result;
			});
			array[10] = new TableDataGetter<JoyGiverDef>("JoyGainFactors", delegate(JoyGiverDef d)
			{
				string result;
				if (d.thingDefs == null)
				{
					result = "";
				}
				else
				{
					result = (from c in d.thingDefs
					select c.GetStatValueAbstract(StatDefOf.JoyGainFactor, null).ToString("F2")).ToCommaList(false);
				}
				return result;
			});
			DebugTables.MakeTablesDialog<JoyGiverDef>(allDefs, array);
		}

		[DebugOutput]
		public static void JoyJobs()
		{
			IEnumerable<JobDef> dataSources = from j in DefDatabase<JobDef>.AllDefs
			where j.joyKind != null
			select j;
			TableDataGetter<JobDef>[] array = new TableDataGetter<JobDef>[7];
			array[0] = new TableDataGetter<JobDef>("defName", (JobDef d) => d.defName);
			array[1] = new TableDataGetter<JobDef>("joyKind", (JobDef d) => d.joyKind.defName);
			array[2] = new TableDataGetter<JobDef>("joyDuration", (JobDef d) => d.joyDuration.ToString());
			array[3] = new TableDataGetter<JobDef>("joyGainRate", (JobDef d) => d.joyGainRate.ToString());
			array[4] = new TableDataGetter<JobDef>("joyMaxParticipants", (JobDef d) => d.joyMaxParticipants.ToString());
			array[5] = new TableDataGetter<JobDef>("joySkill", (JobDef d) => (d.joySkill == null) ? "" : d.joySkill.defName);
			array[6] = new TableDataGetter<JobDef>("joyXpPerTick", (JobDef d) => d.joyXpPerTick.ToString());
			DebugTables.MakeTablesDialog<JobDef>(dataSources, array);
		}

		[DebugOutput]
		public static void Thoughts()
		{
			Func<ThoughtDef, string> stagesText = delegate(ThoughtDef t)
			{
				string text = "";
				string result;
				if (t.stages == null)
				{
					result = null;
				}
				else
				{
					for (int i = 0; i < t.stages.Count; i++)
					{
						ThoughtStage thoughtStage = t.stages[i];
						string text2 = text;
						text = string.Concat(new object[]
						{
							text2,
							"[",
							i,
							"] "
						});
						if (thoughtStage == null)
						{
							text += "null";
						}
						else
						{
							if (thoughtStage.label != null)
							{
								text += thoughtStage.label;
							}
							if (thoughtStage.labelSocial != null)
							{
								if (thoughtStage.label != null)
								{
									text += "/";
								}
								text += thoughtStage.labelSocial;
							}
							text += " ";
							if (thoughtStage.baseMoodEffect != 0f)
							{
								text = text + "[" + thoughtStage.baseMoodEffect.ToStringWithSign("0.##") + " Mo]";
							}
							if (thoughtStage.baseOpinionOffset != 0f)
							{
								text = text + "(" + thoughtStage.baseOpinionOffset.ToStringWithSign("0.##") + " Op)";
							}
						}
						if (i < t.stages.Count - 1)
						{
							text += "\n";
						}
					}
					result = text;
				}
				return result;
			};
			IEnumerable<ThoughtDef> allDefs = DefDatabase<ThoughtDef>.AllDefs;
			TableDataGetter<ThoughtDef>[] array = new TableDataGetter<ThoughtDef>[18];
			array[0] = new TableDataGetter<ThoughtDef>("defName", (ThoughtDef d) => d.defName);
			array[1] = new TableDataGetter<ThoughtDef>("type", (ThoughtDef d) => (!d.IsMemory) ? "situ" : "mem");
			array[2] = new TableDataGetter<ThoughtDef>("social", (ThoughtDef d) => (!d.IsSocial) ? "mood" : "soc");
			array[3] = new TableDataGetter<ThoughtDef>("stages", (ThoughtDef d) => stagesText(d));
			array[4] = new TableDataGetter<ThoughtDef>("best\nmood", (ThoughtDef d) => (from st in d.stages
			where st != null
			select st).Max((ThoughtStage st) => st.baseMoodEffect));
			array[5] = new TableDataGetter<ThoughtDef>("worst\nmood", (ThoughtDef d) => (from st in d.stages
			where st != null
			select st).Min((ThoughtStage st) => st.baseMoodEffect));
			array[6] = new TableDataGetter<ThoughtDef>("stack\nlimit", (ThoughtDef d) => d.stackLimit.ToString());
			array[7] = new TableDataGetter<ThoughtDef>("stack\nlimit\nper o. pawn", (ThoughtDef d) => (d.stackLimitForSameOtherPawn >= 0) ? d.stackLimitForSameOtherPawn.ToString() : "");
			array[8] = new TableDataGetter<ThoughtDef>("stacked\neffect\nmultiplier", (ThoughtDef d) => (d.stackLimit != 1) ? d.stackedEffectMultiplier.ToStringPercent() : "");
			array[9] = new TableDataGetter<ThoughtDef>("duration\n(days)", (ThoughtDef d) => d.durationDays.ToString());
			array[10] = new TableDataGetter<ThoughtDef>("effect\nmultiplying\nstat", (ThoughtDef d) => (d.effectMultiplyingStat != null) ? d.effectMultiplyingStat.defName : "");
			array[11] = new TableDataGetter<ThoughtDef>("game\ncondition", (ThoughtDef d) => (d.gameCondition != null) ? d.gameCondition.defName : "");
			array[12] = new TableDataGetter<ThoughtDef>("hediff", (ThoughtDef d) => (d.hediff != null) ? d.hediff.defName : "");
			array[13] = new TableDataGetter<ThoughtDef>("lerp opinion\nto zero\nafter duration pct", (ThoughtDef d) => d.lerpOpinionToZeroAfterDurationPct.ToStringPercent());
			array[14] = new TableDataGetter<ThoughtDef>("max cumulated\nopinion\noffset", (ThoughtDef d) => (d.maxCumulatedOpinionOffset <= 99999f) ? d.maxCumulatedOpinionOffset.ToString() : "");
			array[15] = new TableDataGetter<ThoughtDef>("next\nthought", (ThoughtDef d) => (d.nextThought != null) ? d.nextThought.defName : "");
			array[16] = new TableDataGetter<ThoughtDef>("nullified\nif not colonist", (ThoughtDef d) => d.nullifiedIfNotColonist.ToStringCheckBlank());
			array[17] = new TableDataGetter<ThoughtDef>("show\nbubble", (ThoughtDef d) => d.showBubble.ToStringCheckBlank());
			DebugTables.MakeTablesDialog<ThoughtDef>(allDefs, array);
		}

		[DebugOutput]
		public static void GenSteps()
		{
			IEnumerable<GenStepDef> dataSources = from x in DefDatabase<GenStepDef>.AllDefsListForReading
			orderby x.order, x.index
			select x;
			TableDataGetter<GenStepDef>[] array = new TableDataGetter<GenStepDef>[4];
			array[0] = new TableDataGetter<GenStepDef>("defName", (GenStepDef x) => x.defName);
			array[1] = new TableDataGetter<GenStepDef>("order", (GenStepDef x) => x.order.ToString("0.##"));
			array[2] = new TableDataGetter<GenStepDef>("class", (GenStepDef x) => x.genStep.GetType().Name);
			array[3] = new TableDataGetter<GenStepDef>("site", (GenStepDef x) => (x.linkWithSite == null) ? "" : x.linkWithSite.defName);
			DebugTables.MakeTablesDialog<GenStepDef>(dataSources, array);
		}

		[DebugOutput]
		public static void WorldGenSteps()
		{
			IEnumerable<WorldGenStepDef> dataSources = from x in DefDatabase<WorldGenStepDef>.AllDefsListForReading
			orderby x.order, x.index
			select x;
			TableDataGetter<WorldGenStepDef>[] array = new TableDataGetter<WorldGenStepDef>[3];
			array[0] = new TableDataGetter<WorldGenStepDef>("defName", (WorldGenStepDef x) => x.defName);
			array[1] = new TableDataGetter<WorldGenStepDef>("order", (WorldGenStepDef x) => x.order.ToString("0.##"));
			array[2] = new TableDataGetter<WorldGenStepDef>("class", (WorldGenStepDef x) => x.worldGenStep.GetType().Name);
			DebugTables.MakeTablesDialog<WorldGenStepDef>(dataSources, array);
		}

		[CompilerGenerated]
		private static ThingDef <MiningResourceGeneration>m__0(ThingDef d)
		{
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].building != null && allDefsListForReading[i].building.mineableThing == d)
				{
					return allDefsListForReading[i];
				}
			}
			return null;
		}

		[CompilerGenerated]
		private static string <MiningResourceGeneration>m__1(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <MiningResourceGeneration>m__2(ThingDef d)
		{
			return d.BaseMarketValue.ToString("F2");
		}

		[CompilerGenerated]
		private static int <MiningResourceGeneration>m__3(ThingDef d)
		{
			return d.stackLimit;
		}

		[CompilerGenerated]
		private static string <MiningResourceGeneration>m__4(ThingDef d)
		{
			return d.deepCommonality.ToString("F2");
		}

		[CompilerGenerated]
		private static object <MiningResourceGeneration>m__5(ThingDef d)
		{
			return d.deepLumpSizeRange;
		}

		[CompilerGenerated]
		private static int <MiningResourceGeneration>m__6(ThingDef d)
		{
			return d.deepCountPerCell;
		}

		[CompilerGenerated]
		private static int <MiningResourceGeneration>m__7(ThingDef d)
		{
			return d.deepCountPerPortion;
		}

		[CompilerGenerated]
		private static string <MiningResourceGeneration>m__8(ThingDef d)
		{
			return ((float)d.deepCountPerPortion * d.BaseMarketValue).ToStringMoney(null);
		}

		[CompilerGenerated]
		private static bool <DefaultStuffs>m__9(ThingDef d)
		{
			return d.MadeFromStuff && !d.IsFrame;
		}

		[CompilerGenerated]
		private static string <DefaultStuffs>m__A(ThingDef d)
		{
			return d.category.ToString();
		}

		[CompilerGenerated]
		private static string <DefaultStuffs>m__B(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <DefaultStuffs>m__C(ThingDef d)
		{
			return GenStuff.DefaultStuffFor(d).defName;
		}

		[CompilerGenerated]
		private static string <DefaultStuffs>m__D(ThingDef d)
		{
			return (from c in d.stuffCategories
			select c.defName).ToCommaList(false);
		}

		[CompilerGenerated]
		private static bool <Beauties>m__E(BuildableDef d)
		{
			ThingDef thingDef = d as ThingDef;
			bool result;
			if (thingDef != null)
			{
				result = BeautyUtility.BeautyRelevant(thingDef.category);
			}
			else
			{
				result = (d is TerrainDef);
			}
			return result;
		}

		[CompilerGenerated]
		private static int <Beauties>m__F(BuildableDef d)
		{
			return (int)d.GetStatValueAbstract(StatDefOf.Beauty, null);
		}

		[CompilerGenerated]
		private static string <Beauties>m__10(BuildableDef d)
		{
			return (!(d is ThingDef)) ? "Terrain" : ((ThingDef)d).category.ToString();
		}

		[CompilerGenerated]
		private static string <Beauties>m__11(BuildableDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <Beauties>m__12(BuildableDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.Beauty, null).ToString();
		}

		[CompilerGenerated]
		private static string <Beauties>m__13(BuildableDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.MarketValue, null).ToString("F1");
		}

		[CompilerGenerated]
		private static string <Beauties>m__14(BuildableDef d)
		{
			return DebugOutputsEconomy.WorkToProduceBest(d).ToString();
		}

		[CompilerGenerated]
		private static string <Beauties>m__15(BuildableDef d)
		{
			return (d.GetStatValueAbstract(StatDefOf.Beauty, null) <= 0f) ? "" : (d.GetStatValueAbstract(StatDefOf.Beauty, null) / d.GetStatValueAbstract(StatDefOf.MarketValue, null)).ToString("F5");
		}

		[CompilerGenerated]
		private static CompProperties_HeatPusher <ThingsPowerAndHeat>m__16(ThingDef d)
		{
			CompProperties_HeatPusher result;
			if (d.comps == null)
			{
				result = null;
			}
			else
			{
				for (int i = 0; i < d.comps.Count; i++)
				{
					CompProperties_HeatPusher compProperties_HeatPusher = d.comps[i] as CompProperties_HeatPusher;
					if (compProperties_HeatPusher != null)
					{
						return compProperties_HeatPusher;
					}
				}
				result = null;
			}
			return result;
		}

		[CompilerGenerated]
		private static string <ThingsPowerAndHeat>m__17(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <ThingsPowerAndHeat>m__18(ThingDef d)
		{
			return (d.GetCompProperties<CompProperties_Power>() != null) ? d.GetCompProperties<CompProperties_Power>().basePowerConsumption.ToString() : "";
		}

		[CompilerGenerated]
		private static string <ThingsPowerAndHeat>m__19(ThingDef d)
		{
			return (d.GetCompProperties<CompProperties_Power>() != null) ? ((!d.GetCompProperties<CompProperties_Power>().shortCircuitInRain) ? "" : "rainfire") : "";
		}

		[CompilerGenerated]
		private static string <ThingsPowerAndHeat>m__1A(ThingDef d)
		{
			return (d.GetCompProperties<CompProperties_Power>() != null) ? ((!d.GetCompProperties<CompProperties_Power>().transmitsPower) ? "" : "transmit") : "";
		}

		[CompilerGenerated]
		private static float <ThingsPowerAndHeat>m__1B(ThingDef d)
		{
			return d.BaseMarketValue;
		}

		[CompilerGenerated]
		private static string <ThingsPowerAndHeat>m__1C(ThingDef d)
		{
			return DebugOutputsEconomy.CostListString(d, true, false);
		}

		[CompilerGenerated]
		private static bool <FoodPoisonChances>m__1D(ThingDef d)
		{
			return d.IsIngestible;
		}

		[CompilerGenerated]
		private static object <FoodPoisonChances>m__1E(ThingDef d)
		{
			return d.category;
		}

		[CompilerGenerated]
		private static string <FoodPoisonChances>m__1F(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <FoodPoisonChances>m__20(ThingDef d)
		{
			CompProperties_FoodPoisonable compProperties = d.GetCompProperties<CompProperties_FoodPoisonable>();
			string result;
			if (compProperties != null)
			{
				result = "poisonable by cook";
			}
			else
			{
				float statValueAbstract = d.GetStatValueAbstract(StatDefOf.FoodPoisonChanceFixedHuman, null);
				if (statValueAbstract != 0f)
				{
					result = statValueAbstract.ToStringPercent();
				}
				else
				{
					result = "";
				}
			}
			return result;
		}

		[CompilerGenerated]
		private static bool <TechLevels>m__21(ThingDef d)
		{
			return d.category == ThingCategory.Building || d.category == ThingCategory.Item;
		}

		[CompilerGenerated]
		private static bool <TechLevels>m__22(ThingDef d)
		{
			return !d.IsFrame && (d.building == null || !d.building.isNaturalRock);
		}

		[CompilerGenerated]
		private static int <TechLevels>m__23(ThingDef d)
		{
			return (int)d.techLevel;
		}

		[CompilerGenerated]
		private static string <TechLevels>m__24(ThingDef d)
		{
			return d.category.ToString();
		}

		[CompilerGenerated]
		private static string <TechLevels>m__25(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <TechLevels>m__26(ThingDef d)
		{
			return d.techLevel.ToString();
		}

		[CompilerGenerated]
		private static string <Stuffs>m__27(ThingDef d, StatDef stat)
		{
			string result;
			if (d.stuffProps.statFactors == null)
			{
				result = "";
			}
			else
			{
				StatModifier statModifier = d.stuffProps.statFactors.FirstOrDefault((StatModifier fa) => fa.stat == stat);
				if (statModifier == null)
				{
					result = "";
				}
				else
				{
					result = stat.ValueToString(statModifier.value, ToStringNumberSense.Absolute);
				}
			}
			return result;
		}

		[CompilerGenerated]
		private static float <Stuffs>m__28(ThingDef d)
		{
			float statValueAbstract = d.GetStatValueAbstract(StatDefOf.SharpDamageMultiplier, null);
			float statFactorFromList = d.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.MeleeWeapon_CooldownMultiplier);
			return statValueAbstract / statFactorFromList;
		}

		[CompilerGenerated]
		private static float <Stuffs>m__29(ThingDef d)
		{
			float statValueAbstract = d.GetStatValueAbstract(StatDefOf.BluntDamageMultiplier, null);
			float statFactorFromList = d.stuffProps.statFactors.GetStatFactorFromList(StatDefOf.MeleeWeapon_CooldownMultiplier);
			return statValueAbstract / statFactorFromList;
		}

		[CompilerGenerated]
		private static bool <Stuffs>m__2A(ThingDef d)
		{
			return d.IsStuff;
		}

		[CompilerGenerated]
		private static float <Stuffs>m__2B(ThingDef d)
		{
			return d.BaseMarketValue;
		}

		[CompilerGenerated]
		private static string <Stuffs>m__2C(ThingDef d)
		{
			return d.stuffProps.categories.Contains(StuffCategoryDefOf.Fabric).ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <Stuffs>m__2D(ThingDef d)
		{
			return d.stuffProps.categories.Contains(StuffCategoryDefOf.Leathery).ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <Stuffs>m__2E(ThingDef d)
		{
			return d.stuffProps.categories.Contains(StuffCategoryDefOf.Metallic).ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <Stuffs>m__2F(ThingDef d)
		{
			return d.stuffProps.categories.Contains(StuffCategoryDefOf.Stony).ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <Stuffs>m__30(ThingDef d)
		{
			return d.stuffProps.categories.Contains(StuffCategoryDefOf.Woody).ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <Stuffs>m__31(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <Stuffs>m__32(ThingDef d)
		{
			return d.BaseMarketValue.ToStringMoney(null);
		}

		[CompilerGenerated]
		private static string <Stuffs>m__33(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.SharpDamageMultiplier, null).ToString("F2");
		}

		[CompilerGenerated]
		private static string <Stuffs>m__34(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.BluntDamageMultiplier, null).ToString("F2");
		}

		[CompilerGenerated]
		private static string <Stuffs>m__35(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Sharp, null).ToString("F2");
		}

		[CompilerGenerated]
		private static string <Stuffs>m__36(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Blunt, null).ToString("F2");
		}

		[CompilerGenerated]
		private static string <Stuffs>m__37(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.StuffPower_Armor_Heat, null).ToString("F2");
		}

		[CompilerGenerated]
		private static string <Stuffs>m__38(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.StuffPower_Insulation_Cold, null).ToString("F2");
		}

		[CompilerGenerated]
		private static string <Stuffs>m__39(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.StuffPower_Insulation_Heat, null).ToString("F2");
		}

		[CompilerGenerated]
		private static string <Stuffs>m__3A(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.Flammability, null).ToString("F2");
		}

		[CompilerGenerated]
		private static bool <Drugs>m__3B(ThingDef d)
		{
			return d.IsDrug;
		}

		[CompilerGenerated]
		private static string <Drugs>m__3C(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <Drugs>m__3D(ThingDef d)
		{
			return (!d.IsPleasureDrug) ? "" : "pleasure";
		}

		[CompilerGenerated]
		private static string <Drugs>m__3E(ThingDef d)
		{
			return (!d.IsNonMedicalDrug) ? "" : "non-medical";
		}

		[CompilerGenerated]
		private static bool <Medicines>m__3F(ThingDef d)
		{
			return typeof(Medicine).IsAssignableFrom(d.thingClass);
		}

		[CompilerGenerated]
		private static float <Medicines>m__40(ThingDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.MedicalPotency, null);
		}

		[CompilerGenerated]
		private static string <Medicines>m__41(float p)
		{
			return p.ToStringPercent();
		}

		[CompilerGenerated]
		private static string <ShootingAccuracy>m__42(int lev)
		{
			return lev.ToString();
		}

		[CompilerGenerated]
		private static string <ShootingAccuracy>m__43(int lev)
		{
			return lev.ToString();
		}

		[CompilerGenerated]
		private static string <ShootingAccuracy>m__44(int lev)
		{
			return lev.ToString();
		}

		[CompilerGenerated]
		private static BodyPartDef <DamageTest>m__45(Hediff_MissingPart hd)
		{
			return hd.Part.def;
		}

		[CompilerGenerated]
		private static BodyPartDef <DamageTest>m__46(Hediff_MissingPart hd)
		{
			return hd.Part.def;
		}

		[CompilerGenerated]
		private static IEnumerable<string> <ListSolidBackstories>m__47(PawnBio bio)
		{
			return bio.adulthood.spawnCategories;
		}

		[CompilerGenerated]
		private static string <ThingSetMakerPossibleDefs>m__48(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <ThingSetMakerPossibleDefs>m__49(ThingDef d)
		{
			return d.BaseMarketValue.ToStringMoney(null);
		}

		[CompilerGenerated]
		private static string <ThingSetMakerPossibleDefs>m__4A(ThingDef d)
		{
			return d.BaseMass.ToStringMass();
		}

		[CompilerGenerated]
		private static bool <ThingSetMakerPossibleDefs>m__4B(ThingDef d)
		{
			return (d.category == ThingCategory.Item && !d.IsCorpse && !d.isUnfinishedThing) || (d.category == ThingCategory.Building && d.Minifiable) || d.category == ThingCategory.Pawn;
		}

		[CompilerGenerated]
		private static float <ThingSetMakerPossibleDefs>m__4C(ThingDef d)
		{
			return d.BaseMarketValue;
		}

		[CompilerGenerated]
		private static bool <WorkDisables>m__4D(PawnKindDef ki)
		{
			return ki.RaceProps.Humanlike;
		}

		[CompilerGenerated]
		private static bool <FoodPreferability>m__4E(ThingDef td)
		{
			return td.ingestible != null;
		}

		[CompilerGenerated]
		private static FoodPreferability <FoodPreferability>m__4F(ThingDef td)
		{
			return td.ingestible.preferability;
		}

		[CompilerGenerated]
		private static string <DefNames>m__50(Type def)
		{
			return def.Name;
		}

		[CompilerGenerated]
		private static string <DefNamesAll>m__51(Type def)
		{
			return def.Name;
		}

		[CompilerGenerated]
		private static string <DefNamesAll>m__52(Def def)
		{
			return def.defName;
		}

		[CompilerGenerated]
		private static string <DefLabels>m__53(Type def)
		{
			return def.Name;
		}

		[CompilerGenerated]
		private static string <BodyParts>m__54(BodyPartDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static int <BodyParts>m__55(BodyPartDef d)
		{
			return d.hitPoints;
		}

		[CompilerGenerated]
		private static string <BodyParts>m__56(BodyPartDef d)
		{
			return d.bleedRate.ToStringPercent();
		}

		[CompilerGenerated]
		private static string <BodyParts>m__57(BodyPartDef d)
		{
			return d.permanentInjuryChanceFactor.ToStringPercent();
		}

		[CompilerGenerated]
		private static float <BodyParts>m__58(BodyPartDef d)
		{
			return d.frostbiteVulnerability;
		}

		[CompilerGenerated]
		private static string <BodyParts>m__59(BodyPartDef d)
		{
			return (!d.IsSolidInDefinition_Debug) ? "" : "S";
		}

		[CompilerGenerated]
		private static string <BodyParts>m__5A(BodyPartDef d)
		{
			return (!d.beautyRelated) ? "" : "B";
		}

		[CompilerGenerated]
		private static string <BodyParts>m__5B(BodyPartDef d)
		{
			return (!d.alive) ? "" : "A";
		}

		[CompilerGenerated]
		private static string <BodyParts>m__5C(BodyPartDef d)
		{
			return (!d.conceptual) ? "" : "C";
		}

		[CompilerGenerated]
		private static string <BodyParts>m__5D(BodyPartDef d)
		{
			return (!d.canSuggestAmputation) ? "no A" : "";
		}

		[CompilerGenerated]
		private static string <BodyParts>m__5E(BodyPartDef d)
		{
			return (!d.socketed) ? "" : "DoL";
		}

		[CompilerGenerated]
		private static string <BodyParts>m__5F(BodyPartDef d)
		{
			return (!d.IsSkinCoveredInDefinition_Debug) ? "" : "skin";
		}

		[CompilerGenerated]
		private static string <BodyParts>m__60(BodyPartDef d)
		{
			return (!d.pawnGeneratorCanAmputate) ? "" : "amp";
		}

		[CompilerGenerated]
		private static ThingDef <BodyParts>m__61(BodyPartDef d)
		{
			return d.spawnThingOnRemoved;
		}

		[CompilerGenerated]
		private static string <BodyParts>m__62(BodyPartDef d)
		{
			string result;
			if (d.hitChanceFactors == null)
			{
				result = "";
			}
			else
			{
				result = (from kvp in d.hitChanceFactors
				select kvp.ToString()).ToCommaList(false);
			}
			return result;
		}

		[CompilerGenerated]
		private static string <BodyParts>m__63(BodyPartDef d)
		{
			string result;
			if (d.tags == null)
			{
				result = "";
			}
			else
			{
				result = (from t in d.tags
				select t.defName).ToCommaList(false);
			}
			return result;
		}

		[CompilerGenerated]
		private static string <TraderKinds>m__64(TraderKindDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <TraderKinds>m__65(TraderKindDef d)
		{
			return d.CalculatedCommonality.ToString("F2");
		}

		[CompilerGenerated]
		private static string <TraderKindThings>m__66(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static bool <TraderKindThings>m__67(ThingDef d)
		{
			return (d.category == ThingCategory.Item && d.BaseMarketValue > 0.001f && !d.isUnfinishedThing && !d.IsCorpse && !d.destroyOnDrop && d != ThingDefOf.Silver && !d.thingCategories.NullOrEmpty<ThingCategoryDef>()) || (d.category == ThingCategory.Building && d.Minifiable) || d.category == ThingCategory.Pawn;
		}

		[CompilerGenerated]
		private static string <TraderKindThings>m__68(ThingDef d)
		{
			return d.thingCategories.NullOrEmpty<ThingCategoryDef>() ? "zzzzzzz" : d.thingCategories[0].defName;
		}

		[CompilerGenerated]
		private static float <TraderKindThings>m__69(ThingDef d)
		{
			return d.BaseMarketValue;
		}

		[CompilerGenerated]
		private static bool <Surgeries>m__6A(RecipeDef d)
		{
			return d.IsSurgery;
		}

		[CompilerGenerated]
		private static float <Surgeries>m__6B(RecipeDef d)
		{
			return d.WorkAmountTotal(null);
		}

		[CompilerGenerated]
		private static string <Surgeries>m__6C(RecipeDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <Surgeries>m__6D(RecipeDef d)
		{
			return d.WorkAmountTotal(null).ToString("F0");
		}

		[CompilerGenerated]
		private static string <Surgeries>m__6E(RecipeDef d)
		{
			return (from ing in d.ingredients
			select ing.ToString()).ToCommaList(false);
		}

		[CompilerGenerated]
		private static string <Surgeries>m__6F(RecipeDef d)
		{
			string result;
			if (d.skillRequirements == null)
			{
				result = "-";
			}
			else
			{
				result = (from ing in d.skillRequirements
				select ing.ToString()).ToCommaList(false);
			}
			return result;
		}

		[CompilerGenerated]
		private static string <Surgeries>m__70(RecipeDef d)
		{
			return d.surgerySuccessChanceFactor.ToStringPercent();
		}

		[CompilerGenerated]
		private static string <Surgeries>m__71(RecipeDef d)
		{
			return d.deathOnFailedSurgeryChance.ToStringPercent();
		}

		[CompilerGenerated]
		private static bool <HitsToKill>m__72(ThingDef d)
		{
			return d.race != null;
		}

		[CompilerGenerated]
		private static <>__AnonType1<ThingDef, float, int> <HitsToKill>m__73(ThingDef x)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < 15; i++)
			{
				PawnGenerationRequest request = new PawnGenerationRequest(x.race.AnyPawnKind, null, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, false, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
				Pawn pawn = PawnGenerator.GeneratePawn(request);
				for (int j = 0; j < 1000; j++)
				{
					pawn.TakeDamage(new DamageInfo(DamageDefOf.Crush, 10f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
					if (pawn.Destroyed)
					{
						num += j + 1;
						break;
					}
				}
				if (!pawn.Destroyed)
				{
					Log.Error("Could not kill pawn " + pawn.ToStringSafe<Pawn>(), false);
				}
				if (pawn.health.ShouldBeDeadFromLethalDamageThreshold())
				{
					num2++;
				}
				if (Find.WorldPawns.Contains(pawn))
				{
					Find.WorldPawns.RemovePawn(pawn);
				}
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			}
			float hits = (float)num / 15f;
			return new
			{
				Race = x,
				Hits = hits,
				DiedDueToDamageThreshold = num2
			};
		}

		[CompilerGenerated]
		private static ThingDef <HitsToKill>m__74(<>__AnonType1<ThingDef, float, int> x)
		{
			return x.Race;
		}

		[CompilerGenerated]
		private static bool <HitsToKill>m__75(ThingDef d)
		{
			return d.race != null;
		}

		[CompilerGenerated]
		private static float <HitsToKill>m__76(ThingDef d)
		{
			return d.race.baseHealthScale;
		}

		[CompilerGenerated]
		private static string <HitsToKill>m__77(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <HitsToKill>m__78(ThingDef d)
		{
			return (!d.race.IsMechanoid) ? "" : "mech";
		}

		[CompilerGenerated]
		private static string <Terrains>m__79(TerrainDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <Terrains>m__7A(TerrainDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.WorkToBuild, null).ToString();
		}

		[CompilerGenerated]
		private static string <Terrains>m__7B(TerrainDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.Beauty, null).ToString();
		}

		[CompilerGenerated]
		private static string <Terrains>m__7C(TerrainDef d)
		{
			return d.GetStatValueAbstract(StatDefOf.Cleanliness, null).ToString();
		}

		[CompilerGenerated]
		private static string <Terrains>m__7D(TerrainDef d)
		{
			return d.pathCost.ToString();
		}

		[CompilerGenerated]
		private static string <Terrains>m__7E(TerrainDef d)
		{
			return d.fertility.ToStringPercentEmptyZero("F0");
		}

		[CompilerGenerated]
		private static string <Terrains>m__7F(TerrainDef d)
		{
			return d.acceptFilth.ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <Terrains>m__80(TerrainDef d)
		{
			return d.acceptTerrainSourceFilth.ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <Terrains>m__81(TerrainDef d)
		{
			return (d.generatedFilth == null) ? "" : d.generatedFilth.defName;
		}

		[CompilerGenerated]
		private static string <Terrains>m__82(TerrainDef d)
		{
			return d.holdSnow.ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <Terrains>m__83(TerrainDef d)
		{
			return d.takeFootprints.ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <Terrains>m__84(TerrainDef d)
		{
			return d.avoidWander.ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <Terrains>m__85(TerrainDef d)
		{
			return d.BuildableByPlayer.ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <Terrains>m__86(TerrainDef d)
		{
			return DebugOutputsEconomy.CostListString(d, false, false);
		}

		[CompilerGenerated]
		private static string <Terrains>m__87(TerrainDef d)
		{
			string result;
			if (d.researchPrerequisites != null)
			{
				result = (from pr in d.researchPrerequisites
				select pr.defName).ToCommaList(false);
			}
			else
			{
				result = "";
			}
			return result;
		}

		[CompilerGenerated]
		private static string <Terrains>m__88(TerrainDef d)
		{
			return (from af in d.affordances
			select af.defName).ToCommaList(false);
		}

		[CompilerGenerated]
		private static bool <TerrainAffordances>m__89(ThingDef d)
		{
			return d.category == ThingCategory.Building && !d.IsFrame;
		}

		[CompilerGenerated]
		private static string <TerrainAffordances>m__8A(BuildableDef d)
		{
			return (!(d is TerrainDef)) ? "building" : "terrain";
		}

		[CompilerGenerated]
		private static string <TerrainAffordances>m__8B(BuildableDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <TerrainAffordances>m__8C(BuildableDef d)
		{
			return (d.terrainAffordanceNeeded == null) ? "" : d.terrainAffordanceNeeded.defName;
		}

		[CompilerGenerated]
		private static MentalBreakIntensity <MentalBreaks>m__8D(MentalBreakDef d)
		{
			return d.intensity;
		}

		[CompilerGenerated]
		private static string <MentalBreaks>m__8E(MentalBreakDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <MentalBreaks>m__8F(MentalBreakDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <MentalBreaks>m__90(MentalBreakDef d)
		{
			return d.intensity.ToString();
		}

		[CompilerGenerated]
		private static string <MentalBreaks>m__91(MentalBreakDef d)
		{
			return (d.baseCommonality / (from x in DefDatabase<MentalBreakDef>.AllDefs
			where x.intensity == d.intensity
			select x).Sum((MentalBreakDef x) => x.baseCommonality)).ToStringPercent();
		}

		[CompilerGenerated]
		private static string <MentalBreaks>m__92(MentalBreakDef d)
		{
			return (d.mentalState != null) ? ((float)d.mentalState.minTicksBeforeRecovery / 60000f).ToString("0.##") : "";
		}

		[CompilerGenerated]
		private static string <MentalBreaks>m__93(MentalBreakDef d)
		{
			return (d.mentalState != null) ? (Mathf.Min((float)d.mentalState.minTicksBeforeRecovery + d.mentalState.recoveryMtbDays * 60000f, (float)d.mentalState.maxTicksBeforeRecovery) / 60000f).ToString("0.##") : "";
		}

		[CompilerGenerated]
		private static string <MentalBreaks>m__94(MentalBreakDef d)
		{
			return (d.mentalState != null) ? ((float)d.mentalState.maxTicksBeforeRecovery / 60000f).ToString("0.##") : "";
		}

		[CompilerGenerated]
		private static string <MentalBreaks>m__95(MentalBreakDef d)
		{
			return (d.mentalState == null || !d.mentalState.recoverFromSleep) ? "" : "recoverFromSleep";
		}

		[CompilerGenerated]
		private static string <MentalBreaks>m__96(MentalBreakDef d)
		{
			return (d.mentalState != null) ? d.mentalState.moodRecoveryThought.ToStringSafe<ThoughtDef>() : "";
		}

		[CompilerGenerated]
		private static string <MentalBreaks>m__97(MentalBreakDef d)
		{
			return (d.mentalState == null) ? "" : d.mentalState.category.ToString();
		}

		[CompilerGenerated]
		private static string <MentalBreaks>m__98(MentalBreakDef d)
		{
			return (d.mentalState == null || !d.mentalState.blockNormalThoughts) ? "" : "blockNormalThoughts";
		}

		[CompilerGenerated]
		private static string <MentalBreaks>m__99(MentalBreakDef d)
		{
			return (d.mentalState == null || !d.mentalState.allowBeatfire) ? "" : "allowBeatfire";
		}

		[CompilerGenerated]
		private static TraitDef <TraitsSampled>m__9A(TraitDegreeData d)
		{
			return DefDatabase<TraitDef>.AllDefs.First((TraitDef td) => td.degreeDatas.Contains(d));
		}

		[CompilerGenerated]
		private static IEnumerable<TraitDegreeData> <TraitsSampled>m__9B(TraitDef tr)
		{
			return tr.degreeDatas;
		}

		[CompilerGenerated]
		private static string <TraitsSampled>m__9C(TraitDegreeData d)
		{
			return d.label;
		}

		[CompilerGenerated]
		private static string <TraitsSampled>m__9D(TraitDegreeData d)
		{
			return d.marketValueFactorOffset.ToString("F0");
		}

		[CompilerGenerated]
		private static bool <BestThingRequestGroup>m__9E(ThingDef x)
		{
			return ListerThings.EverListable(x, ListerThingsUse.Global) || ListerThings.EverListable(x, ListerThingsUse.Region);
		}

		[CompilerGenerated]
		private static string <BestThingRequestGroup>m__9F(ThingDef x)
		{
			return x.label;
		}

		[CompilerGenerated]
		private static string <BestThingRequestGroup>m__A0(ThingDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <BestThingRequestGroup>m__A1(ThingDef d)
		{
			IEnumerable<ThingRequestGroup> source;
			if (!ListerThings.EverListable(d, ListerThingsUse.Region))
			{
				source = Enumerable.Empty<ThingRequestGroup>();
			}
			else
			{
				source = from x in (ThingRequestGroup[])Enum.GetValues(typeof(ThingRequestGroup))
				where x.StoreInRegion() && x.Includes(d)
				select x;
			}
			string result;
			if (!source.Any<ThingRequestGroup>())
			{
				result = "-";
			}
			else
			{
				ThingRequestGroup best = source.MinBy((ThingRequestGroup x) => DefDatabase<ThingDef>.AllDefs.Count((ThingDef y) => ListerThings.EverListable(y, ListerThingsUse.Region) && x.Includes(y)));
				result = string.Concat(new object[]
				{
					best,
					" (defs: ",
					DefDatabase<ThingDef>.AllDefs.Count((ThingDef x) => ListerThings.EverListable(x, ListerThingsUse.Region) && best.Includes(x)),
					")"
				});
			}
			return result;
		}

		[CompilerGenerated]
		private static string <BestThingRequestGroup>m__A2(ThingDef d)
		{
			IEnumerable<ThingRequestGroup> source;
			if (!ListerThings.EverListable(d, ListerThingsUse.Global))
			{
				source = Enumerable.Empty<ThingRequestGroup>();
			}
			else
			{
				source = from x in (ThingRequestGroup[])Enum.GetValues(typeof(ThingRequestGroup))
				where x.Includes(d)
				select x;
			}
			string result;
			if (!source.Any<ThingRequestGroup>())
			{
				result = "-";
			}
			else
			{
				ThingRequestGroup best = source.MinBy((ThingRequestGroup x) => DefDatabase<ThingDef>.AllDefs.Count((ThingDef y) => ListerThings.EverListable(y, ListerThingsUse.Global) && x.Includes(y)));
				result = string.Concat(new object[]
				{
					best,
					" (defs: ",
					DefDatabase<ThingDef>.AllDefs.Count((ThingDef x) => ListerThings.EverListable(x, ListerThingsUse.Global) && best.Includes(x)),
					")"
				});
			}
			return result;
		}

		[CompilerGenerated]
		private static bool <Prosthetics>m__A3(Pawn p)
		{
			return p.health.hediffSet.hediffs.Count == 0;
		}

		[CompilerGenerated]
		private static ThingDef <Prosthetics>m__A4(RecipeDef recipe)
		{
			return (from ic in recipe.ingredients
			select ic.filter.AnyAllowedDef).FirstOrDefault((ThingDef td) => !td.IsMedicine);
		}

		[CompilerGenerated]
		private static string <Prosthetics>m__A5(RecipeDef r)
		{
			return r.defName;
		}

		[CompilerGenerated]
		private static float <Prosthetics>m__A6(RecipeDef r)
		{
			return r.workAmount;
		}

		[CompilerGenerated]
		private static float <Prosthetics>m__A7(RecipeDef r)
		{
			float num = r.ingredients.Sum((IngredientCount ic) => ic.filter.AnyAllowedDef.BaseMarketValue * ic.GetBaseCount());
			float num2 = r.workAmount * 0.0036f;
			return num + num2;
		}

		[CompilerGenerated]
		private static int <Prosthetics>m__A8(RecipeDef r)
		{
			return (from sr in r.skillRequirements
			select sr.minLevel).Max();
		}

		[CompilerGenerated]
		private static int <Prosthetics>m__A9(PawnCapacityDef pc)
		{
			return pc.listOrder;
		}

		[CompilerGenerated]
		private static bool <Prosthetics>m__AA(RecipeDef r)
		{
			return r.workerClass == typeof(Recipe_InstallArtificialBodyPart) || r.workerClass == typeof(Recipe_InstallNaturalBodyPart);
		}

		[CompilerGenerated]
		private static string <JoyGivers>m__AB(JoyGiverDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <JoyGivers>m__AC(JoyGiverDef d)
		{
			return (d.joyKind != null) ? d.joyKind.defName : "null";
		}

		[CompilerGenerated]
		private static string <JoyGivers>m__AD(JoyGiverDef d)
		{
			return d.baseChance.ToString();
		}

		[CompilerGenerated]
		private static string <JoyGivers>m__AE(JoyGiverDef d)
		{
			return d.canDoWhileInBed.ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <JoyGivers>m__AF(JoyGiverDef d)
		{
			return d.desireSit.ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <JoyGivers>m__B0(JoyGiverDef d)
		{
			return d.unroofedOnly.ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <JoyGivers>m__B1(JoyGiverDef d)
		{
			return (d.jobDef != null) ? d.jobDef.defName : "null";
		}

		[CompilerGenerated]
		private static string <JoyGivers>m__B2(JoyGiverDef d)
		{
			return d.pctPawnsEverDo.ToStringPercent();
		}

		[CompilerGenerated]
		private static string <JoyGivers>m__B3(JoyGiverDef d)
		{
			string result;
			if (d.requiredCapacities == null)
			{
				result = "";
			}
			else
			{
				result = (from c in d.requiredCapacities
				select c.defName).ToCommaList(false);
			}
			return result;
		}

		[CompilerGenerated]
		private static string <JoyGivers>m__B4(JoyGiverDef d)
		{
			string result;
			if (d.thingDefs == null)
			{
				result = "";
			}
			else
			{
				result = (from c in d.thingDefs
				select c.defName).ToCommaList(false);
			}
			return result;
		}

		[CompilerGenerated]
		private static string <JoyGivers>m__B5(JoyGiverDef d)
		{
			string result;
			if (d.thingDefs == null)
			{
				result = "";
			}
			else
			{
				result = (from c in d.thingDefs
				select c.GetStatValueAbstract(StatDefOf.JoyGainFactor, null).ToString("F2")).ToCommaList(false);
			}
			return result;
		}

		[CompilerGenerated]
		private static bool <JoyJobs>m__B6(JobDef j)
		{
			return j.joyKind != null;
		}

		[CompilerGenerated]
		private static string <JoyJobs>m__B7(JobDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <JoyJobs>m__B8(JobDef d)
		{
			return d.joyKind.defName;
		}

		[CompilerGenerated]
		private static string <JoyJobs>m__B9(JobDef d)
		{
			return d.joyDuration.ToString();
		}

		[CompilerGenerated]
		private static string <JoyJobs>m__BA(JobDef d)
		{
			return d.joyGainRate.ToString();
		}

		[CompilerGenerated]
		private static string <JoyJobs>m__BB(JobDef d)
		{
			return d.joyMaxParticipants.ToString();
		}

		[CompilerGenerated]
		private static string <JoyJobs>m__BC(JobDef d)
		{
			return (d.joySkill == null) ? "" : d.joySkill.defName;
		}

		[CompilerGenerated]
		private static string <JoyJobs>m__BD(JobDef d)
		{
			return d.joyXpPerTick.ToString();
		}

		[CompilerGenerated]
		private static string <Thoughts>m__BE(ThoughtDef t)
		{
			string text = "";
			string result;
			if (t.stages == null)
			{
				result = null;
			}
			else
			{
				for (int i = 0; i < t.stages.Count; i++)
				{
					ThoughtStage thoughtStage = t.stages[i];
					string text2 = text;
					text = string.Concat(new object[]
					{
						text2,
						"[",
						i,
						"] "
					});
					if (thoughtStage == null)
					{
						text += "null";
					}
					else
					{
						if (thoughtStage.label != null)
						{
							text += thoughtStage.label;
						}
						if (thoughtStage.labelSocial != null)
						{
							if (thoughtStage.label != null)
							{
								text += "/";
							}
							text += thoughtStage.labelSocial;
						}
						text += " ";
						if (thoughtStage.baseMoodEffect != 0f)
						{
							text = text + "[" + thoughtStage.baseMoodEffect.ToStringWithSign("0.##") + " Mo]";
						}
						if (thoughtStage.baseOpinionOffset != 0f)
						{
							text = text + "(" + thoughtStage.baseOpinionOffset.ToStringWithSign("0.##") + " Op)";
						}
					}
					if (i < t.stages.Count - 1)
					{
						text += "\n";
					}
				}
				result = text;
			}
			return result;
		}

		[CompilerGenerated]
		private static string <Thoughts>m__BF(ThoughtDef d)
		{
			return d.defName;
		}

		[CompilerGenerated]
		private static string <Thoughts>m__C0(ThoughtDef d)
		{
			return (!d.IsMemory) ? "situ" : "mem";
		}

		[CompilerGenerated]
		private static string <Thoughts>m__C1(ThoughtDef d)
		{
			return (!d.IsSocial) ? "mood" : "soc";
		}

		[CompilerGenerated]
		private static float <Thoughts>m__C2(ThoughtDef d)
		{
			return (from st in d.stages
			where st != null
			select st).Max((ThoughtStage st) => st.baseMoodEffect);
		}

		[CompilerGenerated]
		private static float <Thoughts>m__C3(ThoughtDef d)
		{
			return (from st in d.stages
			where st != null
			select st).Min((ThoughtStage st) => st.baseMoodEffect);
		}

		[CompilerGenerated]
		private static string <Thoughts>m__C4(ThoughtDef d)
		{
			return d.stackLimit.ToString();
		}

		[CompilerGenerated]
		private static string <Thoughts>m__C5(ThoughtDef d)
		{
			return (d.stackLimitForSameOtherPawn >= 0) ? d.stackLimitForSameOtherPawn.ToString() : "";
		}

		[CompilerGenerated]
		private static string <Thoughts>m__C6(ThoughtDef d)
		{
			return (d.stackLimit != 1) ? d.stackedEffectMultiplier.ToStringPercent() : "";
		}

		[CompilerGenerated]
		private static string <Thoughts>m__C7(ThoughtDef d)
		{
			return d.durationDays.ToString();
		}

		[CompilerGenerated]
		private static string <Thoughts>m__C8(ThoughtDef d)
		{
			return (d.effectMultiplyingStat != null) ? d.effectMultiplyingStat.defName : "";
		}

		[CompilerGenerated]
		private static string <Thoughts>m__C9(ThoughtDef d)
		{
			return (d.gameCondition != null) ? d.gameCondition.defName : "";
		}

		[CompilerGenerated]
		private static string <Thoughts>m__CA(ThoughtDef d)
		{
			return (d.hediff != null) ? d.hediff.defName : "";
		}

		[CompilerGenerated]
		private static string <Thoughts>m__CB(ThoughtDef d)
		{
			return d.lerpOpinionToZeroAfterDurationPct.ToStringPercent();
		}

		[CompilerGenerated]
		private static string <Thoughts>m__CC(ThoughtDef d)
		{
			return (d.maxCumulatedOpinionOffset <= 99999f) ? d.maxCumulatedOpinionOffset.ToString() : "";
		}

		[CompilerGenerated]
		private static string <Thoughts>m__CD(ThoughtDef d)
		{
			return (d.nextThought != null) ? d.nextThought.defName : "";
		}

		[CompilerGenerated]
		private static string <Thoughts>m__CE(ThoughtDef d)
		{
			return d.nullifiedIfNotColonist.ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static string <Thoughts>m__CF(ThoughtDef d)
		{
			return d.showBubble.ToStringCheckBlank();
		}

		[CompilerGenerated]
		private static float <GenSteps>m__D0(GenStepDef x)
		{
			return x.order;
		}

		[CompilerGenerated]
		private static ushort <GenSteps>m__D1(GenStepDef x)
		{
			return x.index;
		}

		[CompilerGenerated]
		private static string <GenSteps>m__D2(GenStepDef x)
		{
			return x.defName;
		}

		[CompilerGenerated]
		private static string <GenSteps>m__D3(GenStepDef x)
		{
			return x.order.ToString("0.##");
		}

		[CompilerGenerated]
		private static string <GenSteps>m__D4(GenStepDef x)
		{
			return x.genStep.GetType().Name;
		}

		[CompilerGenerated]
		private static string <GenSteps>m__D5(GenStepDef x)
		{
			return (x.linkWithSite == null) ? "" : x.linkWithSite.defName;
		}

		[CompilerGenerated]
		private static float <WorldGenSteps>m__D6(WorldGenStepDef x)
		{
			return x.order;
		}

		[CompilerGenerated]
		private static ushort <WorldGenSteps>m__D7(WorldGenStepDef x)
		{
			return x.index;
		}

		[CompilerGenerated]
		private static string <WorldGenSteps>m__D8(WorldGenStepDef x)
		{
			return x.defName;
		}

		[CompilerGenerated]
		private static string <WorldGenSteps>m__D9(WorldGenStepDef x)
		{
			return x.order.ToString("0.##");
		}

		[CompilerGenerated]
		private static string <WorldGenSteps>m__DA(WorldGenStepDef x)
		{
			return x.worldGenStep.GetType().Name;
		}

		[CompilerGenerated]
		private static string <DefaultStuffs>m__DB(StuffCategoryDef c)
		{
			return c.defName;
		}

		[CompilerGenerated]
		private static string <BodyParts>m__DC(KeyValuePair<DamageDef, float> kvp)
		{
			return kvp.ToString();
		}

		[CompilerGenerated]
		private static string <BodyParts>m__DD(BodyPartTagDef t)
		{
			return t.defName;
		}

		[CompilerGenerated]
		private static string <Surgeries>m__DE(IngredientCount ing)
		{
			return ing.ToString();
		}

		[CompilerGenerated]
		private static string <Surgeries>m__DF(SkillRequirement ing)
		{
			return ing.ToString();
		}

		[CompilerGenerated]
		private static string <Terrains>m__E0(ResearchProjectDef pr)
		{
			return pr.defName;
		}

		[CompilerGenerated]
		private static string <Terrains>m__E1(TerrainAffordanceDef af)
		{
			return af.defName;
		}

		[CompilerGenerated]
		private static float <MentalBreaks>m__E2(MentalBreakDef x)
		{
			return x.baseCommonality;
		}

		[CompilerGenerated]
		private static int <BestThingRequestGroup>m__E3(ThingRequestGroup x)
		{
			return DefDatabase<ThingDef>.AllDefs.Count((ThingDef y) => ListerThings.EverListable(y, ListerThingsUse.Region) && x.Includes(y));
		}

		[CompilerGenerated]
		private static int <BestThingRequestGroup>m__E4(ThingRequestGroup x)
		{
			return DefDatabase<ThingDef>.AllDefs.Count((ThingDef y) => ListerThings.EverListable(y, ListerThingsUse.Global) && x.Includes(y));
		}

		[CompilerGenerated]
		private static ThingDef <Prosthetics>m__E5(IngredientCount ic)
		{
			return ic.filter.AnyAllowedDef;
		}

		[CompilerGenerated]
		private static bool <Prosthetics>m__E6(ThingDef td)
		{
			return !td.IsMedicine;
		}

		[CompilerGenerated]
		private static float <Prosthetics>m__E7(IngredientCount ic)
		{
			return ic.filter.AnyAllowedDef.BaseMarketValue * ic.GetBaseCount();
		}

		[CompilerGenerated]
		private static int <Prosthetics>m__E8(SkillRequirement sr)
		{
			return sr.minLevel;
		}

		[CompilerGenerated]
		private static string <JoyGivers>m__E9(PawnCapacityDef c)
		{
			return c.defName;
		}

		[CompilerGenerated]
		private static string <JoyGivers>m__EA(ThingDef c)
		{
			return c.defName;
		}

		[CompilerGenerated]
		private static string <JoyGivers>m__EB(ThingDef c)
		{
			return c.GetStatValueAbstract(StatDefOf.JoyGainFactor, null).ToString("F2");
		}

		[CompilerGenerated]
		private static bool <Thoughts>m__EC(ThoughtStage st)
		{
			return st != null;
		}

		[CompilerGenerated]
		private static float <Thoughts>m__ED(ThoughtStage st)
		{
			return st.baseMoodEffect;
		}

		[CompilerGenerated]
		private static bool <Thoughts>m__EE(ThoughtStage st)
		{
			return st != null;
		}

		[CompilerGenerated]
		private static float <Thoughts>m__EF(ThoughtStage st)
		{
			return st.baseMoodEffect;
		}

		[CompilerGenerated]
		private sealed class <MiningResourceGeneration>c__AnonStorey0
		{
			internal Func<ThingDef, ThingDef> mineable;

			internal Func<ThingDef, float> mineableCommonality;

			internal Func<ThingDef, IntRange> mineableLumpSizeRange;

			internal Func<ThingDef, float> mineableYield;

			public <MiningResourceGeneration>c__AnonStorey0()
			{
			}

			internal float <>m__0(ThingDef d)
			{
				float result;
				if (this.mineable(d) != null)
				{
					result = this.mineable(d).building.mineableScatterCommonality;
				}
				else
				{
					result = 0f;
				}
				return result;
			}

			internal IntRange <>m__1(ThingDef d)
			{
				IntRange result;
				if (this.mineable(d) != null)
				{
					result = this.mineable(d).building.mineableScatterLumpSizeRange;
				}
				else
				{
					result = IntRange.zero;
				}
				return result;
			}

			internal float <>m__2(ThingDef d)
			{
				float result;
				if (this.mineable(d) != null)
				{
					result = (float)this.mineable(d).building.mineableYield;
				}
				else
				{
					result = 0f;
				}
				return result;
			}

			internal bool <>m__3(ThingDef d)
			{
				return d.deepCommonality > 0f || this.mineableCommonality(d) > 0f;
			}

			internal string <>m__4(ThingDef d)
			{
				return this.mineableCommonality(d).ToString("F2");
			}

			internal object <>m__5(ThingDef d)
			{
				return this.mineableLumpSizeRange(d);
			}

			internal float <>m__6(ThingDef d)
			{
				return this.mineableYield(d);
			}
		}

		[CompilerGenerated]
		private sealed class <ThingsPowerAndHeat>c__AnonStorey1
		{
			internal Func<ThingDef, CompProperties_HeatPusher> heatPusher;

			public <ThingsPowerAndHeat>c__AnonStorey1()
			{
			}

			internal bool <>m__0(ThingDef d)
			{
				return (d.category == ThingCategory.Building || d.GetCompProperties<CompProperties_Power>() != null || this.heatPusher(d) != null) && !d.IsFrame;
			}

			internal string <>m__1(ThingDef d)
			{
				return (this.heatPusher(d) != null) ? this.heatPusher(d).compClass.ToString() : "";
			}

			internal string <>m__2(ThingDef d)
			{
				return (this.heatPusher(d) != null) ? this.heatPusher(d).heatPerSecond.ToString() : "";
			}

			internal string <>m__3(ThingDef d)
			{
				return (this.heatPusher(d) != null) ? this.heatPusher(d).heatPushMinTemperature.ToStringTemperature("F1") : "";
			}

			internal string <>m__4(ThingDef d)
			{
				return (this.heatPusher(d) != null) ? this.heatPusher(d).heatPushMaxTemperature.ToStringTemperature("F1") : "";
			}
		}

		[CompilerGenerated]
		private sealed class <Stuffs>c__AnonStorey3
		{
			internal Func<ThingDef, StatDef, string> getStatFactorString;

			internal Func<ThingDef, float> meleeDpsSharpFactorOverall;

			internal Func<ThingDef, float> meleeDpsBluntFactorOverall;

			public <Stuffs>c__AnonStorey3()
			{
			}

			internal string <>m__0(ThingDef d)
			{
				return this.getStatFactorString(d, StatDefOf.MeleeWeapon_CooldownMultiplier);
			}

			internal string <>m__1(ThingDef d)
			{
				return this.meleeDpsSharpFactorOverall(d).ToString("F2");
			}

			internal string <>m__2(ThingDef d)
			{
				return this.meleeDpsBluntFactorOverall(d).ToString("F2");
			}

			internal string <>m__3(ThingDef d)
			{
				return this.getStatFactorString(d, StatDefOf.Flammability);
			}

			internal string <>m__4(ThingDef d)
			{
				return this.getStatFactorString(d, StatDefOf.WorkToMake);
			}

			internal string <>m__5(ThingDef d)
			{
				return this.getStatFactorString(d, StatDefOf.WorkToBuild);
			}

			internal string <>m__6(ThingDef d)
			{
				return this.getStatFactorString(d, StatDefOf.MaxHitPoints);
			}

			internal string <>m__7(ThingDef d)
			{
				return this.getStatFactorString(d, StatDefOf.Beauty);
			}

			internal string <>m__8(ThingDef d)
			{
				return this.getStatFactorString(d, StatDefOf.DoorOpenSpeed);
			}
		}

		[CompilerGenerated]
		private sealed class <Medicines>c__AnonStorey4
		{
			internal float factor;

			public <Medicines>c__AnonStorey4()
			{
			}

			internal string <>m__0(float p)
			{
				return (p * this.factor).ToStringPercent();
			}
		}

		[CompilerGenerated]
		private sealed class <ShootingAccuracy>c__AnonStorey5
		{
			internal StatDef stat;

			internal Func<int, float, int, float> accAtDistance;

			public <ShootingAccuracy>c__AnonStorey5()
			{
			}

			internal float <>m__0(int level, float dist, int traitDegree)
			{
				float num = 1f;
				if (traitDegree != 0)
				{
					float value = TraitDef.Named("ShootingAccuracy").DataAtDegree(traitDegree).statOffsets.First((StatModifier so) => so.stat == this.stat).value;
					num += value;
				}
				foreach (SkillNeed skillNeed in this.stat.skillNeedFactors)
				{
					SkillNeed_Direct skillNeed_Direct = skillNeed as SkillNeed_Direct;
					num *= skillNeed_Direct.valuesPerLevel[level];
				}
				num = this.stat.postProcessCurve.Evaluate(num);
				return Mathf.Pow(num, dist);
			}

			internal string <>m__1(int lev)
			{
				return this.accAtDistance(lev, 1f, 0).ToStringPercent("F2");
			}

			internal string <>m__2(int lev)
			{
				return this.accAtDistance(lev, 10f, 0).ToStringPercent("F2");
			}

			internal string <>m__3(int lev)
			{
				return this.accAtDistance(lev, 20f, 0).ToStringPercent("F2");
			}

			internal string <>m__4(int lev)
			{
				return this.accAtDistance(lev, 30f, 0).ToStringPercent("F2");
			}

			internal string <>m__5(int lev)
			{
				return this.accAtDistance(lev, 50f, 0).ToStringPercent("F2");
			}

			internal string <>m__6(int lev)
			{
				return this.accAtDistance(lev, 1f, 1).ToStringPercent("F2");
			}

			internal string <>m__7(int lev)
			{
				return this.accAtDistance(lev, 10f, 1).ToStringPercent("F2");
			}

			internal string <>m__8(int lev)
			{
				return this.accAtDistance(lev, 20f, 1).ToStringPercent("F2");
			}

			internal string <>m__9(int lev)
			{
				return this.accAtDistance(lev, 30f, 1).ToStringPercent("F2");
			}

			internal string <>m__A(int lev)
			{
				return this.accAtDistance(lev, 50f, 1).ToStringPercent("F2");
			}

			internal string <>m__B(int lev)
			{
				return this.accAtDistance(lev, 1f, -1).ToStringPercent("F2");
			}

			internal string <>m__C(int lev)
			{
				return this.accAtDistance(lev, 10f, -1).ToStringPercent("F2");
			}

			internal string <>m__D(int lev)
			{
				return this.accAtDistance(lev, 20f, -1).ToStringPercent("F2");
			}

			internal string <>m__E(int lev)
			{
				return this.accAtDistance(lev, 30f, -1).ToStringPercent("F2");
			}

			internal string <>m__F(int lev)
			{
				return this.accAtDistance(lev, 50f, -1).ToStringPercent("F2");
			}

			internal bool <>m__10(StatModifier so)
			{
				return so.stat == this.stat;
			}
		}

		[CompilerGenerated]
		private sealed class <BodyPartTagGroups>c__AnonStorey6
		{
			internal BodyDef localBd;

			private static Func<BodyPartRecord, IEnumerable<BodyPartTagDef>> <>f__am$cache0;

			private static Func<BodyPartTagDef, BodyPartTagDef> <>f__am$cache1;

			private static Func<BodyPartRecord, string> <>f__am$cache2;

			public <BodyPartTagGroups>c__AnonStorey6()
			{
			}

			internal void <>m__0()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(this.localBd.defName + "\n----------------");
				using (IEnumerator<BodyPartTagDef> enumerator = (from elem in this.localBd.AllParts.SelectMany((BodyPartRecord part) => part.def.tags)
				orderby elem
				select elem).Distinct<BodyPartTagDef>().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BodyPartTagDef tag = enumerator.Current;
						stringBuilder.AppendLine(tag.defName);
						foreach (BodyPartRecord bodyPartRecord in from part in this.localBd.AllParts
						where part.def.tags.Contains(tag)
						orderby part.def.defName
						select part)
						{
							stringBuilder.AppendLine("  " + bodyPartRecord.def.defName);
						}
					}
				}
				Log.Message(stringBuilder.ToString(), false);
			}

			private static IEnumerable<BodyPartTagDef> <>m__1(BodyPartRecord part)
			{
				return part.def.tags;
			}

			private static BodyPartTagDef <>m__2(BodyPartTagDef elem)
			{
				return elem;
			}

			private static string <>m__3(BodyPartRecord part)
			{
				return part.def.defName;
			}

			private sealed class <BodyPartTagGroups>c__AnonStorey7
			{
				internal BodyPartTagDef tag;

				internal DebugOutputsMisc.<BodyPartTagGroups>c__AnonStorey6 <>f__ref$6;

				public <BodyPartTagGroups>c__AnonStorey7()
				{
				}

				internal bool <>m__0(BodyPartRecord part)
				{
					return part.def.tags.Contains(this.tag);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <ListSolidBackstories>c__AnonStorey8
		{
			internal string catInner;

			public <ListSolidBackstories>c__AnonStorey8()
			{
			}

			internal void <>m__0()
			{
				IEnumerable<PawnBio> enumerable = from b in SolidBioDatabase.allBios
				where b.adulthood.spawnCategories.Contains(this.catInner)
				select b;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"Backstories with category: ",
					this.catInner,
					" (",
					enumerable.Count<PawnBio>(),
					")"
				}));
				foreach (PawnBio pawnBio in enumerable)
				{
					stringBuilder.AppendLine(pawnBio.ToString());
				}
				Log.Message(stringBuilder.ToString(), false);
			}

			internal bool <>m__1(PawnBio b)
			{
				return b.adulthood.spawnCategories.Contains(this.catInner);
			}
		}

		[CompilerGenerated]
		private sealed class <ThingSetMakerTest>c__AnonStorey9
		{
			internal ThingSetMakerDef localDef;

			public <ThingSetMakerTest>c__AnonStorey9()
			{
			}

			internal void <>m__0()
			{
				Action<ThingSetMakerParams> generate = delegate(ThingSetMakerParams parms)
				{
					StringBuilder stringBuilder = new StringBuilder();
					float num = 0f;
					float num2 = 0f;
					for (int i = 0; i < 50; i++)
					{
						List<Thing> list2 = this.localDef.root.Generate(parms);
						if (stringBuilder.Length > 0)
						{
							stringBuilder.AppendLine();
						}
						float num3 = 0f;
						float num4 = 0f;
						for (int j = 0; j < list2.Count; j++)
						{
							stringBuilder.AppendLine("-" + list2[j].LabelCap + " - $" + (list2[j].MarketValue * (float)list2[j].stackCount).ToString("F0"));
							num3 += list2[j].MarketValue * (float)list2[j].stackCount;
							if (!(list2[j] is Pawn))
							{
								num4 += list2[j].GetStatValue(StatDefOf.Mass, true) * (float)list2[j].stackCount;
							}
							list2[j].Destroy(DestroyMode.Vanish);
						}
						num += num3;
						num2 += num4;
						stringBuilder.AppendLine("   Total market value: $" + num3.ToString("F0"));
						stringBuilder.AppendLine("   Total mass: " + num4.ToStringMass());
					}
					StringBuilder stringBuilder2 = new StringBuilder();
					stringBuilder2.AppendLine("Default thing sets generated by: " + this.localDef.defName);
					string nonNullFieldsDebugInfo = Gen.GetNonNullFieldsDebugInfo(this.localDef.root.fixedParams);
					stringBuilder2.AppendLine("root fixedParams: " + ((!nonNullFieldsDebugInfo.NullOrEmpty()) ? nonNullFieldsDebugInfo : "none"));
					string nonNullFieldsDebugInfo2 = Gen.GetNonNullFieldsDebugInfo(parms);
					if (!nonNullFieldsDebugInfo2.NullOrEmpty())
					{
						stringBuilder2.AppendLine("(used custom debug params: " + nonNullFieldsDebugInfo2 + ")");
					}
					stringBuilder2.AppendLine("Average market value: $" + (num / 50f).ToString("F1"));
					stringBuilder2.AppendLine("Average mass: " + (num2 / 50f).ToStringMass());
					stringBuilder2.AppendLine();
					stringBuilder2.Append(stringBuilder.ToString());
					Log.Message(stringBuilder2.ToString(), false);
				};
				if (this.localDef == ThingSetMakerDefOf.TraderStock)
				{
					List<DebugMenuOption> list = new List<DebugMenuOption>();
					foreach (Faction faction in Find.FactionManager.AllFactions)
					{
						if (faction != Faction.OfPlayer)
						{
							Faction localF = faction;
							list.Add(new DebugMenuOption(localF.Name + " (" + localF.def.defName + ")", DebugMenuOptionMode.Action, delegate()
							{
								List<DebugMenuOption> list2 = new List<DebugMenuOption>();
								foreach (TraderKindDef localKind2 in (from x in DefDatabase<TraderKindDef>.AllDefs
								where x.orbital
								select x).Concat(localF.def.caravanTraderKinds).Concat(localF.def.visitorTraderKinds).Concat(localF.def.baseTraderKinds))
								{
									TraderKindDef localKind = localKind2;
									list2.Add(new DebugMenuOption(localKind.defName, DebugMenuOptionMode.Action, delegate()
									{
										ThingSetMakerParams obj = default(ThingSetMakerParams);
										obj.traderFaction = localF;
										obj.traderDef = localKind;
										generate(obj);
									}));
								}
								Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
							}));
						}
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
				}
				else
				{
					generate(this.localDef.debugParams);
				}
			}

			private sealed class <ThingSetMakerTest>c__AnonStoreyC
			{
				internal Action<ThingSetMakerParams> generate;

				internal DebugOutputsMisc.<ThingSetMakerTest>c__AnonStorey9 <>f__ref$9;

				public <ThingSetMakerTest>c__AnonStoreyC()
				{
				}

				internal void <>m__0(ThingSetMakerParams parms)
				{
					StringBuilder stringBuilder = new StringBuilder();
					float num = 0f;
					float num2 = 0f;
					for (int i = 0; i < 50; i++)
					{
						List<Thing> list = this.<>f__ref$9.localDef.root.Generate(parms);
						if (stringBuilder.Length > 0)
						{
							stringBuilder.AppendLine();
						}
						float num3 = 0f;
						float num4 = 0f;
						for (int j = 0; j < list.Count; j++)
						{
							stringBuilder.AppendLine("-" + list[j].LabelCap + " - $" + (list[j].MarketValue * (float)list[j].stackCount).ToString("F0"));
							num3 += list[j].MarketValue * (float)list[j].stackCount;
							if (!(list[j] is Pawn))
							{
								num4 += list[j].GetStatValue(StatDefOf.Mass, true) * (float)list[j].stackCount;
							}
							list[j].Destroy(DestroyMode.Vanish);
						}
						num += num3;
						num2 += num4;
						stringBuilder.AppendLine("   Total market value: $" + num3.ToString("F0"));
						stringBuilder.AppendLine("   Total mass: " + num4.ToStringMass());
					}
					StringBuilder stringBuilder2 = new StringBuilder();
					stringBuilder2.AppendLine("Default thing sets generated by: " + this.<>f__ref$9.localDef.defName);
					string nonNullFieldsDebugInfo = Gen.GetNonNullFieldsDebugInfo(this.<>f__ref$9.localDef.root.fixedParams);
					stringBuilder2.AppendLine("root fixedParams: " + ((!nonNullFieldsDebugInfo.NullOrEmpty()) ? nonNullFieldsDebugInfo : "none"));
					string nonNullFieldsDebugInfo2 = Gen.GetNonNullFieldsDebugInfo(parms);
					if (!nonNullFieldsDebugInfo2.NullOrEmpty())
					{
						stringBuilder2.AppendLine("(used custom debug params: " + nonNullFieldsDebugInfo2 + ")");
					}
					stringBuilder2.AppendLine("Average market value: $" + (num / 50f).ToString("F1"));
					stringBuilder2.AppendLine("Average mass: " + (num2 / 50f).ToStringMass());
					stringBuilder2.AppendLine();
					stringBuilder2.Append(stringBuilder.ToString());
					Log.Message(stringBuilder2.ToString(), false);
				}
			}

			private sealed class <ThingSetMakerTest>c__AnonStoreyA
			{
				internal Faction localF;

				internal DebugOutputsMisc.<ThingSetMakerTest>c__AnonStorey9.<ThingSetMakerTest>c__AnonStoreyC <>f__ref$12;

				private static Func<TraderKindDef, bool> <>f__am$cache0;

				public <ThingSetMakerTest>c__AnonStoreyA()
				{
				}

				internal void <>m__0()
				{
					List<DebugMenuOption> list = new List<DebugMenuOption>();
					foreach (TraderKindDef localKind2 in (from x in DefDatabase<TraderKindDef>.AllDefs
					where x.orbital
					select x).Concat(this.localF.def.caravanTraderKinds).Concat(this.localF.def.visitorTraderKinds).Concat(this.localF.def.baseTraderKinds))
					{
						DebugOutputsMisc.<ThingSetMakerTest>c__AnonStorey9.<ThingSetMakerTest>c__AnonStoreyC <>f__ref$12 = this.<>f__ref$12;
						DebugOutputsMisc.<ThingSetMakerTest>c__AnonStorey9.<ThingSetMakerTest>c__AnonStoreyA <>f__ref$10 = this;
						TraderKindDef localKind = localKind2;
						list.Add(new DebugMenuOption(localKind.defName, DebugMenuOptionMode.Action, delegate()
						{
							ThingSetMakerParams obj = default(ThingSetMakerParams);
							obj.traderFaction = <>f__ref$10.localF;
							obj.traderDef = localKind;
							<>f__ref$12.generate(obj);
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
				}

				private static bool <>m__1(TraderKindDef x)
				{
					return x.orbital;
				}

				private sealed class <ThingSetMakerTest>c__AnonStoreyB
				{
					internal TraderKindDef localKind;

					internal DebugOutputsMisc.<ThingSetMakerTest>c__AnonStorey9.<ThingSetMakerTest>c__AnonStoreyC <>f__ref$12;

					internal DebugOutputsMisc.<ThingSetMakerTest>c__AnonStorey9.<ThingSetMakerTest>c__AnonStoreyA <>f__ref$10;

					public <ThingSetMakerTest>c__AnonStoreyB()
					{
					}

					internal void <>m__0()
					{
						ThingSetMakerParams obj = default(ThingSetMakerParams);
						obj.traderFaction = this.<>f__ref$10.localF;
						obj.traderDef = this.localKind;
						this.<>f__ref$12.generate(obj);
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class <ThingSetMakerPossibleDefs>c__AnonStoreyD
		{
			internal Dictionary<ThingSetMakerDef, List<ThingDef>> generatableThings;

			public <ThingSetMakerPossibleDefs>c__AnonStoreyD()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <ThingSetMakerPossibleDefs>c__AnonStoreyE
		{
			internal ThingSetMakerDef localDef;

			internal DebugOutputsMisc.<ThingSetMakerPossibleDefs>c__AnonStoreyD <>f__ref$13;

			public <ThingSetMakerPossibleDefs>c__AnonStoreyE()
			{
			}

			internal string <>m__0(ThingDef d)
			{
				return this.<>f__ref$13.generatableThings[this.localDef].Contains(d).ToStringCheckBlank();
			}
		}

		[CompilerGenerated]
		private sealed class <ThingSetMakerSampled>c__AnonStoreyF
		{
			internal ThingSetMakerDef localDef;

			public <ThingSetMakerSampled>c__AnonStoreyF()
			{
			}

			internal void <>m__0()
			{
				Action<ThingSetMakerParams> generate = delegate(ThingSetMakerParams parms)
				{
					Dictionary<ThingDef, int> counts = new Dictionary<ThingDef, int>();
					for (int i = 0; i < 500; i++)
					{
						List<Thing> list2 = this.localDef.root.Generate(parms);
						foreach (ThingDef thingDef in (from th in list2
						select th.GetInnerIfMinified().def).Distinct<ThingDef>())
						{
							if (!counts.ContainsKey(thingDef))
							{
								counts.Add(thingDef, 0);
							}
							Dictionary<ThingDef, int> counts2;
							ThingDef key;
							(counts2 = counts)[key = thingDef] = counts2[key] + 1;
						}
						for (int j = 0; j < list2.Count; j++)
						{
							list2[j].Destroy(DestroyMode.Vanish);
						}
					}
					IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
					where counts.ContainsKey(d)
					orderby counts[d] descending
					select d;
					TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
					array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
					array[1] = new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.BaseMarketValue.ToStringMoney(null));
					array[2] = new TableDataGetter<ThingDef>("mass", (ThingDef d) => d.BaseMass.ToStringMass());
					array[3] = new TableDataGetter<ThingDef>("appearance rate in " + this.localDef.defName, (ThingDef d) => ((float)counts[d] / 500f).ToStringPercent());
					DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
				};
				if (this.localDef == ThingSetMakerDefOf.TraderStock)
				{
					List<DebugMenuOption> list = new List<DebugMenuOption>();
					foreach (Faction faction in Find.FactionManager.AllFactions)
					{
						if (faction != Faction.OfPlayer)
						{
							Faction localF = faction;
							list.Add(new DebugMenuOption(localF.Name + " (" + localF.def.defName + ")", DebugMenuOptionMode.Action, delegate()
							{
								List<DebugMenuOption> list2 = new List<DebugMenuOption>();
								foreach (TraderKindDef localKind2 in (from x in DefDatabase<TraderKindDef>.AllDefs
								where x.orbital
								select x).Concat(localF.def.caravanTraderKinds).Concat(localF.def.visitorTraderKinds).Concat(localF.def.baseTraderKinds))
								{
									TraderKindDef localKind = localKind2;
									list2.Add(new DebugMenuOption(localKind.defName, DebugMenuOptionMode.Action, delegate()
									{
										ThingSetMakerParams obj = default(ThingSetMakerParams);
										obj.traderFaction = localF;
										obj.traderDef = localKind;
										generate(obj);
									}));
								}
								Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
							}));
						}
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
				}
				else
				{
					generate(this.localDef.debugParams);
				}
			}

			private sealed class <ThingSetMakerSampled>c__AnonStorey13
			{
				internal Action<ThingSetMakerParams> generate;

				internal DebugOutputsMisc.<ThingSetMakerSampled>c__AnonStoreyF <>f__ref$15;

				private static Func<Thing, ThingDef> <>f__am$cache0;

				private static Func<ThingDef, string> <>f__am$cache1;

				private static Func<ThingDef, string> <>f__am$cache2;

				private static Func<ThingDef, string> <>f__am$cache3;

				public <ThingSetMakerSampled>c__AnonStorey13()
				{
				}

				internal void <>m__0(ThingSetMakerParams parms)
				{
					DebugOutputsMisc.<ThingSetMakerSampled>c__AnonStoreyF <>f__ref$15 = this.<>f__ref$15;
					Dictionary<ThingDef, int> counts = new Dictionary<ThingDef, int>();
					for (int i = 0; i < 500; i++)
					{
						List<Thing> list = this.<>f__ref$15.localDef.root.Generate(parms);
						foreach (ThingDef thingDef in (from th in list
						select th.GetInnerIfMinified().def).Distinct<ThingDef>())
						{
							if (!counts.ContainsKey(thingDef))
							{
								counts.Add(thingDef, 0);
							}
							Dictionary<ThingDef, int> counts2;
							ThingDef key;
							(counts2 = counts)[key = thingDef] = counts2[key] + 1;
						}
						for (int j = 0; j < list.Count; j++)
						{
							list[j].Destroy(DestroyMode.Vanish);
						}
					}
					IEnumerable<ThingDef> dataSources = from d in DefDatabase<ThingDef>.AllDefs
					where counts.ContainsKey(d)
					orderby counts[d] descending
					select d;
					TableDataGetter<ThingDef>[] array = new TableDataGetter<ThingDef>[4];
					array[0] = new TableDataGetter<ThingDef>("defName", (ThingDef d) => d.defName);
					array[1] = new TableDataGetter<ThingDef>("market\nvalue", (ThingDef d) => d.BaseMarketValue.ToStringMoney(null));
					array[2] = new TableDataGetter<ThingDef>("mass", (ThingDef d) => d.BaseMass.ToStringMass());
					array[3] = new TableDataGetter<ThingDef>("appearance rate in " + this.<>f__ref$15.localDef.defName, (ThingDef d) => ((float)counts[d] / 500f).ToStringPercent());
					DebugTables.MakeTablesDialog<ThingDef>(dataSources, array);
				}

				private static ThingDef <>m__1(Thing th)
				{
					return th.GetInnerIfMinified().def;
				}

				private static string <>m__2(ThingDef d)
				{
					return d.defName;
				}

				private static string <>m__3(ThingDef d)
				{
					return d.BaseMarketValue.ToStringMoney(null);
				}

				private static string <>m__4(ThingDef d)
				{
					return d.BaseMass.ToStringMass();
				}

				private sealed class <ThingSetMakerSampled>c__AnonStorey10
				{
					internal Dictionary<ThingDef, int> counts;

					internal DebugOutputsMisc.<ThingSetMakerSampled>c__AnonStoreyF <>f__ref$15;

					public <ThingSetMakerSampled>c__AnonStorey10()
					{
					}

					internal bool <>m__0(ThingDef d)
					{
						return this.counts.ContainsKey(d);
					}

					internal int <>m__1(ThingDef d)
					{
						return this.counts[d];
					}

					internal string <>m__2(ThingDef d)
					{
						return ((float)this.counts[d] / 500f).ToStringPercent();
					}
				}
			}

			private sealed class <ThingSetMakerSampled>c__AnonStorey11
			{
				internal Faction localF;

				internal DebugOutputsMisc.<ThingSetMakerSampled>c__AnonStoreyF.<ThingSetMakerSampled>c__AnonStorey13 <>f__ref$19;

				private static Func<TraderKindDef, bool> <>f__am$cache0;

				public <ThingSetMakerSampled>c__AnonStorey11()
				{
				}

				internal void <>m__0()
				{
					List<DebugMenuOption> list = new List<DebugMenuOption>();
					foreach (TraderKindDef localKind2 in (from x in DefDatabase<TraderKindDef>.AllDefs
					where x.orbital
					select x).Concat(this.localF.def.caravanTraderKinds).Concat(this.localF.def.visitorTraderKinds).Concat(this.localF.def.baseTraderKinds))
					{
						DebugOutputsMisc.<ThingSetMakerSampled>c__AnonStoreyF.<ThingSetMakerSampled>c__AnonStorey13 <>f__ref$19 = this.<>f__ref$19;
						DebugOutputsMisc.<ThingSetMakerSampled>c__AnonStoreyF.<ThingSetMakerSampled>c__AnonStorey11 <>f__ref$17 = this;
						TraderKindDef localKind = localKind2;
						list.Add(new DebugMenuOption(localKind.defName, DebugMenuOptionMode.Action, delegate()
						{
							ThingSetMakerParams obj = default(ThingSetMakerParams);
							obj.traderFaction = <>f__ref$17.localF;
							obj.traderDef = localKind;
							<>f__ref$19.generate(obj);
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
				}

				private static bool <>m__1(TraderKindDef x)
				{
					return x.orbital;
				}

				private sealed class <ThingSetMakerSampled>c__AnonStorey12
				{
					internal TraderKindDef localKind;

					internal DebugOutputsMisc.<ThingSetMakerSampled>c__AnonStoreyF.<ThingSetMakerSampled>c__AnonStorey13 <>f__ref$19;

					internal DebugOutputsMisc.<ThingSetMakerSampled>c__AnonStoreyF.<ThingSetMakerSampled>c__AnonStorey11 <>f__ref$17;

					public <ThingSetMakerSampled>c__AnonStorey12()
					{
					}

					internal void <>m__0()
					{
						ThingSetMakerParams obj = default(ThingSetMakerParams);
						obj.traderFaction = this.<>f__ref$17.localF;
						obj.traderDef = this.localKind;
						this.<>f__ref$19.generate(obj);
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class <WorkDisables>c__AnonStorey14
		{
			internal PawnKindDef pkInner;

			internal Faction faction;

			private static Func<KeyValuePair<string, Backstory>, Backstory> <>f__am$cache0;

			public <WorkDisables>c__AnonStorey14()
			{
			}

			internal void <>m__0()
			{
				int num = 500;
				DefMap<WorkTypeDef, int> defMap = new DefMap<WorkTypeDef, int>();
				for (int i = 0; i < num; i++)
				{
					Pawn pawn = PawnGenerator.GeneratePawn(this.pkInner, this.faction);
					foreach (WorkTypeDef workTypeDef in pawn.story.DisabledWorkTypes)
					{
						DefMap<WorkTypeDef, int> defMap2;
						WorkTypeDef def;
						(defMap2 = defMap)[def = workTypeDef] = defMap2[def] + 1;
					}
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"Generated ",
					num,
					" pawns of kind ",
					this.pkInner.defName,
					" on faction ",
					this.faction.ToStringSafe<Faction>()
				}));
				stringBuilder.AppendLine("Work types disabled:");
				foreach (WorkTypeDef workTypeDef2 in DefDatabase<WorkTypeDef>.AllDefs)
				{
					if (workTypeDef2.workTags != WorkTags.None)
					{
						stringBuilder.AppendLine(string.Concat(new object[]
						{
							"   ",
							workTypeDef2.defName,
							": ",
							defMap[workTypeDef2],
							"        ",
							((float)defMap[workTypeDef2] / (float)num).ToStringPercent()
						}));
					}
				}
				IEnumerable<Backstory> enumerable = from kvp in BackstoryDatabase.allBackstories
				select kvp.Value;
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Backstories WorkTypeDef disable rates (there are " + enumerable.Count<Backstory>() + " backstories):");
				using (IEnumerator<WorkTypeDef> enumerator3 = DefDatabase<WorkTypeDef>.AllDefs.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						WorkTypeDef wt = enumerator3.Current;
						int num2 = 0;
						foreach (Backstory backstory in enumerable)
						{
							if (backstory.DisabledWorkTypes.Any((WorkTypeDef wd) => wt == wd))
							{
								num2++;
							}
						}
						stringBuilder.AppendLine(string.Concat(new object[]
						{
							"   ",
							wt.defName,
							": ",
							num2,
							"     ",
							((float)num2 / (float)BackstoryDatabase.allBackstories.Count).ToStringPercent()
						}));
					}
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Backstories WorkTag disable rates (there are " + enumerable.Count<Backstory>() + " backstories):");
				IEnumerator enumerator5 = Enum.GetValues(typeof(WorkTags)).GetEnumerator();
				try
				{
					while (enumerator5.MoveNext())
					{
						object obj = enumerator5.Current;
						WorkTags workTags = (WorkTags)obj;
						int num3 = 0;
						foreach (Backstory backstory2 in enumerable)
						{
							if ((workTags & backstory2.workDisables) != WorkTags.None)
							{
								num3++;
							}
						}
						stringBuilder.AppendLine(string.Concat(new object[]
						{
							"   ",
							workTags,
							": ",
							num3,
							"     ",
							((float)num3 / (float)BackstoryDatabase.allBackstories.Count).ToStringPercent()
						}));
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator5 as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				Log.Message(stringBuilder.ToString(), false);
			}

			private static Backstory <>m__1(KeyValuePair<string, Backstory> kvp)
			{
				return kvp.Value;
			}

			private sealed class <WorkDisables>c__AnonStorey15
			{
				internal WorkTypeDef wt;

				public <WorkDisables>c__AnonStorey15()
				{
				}

				internal bool <>m__0(WorkTypeDef wd)
				{
					return this.wt == wd;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <DefNames>c__AnonStorey16
		{
			internal Type type;

			public <DefNames>c__AnonStorey16()
			{
			}

			internal void <>m__0()
			{
				IEnumerable source = (IEnumerable)GenGeneric.GetStaticPropertyOnGenericType(typeof(DefDatabase<>), this.type, "AllDefs");
				int num = 0;
				StringBuilder stringBuilder = new StringBuilder();
				foreach (Def def in source.Cast<Def>())
				{
					stringBuilder.AppendLine(def.defName);
					num++;
					if (num >= 500)
					{
						Log.Message(stringBuilder.ToString(), false);
						stringBuilder = new StringBuilder();
						num = 0;
					}
				}
				Log.Message(stringBuilder.ToString(), false);
			}
		}

		[CompilerGenerated]
		private sealed class <DefLabels>c__AnonStorey17
		{
			internal Type type;

			public <DefLabels>c__AnonStorey17()
			{
			}

			internal void <>m__0()
			{
				IEnumerable source = (IEnumerable)GenGeneric.GetStaticPropertyOnGenericType(typeof(DefDatabase<>), this.type, "AllDefs");
				int num = 0;
				StringBuilder stringBuilder = new StringBuilder();
				foreach (Def def in source.Cast<Def>())
				{
					stringBuilder.AppendLine(def.label);
					num++;
					if (num >= 500)
					{
						Log.Message(stringBuilder.ToString(), false);
						stringBuilder = new StringBuilder();
						num = 0;
					}
				}
				Log.Message(stringBuilder.ToString(), false);
			}
		}

		[CompilerGenerated]
		private sealed class <Bodies>c__AnonStorey18
		{
			internal BodyDef localBd;

			private static Func<BodyPartRecord, BodyPartHeight> <>f__am$cache0;

			private static Func<BodyPartRecord, string> <>f__am$cache1;

			private static Func<BodyPartRecord, int> <>f__am$cache2;

			private static Func<BodyPartRecord, string> <>f__am$cache3;

			private static Func<BodyPartRecord, string> <>f__am$cache4;

			private static Func<BodyPartRecord, string> <>f__am$cache5;

			private static Func<BodyPartRecord, string> <>f__am$cache6;

			private static Func<BodyPartRecord, string> <>f__am$cache7;

			public <Bodies>c__AnonStorey18()
			{
			}

			internal void <>m__0()
			{
				IEnumerable<BodyPartRecord> dataSources = from d in this.localBd.AllParts
				orderby d.height descending
				select d;
				TableDataGetter<BodyPartRecord>[] array = new TableDataGetter<BodyPartRecord>[7];
				array[0] = new TableDataGetter<BodyPartRecord>("defName", (BodyPartRecord d) => d.def.defName);
				array[1] = new TableDataGetter<BodyPartRecord>("hitPoints\n(non-adjusted)", (BodyPartRecord d) => d.def.hitPoints);
				array[2] = new TableDataGetter<BodyPartRecord>("coverage", (BodyPartRecord d) => d.coverage.ToStringPercent());
				array[3] = new TableDataGetter<BodyPartRecord>("coverageAbsWithChildren", (BodyPartRecord d) => d.coverageAbsWithChildren.ToStringPercent());
				array[4] = new TableDataGetter<BodyPartRecord>("coverageAbs", (BodyPartRecord d) => d.coverageAbs.ToStringPercent());
				array[5] = new TableDataGetter<BodyPartRecord>("depth", (BodyPartRecord d) => d.depth.ToString());
				array[6] = new TableDataGetter<BodyPartRecord>("height", (BodyPartRecord d) => d.height.ToString());
				DebugTables.MakeTablesDialog<BodyPartRecord>(dataSources, array);
			}

			private static BodyPartHeight <>m__1(BodyPartRecord d)
			{
				return d.height;
			}

			private static string <>m__2(BodyPartRecord d)
			{
				return d.def.defName;
			}

			private static int <>m__3(BodyPartRecord d)
			{
				return d.def.hitPoints;
			}

			private static string <>m__4(BodyPartRecord d)
			{
				return d.coverage.ToStringPercent();
			}

			private static string <>m__5(BodyPartRecord d)
			{
				return d.coverageAbsWithChildren.ToStringPercent();
			}

			private static string <>m__6(BodyPartRecord d)
			{
				return d.coverageAbs.ToStringPercent();
			}

			private static string <>m__7(BodyPartRecord d)
			{
				return d.depth.ToString();
			}

			private static string <>m__8(BodyPartRecord d)
			{
				return d.height.ToString();
			}
		}

		[CompilerGenerated]
		private sealed class <TraderKindThings>c__AnonStorey19
		{
			internal TraderKindDef localTk;

			public <TraderKindThings>c__AnonStorey19()
			{
			}

			internal string <>m__0(ThingDef td)
			{
				return this.localTk.WillTrade(td).ToStringCheckBlank();
			}
		}

		[CompilerGenerated]
		private sealed class <HitsToKill>c__AnonStorey1A
		{
			internal Dictionary<ThingDef, <>__AnonType1<ThingDef, float, int>> data;

			public <HitsToKill>c__AnonStorey1A()
			{
			}

			internal string <>m__0(ThingDef d)
			{
				return this.data[d].Hits.ToString("F0");
			}

			internal string <>m__1(ThingDef d)
			{
				return this.data[d].DiedDueToDamageThreshold + "/" + 15;
			}
		}

		[CompilerGenerated]
		private sealed class <TraitsSampled>c__AnonStorey1D
		{
			internal List<Pawn> testColonists;

			internal Func<TraitDegreeData, TraitDef> getTrait;

			internal Func<TraitDegreeData, float> getPrevalence;

			public <TraitsSampled>c__AnonStorey1D()
			{
			}

			internal float <>m__0(TraitDegreeData d)
			{
				float num = 0f;
				foreach (Pawn pawn in this.testColonists)
				{
					Trait trait = pawn.story.traits.GetTrait(this.getTrait(d));
					if (trait != null && trait.Degree == d.degree)
					{
						num += 1f;
					}
				}
				return num / 4000f;
			}

			internal string <>m__1(TraitDegreeData d)
			{
				return this.getTrait(d).defName;
			}

			internal string <>m__2(TraitDegreeData d)
			{
				return this.getTrait(d).GetGenderSpecificCommonality(Gender.None).ToString("F2");
			}

			internal string <>m__3(TraitDegreeData d)
			{
				return this.getTrait(d).GetGenderSpecificCommonality(Gender.Female).ToString("F2");
			}

			internal string <>m__4(TraitDegreeData d)
			{
				return (this.getTrait(d).degreeDatas.Count <= 0) ? "" : d.degree.ToString();
			}

			internal string <>m__5(TraitDegreeData d)
			{
				return (this.getTrait(d).degreeDatas.Count <= 0) ? "" : d.commonality.ToString("F2");
			}

			internal string <>m__6(TraitDegreeData d)
			{
				return this.getPrevalence(d).ToStringPercent();
			}
		}

		[CompilerGenerated]
		private sealed class <Prosthetics>c__AnonStorey24
		{
			internal Pawn pawn;

			internal Func<RecipeDef, ThingDef> getProstheticItem;

			internal Action refreshPawn;

			internal Func<RecipeDef, BodyPartRecord> getApplicationPoint;

			public <Prosthetics>c__AnonStorey24()
			{
			}

			internal void <>m__0()
			{
				while (this.pawn.health.hediffSet.hediffs.Count > 0)
				{
					this.pawn.health.RemoveHediff(this.pawn.health.hediffSet.hediffs[0]);
				}
			}

			internal BodyPartRecord <>m__1(RecipeDef recipe)
			{
				return recipe.appliedOnFixedBodyParts.SelectMany((BodyPartDef bpd) => this.pawn.def.race.body.GetPartsWithDef(bpd)).FirstOrDefault<BodyPartRecord>();
			}

			internal float <>m__2(RecipeDef r)
			{
				ThingDef thingDef = this.getProstheticItem(r);
				return (thingDef == null) ? 0f : thingDef.BaseMarketValue;
			}

			internal string <>m__3(RecipeDef r)
			{
				return (this.getProstheticItem(r) != null) ? this.getProstheticItem(r).techLevel.ToStringHuman() : "";
			}

			internal string <>m__4(RecipeDef r)
			{
				return (this.getProstheticItem(r) != null) ? this.getProstheticItem(r).thingSetMakerTags.ToCommaList(false) : "";
			}

			internal string <>m__5(RecipeDef r)
			{
				return (this.getProstheticItem(r) != null) ? this.getProstheticItem(r).techHediffsTags.ToCommaList(false) : "";
			}

			internal IEnumerable<BodyPartRecord> <>m__6(BodyPartDef bpd)
			{
				return this.pawn.def.race.body.GetPartsWithDef(bpd);
			}
		}

		[CompilerGenerated]
		private sealed class <Prosthetics>c__AnonStorey25
		{
			internal PawnCapacityDef cap;

			internal DebugOutputsMisc.<Prosthetics>c__AnonStorey24 <>f__ref$36;

			private static Predicate<PawnCapacityUtility.CapacityImpactor> <>f__am$cache0;

			public <Prosthetics>c__AnonStorey25()
			{
			}

			internal string <>m__0(RecipeDef r)
			{
				this.<>f__ref$36.refreshPawn();
				r.Worker.ApplyOnPawn(this.<>f__ref$36.pawn, this.<>f__ref$36.getApplicationPoint(r), null, null, null);
				float num = this.<>f__ref$36.pawn.health.capacities.GetLevel(this.cap) - 1f;
				string result;
				if ((double)Math.Abs(num) > 0.001)
				{
					result = num.ToStringPercent();
				}
				else
				{
					this.<>f__ref$36.refreshPawn();
					BodyPartRecord bodyPartRecord = this.<>f__ref$36.getApplicationPoint(r);
					Thing pawn = this.<>f__ref$36.pawn;
					DamageDef executionCut = DamageDefOf.ExecutionCut;
					float amount = this.<>f__ref$36.pawn.health.hediffSet.GetPartHealth(bodyPartRecord) / 2f;
					float armorPenetration = 999f;
					BodyPartRecord hitPart = bodyPartRecord;
					pawn.TakeDamage(new DamageInfo(executionCut, amount, armorPenetration, -1f, null, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
					List<PawnCapacityUtility.CapacityImpactor> list = new List<PawnCapacityUtility.CapacityImpactor>();
					PawnCapacityUtility.CalculateCapacityLevel(this.<>f__ref$36.pawn.health.hediffSet, this.cap, list);
					if (list.Any((PawnCapacityUtility.CapacityImpactor imp) => imp.IsDirect))
					{
						result = 0f.ToStringPercent();
					}
					else
					{
						result = "";
					}
				}
				return result;
			}

			private static bool <>m__1(PawnCapacityUtility.CapacityImpactor imp)
			{
				return imp.IsDirect;
			}
		}

		[CompilerGenerated]
		private sealed class <Thoughts>c__AnonStorey26
		{
			internal Func<ThoughtDef, string> stagesText;

			public <Thoughts>c__AnonStorey26()
			{
			}

			internal string <>m__0(ThoughtDef d)
			{
				return this.stagesText(d);
			}
		}

		[CompilerGenerated]
		private sealed class <Stuffs>c__AnonStorey2
		{
			internal StatDef stat;

			public <Stuffs>c__AnonStorey2()
			{
			}

			internal bool <>m__0(StatModifier fa)
			{
				return fa.stat == this.stat;
			}
		}

		[CompilerGenerated]
		private sealed class <MentalBreaks>c__AnonStorey1B
		{
			internal MentalBreakDef d;

			public <MentalBreaks>c__AnonStorey1B()
			{
			}

			internal bool <>m__0(MentalBreakDef x)
			{
				return x.intensity == this.d.intensity;
			}
		}

		[CompilerGenerated]
		private sealed class <TraitsSampled>c__AnonStorey1C
		{
			internal TraitDegreeData d;

			public <TraitsSampled>c__AnonStorey1C()
			{
			}

			internal bool <>m__0(TraitDef td)
			{
				return td.degreeDatas.Contains(this.d);
			}
		}

		[CompilerGenerated]
		private sealed class <BestThingRequestGroup>c__AnonStorey1E
		{
			internal ThingDef d;

			public <BestThingRequestGroup>c__AnonStorey1E()
			{
			}

			internal bool <>m__0(ThingRequestGroup x)
			{
				return x.StoreInRegion() && x.Includes(this.d);
			}
		}

		[CompilerGenerated]
		private sealed class <BestThingRequestGroup>c__AnonStorey20
		{
			internal ThingRequestGroup best;

			public <BestThingRequestGroup>c__AnonStorey20()
			{
			}

			internal bool <>m__0(ThingDef x)
			{
				return ListerThings.EverListable(x, ListerThingsUse.Region) && this.best.Includes(x);
			}
		}

		[CompilerGenerated]
		private sealed class <BestThingRequestGroup>c__AnonStorey21
		{
			internal ThingDef d;

			public <BestThingRequestGroup>c__AnonStorey21()
			{
			}

			internal bool <>m__0(ThingRequestGroup x)
			{
				return x.Includes(this.d);
			}
		}

		[CompilerGenerated]
		private sealed class <BestThingRequestGroup>c__AnonStorey23
		{
			internal ThingRequestGroup best;

			public <BestThingRequestGroup>c__AnonStorey23()
			{
			}

			internal bool <>m__0(ThingDef x)
			{
				return ListerThings.EverListable(x, ListerThingsUse.Global) && this.best.Includes(x);
			}
		}

		[CompilerGenerated]
		private sealed class <BestThingRequestGroup>c__AnonStorey1F
		{
			internal ThingRequestGroup x;

			public <BestThingRequestGroup>c__AnonStorey1F()
			{
			}

			internal bool <>m__0(ThingDef y)
			{
				return ListerThings.EverListable(y, ListerThingsUse.Region) && this.x.Includes(y);
			}
		}

		[CompilerGenerated]
		private sealed class <BestThingRequestGroup>c__AnonStorey22
		{
			internal ThingRequestGroup x;

			public <BestThingRequestGroup>c__AnonStorey22()
			{
			}

			internal bool <>m__0(ThingDef y)
			{
				return ListerThings.EverListable(y, ListerThingsUse.Global) && this.x.Includes(y);
			}
		}
	}
}
