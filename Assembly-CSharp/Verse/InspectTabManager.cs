using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000860 RID: 2144
	public static class InspectTabManager
	{
		// Token: 0x04001A57 RID: 6743
		private static Dictionary<Type, InspectTabBase> sharedInstances = new Dictionary<Type, InspectTabBase>();

		// Token: 0x060030AC RID: 12460 RVA: 0x001A6C98 File Offset: 0x001A5098
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
