using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C1 RID: 2497
	public struct StatRequest : IEquatable<StatRequest>
	{
		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x060037D3 RID: 14291 RVA: 0x001DAE94 File Offset: 0x001D9294
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x060037D4 RID: 14292 RVA: 0x001DAEB0 File Offset: 0x001D92B0
		public BuildableDef Def
		{
			get
			{
				return this.defInt;
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x060037D5 RID: 14293 RVA: 0x001DAECC File Offset: 0x001D92CC
		public ThingDef StuffDef
		{
			get
			{
				return this.stuffDefInt;
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x060037D6 RID: 14294 RVA: 0x001DAEE8 File Offset: 0x001D92E8
		public QualityCategory QualityCategory
		{
			get
			{
				return this.qualityCategoryInt;
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x060037D7 RID: 14295 RVA: 0x001DAF04 File Offset: 0x001D9304
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x060037D8 RID: 14296 RVA: 0x001DAF28 File Offset: 0x001D9328
		public bool Empty
		{
			get
			{
				return this.Def == null;
			}
		}

		// Token: 0x060037D9 RID: 14297 RVA: 0x001DAF48 File Offset: 0x001D9348
		public static StatRequest For(Thing thing)
		{
			StatRequest result;
			if (thing == null)
			{
				Log.Error("StatRequest for null thing.", false);
				result = StatRequest.ForEmpty();
			}
			else
			{
				StatRequest statRequest = default(StatRequest);
				statRequest.thingInt = thing;
				statRequest.defInt = thing.def;
				statRequest.stuffDefInt = thing.Stuff;
				thing.TryGetQuality(out statRequest.qualityCategoryInt);
				result = statRequest;
			}
			return result;
		}

		// Token: 0x060037DA RID: 14298 RVA: 0x001DAFB4 File Offset: 0x001D93B4
		public static StatRequest For(BuildableDef def, ThingDef stuffDef, QualityCategory quality = QualityCategory.Normal)
		{
			StatRequest result;
			if (def == null)
			{
				Log.Error("StatRequest for null def.", false);
				result = StatRequest.ForEmpty();
			}
			else
			{
				result = new StatRequest
				{
					thingInt = null,
					defInt = def,
					stuffDefInt = stuffDef,
					qualityCategoryInt = quality
				};
			}
			return result;
		}

		// Token: 0x060037DB RID: 14299 RVA: 0x001DB010 File Offset: 0x001D9410
		public static StatRequest ForEmpty()
		{
			return new StatRequest
			{
				thingInt = null,
				defInt = null,
				stuffDefInt = null,
				qualityCategoryInt = QualityCategory.Normal
			};
		}

		// Token: 0x060037DC RID: 14300 RVA: 0x001DB050 File Offset: 0x001D9450
		public override string ToString()
		{
			string result;
			if (this.Thing != null)
			{
				result = "(" + this.Thing + ")";
			}
			else
			{
				result = string.Concat(new object[]
				{
					"(",
					this.Thing,
					", ",
					(this.StuffDef == null) ? "null" : this.StuffDef.defName,
					")"
				});
			}
			return result;
		}

		// Token: 0x060037DD RID: 14301 RVA: 0x001DB0DC File Offset: 0x001D94DC
		public override int GetHashCode()
		{
			int num = 0;
			num = Gen.HashCombineInt(num, (int)this.defInt.shortHash);
			if (this.thingInt != null)
			{
				num = Gen.HashCombineInt(num, this.thingInt.thingIDNumber);
			}
			if (this.stuffDefInt != null)
			{
				num = Gen.HashCombineInt(num, (int)this.stuffDefInt.shortHash);
			}
			return num;
		}

		// Token: 0x060037DE RID: 14302 RVA: 0x001DB140 File Offset: 0x001D9540
		public override bool Equals(object obj)
		{
			bool result;
			if (!(obj is StatRequest))
			{
				result = false;
			}
			else
			{
				StatRequest statRequest = (StatRequest)obj;
				result = (statRequest.defInt == this.defInt && statRequest.thingInt == this.thingInt && statRequest.stuffDefInt == this.stuffDefInt);
			}
			return result;
		}

		// Token: 0x060037DF RID: 14303 RVA: 0x001DB1A4 File Offset: 0x001D95A4
		public bool Equals(StatRequest other)
		{
			return other.defInt == this.defInt && other.thingInt == this.thingInt && other.stuffDefInt == this.stuffDefInt;
		}

		// Token: 0x060037E0 RID: 14304 RVA: 0x001DB1F0 File Offset: 0x001D95F0
		public static bool operator ==(StatRequest lhs, StatRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060037E1 RID: 14305 RVA: 0x001DB210 File Offset: 0x001D9610
		public static bool operator !=(StatRequest lhs, StatRequest rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x040023C9 RID: 9161
		private Thing thingInt;

		// Token: 0x040023CA RID: 9162
		private BuildableDef defInt;

		// Token: 0x040023CB RID: 9163
		private ThingDef stuffDefInt;

		// Token: 0x040023CC RID: 9164
		private QualityCategory qualityCategoryInt;
	}
}
