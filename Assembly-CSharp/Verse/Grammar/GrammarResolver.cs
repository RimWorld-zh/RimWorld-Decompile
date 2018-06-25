using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using RimWorld;

namespace Verse.Grammar
{
	// Token: 0x02000BE4 RID: 3044
	public static class GrammarResolver
	{
		// Token: 0x04002D73 RID: 11635
		private static SimpleLinearPool<List<GrammarResolver.RuleEntry>> rulePool = new SimpleLinearPool<List<GrammarResolver.RuleEntry>>();

		// Token: 0x04002D74 RID: 11636
		private static Dictionary<string, List<GrammarResolver.RuleEntry>> rules = new Dictionary<string, List<GrammarResolver.RuleEntry>>();

		// Token: 0x04002D75 RID: 11637
		private static int loopCount;

		// Token: 0x04002D76 RID: 11638
		private static StringBuilder logSb;

		// Token: 0x04002D77 RID: 11639
		private const int DepthLimit = 50;

		// Token: 0x04002D78 RID: 11640
		private const int LoopsLimit = 1000;

		// Token: 0x04002D79 RID: 11641
		private static Regex Spaces = new Regex(" +([,.])");

		// Token: 0x06004270 RID: 17008 RVA: 0x0022F4F4 File Offset: 0x0022D8F4
		private static void AddRule(Rule rule)
		{
			List<GrammarResolver.RuleEntry> list = null;
			if (!GrammarResolver.rules.TryGetValue(rule.keyword, out list))
			{
				list = GrammarResolver.rulePool.Get();
				list.Clear();
				GrammarResolver.rules[rule.keyword] = list;
			}
			list.Add(new GrammarResolver.RuleEntry(rule));
		}

		// Token: 0x06004271 RID: 17009 RVA: 0x0022F54C File Offset: 0x0022D94C
		public static string Resolve(string rootKeyword, GrammarRequest request, string debugLabel = null, bool forceLog = false)
		{
			bool flag = forceLog || DebugViewSettings.logGrammarResolution;
			GrammarResolver.rules.Clear();
			GrammarResolver.rulePool.Clear();
			if (flag)
			{
				GrammarResolver.logSb = new StringBuilder();
			}
			List<Rule> list = request.GetRules();
			if (list != null)
			{
				if (flag)
				{
					GrammarResolver.logSb.AppendLine("Custom rules:");
				}
				for (int i = 0; i < list.Count; i++)
				{
					GrammarResolver.AddRule(list[i]);
					if (flag)
					{
						GrammarResolver.logSb.AppendLine("  " + list[i].ToString());
					}
				}
				if (flag)
				{
					GrammarResolver.logSb.AppendLine();
				}
			}
			List<RulePackDef> includes = request.GetIncludes();
			if (includes != null)
			{
				HashSet<RulePackDef> hashSet = new HashSet<RulePackDef>();
				List<RulePackDef> list2 = new List<RulePackDef>(includes);
				if (flag)
				{
					GrammarResolver.logSb.AppendLine("Includes:");
				}
				while (list2.Count > 0)
				{
					RulePackDef rulePackDef = list2[list2.Count - 1];
					list2.RemoveLast<RulePackDef>();
					if (!hashSet.Contains(rulePackDef))
					{
						if (flag)
						{
							GrammarResolver.logSb.AppendLine(string.Format("  {0}", rulePackDef.defName));
						}
						hashSet.Add(rulePackDef);
						List<Rule> rulesImmediate = rulePackDef.RulesImmediate;
						if (rulesImmediate != null)
						{
							foreach (Rule rule in rulePackDef.RulesImmediate)
							{
								GrammarResolver.AddRule(rule);
							}
						}
						if (!rulePackDef.include.NullOrEmpty<RulePackDef>())
						{
							list2.AddRange(rulePackDef.include);
						}
					}
				}
				if (flag)
				{
					GrammarResolver.logSb.AppendLine();
				}
			}
			List<RulePack> includesBare = request.GetIncludesBare();
			if (includesBare != null)
			{
				if (flag)
				{
					GrammarResolver.logSb.AppendLine("Bare includes:");
				}
				for (int j = 0; j < includesBare.Count; j++)
				{
					RulePack rulePack = includesBare[j];
					for (int k = 0; k < rulePack.Rules.Count; k++)
					{
						GrammarResolver.AddRule(rulePack.Rules[k]);
						if (flag)
						{
							GrammarResolver.logSb.AppendLine("  " + rulePack.Rules[k].ToString());
						}
					}
				}
				if (flag)
				{
					GrammarResolver.logSb.AppendLine();
				}
			}
			for (int l = 0; l < RulePackDefOf.GlobalUtility.RulesPlusIncludes.Count; l++)
			{
				GrammarResolver.AddRule(RulePackDefOf.GlobalUtility.RulesPlusIncludes[l]);
			}
			GrammarResolver.loopCount = 0;
			Dictionary<string, string> constants = request.GetConstants();
			if (flag)
			{
				if (constants != null)
				{
					GrammarResolver.logSb.AppendLine("Constants:");
					foreach (KeyValuePair<string, string> keyValuePair in constants)
					{
						GrammarResolver.logSb.AppendLine(string.Format("  {0}: {1}", keyValuePair.Key, keyValuePair.Value));
					}
				}
			}
			string text = "err";
			bool flag2 = false;
			if (!GrammarResolver.TryResolveRecursive(new GrammarResolver.RuleEntry(new Rule_String("", "[" + rootKeyword + "]")), 0, constants, out text, flag))
			{
				flag2 = true;
				text = "Could not resolve any root: " + rootKeyword;
				if (!debugLabel.NullOrEmpty())
				{
					text = text + " debugLabel: " + debugLabel;
				}
				else if (!request.Includes.NullOrEmpty<RulePackDef>())
				{
					text = text + " firstRulePack: " + request.Includes[0].defName;
				}
				if (flag)
				{
					GrammarResolver.logSb.Insert(0, "GrammarResolver failed to resolve a text (rootKeyword: " + rootKeyword + ")\n");
				}
				else
				{
					GrammarResolver.Resolve(rootKeyword, request, debugLabel, true);
				}
			}
			text = GenText.CapitalizeSentences(Find.ActiveLanguageWorker.PostProcessed(text));
			text = GrammarResolver.Spaces.Replace(text, (Match match) => match.Groups[1].Value);
			text = text.Trim();
			if (flag && flag2)
			{
				if (DebugViewSettings.logGrammarResolution)
				{
					Log.Error(GrammarResolver.logSb.ToString().Trim(), false);
				}
				else
				{
					Log.ErrorOnce(GrammarResolver.logSb.ToString().Trim(), GrammarResolver.logSb.ToString().GetHashCode(), false);
				}
			}
			else if (flag)
			{
				Log.Message(GrammarResolver.logSb.ToString().Trim(), false);
			}
			return text;
		}

