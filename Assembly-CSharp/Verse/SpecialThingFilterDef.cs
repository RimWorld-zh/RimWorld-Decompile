using System;
using System.Collections.Generic;

namespace Verse
{
	public class SpecialThingFilterDef : Def
	{
		public ThingCategoryDef parentCategory;

		public string saveKey;

		public bool allowedByDefault = false;

		public bool configurable = true;

		public Type workerClass = null;

		[Unsaved]
		private SpecialThingFilterWorker workerInt = null;

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
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string err = enumerator.Current;
					yield return err;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.workerClass != null)
				yield break;
			yield return "SpecialThingFilterDef " + base.defName + " has no worker class.";
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0103:
			/*Error near IL_0104: Unexpected return in MoveNext()*/;
		}

		public static SpecialThingFilterDef Named(string defName)
		{
			return DefDatabase<SpecialThingFilterDef>.GetNamed(defName, true);
		}
	}
}
