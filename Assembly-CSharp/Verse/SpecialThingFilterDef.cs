using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FCD RID: 4045
	public class SpecialThingFilterDef : Def
	{
		// Token: 0x04003FE0 RID: 16352
		public ThingCategoryDef parentCategory;

		// Token: 0x04003FE1 RID: 16353
		public string saveKey;

		// Token: 0x04003FE2 RID: 16354
		public bool allowedByDefault = false;

		// Token: 0x04003FE3 RID: 16355
		public bool configurable = true;

		// Token: 0x04003FE4 RID: 16356
		public Type workerClass = null;

		// Token: 0x04003FE5 RID: 16357
		[Unsaved]
		private SpecialThingFilterWorker workerInt = null;

		// Token: 0x17000FD3 RID: 4051
		// (get) Token: 0x060061C8 RID: 25032 RVA: 0x00315290 File Offset: 0x00313690
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

		// Token: 0x060061C9 RID: 25033 RVA: 0x003152D0 File Offset: 0x003136D0
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

		// Token: 0x060061CA RID: 25034 RVA: 0x003152FC File Offset: 0x003136FC
		public static SpecialThingFilterDef Named(string defName)
		{
			return DefDatabase<SpecialThingFilterDef>.GetNamed(defName, true);
		}
	}
}
