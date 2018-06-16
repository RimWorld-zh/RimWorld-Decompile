using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000DA1 RID: 3489
	public class DebugLoadIDsSavingErrorsChecker
	{
		// Token: 0x06004DD9 RID: 19929 RVA: 0x0028A4F1 File Offset: 0x002888F1
		public void Clear()
		{
			if (Prefs.DevMode)
			{
				this.deepSaved.Clear();
				this.referenced.Clear();
			}
		}

		// Token: 0x06004DDA RID: 19930 RVA: 0x0028A51C File Offset: 0x0028891C
		public void CheckForErrorsAndClear()
		{
			if (Prefs.DevMode)
			{
				if (!Scribe.saver.savingForDebug)
				{
					foreach (DebugLoadIDsSavingErrorsChecker.ReferencedObject referencedObject in this.referenced)
					{
						if (!this.deepSaved.Contains(referencedObject.loadID))
						{
							Log.Warning(string.Concat(new string[]
							{
								"Object with load ID ",
								referencedObject.loadID,
								" is referenced (xml node name: ",
								referencedObject.label,
								") but is not deep-saved. This will cause errors during loading."
							}), false);
						}
					}
				}
				this.Clear();
			}
		}

		// Token: 0x06004DDB RID: 19931 RVA: 0x0028A5F0 File Offset: 0x002889F0
		public void RegisterDeepSaved(object obj, string label)
		{
			if (Prefs.DevMode)
			{
				if (Scribe.mode != LoadSaveMode.Saving)
				{
					Log.Error(string.Concat(new object[]
					{
						"Registered ",
						obj,
						", but current mode is ",
						Scribe.mode
					}), false);
				}
				else if (obj != null)
				{
					ILoadReferenceable loadReferenceable = obj as ILoadReferenceable;
					if (loadReferenceable != null)
					{
						if (!this.deepSaved.Add(loadReferenceable.GetUniqueLoadID()))
						{
							Log.Warning(string.Concat(new string[]
							{
								"DebugLoadIDsSavingErrorsChecker error: tried to register deep-saved object with loadID ",
								loadReferenceable.GetUniqueLoadID(),
								", but it's already here. label=",
								label,
								" (not cleared after the previous save? different objects have the same load ID? the same object is deep-saved twice?)"
							}), false);
						}
					}
				}
			}
		}

		// Token: 0x06004DDC RID: 19932 RVA: 0x0028A6B8 File Offset: 0x00288AB8
		public void RegisterReferenced(ILoadReferenceable obj, string label)
		{
			if (Prefs.DevMode)
			{
				if (Scribe.mode != LoadSaveMode.Saving)
				{
					Log.Error(string.Concat(new object[]
					{
						"Registered ",
						obj,
						", but current mode is ",
						Scribe.mode
					}), false);
				}
				else if (obj != null)
				{
					this.referenced.Add(new DebugLoadIDsSavingErrorsChecker.ReferencedObject(obj.GetUniqueLoadID(), label));
				}
			}
		}

		// Token: 0x040033F7 RID: 13303
		private HashSet<string> deepSaved = new HashSet<string>();

		// Token: 0x040033F8 RID: 13304
		private HashSet<DebugLoadIDsSavingErrorsChecker.ReferencedObject> referenced = new HashSet<DebugLoadIDsSavingErrorsChecker.ReferencedObject>();

		// Token: 0x02000DA2 RID: 3490
		private struct ReferencedObject : IEquatable<DebugLoadIDsSavingErrorsChecker.ReferencedObject>
		{
			// Token: 0x06004DDD RID: 19933 RVA: 0x0028A73B File Offset: 0x00288B3B
			public ReferencedObject(string loadID, string label)
			{
				this.loadID = loadID;
				this.label = label;
			}

			// Token: 0x06004DDE RID: 19934 RVA: 0x0028A74C File Offset: 0x00288B4C
			public override bool Equals(object obj)
			{
				return obj is DebugLoadIDsSavingErrorsChecker.ReferencedObject && this.Equals((DebugLoadIDsSavingErrorsChecker.ReferencedObject)obj);
			}

			// Token: 0x06004DDF RID: 19935 RVA: 0x0028A780 File Offset: 0x00288B80
			public bool Equals(DebugLoadIDsSavingErrorsChecker.ReferencedObject other)
			{
				return this.loadID == other.loadID && this.label == other.label;
			}

			// Token: 0x06004DE0 RID: 19936 RVA: 0x0028A7C4 File Offset: 0x00288BC4
			public override int GetHashCode()
			{
				int seed = 0;
				seed = Gen.HashCombine<string>(seed, this.loadID);
				return Gen.HashCombine<string>(seed, this.label);
			}

			// Token: 0x06004DE1 RID: 19937 RVA: 0x0028A7F8 File Offset: 0x00288BF8
			public static bool operator ==(DebugLoadIDsSavingErrorsChecker.ReferencedObject lhs, DebugLoadIDsSavingErrorsChecker.ReferencedObject rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06004DE2 RID: 19938 RVA: 0x0028A818 File Offset: 0x00288C18
			public static bool operator !=(DebugLoadIDsSavingErrorsChecker.ReferencedObject lhs, DebugLoadIDsSavingErrorsChecker.ReferencedObject rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x040033F9 RID: 13305
			public string loadID;

			// Token: 0x040033FA RID: 13306
			public string label;
		}
	}
}
