using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public abstract class ScenPart_ThingCount : ScenPart
	{
		protected ThingDef thingDef;

		protected ThingDef stuff;

		protected int count = 1;

		private string countBuf;

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		public override void Randomize()
		{
			this.thingDef = this.PossibleThingDefs().RandomElement();
			this.stuff = GenStuff.RandomStuffFor(this.thingDef);
			if (this.thingDef.statBases.StatListContains(StatDefOf.MarketValue))
			{
				float num = (float)Rand.Range(200, 2000);
				float statValueAbstract = this.thingDef.GetStatValueAbstract(StatDefOf.MarketValue, this.stuff);
				this.count = Mathf.CeilToInt(num / statValueAbstract);
			}
			else
			{
				this.count = Rand.RangeInclusive(1, 100);
			}
		}

		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, (float)(ScenPart.RowHeight * 3.0));
			Rect rect = new Rect(scenPartRect.x, scenPartRect.y, scenPartRect.width, (float)(scenPartRect.height / 3.0));
			Rect rect2 = new Rect(scenPartRect.x, (float)(scenPartRect.y + scenPartRect.height / 3.0), scenPartRect.width, (float)(scenPartRect.height / 3.0));
			Rect rect3 = new Rect(scenPartRect.x, (float)(scenPartRect.y + scenPartRect.height * 2.0 / 3.0), scenPartRect.width, (float)(scenPartRect.height / 3.0));
			if (Widgets.ButtonText(rect, this.thingDef.LabelCap, true, false, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (ThingDef item in from t in this.PossibleThingDefs()
				orderby t.label
				select t)
				{
					ThingDef localTd = item;
					list.Add(new FloatMenuOption(localTd.LabelCap, (Action)delegate
					{
						this.thingDef = localTd;
						this.stuff = GenStuff.DefaultStuffFor(localTd);
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			if (this.thingDef.MadeFromStuff && Widgets.ButtonText(rect2, this.stuff.LabelCap, true, false, true))
			{
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				foreach (ThingDef item2 in from t in GenStuff.AllowedStuffsFor(this.thingDef)
				orderby t.label
				select t)
				{
					ThingDef localSd = item2;
					list2.Add(new FloatMenuOption(localSd.LabelCap, (Action)delegate
					{
						this.stuff = localSd;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			Widgets.TextFieldNumeric<int>(rect3, ref this.count, ref this.countBuf, 1f, 1E+09f);
		}

		public override bool TryMerge(ScenPart other)
		{
			ScenPart_ThingCount scenPart_ThingCount = other as ScenPart_ThingCount;
			if (scenPart_ThingCount != null && base.GetType() == scenPart_ThingCount.GetType() && this.thingDef == scenPart_ThingCount.thingDef && this.stuff == scenPart_ThingCount.stuff && this.count >= 0 && scenPart_ThingCount.count >= 0)
			{
				this.count += scenPart_ThingCount.count;
				return true;
			}
			return false;
		}

		protected virtual IEnumerable<ThingDef> PossibleThingDefs()
		{
			return DefDatabase<ThingDef>.AllDefs.Where((Func<ThingDef, bool>)delegate(ThingDef d)
			{
				if (d.category == ThingCategory.Item && d.scatterableOnMapGen && !d.destroyOnDrop)
				{
					goto IL_0050;
				}
				if (d.category == ThingCategory.Building && d.Minifiable)
				{
					goto IL_0050;
				}
				int result = (d.category == ThingCategory.Building && d.scatterableOnMapGen) ? 1 : 0;
				goto IL_0051;
				IL_0051:
				return (byte)result != 0;
				IL_0050:
				result = 1;
				goto IL_0051;
			});
		}
	}
}
