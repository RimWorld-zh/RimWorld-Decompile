using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;

namespace Verse.Grammar
{
	public static class GrammarResolver
	{
		private class RuleEntry
		{
			public Rule rule;

			public bool knownUnresolvable;

			public bool constantConstraintsChecked;

			public bool constantConstraintsValid;

			public int uses = 0;

			public float SelectionWeight
			{
				get
				{
					return (float)(this.rule.BaseSelectionWeight * 100000.0 / (float)((this.uses + 1) * 1000));
				}
			}

			public RuleEntry(Rule rule)
			{
				this.rule = rule;
				this.knownUnresolvable = false;
			}

			public void MarkKnownUnresolvable()
			{
				this.knownUnresolvable = true;
			}

			public bool ValidateConstantConstraints(Dictionary<string, string> constraints)
			{
				if (!this.constantConstraintsChecked)
				{
					this.constantConstraintsValid = true;
					if (this.rule.constantConstraints != null)
					{
						for (int i = 0; i < this.rule.constantConstraints.Count; i++)
						{
							Rule.ConstrantConstraint constrantConstraint = this.rule.constantConstraints[i];
							if (!(constrantConstraint.value == "") || constraints != null)
							{
								if (constraints == null)
								{
									this.constantConstraintsValid = false;
									break;
								}
								if (constraints.TryGetValue(constrantConstraint.key) != constrantConstraint.value)
								{
									this.constantConstraintsValid = false;
									break;
								}
							}
						}
					}
					this.constantConstraintsChecked = true;
				}
				return this.constantConstraintsValid;
			}

			public override string ToString()
			{
				return this.rule.ToString();
			}
		}

		private static SimpleLinearPool<List<RuleEntry>> rulePool = new SimpleLinearPool<List<RuleEntry>>();

		private static Dictionary<string, List<RuleEntry>> rules = new Dictionary<string, List<RuleEntry>>();

		private static int loopCount;

		private static StringBuilder logSb;

		private const int DepthLimit = 50;

		private const int LoopsLimit = 1000;

		private static bool LogOn
		{
			get
			{
				return DebugViewSettings.logGrammarResolution;
			}
		}

		private static void AddRule(Rule rule)
		{
			List<RuleEntry> list = null;
			if (!GrammarResolver.rules.TryGetValue(rule.keyword, out list))
			{
				list = GrammarResolver.rulePool.Get();
				list.Clear();
				GrammarResolver.rules[rule.keyword] = list;
			}
			list.Add(new RuleEntry(rule));
		}

		public static string Resolve(string rootKeyword, List<Rule> rawRules, Dictionary<string, string> constants = null, string debugLabel = null)
		{
			GrammarResolver.rules.Clear();
			GrammarResolver.rulePool.Clear();
			for (int i = 0; i < rawRules.Count; i++)
			{
				GrammarResolver.AddRule(rawRules[i]);
			}
			for (int j = 0; j < RulePackDefOf.GlobalUtility.Rules.Count; j++)
			{
				GrammarResolver.AddRule(RulePackDefOf.GlobalUtility.Rules[j]);
			}
			GrammarResolver.loopCount = 0;
			if (GrammarResolver.LogOn)
			{
				GrammarResolver.logSb = new StringBuilder();
				if (constants != null)
				{
					GrammarResolver.logSb.AppendLine("Constants:");
					foreach (KeyValuePair<string, string> item in constants)
					{
						GrammarResolver.logSb.AppendLine(string.Format("  {0}: {1}", item.Key, item.Value));
					}
				}
			}
			string text = "err";
			if (!GrammarResolver.TryResolveRecursive(new RuleEntry(new Rule_String("", "[" + rootKeyword + "]")), 0, constants, out text))
			{
				text = "Could not resolve any root: " + rootKeyword;
				if (!debugLabel.NullOrEmpty())
				{
					text = text + " debugLabel: " + debugLabel;
				}
				if (GrammarResolver.LogOn)
				{
					GrammarResolver.logSb.Insert(0, "FAILED TO RESOLVE\n");
				}
			}
			text = GenText.CapitalizeSentences(Find.ActiveLanguageWorker.PostProcessed(text));
			if (GrammarResolver.LogOn)
			{
				Log.Message(GrammarResolver.logSb.ToString().Trim());
			}
			return text;
		}

