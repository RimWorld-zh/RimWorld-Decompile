using System;
using System.Collections.Generic;

namespace Verse
{
	[AttributeUsage(AttributeTargets.Field)]
	public class DefaultEmptyListAttribute : DefaultValueAttribute
	{
		public DefaultEmptyListAttribute(Type type) : base(type)
		{
		}

		public override bool ObjIsDefault(object obj)
		{
			bool result;
			if (obj == null)
			{
				result = false;
			}
			else if (obj.GetType().GetGenericTypeDefinition() != typeof(List<>))
			{
				result = false;
			}
			else
			{
				Type[] genericArguments = obj.GetType().GetGenericArguments();
				result = ((byte)((genericArguments.Length == 1 && genericArguments[0] == (Type)base.value) ? (((int)obj.GetType().GetProperty("Count").GetValue(obj, null) == 0) ? 1 : 0) : 0) != 0);
			}
			return result;
		}
	}
}
