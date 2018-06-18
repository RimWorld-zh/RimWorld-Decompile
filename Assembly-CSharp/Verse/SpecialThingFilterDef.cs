using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FC8 RID: 4040
	public class SpecialThingFilterDef : Def
	{
		// Token: 0x17000FCE RID: 4046
		// (get) Token: 0x0600618F RID: 24975 RVA: 0x003126DC File Offset: 0x00310ADC
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

		// Token: 0x06006190 RID: 24976 RVA: 0x0031271C File Offset: 0x00310B1C
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

		// Token: 0x06006191 RID: 24977 RVA: 0x00312748 File Offset: 0x00310B48
		public static SpecialThingFilterDef Named(string defName)
		{
			return DefDatabase<SpecialThingFilterDef>.GetNamed(defName, true);
		}

		// Token: 0x04003FBB RID: 16315
		public ThingCategoryDef parentCategory;

		// Token: 0x04003FBC RID: 16316
		public string saveKey;

		// Token: 0x04003FBD RID: 16317
		public bool allowedByDefault = false;

		// Token: 0x04003FBE RID: 16318
		public bool configurable = true;

		// Token: 0x04003FBF RID: 16319
		public Type workerClass = null;

		// Token: 0x04003FC0 RID: 16320
		[Unsaved]
		private SpecialThingFilterWorker workerInt = null;
	}
}
