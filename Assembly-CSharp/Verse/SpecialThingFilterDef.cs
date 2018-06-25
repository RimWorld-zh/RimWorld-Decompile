using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FCE RID: 4046
	public class SpecialThingFilterDef : Def
	{
		// Token: 0x04003FE8 RID: 16360
		public ThingCategoryDef parentCategory;

		// Token: 0x04003FE9 RID: 16361
		public string saveKey;

		// Token: 0x04003FEA RID: 16362
		public bool allowedByDefault = false;

		// Token: 0x04003FEB RID: 16363
		public bool configurable = true;

		// Token: 0x04003FEC RID: 16364
		public Type workerClass = null;

		// Token: 0x04003FED RID: 16365
		[Unsaved]
		private SpecialThingFilterWorker workerInt = null;

		// Token: 0x17000FD3 RID: 4051
		// (get) Token: 0x060061C8 RID: 25032 RVA: 0x003154D4 File Offset: 0x003138D4
		public SpecialThingFilterWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (SpecialThingFilterWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x060061C9 RID: 25033 RVA: 0x00315514 File Offset: 0x00313914
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string err in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return err;
			}
			if (this.workerClass == null)
			{
				yield return "SpecialThingFilterDef " + this.defName + " has no worker class.";
			}
			yield break;
		}

		// Token: 0x060061CA RID: 25034 RVA: 0x00315540 File Offset: 0x00313940
		public static SpecialThingFilterDef Named(string defName)
		{
			return DefDatabase<SpecialThingFilterDef>.GetNamed(defName, true);
		}
	}
}
