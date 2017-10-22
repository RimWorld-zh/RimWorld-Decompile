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
		public string label = (string)null;

		[Description("A human-readable description given when the Def is inspected by players.")]
		[DefaultValue(null)]
		public string description = (string)null;

		[Description("Mod-specific data. Not used by core game code.")]
		[DefaultValue(null)]
		public List<DefModExtension> modExtensions;

		[Unsaved]
		public ushort shortHash;

		[Unsaved]
		public ushort index = (ushort)65535;

		[Unsaved]
		private string cachedLabelCap = (string)null;

		[Unsaved]
		public ushort debugRandomId = (ushort)Rand.RangeInclusive(0, 65535);

		public const string DefaultDefName = "UnnamedDef";

		private static Regex AllowedDefnamesRegex = new Regex("^[a-zA-Z0-9\\-_]*$");

		public string LabelCap
		{
			get
			{
				string result;
				if (this.label.NullOrEmpty())
				{
					result = (string)null;
				}
				else
				{
					if (this.cachedLabelCap.NullOrEmpty())
					{
						this.cachedLabelCap = this.label.CapitalizeFirst();
					}
					result = this.cachedLabelCap;
				}
				return result;
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
			IL_01fd:
			/*Error near IL_01fe: Unexpected return in MoveNext()*/;
		}

		public virtual void ClearCachedData()
		{
			this.cachedLabelCap = (string)null;
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
			T result;
			int i;
			if (this.modExtensions == null)
			{
				result = (T)null;
			}
			else
			{
				for (i = 0; i < this.modExtensions.Count; i++)
				{
					if (this.modExtensions[i] is T)
						goto IL_0036;
				}
				result = (T)null;
			}
			goto IL_0074;
			IL_0036:
			result = (T)(this.modExtensions[i] as T);
			goto IL_0074;
			IL_0074:
			return result;
		}

		public bool HasModExtension<T>() where T : DefModExtension
		{
			return this.GetModExtension<T>() != null;
		}
	}
}
