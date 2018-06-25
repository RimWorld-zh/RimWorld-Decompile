using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	internal static class RecipeDefGenerator
	{
		public static IEnumerable<RecipeDef> ImpliedRecipeDefs()
		{
			foreach (RecipeDef r in RecipeDefGenerator.DefsFromRecipeMakers().Concat(RecipeDefGenerator.DrugAdministerDefs()))
			{
				yield return r;
			}
			yield break;
		}

		private static IEnumerable<RecipeDef> DefsFromRecipeMakers()
		{
			foreach (ThingDef def in from d in DefDatabase<ThingDef>.AllDefs
			where d.recipeMaker != null
			select d)
			{
				RecipeMakerProperties rm = def.recipeMaker;
				RecipeDef r = new RecipeDef();
				r.defName = "Make_" + def.defName;
				r.label = "RecipeMake".Translate(new object[]
				{
					def.label
				});
				r.jobString = "RecipeMakeJobString".Translate(new object[]
				{
					def.label
				});
				r.modContentPack = def.modContentPack;
				r.workAmount = (float)rm.workAmount;
				r.workSpeedStat = rm.workSpeedStat;
				r.efficiencyStat = rm.efficiencyStat;
				if (def.MadeFromStuff)
				{
					IngredientCount ingredientCount = new IngredientCount();
					ingredientCount.SetBaseCount((float)def.costStuffCount);
					ingredientCount.filter.SetAllowAllWhoCanMake(def);
					r.ingredients.Add(ingredientCount);
					r.fixedIngredientFilter.SetAllowAllWhoCanMake(def);
					r.productHasIngredientStuff = true;
				}
				if (def.costList != null)
				{
					foreach (ThingDefCountClass thingDefCountClass in def.costList)
					{
						IngredientCount ingredientCount2 = new IngredientCount();
						ingredientCount2.SetBaseCount((float)thingDefCountClass.count);
						ingredientCount2.filter.SetAllow(thingDefCountClass.thingDef, true);
						r.ingredients.Add(ingredientCount2);
					}
				}
				r.defaultIngredientFilter = rm.defaultIngredientFilter;
				r.products.Add(new ThingDefCountClass(def, rm.productCount));
				r.targetCountAdjustment = rm.targetCountAdjustment;
				r.skillRequirements = rm.skillRequirements.ListFullCopyOrNull<SkillRequirement>();
				r.workSkill = rm.workSkill;
				r.workSkillLearnFactor = rm.workSkillLearnPerTick;
				r.unfinishedThingDef = rm.unfinishedThingDef;
				r.recipeUsers = rm.recipeUsers.ListFullCopyOrNull<ThingDef>();
				r.effectWorking = rm.effectWorking;
				r.soundWorking = rm.soundWorking;
				r.researchPrerequisite = rm.researchPrerequisite;
				r.factionPrerequisiteTags = rm.factionPrerequisiteTags;
				yield return r;
			}
			yield break;
		}

		private static IEnumerable<RecipeDef> DrugAdministerDefs()
		{
			foreach (ThingDef def in from d in DefDatabase<ThingDef>.AllDefs
			where d.IsDrug
			select d)
			{
				RecipeDef r = new RecipeDef();
				r.defName = "Administer_" + def.defName;
				r.label = "RecipeAdminister".Translate(new object[]
				{
					def.label
				});
				r.jobString = "RecipeAdministerJobString".Translate(new object[]
				{
					def.label
				});
				r.workerClass = typeof(Recipe_AdministerIngestible);
				r.targetsBodyPart = false;
				r.anesthetize = false;
				r.surgerySuccessChanceFactor = 99999f;
				r.modContentPack = def.modContentPack;
				r.workAmount = (float)def.ingestible.baseIngestTicks;
				IngredientCount ic = new IngredientCount();
				ic.SetBaseCount(1f);
				ic.filter.SetAllow(def, true);
				r.ingredients.Add(ic);
				r.fixedIngredientFilter.SetAllow(def, true);
				r.recipeUsers = new List<ThingDef>();
				foreach (ThingDef item in DefDatabase<ThingDef>.AllDefs.Where((ThingDef d) => d.category == ThingCategory.Pawn && d.race.IsFlesh))
				{
					r.recipeUsers.Add(item);
				}
				yield return r;
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <ImpliedRecipeDefs>c__Iterator0 : IEnumerable, IEnumerable<RecipeDef>, IEnumerator, IDisposable, IEnumerator<RecipeDef>
		{
			internal IEnumerator<RecipeDef> $locvar0;

			internal RecipeDef <r>__1;

			internal RecipeDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ImpliedRecipeDefs>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = RecipeDefGenerator.DefsFromRecipeMakers().Concat(RecipeDefGenerator.DrugAdministerDefs()).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						r = enumerator.Current;
						this.$current = r;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			RecipeDef IEnumerator<RecipeDef>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.RecipeDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<RecipeDef> IEnumerable<RecipeDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new RecipeDefGenerator.<ImpliedRecipeDefs>c__Iterator0();
			}
		}

		[CompilerGenerated]
		private sealed class <DefsFromRecipeMakers>c__Iterator1 : IEnumerable, IEnumerable<RecipeDef>, IEnumerator, IDisposable, IEnumerator<RecipeDef>
		{
			internal IEnumerator<ThingDef> $locvar0;

			internal ThingDef <def>__1;

			internal RecipeMakerProperties <rm>__2;

			internal RecipeDef <r>__2;

			internal RecipeDef $current;

			internal bool $disposing;

			internal int $PC;

			private static Func<ThingDef, bool> <>f__am$cache0;

			[DebuggerHidden]
			public <DefsFromRecipeMakers>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = (from d in DefDatabase<ThingDef>.AllDefs
					where d.recipeMaker != null
					select d).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						def = enumerator.Current;
						rm = def.recipeMaker;
						r = new RecipeDef();
						r.defName = "Make_" + def.defName;
						r.label = "RecipeMake".Translate(new object[]
						{
							def.label
						});
						r.jobString = "RecipeMakeJobString".Translate(new object[]
						{
							def.label
						});
						r.modContentPack = def.modContentPack;
						r.workAmount = (float)rm.workAmount;
						r.workSpeedStat = rm.workSpeedStat;
						r.efficiencyStat = rm.efficiencyStat;
						if (def.MadeFromStuff)
						{
							IngredientCount ingredientCount = new IngredientCount();
							ingredientCount.SetBaseCount((float)def.costStuffCount);
							ingredientCount.filter.SetAllowAllWhoCanMake(def);
							r.ingredients.Add(ingredientCount);
							r.fixedIngredientFilter.SetAllowAllWhoCanMake(def);
							r.productHasIngredientStuff = true;
						}
						if (def.costList != null)
						{
							foreach (ThingDefCountClass thingDefCountClass in def.costList)
							{
								IngredientCount ingredientCount2 = new IngredientCount();
								ingredientCount2.SetBaseCount((float)thingDefCountClass.count);
								ingredientCount2.filter.SetAllow(thingDefCountClass.thingDef, true);
								r.ingredients.Add(ingredientCount2);
							}
						}
						r.defaultIngredientFilter = rm.defaultIngredientFilter;
						r.products.Add(new ThingDefCountClass(def, rm.productCount));
						r.targetCountAdjustment = rm.targetCountAdjustment;
						r.skillRequirements = rm.skillRequirements.ListFullCopyOrNull<SkillRequirement>();
						r.workSkill = rm.workSkill;
						r.workSkillLearnFactor = rm.workSkillLearnPerTick;
						r.unfinishedThingDef = rm.unfinishedThingDef;
						r.recipeUsers = rm.recipeUsers.ListFullCopyOrNull<ThingDef>();
						r.effectWorking = rm.effectWorking;
						r.soundWorking = rm.soundWorking;
						r.researchPrerequisite = rm.researchPrerequisite;
						r.factionPrerequisiteTags = rm.factionPrerequisiteTags;
						this.$current = r;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			RecipeDef IEnumerator<RecipeDef>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.RecipeDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<RecipeDef> IEnumerable<RecipeDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new RecipeDefGenerator.<DefsFromRecipeMakers>c__Iterator1();
			}

			private static bool <>m__0(ThingDef d)
			{
				return d.recipeMaker != null;
			}
		}

		[CompilerGenerated]
		private sealed class <DrugAdministerDefs>c__Iterator2 : IEnumerable, IEnumerable<RecipeDef>, IEnumerator, IDisposable, IEnumerator<RecipeDef>
		{
			internal IEnumerator<ThingDef> $locvar0;

			internal ThingDef <def>__1;

			internal RecipeDef <r>__2;

			internal IngredientCount <ic>__2;

			internal IEnumerator<ThingDef> $locvar1;

			internal RecipeDef $current;

			internal bool $disposing;

			internal int $PC;

			private static Func<ThingDef, bool> <>f__am$cache0;

			private static Func<ThingDef, bool> <>f__am$cache1;

			[DebuggerHidden]
			public <DrugAdministerDefs>c__Iterator2()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = (from d in DefDatabase<ThingDef>.AllDefs
					where d.IsDrug
					select d).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						def = enumerator.Current;
						r = new RecipeDef();
						r.defName = "Administer_" + def.defName;
						r.label = "RecipeAdminister".Translate(new object[]
						{
							def.label
						});
						r.jobString = "RecipeAdministerJobString".Translate(new object[]
						{
							def.label
						});
						r.workerClass = typeof(Recipe_AdministerIngestible);
						r.targetsBodyPart = false;
						r.anesthetize = false;
						r.surgerySuccessChanceFactor = 99999f;
						r.modContentPack = def.modContentPack;
						r.workAmount = (float)def.ingestible.baseIngestTicks;
						ic = new IngredientCount();
						ic.SetBaseCount(1f);
						ic.filter.SetAllow(def, true);
						r.ingredients.Add(ic);
						r.fixedIngredientFilter.SetAllow(def, true);
						r.recipeUsers = new List<ThingDef>();
						enumerator2 = (from d in DefDatabase<ThingDef>.AllDefs
						where d.category == ThingCategory.Pawn && d.race.IsFlesh
						select d).GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								ThingDef item = enumerator2.Current;
								r.recipeUsers.Add(item);
							}
						}
						finally
						{
							if (enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
						this.$current = r;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				this.$PC = -1;
				return false;
			}

			RecipeDef IEnumerator<RecipeDef>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.RecipeDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<RecipeDef> IEnumerable<RecipeDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new RecipeDefGenerator.<DrugAdministerDefs>c__Iterator2();
			}

			private static bool <>m__0(ThingDef d)
			{
				return d.IsDrug;
			}

			private static bool <>m__1(ThingDef d)
			{
				return d.category == ThingCategory.Pawn && d.race.IsFlesh;
			}
		}
	}
}
