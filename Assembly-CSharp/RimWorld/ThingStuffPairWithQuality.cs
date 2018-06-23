using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F07 RID: 3847
	public struct ThingStuffPairWithQuality : IEquatable<ThingStuffPairWithQuality>
	{
		// Token: 0x04003CF6 RID: 15606
		public ThingDef thing;

		// Token: 0x04003CF7 RID: 15607
		public ThingDef stuff;

		// Token: 0x04003CF8 RID: 15608
		public QualityCategory? quality;

		// Token: 0x06005C71 RID: 23665 RVA: 0x002EF0EC File Offset: 0x002ED4EC
		public ThingStuffPairWithQuality(ThingDef thing, ThingDef stuff, QualityCategory quality)
		{
			this.thing = thing;
			this.stuff = stuff;
			this.quality = new QualityCategory?(quality);
			if (quality != QualityCategory.Normal && !thing.HasComp(typeof(CompQuality)))
			{
				Log.Warning(string.Concat(new object[]
				{
					"Created ThingStuffPairWithQuality with quality",
					quality,
					" but ",
					thing,
					" doesn't have CompQuality."
				}), false);
				quality = QualityCategory.Normal;
			}
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
		}

		// Token: 0x17000EDA RID: 3802
		// (get) Token: 0x06005C72 RID: 23666 RVA: 0x002EF1B4 File Offset: 0x002ED5B4
		public QualityCategory Quality
		{
			get
			{
				QualityCategory? qualityCategory = this.quality;
				return (qualityCategory == null) ? QualityCategory.Normal : qualityCategory.Value;
			}
		}

		// Token: 0x06005C73 RID: 23667 RVA: 0x002EF1EC File Offset: 0x002ED5EC
		public float GetStatValue(StatDef stat)
		{
			return stat.Worker.GetValue(StatRequest.For(this.thing, this.stuff, this.Quality), true);
		}

		// Token: 0x06005C74 RID: 23668 RVA: 0x002EF224 File Offset: 0x002ED624
		public static bool operator ==(ThingStuffPairWithQuality a, ThingStuffPairWithQuality b)
		{
			bool result;
			if (a.thing == b.thing && a.stuff == b.stuff)
			{
				QualityCategory? qualityCategory = a.quality;
				QualityCategory valueOrDefault = qualityCategory.GetValueOrDefault();
				QualityCategory? qualityCategory2 = b.quality;
				result = (valueOrDefault == qualityCategory2.GetValueOrDefault() && qualityCategory != null == (qualityCategory2 != null));
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06005C75 RID: 23669 RVA: 0x002EF298 File Offset: 0x002ED698
		public static bool operator !=(ThingStuffPairWithQuality a, ThingStuffPairWithQuality b)
		{
			return !(a == b);
		}

		// Token: 0x06005C76 RID: 23670 RVA: 0x002EF2B8 File Offset: 0x002ED6B8
		public override bool Equals(object obj)
		{
			return obj is ThingStuffPairWithQuality && this.Equals((ThingStuffPairWithQuality)obj);
		}

		// Token: 0x06005C77 RID: 23671 RVA: 0x002EF2EC File Offset: 0x002ED6EC
		public bool Equals(ThingStuffPairWithQuality other)
		{
			return this == other;
		}

		// Token: 0x06005C78 RID: 23672 RVA: 0x002EF310 File Offset: 0x002ED710
		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombine<ThingDef>(seed, this.thing);
			seed = Gen.HashCombine<ThingDef>(seed, this.stuff);
			return Gen.HashCombine<QualityCategory?>(seed, this.quality);
		}

		// Token: 0x06005C79 RID: 23673 RVA: 0x002EF350 File Offset: 0x002ED750
		public static explicit operator ThingStuffPairWithQuality(ThingStuffPair p)
		{
			return new ThingStuffPairWithQuality(p.thing, p.stuff, QualityCategory.Normal);
		}

		// Token: 0x06005C7A RID: 23674 RVA: 0x002EF37C File Offset: 0x002ED77C
		public Thing MakeThing()
		{
			Thing result = ThingMaker.MakeThing(this.thing, this.stuff);
			CompQuality compQuality = result.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				compQuality.SetQuality(this.Quality, ArtGenerationContext.Outsider);
			}
			return result;
		}
	}
}
