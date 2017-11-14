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
			if (thing == null)
			{
				Log.Error("StatRequest for null thing.");
				return StatRequest.ForEmpty();
			}
			StatRequest result = default(StatRequest);
			result.thingInt = thing;
			result.defInt = thing.def;
			result.stuffDefInt = thing.Stuff;
			thing.TryGetQuality(out result.qualityCategoryInt);
			return result;
		}

		public static StatRequest For(BuildableDef def, ThingDef stuffDef, QualityCategory quality = QualityCategory.Normal)
		{
			if (def == null)
			{
				Log.Error("StatRequest for null def.");
				return StatRequest.ForEmpty();
			}
			StatRequest result = default(StatRequest);
			result.thingInt = null;
			result.defInt = def;
			result.stuffDefInt = stuffDef;
			result.qualityCategoryInt = quality;
			return result;
		}

		public static StatRequest ForEmpty()
		{
			StatRequest result = default(StatRequest);
			result.thingInt = null;
			result.defInt = null;
			result.stuffDefInt = null;
			result.qualityCategoryInt = QualityCategory.Normal;
			return result;
		}

		public override string ToString()
		{
			if (this.Thing != null)
			{
				return "(" + this.Thing + ")";
			}
			return "(" + this.Thing + ", " + ((this.StuffDef == null) ? "null" : this.StuffDef.defName) + ")";
		}

		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombineInt(seed, this.defInt.shortHash);
			if (this.thingInt != null)
			{
				seed = Gen.HashCombineInt(seed, this.thingInt.thingIDNumber);
			}
			if (this.stuffDefInt != null)
			{
				seed = Gen.HashCombineInt(seed, this.stuffDefInt.shortHash);
			}
			return seed;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is StatRequest))
			{
				return false;
			}
			StatRequest statRequest = (StatRequest)obj;
			return statRequest.defInt == this.defInt && statRequest.thingInt == this.thingInt && statRequest.stuffDefInt == this.stuffDefInt;
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
