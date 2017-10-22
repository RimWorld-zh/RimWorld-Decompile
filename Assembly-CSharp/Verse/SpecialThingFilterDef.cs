using System;
using System.Collections.Generic;

namespace Verse
{
	public class SpecialThingFilterDef : Def
	{
		public ThingCategoryDef parentCategory;

		public string saveKey;

		public bool allowedByDefault;

		public bool configurable = true;

		public Type workerClass;

		[Unsaved]
		private SpecialThingFilterWorker workerInt;

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

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string item in base.ConfigErrors())
			{
				yield return item;
			}
			if (this.workerClass == null)
			{
				yield return "SpecialThingFilterDef " + base.defName + " has no worker class.";
			}
		}

		public static SpecialThingFilterDef Named(string defName)
		{
			return DefDatabase<SpecialThingFilterDef>.GetNamed(defName, true);
		}
	}
}
