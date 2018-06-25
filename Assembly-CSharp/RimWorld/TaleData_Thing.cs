using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	public class TaleData_Thing : TaleData
	{
		public int thingID;

		public ThingDef thingDef;

		public ThingDef stuff;

		public string title;

		public QualityCategory quality;

		[CompilerGenerated]
		private static Func<ThingDef, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Predicate<CompProperties> <>f__am$cache1;

		public TaleData_Thing()
		{
		}

		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.thingID, "thingID", 0, false);
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Values.Look<QualityCategory>(ref this.quality, "quality", QualityCategory.Awful, false);
		}

		public override IEnumerable<Rule> GetRules(string prefix)
		{
			yield return new Rule_String(prefix + "_label", this.thingDef.label);
			yield return new Rule_String(prefix + "_definite", Find.ActiveLanguageWorker.WithDefiniteArticle(this.thingDef.label));
			yield return new Rule_String(prefix + "_indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(this.thingDef.label));
			if (this.stuff != null)
			{
				yield return new Rule_String(prefix + "_stuffLabel", this.stuff.label);
			}
			if (this.title != null)
			{
				yield return new Rule_String(prefix + "_title", this.title);
			}
			yield return new Rule_String(prefix + "_quality", this.quality.GetLabel());
			yield break;
		}

		public static TaleData_Thing GenerateFrom(Thing t)
		{
			TaleData_Thing taleData_Thing = new TaleData_Thing();
			taleData_Thing.thingID = t.thingIDNumber;
			taleData_Thing.thingDef = t.def;
			taleData_Thing.stuff = t.Stuff;
			t.TryGetQuality(out taleData_Thing.quality);
			CompArt compArt = t.TryGetComp<CompArt>();
			if (compArt != null && compArt.Active)
			{
				taleData_Thing.title = compArt.Title;
			}
			return taleData_Thing;
		}

		public static TaleData_Thing GenerateRandom()
		{
			ThingDef thingDef = DefDatabase<ThingDef>.AllDefs.Where(delegate(ThingDef d)
			{
				bool result;
				if (d.comps != null)
				{
					result = d.comps.Any((CompProperties cp) => cp.compClass == typeof(CompArt));
				}
				else
				{
					result = false;
				}
				return result;
			}).RandomElement<ThingDef>();
			ThingDef thingDef2 = GenStuff.RandomStuffFor(thingDef);
			Thing thing = ThingMaker.MakeThing(thingDef, thingDef2);
			ArtGenerationContext source = (Rand.Value >= 0.5f) ? ArtGenerationContext.Outsider : ArtGenerationContext.Colony;
			CompQuality compQuality = thing.TryGetComp<CompQuality>();
			if (compQuality != null && compQuality.Quality < thing.TryGetComp<CompArt>().Props.minQualityForArtistic)
			{
				compQuality.SetQuality(thing.TryGetComp<CompArt>().Props.minQualityForArtistic, source);
			}
			thing.TryGetComp<CompArt>().InitializeArt(source);
			return TaleData_Thing.GenerateFrom(thing);
		}

		[CompilerGenerated]
		private static bool <GenerateRandom>m__0(ThingDef d)
		{
			bool result;
			if (d.comps != null)
			{
				result = d.comps.Any((CompProperties cp) => cp.compClass == typeof(CompArt));
			}
			else
			{
				result = false;
			}
			return result;
		}

		[CompilerGenerated]
		private static bool <GenerateRandom>m__1(CompProperties cp)
		{
			return cp.compClass == typeof(CompArt);
		}

		[CompilerGenerated]
		private sealed class <GetRules>c__Iterator0 : IEnumerable, IEnumerable<Rule>, IEnumerator, IDisposable, IEnumerator<Rule>
		{
			internal string prefix;

			internal TaleData_Thing $this;

			internal Rule $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <GetRules>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.$current = new Rule_String(prefix + "_label", this.thingDef.label);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$current = new Rule_String(prefix + "_definite", Find.ActiveLanguageWorker.WithDefiniteArticle(this.thingDef.label));
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				case 2u:
					this.$current = new Rule_String(prefix + "_indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(this.thingDef.label));
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					if (this.stuff != null)
					{
						this.$current = new Rule_String(prefix + "_stuffLabel", this.stuff.label);
						if (!this.$disposing)
						{
							this.$PC = 4;
						}
						return true;
					}
					break;
				case 4u:
					break;
				case 5u:
					goto IL_1A0;
				case 6u:
					this.$PC = -1;
					return false;
				default:
					return false;
				}
				if (this.title != null)
				{
					this.$current = new Rule_String(prefix + "_title", this.title);
					if (!this.$disposing)
					{
						this.$PC = 5;
					}
					return true;
				}
				IL_1A0:
				this.$current = new Rule_String(prefix + "_quality", this.quality.GetLabel());
				if (!this.$disposing)
				{
					this.$PC = 6;
				}
				return true;
			}

			Rule IEnumerator<Rule>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.Grammar.Rule>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Rule> IEnumerable<Rule>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				TaleData_Thing.<GetRules>c__Iterator0 <GetRules>c__Iterator = new TaleData_Thing.<GetRules>c__Iterator0();
				<GetRules>c__Iterator.$this = this;
				<GetRules>c__Iterator.prefix = prefix;
				return <GetRules>c__Iterator;
			}
		}
	}
}
