using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000864 RID: 2148
	public static class InspectTabManager
	{
		// Token: 0x060030B1 RID: 12465 RVA: 0x001A69E8 File Offset: 0x001A4DE8
		public static InspectTabBase GetSharedInstance(Type tabType)
		{
			InspectTabBase inspectTabBase;
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

		// Token: 0x04001A59 RID: 6745
		private static Dictionary<Type, InspectTabBase> sharedInstances = new Dictionary<Type, InspectTabBase>();
	}
}
