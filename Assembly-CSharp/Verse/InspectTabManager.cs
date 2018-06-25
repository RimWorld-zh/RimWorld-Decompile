using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000862 RID: 2146
	public static class InspectTabManager
	{
		// Token: 0x04001A5B RID: 6747
		private static Dictionary<Type, InspectTabBase> sharedInstances = new Dictionary<Type, InspectTabBase>();

		// Token: 0x060030AF RID: 12463 RVA: 0x001A7050 File Offset: 0x001A5450
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
	}
}
