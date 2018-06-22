using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009BD RID: 2493
	public struct StatRequest : IEquatable<StatRequest>
	{
		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x060037CF RID: 14287 RVA: 0x001DB140 File Offset: 0x001D9540
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x060037D0 RID: 14288 RVA: 0x001DB15C File Offset: 0x001D955C
		public BuildableDef Def
		{
			get
			{
				return this.defInt;
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x060037D1 RID: 14289 RVA: 0x001DB178 File Offset: 0x001D9578
		public ThingDef StuffDef
		{
			get
			{
				return this.stuffDefInt;
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x060037D2 RID: 14290 RVA: 0x001DB194 File Offset: 0x001D9594
		public QualityCategory QualityCategory
		{
			get
			{
				return this.qualityCategoryInt;
			}
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x060037D3 RID: 14291 RVA: 0x001DB1B0 File Offset: 0x001D95B0
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x060037D4 RID: 14292 RVA: 0x001DB1D4 File Offset: 0x001D95D4
		public bool Empty
		{
			get
			{
				return this.Def == null;
			}
		}

		// Token: 0x060037D5 RID: 14293 RVA: 0x001DB1F4 File Offset: 0x001D95F4
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

		// Token: 0x060037D6 RID: 14294 RVA: 0x001DB260 File Offset: 0x001D9660
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

		// Token: 0x060037D7 RID: 14295 RVA: 0x001DB2BC File Offset: 0x001D96BC
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

		// Token: 0x060037D8 RID: 14296 RVA: 0x001DB2FC File Offset: 0x001D96FC
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

		// Token: 0x060037D9 RID: 14297 RVA: 0x001DB388 File Offset: 0x001D9788
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

		// Token: 0x060037DA RID: 14298 RVA: 0x001DB3EC File Offset: 0x001D97EC
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

		// Token: 0x060037DB RID: 14299 RVA: 0x001DB450 File Offset: 0x001D9850
		public bool Equals(StatRequest other)
		{
			return other.defInt == this.defInt && other.thingInt == this.thingInt && other.stuffDefInt == this.stuffDefInt;
		}

		// Token: 0x060037DC RID: 14300 RVA: 0x001DB49C File Offset: 0x001D989C
		public static bool operator ==(StatRequest lhs, StatRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060037DD RID: 14301 RVA: 0x001DB4BC File Offset: 0x001D98BC
		public static bool operator !=(StatRequest lhs, StatRequest rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x040023C4 RID: 9156
		private Thing thingInt;

		// Token: 0x040023C5 RID: 9157
		private BuildableDef defInt;

		// Token: 0x040023C6 RID: 9158
		private ThingDef stuffDefInt;

		// Token: 0x040023C7 RID: 9159
		private QualityCategory qualityCategoryInt;
	}
}
