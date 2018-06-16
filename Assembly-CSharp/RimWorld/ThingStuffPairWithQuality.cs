using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F08 RID: 3848
	public struct ThingStuffPairWithQuality : IEquatable<ThingStuffPairWithQuality>
	{
		// Token: 0x06005C4B RID: 23627 RVA: 0x002ECFDC File Offset: 0x002EB3DC
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

		// Token: 0x17000ED7 RID: 3799
		// (get) Token: 0x06005C4C RID: 23628 RVA: 0x002ED0A4 File Offset: 0x002EB4A4
		public QualityCategory Quality
		{
			get
			{
				QualityCategory? qualityCategory = this.quality;
				return (qualityCategory == null) ? QualityCategory.Normal : qualityCategory.Value;
			}
		}

		// Token: 0x06005C4D RID: 23629 RVA: 0x002ED0DC File Offset: 0x002EB4DC
		public float GetStatValue(StatDef stat)
		{
			return stat.Worker.GetValue(StatRequest.For(this.thing, this.stuff, this.Quality), true);
		}

		// Token: 0x06005C4E RID: 23630 RVA: 0x002ED114 File Offset: 0x002EB514
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

		// Token: 0x06005C4F RID: 23631 RVA: 0x002ED188 File Offset: 0x002EB588
		public static bool operator !=(ThingStuffPairWithQuality a, ThingStuffPairWithQuality b)
		{
			return !(a == b);
		}

		// Token: 0x06005C50 RID: 23632 RVA: 0x002ED1A8 File Offset: 0x002EB5A8
		public override bool Equals(object obj)
		{
			return obj is ThingStuffPairWithQuality && this.Equals((ThingStuffPairWithQuality)obj);
		}

		// Token: 0x06005C51 RID: 23633 RVA: 0x002ED1DC File Offset: 0x002EB5DC
		public bool Equals(ThingStuffPairWithQuality other)
		{
			return this == other;
		}

		// Token: 0x06005C52 RID: 23634 RVA: 0x002ED200 File Offset: 0x002EB600
		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombine<ThingDef>(seed, this.thing);
			seed = Gen.HashCombine<ThingDef>(seed, this.stuff);
			return Gen.HashCombine<QualityCategory?>(seed, this.quality);
		}

		// Token: 0x06005C53 RID: 23635 RVA: 0x002ED240 File Offset: 0x002EB640
		public static explicit operator ThingStuffPairWithQuality(ThingStuffPair p)
		{
			return new ThingStuffPairWithQuality(p.thing, p.stuff, QualityCategory.Normal);
		}

		// Token: 0x06005C54 RID: 23636 RVA: 0x002ED26C File Offset: 0x002EB66C
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

		// Token: 0x04003CE4 RID: 15588
		public ThingDef thing;

		// Token: 0x04003CE5 RID: 15589
		public ThingDef stuff;

		// Token: 0x04003CE6 RID: 15590
		public QualityCategory? quality;
	}
}