		// Token: 0x06004272 RID: 17010 RVA: 0x0022FA7C File Offset: 0x0022DE7C
		private static bool TryResolveRecursive(GrammarResolver.RuleEntry entry, int depth, Dictionary<string, string> constants, out string output, bool log)
		{
			if (log)
			{
				GrammarResolver.logSb.AppendLine();
				GrammarResolver.logSb.Append(depth.ToStringCached() + " ");
				for (int i = 0; i < depth; i++)
				{
					GrammarResolver.logSb.Append("   ");
				}
				GrammarResolver.logSb.Append(entry + " ");
			}
			GrammarResolver.loopCount++;
			bool result;
			if (GrammarResolver.loopCount > 1000)
			{
				Log.Error("Hit loops limit resolving grammar.", false);
				output = "HIT_LOOPS_LIMIT";
				if (log)
				{
					GrammarResolver.logSb.Append("UNRESOLVABLE: Hit loops limit");
				}
				result = false;
			}
			else if (depth > 50)
			{
				Log.Error("Grammar recurred too deep while resolving keyword (>" + 50 + " deep)", false);
				output = "DEPTH_LIMIT_REACHED";
				if (log)
				{
					GrammarResolver.logSb.Append("UNRESOLVABLE: Depth limit reached");
				}
				result = false;
			}
			else
			{
				string text = entry.rule.Generate();
				bool flag = false;
				int num = -1;
				for (int j = 0; j < text.Length; j++)
				{
					char c = text[j];
					if (c == '[')
					{
						num = j;
					}
					if (c == ']')
					{
						if (num == -1)
						{
							Log.Error("Could not resolve rule " + text + ": mismatched brackets.", false);
							output = "MISMATCHED_BRACKETS";
							if (log)
							{
								GrammarResolver.logSb.Append("UNRESOLVABLE: Mismatched brackets");
							}
							flag = true;
						}
						else
						{
							string text2 = text.Substring(num + 1, j - num - 1);
							string str;
							for (;;)
							{
								GrammarResolver.RuleEntry ruleEntry = GrammarResolver.RandomPossiblyResolvableEntry(text2, constants);
								if (ruleEntry == null)
								{
									goto Block_11;
								}
								ruleEntry.uses++;
								if (GrammarResolver.TryResolveRecursive(ruleEntry, depth + 1, constants, out str, log))
								{
									goto Block_13;
								}
								ruleEntry.MarkKnownUnresolvable();
							}
							IL_239:
							goto IL_23A;
							Block_11:
							entry.MarkKnownUnresolvable();
							output = "CANNOT_RESOLVE_SUBSYMBOL:" + text2;
							if (log)
							{
								GrammarResolver.logSb.Append("UNRESOLVABLE: Cannot resolve subsymbol '" + text2 + "'");
							}
							flag = true;
							goto IL_239;
							Block_13:
							text = text.Substring(0, num) + str + text.Substring(j + 1);
							j = num;
						}
						IL_23A:;
					}
				}
				output = text;
				result = !flag;
			}
			return result;
		}

