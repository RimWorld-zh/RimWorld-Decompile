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
			ThingRequest result = default(ThingRequest);
			result.singleDef = null;
			result.group = ThingRequestGroup.Undefined;
			return result;
		}

		public static ThingRequest ForDef(ThingDef singleDef)
		{
			ThingRequest result = default(ThingRequest);
			result.singleDef = singleDef;
			result.group = ThingRequestGroup.Undefined;
			return result;
		}

		public static ThingRequest ForGroup(ThingRequestGroup group)
		{
			ThingRequest result = default(ThingRequest);
			result.singleDef = null;
			result.group = group;
			return result;
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
			string str = (this.singleDef == null) ? ("group " + this.group.ToString()) : ("singleDef " + this.singleDef.defName);
			return "ThingRequest(" + str + ")";
		}
	}
}
