using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000DA8 RID: 3496
	public static class Scribe_TargetInfo
	{
		// Token: 0x06004E20 RID: 20000 RVA: 0x0028E512 File Offset: 0x0028C912
		public static void Look(ref LocalTargetInfo value, string label)
		{
			Scribe_TargetInfo.Look(ref value, false, label, LocalTargetInfo.Invalid);
		}

		// Token: 0x06004E21 RID: 20001 RVA: 0x0028E522 File Offset: 0x0028C922
		public static void Look(ref LocalTargetInfo value, bool saveDestroyedThings, string label)
		{
			Scribe_TargetInfo.Look(ref value, saveDestroyedThings, label, LocalTargetInfo.Invalid);
		}

		// Token: 0x06004E22 RID: 20002 RVA: 0x0028E532 File Offset: 0x0028C932
		public static void Look(ref LocalTargetInfo value, string label, LocalTargetInfo defaultValue)
		{
			Scribe_TargetInfo.Look(ref value, false, label, defaultValue);
		}

		// Token: 0x06004E23 RID: 20003 RVA: 0x0028E540 File Offset: 0x0028C940
		public static void Look(ref LocalTargetInfo value, bool saveDestroyedThings, string label, LocalTargetInfo defaultValue)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (!value.Equals(defaultValue))
				{
					if (value.Thing != null)
					{
						if (Scribe_References.CheckSaveReferenceToDestroyedThing(value.Thing, label, saveDestroyedThings))
						{
							return;
						}
					}
					Scribe.saver.WriteElement(label, value.ToString());
				}
			}
			else if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				value = ScribeExtractor.LocalTargetInfoFromNode(Scribe.loader.curXmlParent[label], label, defaultValue);
			}
			else if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				value = ScribeExtractor.ResolveLocalTargetInfo(value, label);
			}
		}

		// Token: 0x06004E24 RID: 20004 RVA: 0x0028E5F7 File Offset: 0x0028C9F7
		public static void Look(ref TargetInfo value, string label)
		{
			Scribe_TargetInfo.Look(ref value, false, label, TargetInfo.Invalid);
		}

		// Token: 0x06004E25 RID: 20005 RVA: 0x0028E607 File Offset: 0x0028CA07
		public static void Look(ref TargetInfo value, bool saveDestroyedThings, string label)
		{
			Scribe_TargetInfo.Look(ref value, saveDestroyedThings, label, TargetInfo.Invalid);
		}

		// Token: 0x06004E26 RID: 20006 RVA: 0x0028E617 File Offset: 0x0028CA17
		public static void Look(ref TargetInfo value, string label, TargetInfo defaultValue)
		{
			Scribe_TargetInfo.Look(ref value, false, label, defaultValue);
		}

		// Token: 0x06004E27 RID: 20007 RVA: 0x0028E624 File Offset: 0x0028CA24
		public static void Look(ref TargetInfo value, bool saveDestroyedThings, string label, TargetInfo defaultValue)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (!value.Equals(defaultValue))
				{
					if (value.Thing != null)
					{
						if (Scribe_References.CheckSaveReferenceToDestroyedThing(value.Thing, label, saveDestroyedThings))
						{
							return;
						}
					}
					if (!value.HasThing && value.Cell.IsValid && (value.Map == null || !Find.Maps.Contains(value.Map)))
					{
						Scribe.saver.WriteElement(label, "null");
					}
					else
					{
						Scribe.saver.WriteElement(label, value.ToString());
					}
				}
			}
			else if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				value = ScribeExtractor.TargetInfoFromNode(Scribe.loader.curXmlParent[label], label, defaultValue);
			}
			else if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				value = ScribeExtractor.ResolveTargetInfo(value, label);
			}
		}

		// Token: 0x06004E28 RID: 20008 RVA: 0x0028E72F File Offset: 0x0028CB2F
		public static void Look(ref GlobalTargetInfo value, string label)
		{
			Scribe_TargetInfo.Look(ref value, false, label, GlobalTargetInfo.Invalid);
		}

		// Token: 0x06004E29 RID: 20009 RVA: 0x0028E73F File Offset: 0x0028CB3F
		public static void Look(ref GlobalTargetInfo value, bool saveDestroyedThings, string label)
		{
			Scribe_TargetInfo.Look(ref value, saveDestroyedThings, label, GlobalTargetInfo.Invalid);
		}

		// Token: 0x06004E2A RID: 20010 RVA: 0x0028E74F File Offset: 0x0028CB4F
		public static void Look(ref GlobalTargetInfo value, string label, GlobalTargetInfo defaultValue)
		{
			Scribe_TargetInfo.Look(ref value, false, label, defaultValue);
		}

		// Token: 0x06004E2B RID: 20011 RVA: 0x0028E75C File Offset: 0x0028CB5C
		public static void Look(ref GlobalTargetInfo value, bool saveDestroyedThings, string label, GlobalTargetInfo defaultValue)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (!value.Equals(defaultValue))
				{
					if (value.Thing != null)
					{
						if (Scribe_References.CheckSaveReferenceToDestroyedThing(value.Thing, label, saveDestroyedThings))
						{
							return;
						}
					}
					if (value.WorldObject != null && !value.WorldObject.Spawned)
					{
						Scribe.saver.WriteElement(label, "null");
					}
					else if (!value.HasThing && !value.HasWorldObject && value.Cell.IsValid && (value.Map == null || !Find.Maps.Contains(value.Map)))
					{
						Scribe.saver.WriteElement(label, "null");
					}
					else
					{
						Scribe.saver.WriteElement(label, value.ToString());
					}
				}
			}
			else if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				value = ScribeExtractor.GlobalTargetInfoFromNode(Scribe.loader.curXmlParent[label], label, defaultValue);
			}
			else if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				value = ScribeExtractor.ResolveGlobalTargetInfo(value, label);
			}
		}
	}
}
