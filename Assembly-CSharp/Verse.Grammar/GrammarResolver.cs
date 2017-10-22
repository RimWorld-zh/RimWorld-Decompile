using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Verse.Grammar
{
	public static class GrammarResolver
	{
		private class RuleEntry
		{
			public Rule rule;

			public bool knownUnresolvable;

			public int uses;

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

			public override string ToString()
			{
				return this.rule.ToString();
			}
		}

		private const int DepthLimit = 50;

		private const int LoopsLimit = 100;

		private static List<RuleEntry> rules = new List<RuleEntry>();

		private static int loopCount;

		private static StringBuilder logSb;

		private static List<RuleEntry> matchingRules = new List<RuleEntry>();

		private static bool LogOn
		{
			get
			{
				return DebugViewSettings.logGrammarResolution;
			}
		}

		public static string Resolve(string rootKeyword, List<Rule> rawRules, string debugLabel = null)
		{
			GrammarResolver.rules.Clear();
			for (int i = 0; i < rawRules.Count; i++)
			{
				GrammarResolver.rules.Add(new RuleEntry(rawRules[i]));
			}
			for (int j = 0; j < RulePackDefOf.GlobalUtility.Rules.Count; j++)
			{
				GrammarResolver.rules.Add(new RuleEntry(RulePackDefOf.GlobalUtility.Rules[j]));
			}
			GrammarResolver.loopCount = 0;
			if (GrammarResolver.LogOn)
			{
				GrammarResolver.logSb = new StringBuilder();
			}
			string text = "err";
			bool flag = false;
			foreach (RuleEntry item in (from r in GrammarResolver.rules
			where r.rule.keyword == rootKeyword
			select r).InRandomOrder(null))
			{
				if (GrammarResolver.TryResolveRecursive(item, 0, out text))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
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

		private static bool TryResolveRecursive(RuleEntry entry, int depth, out string output)
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
			if (GrammarResolver.loopCount > 100)
			{
				Log.Error("Hit loops limit resolving grammar.");
				output = "HIT_LOOPS_LIMIT";
				if (GrammarResolver.LogOn)
				{
					GrammarResolver.logSb.Append("UNRESOLVABLE: Hit loops limit");
				}
				return false;
			}
			if (depth > 50)
			{
				Log.Error("Grammar recurred too deep while resolving keyword (>" + 50 + " deep)");
				output = "DEPTH_LIMIT_REACHED";
				if (GrammarResolver.LogOn)
				{
					GrammarResolver.logSb.Append("UNRESOLVABLE: Depth limit reached");
				}
				return false;
			}
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
							RuleEntry ruleEntry = GrammarResolver.RandomPossiblyResolvableEntry(text2);
							if (ruleEntry != null)
							{
								ruleEntry.uses++;
								if (GrammarResolver.TryResolveRecursive(ruleEntry, depth + 1, out str))
									goto IL_01f0;
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
				IL_01f0:
				text = text.Substring(0, num2) + str + text.Substring(i + 1);
				i = num2;
			}
			output = text;
			return !flag;
		}

		private static RuleEntry RandomPossiblyResolvableEntry(string keyword)
		{
			GrammarResolver.matchingRules.Clear();
			for (int i = 0; i < GrammarResolver.rules.Count; i++)
			{
				if (GrammarResolver.rules[i].rule.keyword == keyword && !GrammarResolver.rules[i].knownUnresolvable)
				{
					GrammarResolver.matchingRules.Add(GrammarResolver.rules[i]);
				}
			}
			if (GrammarResolver.matchingRules.Count == 0)
			{
				return null;
			}
			return GrammarResolver.matchingRules.RandomElementByWeight((Func<RuleEntry, float>)((RuleEntry r) => r.SelectionWeight));
		}
	}
}
