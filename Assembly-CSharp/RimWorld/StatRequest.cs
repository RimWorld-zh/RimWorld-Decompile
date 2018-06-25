using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009BF RID: 2495
	public struct StatRequest : IEquatable<StatRequest>
	{
		// Token: 0x040023C5 RID: 9157
		private Thing thingInt;

		// Token: 0x040023C6 RID: 9158
		private BuildableDef defInt;

		// Token: 0x040023C7 RID: 9159
		private ThingDef stuffDefInt;

		// Token: 0x040023C8 RID: 9160
		private QualityCategory qualityCategoryInt;

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x060037D3 RID: 14291 RVA: 0x001DB280 File Offset: 0x001D9680
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x060037D4 RID: 14292 RVA: 0x001DB29C File Offset: 0x001D969C
		public BuildableDef Def
		{
			get
			{
				return this.defInt;
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x060037D5 RID: 14293 RVA: 0x001DB2B8 File Offset: 0x001D96B8
		public ThingDef StuffDef
		{
			get
			{
				return this.stuffDefInt;
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x060037D6 RID: 14294 RVA: 0x001DB2D4 File Offset: 0x001D96D4
		public QualityCategory QualityCategory
		{
			get
			{
				return this.qualityCategoryInt;
			}
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x060037D7 RID: 14295 RVA: 0x001DB2F0 File Offset: 0x001D96F0
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x060037D8 RID: 14296 RVA: 0x001DB314 File Offset: 0x001D9714
		public bool Empty
		{
			get
			{
				return this.Def == null;
			}
		}

		// Token: 0x060037D9 RID: 14297 RVA: 0x001DB334 File Offset: 0x001D9734
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

		// Token: 0x060037DA RID: 14298 RVA: 0x001DB3A0 File Offset: 0x001D97A0
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

		// Token: 0x060037DB RID: 14299 RVA: 0x001DB3FC File Offset: 0x001D97FC
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

		// Token: 0x060037DC RID: 14300 RVA: 0x001DB43C File Offset: 0x001D983C
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

		// Token: 0x060037DD RID: 14301 RVA: 0x001DB4C8 File Offset: 0x001D98C8
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

		// Token: 0x060037DE RID: 14302 RVA: 0x001DB52C File Offset: 0x001D992C
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

		// Token: 0x060037DF RID: 14303 RVA: 0x001DB590 File Offset: 0x001D9990
		public bool Equals(StatRequest other)
		{
			return other.defInt == this.defInt && other.thingInt == this.thingInt && other.stuffDefInt == this.stuffDefInt;
		}

		// Token: 0x060037E0 RID: 14304 RVA: 0x001DB5DC File Offset: 0x001D99DC
		public static bool operator ==(StatRequest lhs, StatRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060037E1 RID: 14305 RVA: 0x001DB5FC File Offset: 0x001D99FC
		public static bool operator !=(StatRequest lhs, StatRequest rhs)
		{
			return !(lhs == rhs);
		}
	}
}
