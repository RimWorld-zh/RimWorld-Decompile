using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200065E RID: 1630
	public class TaleData_Thing : TaleData
	{
		// Token: 0x0400135C RID: 4956
		public int thingID;

		// Token: 0x0400135D RID: 4957
		public ThingDef thingDef;

		// Token: 0x0400135E RID: 4958
		public ThingDef stuff;

		// Token: 0x0400135F RID: 4959
		public string title;

		// Token: 0x04001360 RID: 4960
		public QualityCategory quality;

		// Token: 0x06002209 RID: 8713 RVA: 0x001210A0 File Offset: 0x0011F4A0
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.thingID, "thingID", 0, false);
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Values.Look<QualityCategory>(ref this.quality, "quality", QualityCategory.Awful, false);
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x00121104 File Offset: 0x0011F504
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

		// Token: 0x0600220B RID: 8715 RVA: 0x00121138 File Offset: 0x0011F538
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

		// Token: 0x0600220C RID: 8716 RVA: 0x001211AC File Offset: 0x0011F5AC
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
	}
}
