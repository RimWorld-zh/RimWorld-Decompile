using RimWorld;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Verse
{
	public class Def : Editable
	{
		[Description("The name of this Def. It is used as an identifier by the game code.")]
		[NoTranslate]
		public string defName = "UnnamedDef";

		[Description("A human-readable label used to identify this in game.")]
		[DefaultValue(null)]
		public string label;

		[Description("A human-readable description given when the Def is inspected by players.")]
		[DefaultValue(null)]
		public string description;

		[Description("Mod-specific data. Not used by core game code.")]
		[DefaultValue(null)]
		public List<DefModExtension> modExtensions;

		[Unsaved]
		public ushort shortHash;

		[Unsaved]
		public ushort index = 65535;

		[Unsaved]
		private string cachedLabelCap;

		[Unsaved]
		public ushort debugRandomId = (ushort)Rand.RangeInclusive(0, 65535);

		public const string DefaultDefName = "UnnamedDef";

		private static Regex AllowedDefnamesRegex = new Regex("^[a-zA-Z0-9\\-_]*$");

		public string LabelCap
		{
			get
			{
				if (this.label.NullOrEmpty())
				{
					return null;
				}
				if (this.cachedLabelCap.NullOrEmpty())
				{
					this.cachedLabelCap = this.label.CapitalizeFirst();
				}
				return this.cachedLabelCap;
			}
		}

		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			yield break;
		}

		public override IEnumerable<string> ConfigErrors()
		{
			if (this.defName == "UnnamedDef")
			{
				yield return base.GetType() + " lacks defName. Label=" + this.label;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.defName == "null")
			{
				yield return "defName cannot be the string 'null'.";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (!Def.AllowedDefnamesRegex.IsMatch(this.defName))
			{
				yield return "defName " + this.defName + " should only contain letters, numbers, underscores, or dashes.";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.modExtensions != null)
			{
				for (int i = 0; i < this.modExtensions.Count; i++)
				{
					using (IEnumerator<string> enumerator = this.modExtensions[i].ConfigErrors().GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							string err = enumerator.Current;
							yield return err;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
			}
			yield break;
			IL_01f5:
			/*Error near IL_01f6: Unexpected return in MoveNext()*/;
		}

		public virtual void ClearCachedData()
		{
			this.cachedLabelCap = null;
		}

		public override string ToString()
		{
			return this.defName;
		}

		public override int GetHashCode()
		{
			return this.defName.GetHashCode();
		}

		public T GetModExtension<T>() where T : DefModExtension
		{
			if (this.modExtensions == null)
			{
				return (T)null;
			}
			for (int i = 0; i < this.modExtensions.Count; i++)
			{
				if (this.modExtensions[i] is T)
				{
					return (T)(this.modExtensions[i] as T);
				}
			}
			return (T)null;
		}

		public bool HasModExtension<T>() where T : DefModExtension
		{
			return this.GetModExtension<T>() != null;
		}
	}
}