		// Token: 0x06004273 RID: 17011 RVA: 0x0022FCE8 File Offset: 0x0022E0E8
		private static GrammarResolver.RuleEntry RandomPossiblyResolvableEntry(string keyword, Dictionary<string, string> constants)
		{
			List<GrammarResolver.RuleEntry> list = GrammarResolver.rules.TryGetValue(keyword, null);
			GrammarResolver.RuleEntry result;
			if (list == null)
			{
				result = null;
			}
			else
			{
				result = list.RandomElementByWeightWithFallback(delegate(GrammarResolver.RuleEntry rule)
				{
					float result2;
					if (rule.knownUnresolvable || !rule.ValidateConstantConstraints(constants))
					{
						result2 = 0f;
					}
					else
					{
						result2 = rule.SelectionWeight;
					}
					return result2;
				}, null);
			}
			return result;
		}

		// Token: 0x02000BE5 RID: 3045
		private class RuleEntry
		{
			// Token: 0x04002D7B RID: 11643
			public Rule rule;

			// Token: 0x04002D7C RID: 11644
			public bool knownUnresolvable;

			// Token: 0x04002D7D RID: 11645
			public bool constantConstraintsChecked;

			// Token: 0x04002D7E RID: 11646
			public bool constantConstraintsValid;

			// Token: 0x04002D7F RID: 11647
			public int uses = 0;

			// Token: 0x06004276 RID: 17014 RVA: 0x0022FD81 File Offset: 0x0022E181
			public RuleEntry(Rule rule)
			{
				this.rule = rule;
				this.knownUnresolvable = false;
			}

			// Token: 0x17000A74 RID: 2676
			// (get) Token: 0x06004277 RID: 17015 RVA: 0x0022FDA0 File Offset: 0x0022E1A0
			public float SelectionWeight
			{
				get
				{
					return this.rule.BaseSelectionWeight * 100000f / (float)((this.uses + 1) * 1000);
				}
			}

			// Token: 0x06004278 RID: 17016 RVA: 0x0022FDD6 File Offset: 0x0022E1D6
			public void MarkKnownUnresolvable()
			{
				this.knownUnresolvable = true;
			}

			// Token: 0x06004279 RID: 17017 RVA: 0x0022FDE0 File Offset: 0x0022E1E0
			public bool ValidateConstantConstraints(Dictionary<string, string> constraints)
			{
				if (!this.constantConstraintsChecked)
				{
					this.constantConstraintsValid = true;
					if (this.rule.constantConstraints != null)
					{
						for (int i = 0; i < this.rule.constantConstraints.Count; i++)
						{
							Rule.ConstantConstraint constantConstraint = this.rule.constantConstraints[i];
							string a = (constraints == null) ? "" : constraints.TryGetValue(constantConstraint.key, "");
							if (a == constantConstraint.value != constantConstraint.equality)
							{
								this.constantConstraintsValid = false;
								break;
							}
						}
					}
					this.constantConstraintsChecked = true;
				}
				return this.constantConstraintsValid;
			}

			// Token: 0x0600427A RID: 17018 RVA: 0x0022FEA8 File Offset: 0x0022E2A8
			public override string ToString()
			{
				return this.rule.ToString();
			}
		}
	}
}
