using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	public class DesignationCategoryDef : Def
	{
		public List<Type> specialDesignatorClasses = new List<Type>();

		public int order;

		public bool showPowerGrid;

		[Unsaved]
		private List<Designator> resolvedDesignators = new List<Designator>();

		[Unsaved]
		public KeyBindingCategoryDef bindingCatDef;

		[Unsaved]
		public string cachedHighlightClosedTag;

		public IEnumerable<Designator> ResolvedAllowedDesignators
		{
			get
			{
				GameRules rules = Current.Game.Rules;
				int i = 0;
				Designator des;
				while (true)
				{
					if (i < this.resolvedDesignators.Count)
					{
						des = this.resolvedDesignators[i];
						if (!rules.DesignatorAllowed(des))
						{
							i++;
							continue;
						}
						break;
					}
					yield break;
				}
				yield return des;
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public List<Designator> AllResolvedDesignators
		{
			get
			{
				return this.resolvedDesignators;
			}
		}

		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.ResolveDesignators();
			});
			this.cachedHighlightClosedTag = "DesignationCategoryButton-" + base.defName + "-Closed";
		}

		private void ResolveDesignators()
		{
			this.resolvedDesignators.Clear();
			foreach (Type specialDesignatorClass in this.specialDesignatorClasses)
			{
				Designator designator = null;
				try
				{
					designator = (Designator)Activator.CreateInstance(specialDesignatorClass);
				}
				catch (Exception ex)
				{
					Log.Error("DesignationCategoryDef" + base.defName + " could not instantiate special designator from class " + specialDesignatorClass + ".\n Exception: \n" + ex.ToString());
				}
				if (designator != null)
				{
					this.resolvedDesignators.Add(designator);
				}
			}
			IEnumerable<BuildableDef> enumerable = from tDef in DefDatabase<ThingDef>.AllDefs.Cast<BuildableDef>().Concat(DefDatabase<TerrainDef>.AllDefs.Cast<BuildableDef>())
			where tDef.designationCategory == this
			select tDef;
			foreach (BuildableDef item in enumerable)
			{
				this.resolvedDesignators.Add(new Designator_Build(item));
			}
		}
	}
}
