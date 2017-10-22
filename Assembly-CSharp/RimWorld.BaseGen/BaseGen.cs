using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	public static class BaseGen
	{
		public static GlobalSettings globalSettings = new GlobalSettings();

		public static SymbolStack symbolStack = new SymbolStack();

		private static Dictionary<string, List<RuleDef>> rulesBySymbol = new Dictionary<string, List<RuleDef>>();

		private static bool working;

		private const int MaxResolvedSymbols = 100000;

		private static List<SymbolResolver> tmpResolvers = new List<SymbolResolver>();

		public static void Reset()
		{
			BaseGen.rulesBySymbol.Clear();
			List<RuleDef> allDefsListForReading = DefDatabase<RuleDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				List<RuleDef> list = default(List<RuleDef>);
				if (!BaseGen.rulesBySymbol.TryGetValue(allDefsListForReading[i].symbol, out list))
				{
					list = new List<RuleDef>();
					BaseGen.rulesBySymbol.Add(allDefsListForReading[i].symbol, list);
				}
				list.Add(allDefsListForReading[i]);
			}
		}

		public static void Generate()
		{
			if (BaseGen.working)
			{
				Log.Error("Cannot call Generate() while already generating. Nested calls are not allowed.");
			}
			else
			{
				BaseGen.working = true;
				try
				{
					if (BaseGen.symbolStack.Empty)
					{
						Log.Warning("Symbol stack is empty.");
					}
					else if (BaseGen.globalSettings.map == null)
					{
						Log.Error("Called BaseGen.Resolve() with null map.");
					}
					else
					{
						int num = BaseGen.symbolStack.Count - 1;
						int num2 = 0;
						while (true)
						{
							if (!BaseGen.symbolStack.Empty)
							{
								num2++;
								if (num2 <= 100000)
								{
									Pair<string, ResolveParams> toResolve = BaseGen.symbolStack.Pop();
									if (BaseGen.symbolStack.Count == num)
									{
										GlobalSettings obj = BaseGen.globalSettings;
										ResolveParams second = toResolve.Second;
										obj.mainRect = second.rect;
										num--;
									}
									try
									{
										BaseGen.Resolve(toResolve);
									}
									catch (Exception ex)
									{
										Log.Error("Error while resolving symbol \"" + toResolve.First + "\" with params=" + toResolve.Second + "\n\nException: " + ex);
									}
									continue;
								}
								break;
							}
							return;
						}
						Log.Error("Error in BaseGen: Too many iterations. Infinite loop?");
					}
				}
				catch (Exception arg)
				{
					Log.Error("Error in BaseGen: " + arg);
				}
				finally
				{
					BaseGen.working = false;
					BaseGen.symbolStack.Clear();
					BaseGen.globalSettings.Clear();
				}
			}
		}

		private static void Resolve(Pair<string, ResolveParams> toResolve)
		{
			string first = toResolve.First;
			ResolveParams second = toResolve.Second;
			BaseGen.tmpResolvers.Clear();
			List<RuleDef> list = default(List<RuleDef>);
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
			if (!BaseGen.tmpResolvers.Any())
			{
				Log.Warning("Could not find any RuleDef for symbol \"" + first + "\" with any resolver that could resolve " + second);
			}
			else
			{
				SymbolResolver symbolResolver2 = BaseGen.tmpResolvers.RandomElementByWeight((Func<SymbolResolver, float>)((SymbolResolver x) => x.selectionWeight));
				symbolResolver2.Resolve(second);
			}
		}
	}
}
