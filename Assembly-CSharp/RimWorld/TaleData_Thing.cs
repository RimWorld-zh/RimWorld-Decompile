using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.thingID, "thingID", 0, false);
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Values.Look<QualityCategory>(ref this.quality, "quality", QualityCategory.Awful, false);
		}

		[DebuggerHidden]
		public override IEnumerable<Rule> GetRules(string prefix)
		{
			TaleData_Thing.<GetRules>c__Iterator12D <GetRules>c__Iterator12D = new TaleData_Thing.<GetRules>c__Iterator12D();
			<GetRules>c__Iterator12D.prefix = prefix;
			<GetRules>c__Iterator12D.<$>prefix = prefix;
			<GetRules>c__Iterator12D.<>f__this = this;
			TaleData_Thing.<GetRules>c__Iterator12D expr_1C = <GetRules>c__Iterator12D;
			expr_1C.$PC = -2;
			return expr_1C;
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
				bool arg_36_0;
				if (d.comps != null)
				{
					arg_36_0 = d.comps.Any((CompProperties cp) => cp.compClass == typeof(CompArt));
				}
				else
				{
					arg_36_0 = false;
				}
				return arg_36_0;
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
	}
}
