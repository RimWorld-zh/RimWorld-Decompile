using System;

namespace Verse
{
	public struct ThingRequest
	{
		public ThingDef singleDef;

		public ThingRequestGroup group;

		public bool IsUndefined
		{
			get
			{
				return this.singleDef == null && this.group == ThingRequestGroup.Undefined;
			}
		}

		public static ThingRequest ForUndefined()
		{
			return new ThingRequest
			{
				singleDef = null,
				group = ThingRequestGroup.Undefined
			};
		}

		public static ThingRequest ForDef(ThingDef singleDef)
		{
			return new ThingRequest
			{
				singleDef = singleDef,
				group = ThingRequestGroup.Undefined
			};
		}

		public static ThingRequest ForGroup(ThingRequestGroup group)
		{
			return new ThingRequest
			{
				singleDef = null,
				group = group
			};
		}

		public bool Accepts(Thing t)
		{
			if (this.singleDef != null)
			{
				return t.def == this.singleDef;
			}
			if (this.group != ThingRequestGroup.Everything)
			{
				return this.group.Includes(t.def);
			}
			return true;
		}

		public override string ToString()
		{
			string str = (this.singleDef == null) ? ("group " + ((Enum)(object)this.group).ToString()) : ("singleDef " + this.singleDef.defName);
			return "ThingRequest(" + str + ")";
		}
	}
}
