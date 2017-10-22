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
				for (int i = 0; i < this.resolvedDesignators.Count; i++)
				{
					Designator des = this.resolvedDesignators[i];
					if (rules.DesignatorAllowed(des))
					{
						yield return des;
					}
				}
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
			LongEventHandler.ExecuteWhenFinished((Action)delegate
			{
				this.ResolveDesignators();
			});
			this.cachedHighlightClosedTag = "DesignationCategoryButton-" + base.defName + "-Closed";
		}

		private void ResolveDesignators()
		{
			this.resolvedDesignators.Clear();
			List<Type>.Enumerator enumerator = this.specialDesignatorClasses.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Type current = enumerator.Current;
					Designator designator = null;
					try
					{
						designator = (Designator)Activator.CreateInstance(current);
					}
					catch (Exception ex)
					{
						Log.Error("DesignationCategoryDef" + base.defName + " could not instantiate special designator from class " + current + ".\n Exception: \n" + ex.ToString());
					}
					if (designator != null)
					{
						this.resolvedDesignators.Add(designator);
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
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
