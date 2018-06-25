using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F0B RID: 3851
	public struct ThingStuffPair : IEquatable<ThingStuffPair>
	{
		// Token: 0x04003CF9 RID: 15609
		public ThingDef thing;

		// Token: 0x04003CFA RID: 15610
		public ThingDef stuff;

		// Token: 0x04003CFB RID: 15611
		public float commonalityMultiplier;

		// Token: 0x04003CFC RID: 15612
		private float cachedPrice;

		// Token: 0x04003CFD RID: 15613
		private float cachedInsulationCold;

		// Token: 0x04003CFE RID: 15614
		private float cachedInsulationHeat;

		// Token: 0x06005C6C RID: 23660 RVA: 0x002EF454 File Offset: 0x002ED854
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

		// Token: 0x17000ED5 RID: 3797
		// (get) Token: 0x06005C6D RID: 23661 RVA: 0x002EF4F8 File Offset: 0x002ED8F8
		public float Price
		{
			get
			{
				return this.cachedPrice;
			}
		}

		// Token: 0x17000ED6 RID: 3798
		// (get) Token: 0x06005C6E RID: 23662 RVA: 0x002EF514 File Offset: 0x002ED914
		public float InsulationCold
		{
			get
			{
				return this.cachedInsulationCold;
			}
		}

		// Token: 0x17000ED7 RID: 3799
		// (get) Token: 0x06005C6F RID: 23663 RVA: 0x002EF530 File Offset: 0x002ED930
		public float InsulationHeat
		{
			get
			{
				return this.cachedInsulationHeat;
			}
		}

		// Token: 0x17000ED8 RID: 3800
		// (get) Token: 0x06005C70 RID: 23664 RVA: 0x002EF54C File Offset: 0x002ED94C
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

		// Token: 0x06005C71 RID: 23665 RVA: 0x002EF5B8 File Offset: 0x002ED9B8
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

		// Token: 0x06005C72 RID: 23666 RVA: 0x002EF728 File Offset: 0x002EDB28
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

		// Token: 0x06005C73 RID: 23667 RVA: 0x002EF7DC File Offset: 0x002EDBDC
		public static bool operator ==(ThingStuffPair a, ThingStuffPair b)
		{
			return a.thing == b.thing && a.stuff == b.stuff && a.commonalityMultiplier == b.commonalityMultiplier;
		}

		// Token: 0x06005C74 RID: 23668 RVA: 0x002EF82C File Offset: 0x002EDC2C
		public static bool operator !=(ThingStuffPair a, ThingStuffPair b)
		{
			return !(a == b);
		}

		// Token: 0x06005C75 RID: 23669 RVA: 0x002EF84C File Offset: 0x002EDC4C
		public override bool Equals(object obj)
		{
			return obj is ThingStuffPair && this.Equals((ThingStuffPair)obj);
		}

		// Token: 0x06005C76 RID: 23670 RVA: 0x002EF880 File Offset: 0x002EDC80
		public bool Equals(ThingStuffPair other)
		{
			return this == other;
		}

		// Token: 0x06005C77 RID: 23671 RVA: 0x002EF8A4 File Offset: 0x002EDCA4
		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombine<ThingDef>(seed, this.thing);
			seed = Gen.HashCombine<ThingDef>(seed, this.stuff);
			return Gen.HashCombineStruct<float>(seed, this.commonalityMultiplier);
		}

		// Token: 0x06005C78 RID: 23672 RVA: 0x002EF8E4 File Offset: 0x002EDCE4
		public static explicit operator ThingStuffPair(ThingStuffPairWithQuality p)
		{
			return new ThingStuffPair(p.thing, p.stuff, 1f);
		}
	}
}
