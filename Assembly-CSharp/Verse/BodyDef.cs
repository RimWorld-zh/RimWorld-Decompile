using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000AFA RID: 2810
	public class BodyDef : Def
	{
		// Token: 0x04002766 RID: 10086
		public BodyPartRecord corePart = null;

		// Token: 0x04002767 RID: 10087
		[Unsaved]
		private List<BodyPartRecord> cachedAllParts = new List<BodyPartRecord>();

		// Token: 0x04002768 RID: 10088
		[Unsaved]
		private List<BodyPartRecord> cachedPartsVulnerableToFrostbite = null;

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x06003E51 RID: 15953 RVA: 0x0020D7CC File Offset: 0x0020BBCC
		public List<BodyPartRecord> AllParts
		{
			get
			{
				return this.cachedAllParts;
			}
		}

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x06003E52 RID: 15954 RVA: 0x0020D7E8 File Offset: 0x0020BBE8
		public List<BodyPartRecord> AllPartsVulnerableToFrostbite
		{
			get
			{
				return this.cachedPartsVulnerableToFrostbite;
			}
		}

		// Token: 0x06003E53 RID: 15955 RVA: 0x0020D804 File Offset: 0x0020BC04
		public IEnumerable<BodyPartRecord> GetPartsWithTag(BodyPartTagDef tag)
		{
			for (int i = 0; i < this.AllParts.Count; i++)
			{
				BodyPartRecord part = this.AllParts[i];
				if (part.def.tags.Contains(tag))
				{
					yield return part;
				}
			}
			yield break;
		}

		// Token: 0x06003E54 RID: 15956 RVA: 0x0020D838 File Offset: 0x0020BC38
		public IEnumerable<BodyPartRecord> GetPartsWithDef(BodyPartDef def)
		{
			for (int i = 0; i < this.AllParts.Count; i++)
			{
				BodyPartRecord part = this.AllParts[i];
				if (part.def == def)
				{
					yield return part;
				}
			}
			yield break;
		}

		// Token: 0x06003E55 RID: 15957 RVA: 0x0020D86C File Offset: 0x0020BC6C
		public bool HasPartWithTag(BodyPartTagDef tag)
		{
			for (int i = 0; i < this.AllParts.Count; i++)
			{
				BodyPartRecord bodyPartRecord = this.AllParts[i];
				if (bodyPartRecord.def.tags.Contains(tag))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003E56 RID: 15958 RVA: 0x0020D8CC File Offset: 0x0020BCCC
		public BodyPartRecord GetPartAtIndex(int index)
		{
			BodyPartRecord result;
			if (index < 0 || index >= this.cachedAllParts.Count)
			{
				result = null;
			}
			else
			{
				result = this.cachedAllParts[index];
			}
			return result;
		}

		// Token: 0x06003E57 RID: 15959 RVA: 0x0020D90C File Offset: 0x0020BD0C
		public int GetIndexOfPart(BodyPartRecord rec)
		{
			for (int i = 0; i < this.cachedAllParts.Count; i++)
			{
				if (this.cachedAllParts[i] == rec)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06003E58 RID: 15960 RVA: 0x0020D95C File Offset: 0x0020BD5C
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return e;
			}
			if (this.cachedPartsVulnerableToFrostbite.NullOrEmpty<BodyPartRecord>())
			{
				yield return "no parts vulnerable to frostbite";
			}
			foreach (BodyPartRecord part in this.AllParts)
			{
				if (part.def.conceptual && part.coverageAbs != 0f)
				{
					yield return string.Format("part {0} is tagged conceptual, but has nonzero coverage", part);
				}
			}
			yield break;
		}

		// Token: 0x06003E59 RID: 15961 RVA: 0x0020D988 File Offset: 0x0020BD88
		public override void ResolveReferences()
		{
			if (this.corePart != null)
			{
				this.CacheDataRecursive(this.corePart);
			}
			this.cachedPartsVulnerableToFrostbite = new List<BodyPartRecord>();
			List<BodyPartRecord> allParts = this.AllParts;
			for (int i = 0; i < allParts.Count; i++)
			{
				if (allParts[i].def.frostbiteVulnerability > 0f)
				{
					this.cachedPartsVulnerableToFrostbite.Add(allParts[i]);
				}
			}
		}

		// Token: 0x06003E5A RID: 15962 RVA: 0x0020DA0C File Offset: 0x0020BE0C
		private void CacheDataRecursive(BodyPartRecord node)
		{
			if (node.def == null)
			{
				Log.Error("BodyPartRecord with null def. body=" + this, false);
			}
			else
			{
				node.body = this;
				for (int i = 0; i < node.parts.Count; i++)
				{
					node.parts[i].parent = node;
				}
				if (node.parent != null)
				{
					node.coverageAbsWithChildren = node.parent.coverageAbsWithChildren * node.coverage;
				}
				else
				{
					node.coverageAbsWithChildren = 1f;
				}
				float num = 1f;
				for (int j = 0; j < node.parts.Count; j++)
				{
					num -= node.parts[j].coverage;
				}
				if (num <= 0f)
				{
					num = 0f;
					Log.Warning(string.Concat(new string[]
					{
						"BodyDef ",
						this.defName,
						" has BodyPartRecord of ",
						node.def.defName,
						" whose children have more or equal total coverage than 1. This means parent can't be hit independently at all."
					}), false);
				}
				node.coverageAbs = node.coverageAbsWithChildren * num;
				if (node.height == BodyPartHeight.Undefined)
				{
					node.height = BodyPartHeight.Middle;
				}
				if (node.depth == BodyPartDepth.Undefined)
				{
					node.depth = BodyPartDepth.Outside;
				}
				for (int k = 0; k < node.parts.Count; k++)
				{
					if (node.parts[k].height == BodyPartHeight.Undefined)
					{
						node.parts[k].height = node.height;
					}
					if (node.parts[k].depth == BodyPartDepth.Undefined)
					{
						node.parts[k].depth = node.depth;
					}
				}
				this.cachedAllParts.Add(node);
				for (int l = 0; l < node.parts.Count; l++)
				{
					this.CacheDataRecursive(node.parts[l]);
				}
			}
		}
	}
}
