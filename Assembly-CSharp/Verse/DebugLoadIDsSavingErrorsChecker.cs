using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000D9F RID: 3487
	public class DebugLoadIDsSavingErrorsChecker
	{
		// Token: 0x04003400 RID: 13312
		private HashSet<string> deepSaved = new HashSet<string>();

		// Token: 0x04003401 RID: 13313
		private HashSet<DebugLoadIDsSavingErrorsChecker.ReferencedObject> referenced = new HashSet<DebugLoadIDsSavingErrorsChecker.ReferencedObject>();

		// Token: 0x06004DF0 RID: 19952 RVA: 0x0028BBAD File Offset: 0x00289FAD
		public void Clear()
		{
			if (Prefs.DevMode)
			{
				this.deepSaved.Clear();
				this.referenced.Clear();
			}
		}

		// Token: 0x06004DF1 RID: 19953 RVA: 0x0028BBD8 File Offset: 0x00289FD8
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

		// Token: 0x06004DF2 RID: 19954 RVA: 0x0028BCAC File Offset: 0x0028A0AC
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

		// Token: 0x06004DF3 RID: 19955 RVA: 0x0028BD74 File Offset: 0x0028A174
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

		// Token: 0x02000DA0 RID: 3488
		private struct ReferencedObject : IEquatable<DebugLoadIDsSavingErrorsChecker.ReferencedObject>
		{
			// Token: 0x04003402 RID: 13314
			public string loadID;

			// Token: 0x04003403 RID: 13315
			public string label;

			// Token: 0x06004DF4 RID: 19956 RVA: 0x0028BDF7 File Offset: 0x0028A1F7
			public ReferencedObject(string loadID, string label)
			{
				this.loadID = loadID;
				this.label = label;
			}

			// Token: 0x06004DF5 RID: 19957 RVA: 0x0028BE08 File Offset: 0x0028A208
			public override bool Equals(object obj)
			{
				return obj is DebugLoadIDsSavingErrorsChecker.ReferencedObject && this.Equals((DebugLoadIDsSavingErrorsChecker.ReferencedObject)obj);
			}

			// Token: 0x06004DF6 RID: 19958 RVA: 0x0028BE3C File Offset: 0x0028A23C
			public bool Equals(DebugLoadIDsSavingErrorsChecker.ReferencedObject other)
			{
				return this.loadID == other.loadID && this.label == other.label;
			}

			// Token: 0x06004DF7 RID: 19959 RVA: 0x0028BE80 File Offset: 0x0028A280
			public override int GetHashCode()
			{
				int seed = 0;
				seed = Gen.HashCombine<string>(seed, this.loadID);
				return Gen.HashCombine<string>(seed, this.label);
			}

			// Token: 0x06004DF8 RID: 19960 RVA: 0x0028BEB4 File Offset: 0x0028A2B4
			public static bool operator ==(DebugLoadIDsSavingErrorsChecker.ReferencedObject lhs, DebugLoadIDsSavingErrorsChecker.ReferencedObject rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x06004DF9 RID: 19961 RVA: 0x0028BED4 File Offset: 0x0028A2D4
			public static bool operator !=(DebugLoadIDsSavingErrorsChecker.ReferencedObject lhs, DebugLoadIDsSavingErrorsChecker.ReferencedObject rhs)
			{
				return !(lhs == rhs);
			}
		}
	}
}
