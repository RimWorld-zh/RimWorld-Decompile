using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02000390 RID: 912
	public static class BaseGen
	{
		// Token: 0x040009A7 RID: 2471
		public static GlobalSettings globalSettings = new GlobalSettings();

		// Token: 0x040009A8 RID: 2472
		public static SymbolStack symbolStack = new SymbolStack();

		// Token: 0x040009A9 RID: 2473
		private static Dictionary<string, List<RuleDef>> rulesBySymbol = new Dictionary<string, List<RuleDef>>();

		// Token: 0x040009AA RID: 2474
		private static bool working;

		// Token: 0x040009AB RID: 2475
		private const int MaxResolvedSymbols = 100000;

		// Token: 0x040009AC RID: 2476
		private static List<SymbolResolver> tmpResolvers = new List<SymbolResolver>();

		// Token: 0x06000FE4 RID: 4068 RVA: 0x00085578 File Offset: 0x00083978
		public static void Reset()
		{
			BaseGen.rulesBySymbol.Clear();
			List<RuleDef> allDefsListForReading = DefDatabase<RuleDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				List<RuleDef> list;
				if (!BaseGen.rulesBySymbol.TryGetValue(allDefsListForReading[i].symbol, out list))
				{
					list = new List<RuleDef>();
					BaseGen.rulesBySymbol.Add(allDefsListForReading[i].symbol, list);
				}
				list.Add(allDefsListForReading[i]);
			}
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x000855F8 File Offset: 0x000839F8
		public static void Generate()
		{
			if (BaseGen.working)
			{
				Log.Error("Cannot call Generate() while already generating. Nested calls are not allowed.", false);
			}
			else
			{
				BaseGen.working = true;
				try
				{
					if (BaseGen.symbolStack.Empty)
					{
						Log.Warning("Symbol stack is empty.", false);
					}
					else if (BaseGen.globalSettings.map == null)
					{
						Log.Error("Called BaseGen.Resolve() with null map.", false);
					}
					else
					{
						int num = BaseGen.symbolStack.Count - 1;
						int num2 = 0;
						while (!BaseGen.symbolStack.Empty)
						{
							num2++;
							if (num2 > 100000)
							{
								Log.Error("Error in BaseGen: Too many iterations. Infinite loop?", false);
								break;
							}
							Pair<string, ResolveParams> toResolve = BaseGen.symbolStack.Pop();
							if (BaseGen.symbolStack.Count == num)
							{
								BaseGen.globalSettings.mainRect = toResolve.Second.rect;
								num--;
							}
							try
							{
								BaseGen.Resolve(toResolve);
							}
							catch (Exception ex)
							{
								Log.Error(string.Concat(new object[]
								{
									"Error while resolving symbol \"",
									toResolve.First,
									"\" with params=",
									toResolve.Second,
									"\n\nException: ",
									ex
								}), false);
							}
						}
					}
				}
				catch (Exception arg)
				{
					Log.Error("Error in BaseGen: " + arg, false);
				}
				finally
				{
					BaseGen.working = false;
					BaseGen.symbolStack.Clear();
					BaseGen.globalSettings.Clear();
				}
			}
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x000857D0 File Offset: 0x00083BD0
		private static void Resolve(Pair<string, ResolveParams> toResolve)
		{
			string first = toResolve.First;
			ResolveParams second = toResolve.Second;
			BaseGen.tmpResolvers.Clear();
			List<RuleDef> list;
			if (BaseGen.rulesBySymbol.TryGetValue(first, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					RuleDef ruleDef = list[i];
					for (int j = 0; j < ruleDef.resolvers.Count; j++)
					{
						SymbolResolver symbolResolver = ruleDef.resolvers[j];
						if (symbolResolver.CanResolve(second))
						{
							BaseGen.tmpResolvers.Add(symbolResolver);
						}
					}
				}
			}
			if (!BaseGen.tmpResolvers.Any<SymbolResolver>())
			{
				Log.Warning(string.Concat(new object[]
				{
					"Could not find any RuleDef for symbol \"",
					first,
					"\" with any resolver that could resolve ",
					second
				}), false);
			}
			else
			{
				SymbolResolver symbolResolver2 = BaseGen.tmpResolvers.RandomElementByWeight((SymbolResolver x) => x.selectionWeight);
				symbolResolver2.Resolve(second);
			}
		}
	}
}
