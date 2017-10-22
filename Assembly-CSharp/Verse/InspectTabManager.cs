using System;
using System.Collections.Generic;

namespace Verse
{
	public static class InspectTabManager
	{
		private static Dictionary<Type, InspectTabBase> sharedInstances = new Dictionary<Type, InspectTabBase>();

		public static InspectTabBase GetSharedInstance(Type tabType)
		{
			InspectTabBase inspectTabBase = default(InspectTabBase);
			InspectTabBase result;
			if (InspectTabManager.sharedInstances.TryGetValue(tabType, out inspectTabBase))
			{
				result = inspectTabBase;
			}
			else
			{
				inspectTabBase = (InspectTabBase)Activator.CreateInstance(tabType);
				InspectTabManager.sharedInstances.Add(tabType, inspectTabBase);
				result = inspectTabBase;
			}
			return result;
		}
	}
}
