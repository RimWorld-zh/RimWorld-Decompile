using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public struct ThingStuffPair
	{
		public ThingDef thing;

		public ThingDef stuff;

		public float commonalityMultiplier;

		private float cachedPrice;

		private float cachedInsulationCold;

		public float Price
		{
			get
			{
				return this.cachedPrice;
			}
		}

		public float InsulationCold
		{
			get
			{
				return this.cachedInsulationCold;
			}
		}

		public float Commonality
		{
			get
			{
				float num = this.commonalityMultiplier;
				num *= this.thing.generateCommonality;
				if (this.stuff != null)
				{
					num *= this.stuff.stuffProps.commonality;
				}
				if (PawnWeaponGenerator.IsDerpWeapon(this.thing, this.stuff))
				{
					num = (float)(num * 0.0099999997764825821);
				}
				return num;
			}
		}

		public ThingStuffPair(ThingDef thing, ThingDef stuff, float commonalityMultiplier = 1f)
		{
			this.thing = thing;
			this.stuff = stuff;
			this.commonalityMultiplier = commonalityMultiplier;
			this.cachedPrice = thing.GetStatValueAbstract(StatDefOf.MarketValue, stuff);
			this.cachedInsulationCold = thing.GetStatValueAbstract(StatDefOf.Insulation_Cold, null);
		}

		public static List<ThingStuffPair> AllWith(Predicate<ThingDef> thingValidator)
		{
			List<ThingStuffPair> list = new List<ThingStuffPair>();
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ThingDef thingDef = allDefsListForReading[i];
				if (thingValidator(thingDef))
				{
					if (!thingDef.MadeFromStuff)
					{
						list.Add(new ThingStuffPair(thingDef, null, 1f));
					}
					else
					{
						IEnumerable<ThingDef> enumerable = from st in DefDatabase<ThingDef>.AllDefs
						where st.IsStuff && st.stuffProps.CanMake(thingDef)
						select st;
						int num = enumerable.Count();
						float num2 = enumerable.Average((Func<ThingDef, float>)((ThingDef st) => st.stuffProps.commonality));
						float num3 = (float)(1.0 / (float)num / num2);
						foreach (ThingDef item in enumerable)
						{
							list.Add(new ThingStuffPair(thingDef, item, num3));
						}
					}
				}
			}
			return (from p in list
			orderby p.Price descending
			select p).ToList();
		}

		public override string ToString()
		{
			if (this.thing == null)
			{
				return "(null)";
			}
			string text = (this.stuff != null) ? (this.thing.label + " " + this.stuff.LabelAsStuff) : this.thing.label;
			return text + " $" + this.Price.ToString("F0") + " c=" + this.Commonality.ToString("F4");
		}
	}
}
