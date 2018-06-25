using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F0C RID: 3852
	public struct ThingStuffPairWithQuality : IEquatable<ThingStuffPairWithQuality>
	{
		// Token: 0x04003D01 RID: 15617
		public ThingDef thing;

		// Token: 0x04003D02 RID: 15618
		public ThingDef stuff;

		// Token: 0x04003D03 RID: 15619
		public QualityCategory? quality;

		// Token: 0x06005C7B RID: 23675 RVA: 0x002EF98C File Offset: 0x002EDD8C
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

		// Token: 0x17000ED9 RID: 3801
		// (get) Token: 0x06005C7C RID: 23676 RVA: 0x002EFA54 File Offset: 0x002EDE54
		public QualityCategory Quality
		{
			get
			{
				QualityCategory? qualityCategory = this.quality;
				return (qualityCategory == null) ? QualityCategory.Normal : qualityCategory.Value;
			}
		}

		// Token: 0x06005C7D RID: 23677 RVA: 0x002EFA8C File Offset: 0x002EDE8C
		public float GetStatValue(StatDef stat)
		{
			return stat.Worker.GetValue(StatRequest.For(this.thing, this.stuff, this.Quality), true);
		}

		// Token: 0x06005C7E RID: 23678 RVA: 0x002EFAC4 File Offset: 0x002EDEC4
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

		// Token: 0x06005C7F RID: 23679 RVA: 0x002EFB38 File Offset: 0x002EDF38
		public static bool operator !=(ThingStuffPairWithQuality a, ThingStuffPairWithQuality b)
		{
			return !(a == b);
		}

		// Token: 0x06005C80 RID: 23680 RVA: 0x002EFB58 File Offset: 0x002EDF58
		public override bool Equals(object obj)
		{
			return obj is ThingStuffPairWithQuality && this.Equals((ThingStuffPairWithQuality)obj);
		}

		// Token: 0x06005C81 RID: 23681 RVA: 0x002EFB8C File Offset: 0x002EDF8C
		public bool Equals(ThingStuffPairWithQuality other)
		{
			return this == other;
		}

		// Token: 0x06005C82 RID: 23682 RVA: 0x002EFBB0 File Offset: 0x002EDFB0
		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombine<ThingDef>(seed, this.thing);
			seed = Gen.HashCombine<ThingDef>(seed, this.stuff);
			return Gen.HashCombine<QualityCategory?>(seed, this.quality);
		}

		// Token: 0x06005C83 RID: 23683 RVA: 0x002EFBF0 File Offset: 0x002EDFF0
		public static explicit operator ThingStuffPairWithQuality(ThingStuffPair p)
		{
			return new ThingStuffPairWithQuality(p.thing, p.stuff, QualityCategory.Normal);
		}

		// Token: 0x06005C84 RID: 23684 RVA: 0x002EFC1C File Offset: 0x002EE01C
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
