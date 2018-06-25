using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000DA0 RID: 3488
	public class DebugLoadIDsSavingErrorsChecker
	{
		// Token: 0x04003407 RID: 13319
		private HashSet<string> deepSaved = new HashSet<string>();

		// Token: 0x04003408 RID: 13320
		private HashSet<DebugLoadIDsSavingErrorsChecker.ReferencedObject> referenced = new HashSet<DebugLoadIDsSavingErrorsChecker.ReferencedObject>();

		// Token: 0x06004DF0 RID: 19952 RVA: 0x0028BE8D File Offset: 0x0028A28D
		public void Clear()
		{
			if (Prefs.DevMode)
			{
				this.deepSaved.Clear();
				this.referenced.Clear();
			}
		}

		// Token: 0x06004DF1 RID: 19953 RVA: 0x0028BEB8 File Offset: 0x0028A2B8
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

		// Token: 0x06004DF2 RID: 19954 RVA: 0x0028BF8C File Offset: 0x0028A38C
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

		// Token: 0x06004DF3 RID: 19955 RVA: 0x0028C054 File Offset: 0x0028A454
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

		// Token: 0x02000DA1 RID: 3489
		private struct ReferencedObject : IEquatable<DebugLoadIDsSavingErrorsChecker.ReferencedObject>
		{
			// Token: 0x04003409 RID: 13321
			public string loadID;

			// Token: 0x0400340A RID: 13322
			public string label;

			// Token: 0x06004DF4 RID: 19956 RVA: 0x0028C0D7 File Offset: 0x0028A4D7
			public ReferencedObject(string loadID, string label)
			{
				this.loadID = loadID;
				this.label = label;
			}

			// Token: 0x06004DF5 RID: 19957 RVA: 0x0028C0E8 File Offset: 0x0028A4E8
			public override bool Equals(object obj)
			{
				return obj is DebugLoadIDsSavingErrorsChecker.ReferencedObject && this.Equals((DebugLoadIDsSavingErrorsChecker.ReferencedObject)obj);
			}

			// Token: 0x06004DF6 RID: 19958 RVA: 0x0028C11C File Offset: 0x0028A51C
			public bool Equals(DebugLoadIDsSavingErrorsChecker.ReferencedObject other)
			{
				return this.loadID == other.loadID && this.label == other.label;
			}

			// Token: 0x06004DF7 RID: 19959 RVA: 0x0028C160 File Offset: 0x0028A560
			public override int GetHashCode()
			{
				int seed = 0;
				seed = Gen.HashCombine<string>(seed, this.loadID);
				return Gen.HashCombine<string>(seed, this.label);
			}

			// Token: 0x06004DF8 RID: 19960 RVA: 0x0028C194 File Offset: 0x0028A594
			public static bool operator ==(DebugLoadIDsSavingErrorsChecker.ReferencedObject lhs, DebugLoadIDsSavingErrorsChecker.ReferencedObject rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06004DF9 RID: 19961 RVA: 0x0028C1B4 File Offset: 0x0028A5B4
			public static bool operator !=(DebugLoadIDsSavingErrorsChecker.ReferencedObject lhs, DebugLoadIDsSavingErrorsChecker.ReferencedObject rhs)
			{
				return !(lhs == rhs);
			}
		}
	}
}
