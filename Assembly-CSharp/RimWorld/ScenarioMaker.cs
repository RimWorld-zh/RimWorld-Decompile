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
	public static class ScenarioMaker
	{
		private static Scenario scen;

		public static Scenario GeneratingScenario
		{
			get
			{
				return ScenarioMaker.scen;
			}
		}

		public static Scenario GenerateNewRandomScenario(string seed)
		{
			Rand.PushState();
			Rand.Seed = seed.GetHashCode();
			int @int = Rand.Int;
			int[] array = new int[10];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Rand.Int;
			}
			int int2 = Rand.Int;
			ScenarioMaker.scen = new Scenario();
			ScenarioMaker.scen.Category = ScenarioCategory.CustomLocal;
			ScenarioMaker.scen.name = NameGenerator.GenerateName(RulePackDefOf.NamerScenario, null, false, null, null);
			ScenarioMaker.scen.description = null;
			ScenarioMaker.scen.summary = null;
			Rand.Seed = @int;
			ScenarioMaker.scen.playerFaction = (ScenPart_PlayerFaction)ScenarioMaker.MakeScenPart(ScenPartDefOf.PlayerFaction);
			ScenarioMaker.scen.parts.Add(ScenarioMaker.MakeScenPart(ScenPartDefOf.ConfigPage_ConfigureStartingPawns));
			ScenarioMaker.scen.parts.Add(ScenarioMaker.MakeScenPart(ScenPartDefOf.PlayerPawnsArriveMethod));
			Rand.Seed = array[0];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.PlayerPawnFilter, Rand.RangeInclusive(-1, 2));
			Rand.Seed = array[1];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.StartingImportant, Rand.RangeInclusive(0, 2));
			Rand.Seed = array[2];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.PlayerPawnModifier, Rand.RangeInclusive(-1, 2));
			Rand.Seed = array[3];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.Rule, Rand.RangeInclusive(-2, 3));
			Rand.Seed = array[4];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.StartingItem, Rand.RangeInclusive(0, 6));
			Rand.Seed = array[5];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.WorldThing, Rand.RangeInclusive(-3, 6));
			Rand.Seed = array[6];
			ScenarioMaker.AddCategoryScenParts(ScenarioMaker.scen, ScenPartCategory.GameCondition, Rand.RangeInclusive(-1, 2));
			Rand.Seed = int2;
			foreach (ScenPart scenPart in ScenarioMaker.scen.AllParts)
			{
				scenPart.Randomize();
			}
			for (int j = 0; j < ScenarioMaker.scen.parts.Count; j++)
			{
				for (int k = 0; k < ScenarioMaker.scen.parts.Count; k++)
				{
					if (j != k)
					{
						if (ScenarioMaker.scen.parts[j].TryMerge(ScenarioMaker.scen.parts[k]))
						{
							ScenarioMaker.scen.parts.RemoveAt(k);
							k--;
							if (j > k)
							{
								j--;
							}
						}
					}
				}
			}
			for (int l = 0; l < ScenarioMaker.scen.parts.Count; l++)
			{
				for (int m = 0; m < ScenarioMaker.scen.parts.Count; m++)
				{
					if (l != m)
					{
						if (!ScenarioMaker.scen.parts[l].CanCoexistWith(ScenarioMaker.scen.parts[m]))
						{
							ScenarioMaker.scen.parts.RemoveAt(m);
							m--;
							if (l > m)
							{
								l--;
							}
						}
					}
				}
			}
			foreach (string text in ScenarioMaker.scen.ConfigErrors())
			{
				Log.Error(text, false);
			}
			Rand.PopState();
			Scenario result = ScenarioMaker.scen;
			ScenarioMaker.scen = null;
			return result;
		}

		private static void AddCategoryScenParts(Scenario scen, ScenPartCategory cat, int count)
		{
			scen.parts.AddRange(ScenarioMaker.RandomScenPartsOfCategory(scen, cat, count));
		}

		private static IEnumerable<ScenPart> RandomScenPartsOfCategory(Scenario scen, ScenPartCategory cat, int count)
		{
			if (count <= 0)
			{
				yield break;
			}
			IEnumerable<ScenPartDef> allowedParts = from d in ScenarioMaker.AddableParts(scen)
			where d.category == cat
			select d;
			int numYielded = 0;
			int numTries = 0;
			while (numYielded < count)
			{
				if (!allowedParts.Any<ScenPartDef>())
				{
					yield break;
				}
				ScenPartDef def = allowedParts.RandomElementByWeight((ScenPartDef d) => d.selectionWeight);
				ScenPart newPart = ScenarioMaker.MakeScenPart(def);
				if (ScenarioMaker.CanAddPart(scen, newPart))
				{
					yield return newPart;
					numYielded++;
				}
				numTries++;
				if (numTries > 100)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not add ScenPart of category ",
						cat,
						" to scenario ",
						scen,
						" after 50 tries."
					}), false);
					yield break;
				}
			}
			yield break;
		}

		public static IEnumerable<ScenPartDef> AddableParts(Scenario scen)
		{
			return from d in DefDatabase<ScenPartDef>.AllDefs
			where scen.AllParts.Count((ScenPart p) => p.def == d) < d.maxUses
			select d;
		}

		private static bool CanAddPart(Scenario scen, ScenPart newPart)
		{
			for (int i = 0; i < scen.parts.Count; i++)
			{
				if (!newPart.CanCoexistWith(scen.parts[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static ScenPart MakeScenPart(ScenPartDef def)
		{
			ScenPart scenPart = (ScenPart)Activator.CreateInstance(def.scenPartClass);
			scenPart.def = def;
			return scenPart;
		}

		[CompilerGenerated]
		private sealed class <RandomScenPartsOfCategory>c__Iterator0 : IEnumerable, IEnumerable<ScenPart>, IEnumerator, IDisposable, IEnumerator<ScenPart>
		{
			internal int count;

			internal Scenario scen;

			internal ScenPartCategory cat;

			internal IEnumerable<ScenPartDef> <allowedParts>__0;

			internal int <numYielded>__0;

			internal int <numTries>__0;

			internal ScenPartDef <def>__1;

			internal ScenPart <newPart>__1;

			internal ScenPart $current;

			internal bool $disposing;

			internal int $PC;

			private ScenarioMaker.<RandomScenPartsOfCategory>c__Iterator0.<RandomScenPartsOfCategory>c__AnonStorey1 $locvar0;

			private static Func<ScenPartDef, float> <>f__am$cache0;

			[DebuggerHidden]
			public <RandomScenPartsOfCategory>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (count <= 0)
					{
						return false;
					}
					allowedParts = from d in ScenarioMaker.AddableParts(scen)
					where d.category == cat
					select d;
					numYielded = 0;
					numTries = 0;
					goto IL_191;
				case 1u:
					numYielded++;
					break;
				default:
					return false;
				}
				IL_12C:
				numTries++;
				if (numTries > 100)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not add ScenPart of category ",
						<RandomScenPartsOfCategory>c__AnonStorey.cat,
						" to scenario ",
						scen,
						" after 50 tries."
					}), false);
					return false;
				}
				IL_191:
				if (numYielded >= count)
				{
					this.$PC = -1;
				}
				else if (allowedParts.Any<ScenPartDef>())
				{
					def = allowedParts.RandomElementByWeight((ScenPartDef d) => d.selectionWeight);
					newPart = ScenarioMaker.MakeScenPart(def);
					if (ScenarioMaker.CanAddPart(scen, newPart))
					{
						this.$current = newPart;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_12C;
				}
				return false;
			}

			ScenPart IEnumerator<ScenPart>.Current
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
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<RimWorld.ScenPart>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<ScenPart> IEnumerable<ScenPart>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ScenarioMaker.<RandomScenPartsOfCategory>c__Iterator0 <RandomScenPartsOfCategory>c__Iterator = new ScenarioMaker.<RandomScenPartsOfCategory>c__Iterator0();
				<RandomScenPartsOfCategory>c__Iterator.count = count;
				<RandomScenPartsOfCategory>c__Iterator.scen = scen;
				<RandomScenPartsOfCategory>c__Iterator.cat = cat;
				return <RandomScenPartsOfCategory>c__Iterator;
			}

			private static float <>m__0(ScenPartDef d)
			{
				return d.selectionWeight;
			}

			private sealed class <RandomScenPartsOfCategory>c__AnonStorey1
			{
				internal ScenPartCategory cat;

				internal ScenarioMaker.<RandomScenPartsOfCategory>c__Iterator0 <>f__ref$0;

				public <RandomScenPartsOfCategory>c__AnonStorey1()
				{
				}

				internal bool <>m__0(ScenPartDef d)
				{
					return d.category == this.cat;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <AddableParts>c__AnonStorey2
		{
			internal Scenario scen;

			public <AddableParts>c__AnonStorey2()
			{
			}

			internal bool <>m__0(ScenPartDef d)
			{
				return this.scen.AllParts.Count((ScenPart p) => p.def == d) < d.maxUses;
			}

			private sealed class <AddableParts>c__AnonStorey3
			{
				internal ScenPartDef d;

				internal ScenarioMaker.<AddableParts>c__AnonStorey2 <>f__ref$2;

				public <AddableParts>c__AnonStorey3()
				{
				}

				internal bool <>m__0(ScenPart p)
				{
					return p.def == this.d;
				}
			}
		}
	}
}
