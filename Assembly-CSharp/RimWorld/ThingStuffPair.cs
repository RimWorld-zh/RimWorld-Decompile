using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F06 RID: 3846
	public struct ThingStuffPair : IEquatable<ThingStuffPair>
	{
		// Token: 0x06005C62 RID: 23650 RVA: 0x002EEBB4 File Offset: 0x002ECFB4
		public ThingStuffPair(ThingDef thing, ThingDef stuff, float commonalityMultiplier = 1f)
		{
			this.thing = thing;
			this.stuff = stuff;
			this.commonalityMultiplier = commonalityMultiplier;
			if (stuff != null && !thing.MadeFromStuff)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Created ThingStuffPairWithQuality with stuff ",
					stuff,
					" but ",
					thing,
					" is not made from stuff."
				}), false);
				stuff = null;
			}
			this.cachedPrice = thing.GetStatValueAbstract(StatDefOf.MarketValue, stuff);
			this.cachedInsulationCold = thing.GetStatValueAbstract(StatDefOf.Insulation_Cold, stuff);
			this.cachedInsulationHeat = thing.GetStatValueAbstract(StatDefOf.Insulation_Heat, stuff);
		}

		// Token: 0x17000ED6 RID: 3798
		// (get) Token: 0x06005C63 RID: 23651 RVA: 0x002EEC58 File Offset: 0x002ED058
		public float Price
		{
			get
			{
				return this.cachedPrice;
			}
		}

		// Token: 0x17000ED7 RID: 3799
		// (get) Token: 0x06005C64 RID: 23652 RVA: 0x002EEC74 File Offset: 0x002ED074
		public float InsulationCold
		{
			get
			{
				return this.cachedInsulationCold;
			}
		}

		// Token: 0x17000ED8 RID: 3800
		// (get) Token: 0x06005C65 RID: 23653 RVA: 0x002EEC90 File Offset: 0x002ED090
		public float InsulationHeat
		{
			get
			{
				return this.cachedInsulationHeat;
			}
		}

		// Token: 0x17000ED9 RID: 3801
		// (get) Token: 0x06005C66 RID: 23654 RVA: 0x002EECAC File Offset: 0x002ED0AC
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
					num *= 0.01f;
				}
				return num;
			}
		}

		// Token: 0x06005C67 RID: 23655 RVA: 0x002EED18 File Offset: 0x002ED118
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
						int num = enumerable.Count<ThingDef>();
						float num2 = enumerable.Average((ThingDef st) => st.stuffProps.commonality);
						float num3 = 1f / (float)num / num2;
						foreach (ThingDef thingDef2 in enumerable)
						{
							list.Add(new ThingStuffPair(thingDef, thingDef2, num3));
						}
					}
				}
			}
			return (from p in list
			orderby p.Price descending
			select p).ToList<ThingStuffPair>();
		}

		// Token: 0x06005C68 RID: 23656 RVA: 0x002EEE88 File Offset: 0x002ED288
		public override string ToString()
		{
			string result;
			if (this.thing == null)
			{
				result = "(null)";
			}
			else
			{
				string text;
				if (this.stuff == null)
				{
					text = this.thing.label;
				}
				else
				{
					text = this.thing.label + " " + this.stuff.LabelAsStuff;
				}
				result = string.Concat(new string[]
				{
					text,
					" $",
					this.Price.ToString("F0"),
					" c=",
					this.Commonality.ToString("F4")
				});
			}
			return result;
		}

		// Token: 0x06005C69 RID: 23657 RVA: 0x002EEF3C File Offset: 0x002ED33C
		public static bool operator ==(ThingStuffPair a, ThingStuffPair b)
		{
			return a.thing == b.thing && a.stuff == b.stuff && a.commonalityMultiplier == b.commonalityMultiplier;
		}

		// Token: 0x06005C6A RID: 23658 RVA: 0x002EEF8C File Offset: 0x002ED38C
		public static bool operator !=(ThingStuffPair a, ThingStuffPair b)
		{
			return !(a == b);
		}

		// Token: 0x06005C6B RID: 23659 RVA: 0x002EEFAC File Offset: 0x002ED3AC
		public override bool Equals(object obj)
		{
			return obj is ThingStuffPair && this.Equals((ThingStuffPair)obj);
		}

		// Token: 0x06005C6C RID: 23660 RVA: 0x002EEFE0 File Offset: 0x002ED3E0
		public bool Equals(ThingStuffPair other)
		{
			return this == other;
		}

		// Token: 0x06005C6D RID: 23661 RVA: 0x002EF004 File Offset: 0x002ED404
		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombine<ThingDef>(seed, this.thing);
			seed = Gen.HashCombine<ThingDef>(seed, this.stuff);
			return Gen.HashCombineStruct<float>(seed, this.commonalityMultiplier);
		}

		// Token: 0x06005C6E RID: 23662 RVA: 0x002EF044 File Offset: 0x002ED444
		public static explicit operator ThingStuffPair(ThingStuffPairWithQuality p)
		{
			return new ThingStuffPair(p.thing, p.stuff, 1f);
		}

		// Token: 0x04003CEE RID: 15598
		public ThingDef thing;

		// Token: 0x04003CEF RID: 15599
		public ThingDef stuff;

		// Token: 0x04003CF0 RID: 15600
		public float commonalityMultiplier;

		// Token: 0x04003CF1 RID: 15601
		private float cachedPrice;

		// Token: 0x04003CF2 RID: 15602
		private float cachedInsulationCold;

		// Token: 0x04003CF3 RID: 15603
		private float cachedInsulationHeat;
	}
}
