using System;
using System.Collections.Generic;
using System.Diagnostics;

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

		[DebuggerHidden]
		public override IEnumerable<string> ConfigErrors()
		{
			SpecialThingFilterDef.<ConfigErrors>c__Iterator257 <ConfigErrors>c__Iterator = new SpecialThingFilterDef.<ConfigErrors>c__Iterator257();
			<ConfigErrors>c__Iterator.<>f__this = this;
			SpecialThingFilterDef.<ConfigErrors>c__Iterator257 expr_0E = <ConfigErrors>c__Iterator;
			expr_0E.$PC = -2;
			return expr_0E;
		}

		public static SpecialThingFilterDef Named(string defName)
		{
			return DefDatabase<SpecialThingFilterDef>.GetNamed(defName, true);
		}
	}
}
