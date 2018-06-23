using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FC9 RID: 4041
	public class SpecialThingFilterDef : Def
	{
		// Token: 0x04003FD8 RID: 16344
		public ThingCategoryDef parentCategory;

		// Token: 0x04003FD9 RID: 16345
		public string saveKey;

		// Token: 0x04003FDA RID: 16346
		public bool allowedByDefault = false;

		// Token: 0x04003FDB RID: 16347
		public bool configurable = true;

		// Token: 0x04003FDC RID: 16348
		public Type workerClass = null;

		// Token: 0x04003FDD RID: 16349
		[Unsaved]
		private SpecialThingFilterWorker workerInt = null;

		// Token: 0x17000FD2 RID: 4050
		// (get) Token: 0x060061B8 RID: 25016 RVA: 0x003147B0 File Offset: 0x00312BB0
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

		// Token: 0x060061B9 RID: 25017 RVA: 0x003147F0 File Offset: 0x00312BF0
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

		// Token: 0x060061BA RID: 25018 RVA: 0x0031481C File Offset: 0x00312C1C
		public static SpecialThingFilterDef Named(string defName)
		{
			return DefDatabase<SpecialThingFilterDef>.GetNamed(defName, true);
		}
	}
}
