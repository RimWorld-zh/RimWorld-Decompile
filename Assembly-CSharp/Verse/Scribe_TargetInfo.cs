using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000DAB RID: 3499
	public static class Scribe_TargetInfo
	{
		// Token: 0x06004E24 RID: 20004 RVA: 0x0028E91E File Offset: 0x0028CD1E
		public static void Look(ref LocalTargetInfo value, string label)
		{
			Scribe_TargetInfo.Look(ref value, false, label, LocalTargetInfo.Invalid);
		}

		// Token: 0x06004E25 RID: 20005 RVA: 0x0028E92E File Offset: 0x0028CD2E
		public static void Look(ref LocalTargetInfo value, bool saveDestroyedThings, string label)
		{
			Scribe_TargetInfo.Look(ref value, saveDestroyedThings, label, LocalTargetInfo.Invalid);
		}

		// Token: 0x06004E26 RID: 20006 RVA: 0x0028E93E File Offset: 0x0028CD3E
		public static void Look(ref LocalTargetInfo value, string label, LocalTargetInfo defaultValue)
		{
			Scribe_TargetInfo.Look(ref value, false, label, defaultValue);
		}

		// Token: 0x06004E27 RID: 20007 RVA: 0x0028E94C File Offset: 0x0028CD4C
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

		// Token: 0x06004E28 RID: 20008 RVA: 0x0028EA03 File Offset: 0x0028CE03
		public static void Look(ref TargetInfo value, string label)
		{
			Scribe_TargetInfo.Look(ref value, false, label, TargetInfo.Invalid);
		}

		// Token: 0x06004E29 RID: 20009 RVA: 0x0028EA13 File Offset: 0x0028CE13
		public static void Look(ref TargetInfo value, bool saveDestroyedThings, string label)
		{
			Scribe_TargetInfo.Look(ref value, saveDestroyedThings, label, TargetInfo.Invalid);
		}

		// Token: 0x06004E2A RID: 20010 RVA: 0x0028EA23 File Offset: 0x0028CE23
		public static void Look(ref TargetInfo value, string label, TargetInfo defaultValue)
		{
			Scribe_TargetInfo.Look(ref value, false, label, defaultValue);
		}

		// Token: 0x06004E2B RID: 20011 RVA: 0x0028EA30 File Offset: 0x0028CE30
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

		// Token: 0x06004E2C RID: 20012 RVA: 0x0028EB3B File Offset: 0x0028CF3B
		public static void Look(ref GlobalTargetInfo value, string label)
		{
			Scribe_TargetInfo.Look(ref value, false, label, GlobalTargetInfo.Invalid);
		}

		// Token: 0x06004E2D RID: 20013 RVA: 0x0028EB4B File Offset: 0x0028CF4B
		public static void Look(ref GlobalTargetInfo value, bool saveDestroyedThings, string label)
		{
			Scribe_TargetInfo.Look(ref value, saveDestroyedThings, label, GlobalTargetInfo.Invalid);
		}

		// Token: 0x06004E2E RID: 20014 RVA: 0x0028EB5B File Offset: 0x0028CF5B
		public static void Look(ref GlobalTargetInfo value, string label, GlobalTargetInfo defaultValue)
		{
			Scribe_TargetInfo.Look(ref value, false, label, defaultValue);
		}

		// Token: 0x06004E2F RID: 20015 RVA: 0x0028EB68 File Offset: 0x0028CF68
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
