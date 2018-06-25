using System;
using Verse;

namespace RimWorld
{
	public struct StatRequest : IEquatable<StatRequest>
	{
		private Thing thingInt;

		private BuildableDef defInt;

		private ThingDef stuffDefInt;

		private QualityCategory qualityCategoryInt;

		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		public BuildableDef Def
		{
			get
			{
				return this.defInt;
			}
		}

		public ThingDef StuffDef
		{
			get
			{
				return this.stuffDefInt;
			}
		}

		public QualityCategory QualityCategory
		{
			get
			{
				return this.qualityCategoryInt;
			}
		}

		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		public bool Empty
		{
			get
			{
				return this.Def == null;
			}
		}

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

		public bool Equals(StatRequest other)
		{
			return other.defInt == this.defInt && other.thingInt == this.thingInt && other.stuffDefInt == this.stuffDefInt;
		}

		public static bool operator ==(StatRequest lhs, StatRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		public static bool operator !=(StatRequest lhs, StatRequest rhs)
		{
			return !(lhs == rhs);
		}
	}
}
