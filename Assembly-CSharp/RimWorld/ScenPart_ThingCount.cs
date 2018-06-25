using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000650 RID: 1616
	public abstract class ScenPart_ThingCount : ScenPart
	{
		// Token: 0x04001311 RID: 4881
		protected ThingDef thingDef;

		// Token: 0x04001312 RID: 4882
		protected ThingDef stuff;

		// Token: 0x04001313 RID: 4883
		protected int count = 1;

		// Token: 0x04001314 RID: 4884
		private string countBuf;

		// Token: 0x0600218C RID: 8588 RVA: 0x0011C39B File Offset: 0x0011A79B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x0011C3D8 File Offset: 0x0011A7D8
		public override void Randomize()
		{
			this.thingDef = this.PossibleThingDefs().RandomElement<ThingDef>();
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

		// Token: 0x0600218E RID: 8590 RVA: 0x0011C470 File Offset: 0x0011A870
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 3f);
			Rect rect = new Rect(scenPartRect.x, scenPartRect.y, scenPartRect.width, scenPartRect.height / 3f);
			Rect rect2 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height / 3f, scenPartRect.width, scenPartRect.height / 3f);
			Rect rect3 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height * 2f / 3f, scenPartRect.width, scenPartRect.height / 3f);
			if (Widgets.ButtonText(rect, this.thingDef.LabelCap, true, false, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (ThingDef localTd2 in from t in this.PossibleThingDefs()
				orderby t.label
				select t)
				{
					ThingDef localTd = localTd2;
					list.Add(new FloatMenuOption(localTd.LabelCap, delegate()
					{
						this.thingDef = localTd;
						this.stuff = GenStuff.DefaultStuffFor(localTd);
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			if (this.thingDef.MadeFromStuff)
			{
				if (Widgets.ButtonText(rect2, this.stuff.LabelCap, true, false, true))
				{
					List<FloatMenuOption> list2 = new List<FloatMenuOption>();
					foreach (ThingDef localSd2 in from t in GenStuff.AllowedStuffsFor(this.thingDef, TechLevel.Undefined)
					orderby t.label
					select t)
					{
						ThingDef localSd = localSd2;
						list2.Add(new FloatMenuOption(localSd.LabelCap, delegate()
						{
							this.stuff = localSd;
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					Find.WindowStack.Add(new FloatMenu(list2));
				}
			}
			Widgets.TextFieldNumeric<int>(rect3, ref this.count, ref this.countBuf, 1f, 1E+09f);
		}

		// Token: 0x0600218F RID: 8591 RVA: 0x0011C734 File Offset: 0x0011AB34
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_ThingCount scenPart_ThingCount = other as ScenPart_ThingCount;
			bool result;
			if (scenPart_ThingCount != null && base.GetType() == scenPart_ThingCount.GetType() && this.thingDef == scenPart_ThingCount.thingDef && this.stuff == scenPart_ThingCount.stuff && this.count >= 0 && scenPart_ThingCount.count >= 0)
			{
				this.count += scenPart_ThingCount.count;
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x0011C7C0 File Offset: 0x0011ABC0
		protected virtual IEnumerable<ThingDef> PossibleThingDefs()
		{
			return from d in DefDatabase<ThingDef>.AllDefs
			where (d.category == ThingCategory.Item && d.scatterableOnMapGen && !d.destroyOnDrop) || (d.category == ThingCategory.Building && d.Minifiable) || (d.category == ThingCategory.Building && d.scatterableOnMapGen)
			select d;
		}
	}
}
