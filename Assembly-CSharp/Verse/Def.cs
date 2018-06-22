using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RimWorld;

namespace Verse
{
	// Token: 0x02000AF6 RID: 2806
	public class Def : Editable
	{
		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x06003E30 RID: 15920 RVA: 0x000633F0 File Offset: 0x000617F0
		public string LabelCap
		{
			get
			{
				string result;
				if (this.label.NullOrEmpty())
				{
					result = null;
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

		// Token: 0x06003E31 RID: 15921 RVA: 0x00063448 File Offset: 0x00061848
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			yield break;
		}

		// Token: 0x06003E32 RID: 15922 RVA: 0x0006346C File Offset: 0x0006186C
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.defName == "UnnamedDef")
			{
				yield return base.GetType() + " lacks defName. Label=" + this.label;
			}
			if (this.defName == "null")
			{
				yield return "defName cannot be the string 'null'.";
			}
			if (!Def.AllowedDefnamesRegex.IsMatch(this.defName))
			{
				yield return "defName " + this.defName + " should only contain letters, numbers, underscores, or dashes.";
			}
			if (this.modExtensions != null)
			{
				for (int i = 0; i < this.modExtensions.Count; i++)
				{
					foreach (string err in this.modExtensions[i].ConfigErrors())
					{
						yield return err;
					}
				}
			}
			if (this.description != null)
			{
				if (this.description == "")
				{
					yield return "empty description";
				}
				if (char.IsWhiteSpace(this.description[0]))
				{
					yield return "description has leading whitespace";
				}
				if (char.IsWhiteSpace(this.description[this.description.Length - 1]))
				{
					yield return "description has trailing whitespace";
				}
			}
			yield break;
		}

		// Token: 0x06003E33 RID: 15923 RVA: 0x00063496 File Offset: 0x00061896
		public virtual void ClearCachedData()
		{
			this.cachedLabelCap = null;
		}

		// Token: 0x06003E34 RID: 15924 RVA: 0x000634A0 File Offset: 0x000618A0
		public override string ToString()
		{
			return this.defName;
		}

		// Token: 0x06003E35 RID: 15925 RVA: 0x000634BC File Offset: 0x000618BC
		public override int GetHashCode()
		{
			return this.defName.GetHashCode();
		}

		// Token: 0x06003E36 RID: 15926 RVA: 0x000634DC File Offset: 0x000618DC
		public T GetModExtension<T>() where T : DefModExtension
		{
			T result;
			if (this.modExtensions == null)
			{
				result = (T)((object)null);
			}
			else
			{
				for (int i = 0; i < this.modExtensions.Count; i++)
				{
					if (this.modExtensions[i] is T)
					{
						return this.modExtensions[i] as T;
					}
				}
				result = (T)((object)null);
			}
			return result;
		}

		// Token: 0x06003E37 RID: 15927 RVA: 0x00063560 File Offset: 0x00061960
		public bool HasModExtension<T>() where T : DefModExtension
		{
			return this.GetModExtension<T>() != null;
		}

		// Token: 0x04002749 RID: 10057
		[Description("The name of this Def. It is used as an identifier by the game code.")]
		[NoTranslate]
		public string defName = "UnnamedDef";

		// Token: 0x0400274A RID: 10058
		[Description("A human-readable label used to identify this in game.")]
		[DefaultValue(null)]
		[MustTranslate]
		public string label = null;

		// Token: 0x0400274B RID: 10059
		[Description("A human-readable description given when the Def is inspected by players.")]
		[DefaultValue(null)]
		[MustTranslate]
		public string description = null;

		// Token: 0x0400274C RID: 10060
		[Description("Disables config error checking. Intended for mod use. (Be careful!)")]
		[DefaultValue(false)]
		[MustTranslate]
		public bool ignoreConfigErrors = false;

		// Token: 0x0400274D RID: 10061
		[Description("Mod-specific data. Not used by core game code.")]
		[DefaultValue(null)]
		public List<DefModExtension> modExtensions;

		// Token: 0x0400274E RID: 10062
		[Unsaved]
		public ushort shortHash;

		// Token: 0x0400274F RID: 10063
		[Unsaved]
		public ushort index = ushort.MaxValue;

		// Token: 0x04002750 RID: 10064
		[Unsaved]
		public ModContentPack modContentPack;

		// Token: 0x04002751 RID: 10065
		[Unsaved]
		private string cachedLabelCap = null;

		// Token: 0x04002752 RID: 10066
		[Unsaved]
		public bool generated;

		// Token: 0x04002753 RID: 10067
		[Unsaved]
		public ushort debugRandomId = (ushort)Rand.RangeInclusive(0, 65535);

		// Token: 0x04002754 RID: 10068
		public const string DefaultDefName = "UnnamedDef";

		// Token: 0x04002755 RID: 10069
		private static Regex AllowedDefnamesRegex = new Regex("^[a-zA-Z0-9\\-_]*$");
	}
}
