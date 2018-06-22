using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200065C RID: 1628
	public class TaleData_Thing : TaleData
	{
		// Token: 0x06002206 RID: 8710 RVA: 0x00120CE8 File Offset: 0x0011F0E8
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.thingID, "thingID", 0, false);
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Values.Look<QualityCategory>(ref this.quality, "quality", QualityCategory.Awful, false);
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x00120D4C File Offset: 0x0011F14C
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

		// Token: 0x06002208 RID: 8712 RVA: 0x00120D80 File Offset: 0x0011F180
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

		// Token: 0x06002209 RID: 8713 RVA: 0x00120DF4 File Offset: 0x0011F1F4
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

		// Token: 0x04001358 RID: 4952
		public int thingID;

		// Token: 0x04001359 RID: 4953
		public ThingDef thingDef;

		// Token: 0x0400135A RID: 4954
		public ThingDef stuff;

		// Token: 0x0400135B RID: 4955
		public string title;

		// Token: 0x0400135C RID: 4956
		public QualityCategory quality;
	}
}