		private static bool TryResolveRecursive(RuleEntry entry, int depth, Dictionary<string, string> constants, out string output)
		{
			if (GrammarResolver.LogOn)
			{
				GrammarResolver.logSb.AppendLine();
				GrammarResolver.logSb.Append(depth.ToStringCached() + " ");
				for (int num = 0; num < depth; num++)
				{
					GrammarResolver.logSb.Append("   ");
				}
				GrammarResolver.logSb.Append(entry + " ");
			}
			GrammarResolver.loopCount++;
			bool result;
			if (GrammarResolver.loopCount > 1000)
			{
				Log.Error("Hit loops limit resolving grammar.");
				output = "HIT_LOOPS_LIMIT";
				if (GrammarResolver.LogOn)
				{
					GrammarResolver.logSb.Append("UNRESOLVABLE: Hit loops limit");
				}
				result = false;
			}
			else if (depth > 50)
			{
				Log.Error("Grammar recurred too deep while resolving keyword (>" + 50 + " deep)");
				output = "DEPTH_LIMIT_REACHED";
				if (GrammarResolver.LogOn)
				{
					GrammarResolver.logSb.Append("UNRESOLVABLE: Depth limit reached");
				}
				result = false;
			}
			else
			{
				string text = entry.rule.Generate();
				bool flag = false;
				int num2 = -1;
				for (int i = 0; i < text.Length; i++)
				{
					char c = text[i];
					if (c == '[')
					{
						num2 = i;
					}
					string str = default(string);
					if (c == ']')
					{
						if (num2 == -1)
						{
							Log.Error("Could not resolve rule " + text + ": mismatched brackets.");
							output = "MISMATCHED_BRACKETS";
							if (GrammarResolver.LogOn)
							{
								GrammarResolver.logSb.Append("UNRESOLVABLE: Mismatched brackets");
							}
							flag = true;
						}
						else
						{
							string text2 = text.Substring(num2 + 1, i - num2 - 1);
							while (true)
							{
								RuleEntry ruleEntry = GrammarResolver.RandomPossiblyResolvableEntry(text2, constants);
								if (ruleEntry != null)
								{
									ruleEntry.uses++;
									if (GrammarResolver.TryResolveRecursive(ruleEntry, depth + 1, constants, out str))
										goto IL_0211;
									ruleEntry.MarkKnownUnresolvable();
									continue;
								}
								break;
							}
							entry.MarkKnownUnresolvable();
							output = "CANNOT_RESOLVE_SUBKEYWORD:" + text2;
							if (GrammarResolver.LogOn)
							{
								GrammarResolver.logSb.Append("UNRESOLVABLE: Cannot resolve sub-keyword '" + text2 + "'");
							}
							flag = true;
						}
					}
					continue;
					IL_0211:
					text = text.Substring(0, num2) + str + text.Substring(i + 1);
					i = num2;
				}
				output = text;
				result = !flag;
			}
			return result;
		}

		private static RuleEntry RandomPossiblyResolvableEntry(string keyword, Dictionary<string, string> constants)
		{
			List<RuleEntry> list = GrammarResolver.rules.TryGetValue(keyword);
			return (list != null) ? list.RandomElementByWeightWithFallback((Func<RuleEntry, float>)((RuleEntry rule) => (float)((!rule.knownUnresolvable && rule.ValidateConstantConstraints(constants)) ? rule.SelectionWeight : 0.0)), null) : null;
		}
	}
}
