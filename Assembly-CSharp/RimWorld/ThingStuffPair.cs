using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F07 RID: 3847
	public struct ThingStuffPair : IEquatable<ThingStuffPair>
	{
		// Token: 0x06005C3C RID: 23612 RVA: 0x002ECAA4 File Offset: 0x002EAEA4
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

		// Token: 0x17000ED3 RID: 3795
		// (get) Token: 0x06005C3D RID: 23613 RVA: 0x002ECB48 File Offset: 0x002EAF48
		public float Price
		{
			get
			{
				return this.cachedPrice;
			}
		}

		// Token: 0x17000ED4 RID: 3796
		// (get) Token: 0x06005C3E RID: 23614 RVA: 0x002ECB64 File Offset: 0x002EAF64
		public float InsulationCold
		{
			get
			{
				return this.cachedInsulationCold;
			}
		}

		// Token: 0x17000ED5 RID: 3797
		// (get) Token: 0x06005C3F RID: 23615 RVA: 0x002ECB80 File Offset: 0x002EAF80
		public float InsulationHeat
		{
			get
			{
				return this.cachedInsulationHeat;
			}
		}

		// Token: 0x17000ED6 RID: 3798
		// (get) Token: 0x06005C40 RID: 23616 RVA: 0x002ECB9C File Offset: 0x002EAF9C
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

		// Token: 0x06005C41 RID: 23617 RVA: 0x002ECC08 File Offset: 0x002EB008
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

		// Token: 0x06005C42 RID: 23618 RVA: 0x002ECD78 File Offset: 0x002EB178
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

		// Token: 0x06005C43 RID: 23619 RVA: 0x002ECE2C File Offset: 0x002EB22C
		public static bool operator ==(ThingStuffPair a, ThingStuffPair b)
		{
			return a.thing == b.thing && a.stuff == b.stuff && a.commonalityMultiplier == b.commonalityMultiplier;
		}

		// Token: 0x06005C44 RID: 23620 RVA: 0x002ECE7C File Offset: 0x002EB27C
		public static bool operator !=(ThingStuffPair a, ThingStuffPair b)
		{
			return !(a == b);
		}

		// Token: 0x06005C45 RID: 23621 RVA: 0x002ECE9C File Offset: 0x002EB29C
		public override bool Equals(object obj)
		{
			return obj is ThingStuffPair && this.Equals((ThingStuffPair)obj);
		}

		// Token: 0x06005C46 RID: 23622 RVA: 0x002ECED0 File Offset: 0x002EB2D0
		public bool Equals(ThingStuffPair other)
		{
			return this == other;
		}

		// Token: 0x06005C47 RID: 23623 RVA: 0x002ECEF4 File Offset: 0x002EB2F4
		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombine<ThingDef>(seed, this.thing);
			seed = Gen.HashCombine<ThingDef>(seed, this.stuff);
			return Gen.HashCombineStruct<float>(seed, this.commonalityMultiplier);
		}

		// Token: 0x06005C48 RID: 23624 RVA: 0x002ECF34 File Offset: 0x002EB334
		public static explicit operator ThingStuffPair(ThingStuffPairWithQuality p)
		{
			return new ThingStuffPair(p.thing, p.stuff, 1f);
		}

		// Token: 0x04003CDC RID: 15580
		public ThingDef thing;

		// Token: 0x04003CDD RID: 15581
		public ThingDef stuff;

		// Token: 0x04003CDE RID: 15582
		public float commonalityMultiplier;

		// Token: 0x04003CDF RID: 15583
		private float cachedPrice;

		// Token: 0x04003CE0 RID: 15584
		private float cachedInsulationCold;

		// Token: 0x04003CE1 RID: 15585
		private float cachedInsulationHeat;
	}
}
