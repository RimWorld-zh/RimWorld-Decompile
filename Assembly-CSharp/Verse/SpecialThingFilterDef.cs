using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FC9 RID: 4041
	public class SpecialThingFilterDef : Def
	{
		// Token: 0x17000FCF RID: 4047
		// (get) Token: 0x06006191 RID: 24977 RVA: 0x00312600 File Offset: 0x00310A00
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

		// Token: 0x06006192 RID: 24978 RVA: 0x00312640 File Offset: 0x00310A40
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

		// Token: 0x06006193 RID: 24979 RVA: 0x0031266C File Offset: 0x00310A6C
		public static SpecialThingFilterDef Named(string defName)
		{
			return DefDatabase<SpecialThingFilterDef>.GetNamed(defName, true);
		}

		// Token: 0x04003FBC RID: 16316
		public ThingCategoryDef parentCategory;

		// Token: 0x04003FBD RID: 16317
		public string saveKey;

		// Token: 0x04003FBE RID: 16318
		public bool allowedByDefault = false;

		// Token: 0x04003FBF RID: 16319
		public bool configurable = true;

		// Token: 0x04003FC0 RID: 16320
		public Type workerClass = null;

		// Token: 0x04003FC1 RID: 16321
		[Unsaved]
		private SpecialThingFilterWorker workerInt = null;
	}
}
