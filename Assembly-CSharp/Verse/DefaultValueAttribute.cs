using System;

namespace Verse
{
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultValueAttribute : Attribute
	{
		public object value;

		public DefaultValueAttribute(object value)
		{
			this.value = value;
		}

		public virtual bool ObjIsDefault(object obj)
		{
			bool result;
			if (obj == null)
			{
				result = (this.value == null);
			}
			else
			{
				result = (this.value != null && this.value.Equals(obj));
			}
			return result;
		}
	}
}
