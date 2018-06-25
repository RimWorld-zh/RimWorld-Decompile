using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000574 RID: 1396
	public class WorldFeatureTextMesh_Legacy : WorldFeatureTextMesh
	{
		// Token: 0x04000F73 RID: 3955
		private TextMesh textMesh;

		// Token: 0x04000F74 RID: 3956
		private const float TextScale = 0.23f;

		// Token: 0x04000F75 RID: 3957
		private const int MinFontSize = 13;

		// Token: 0x04000F76 RID: 3958
		private const int MaxFontSize = 40;

		// Token: 0x04000F77 RID: 3959
		[TweakValue("Interface.World", 0f, 10f)]
		private static float TextScaleFactor = 7.5f;

		// Token: 0x06001A83 RID: 6787 RVA: 0x000E4E04 File Offset: 0x000E3204
		private static void TextScaleFactor_Changed()
		{
			Find.WorldFeatures.textsCreated = false;
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06001A84 RID: 6788 RVA: 0x000E4E14 File Offset: 0x000E3214
		public override bool Active
		{
			get
			{
				return this.textMesh.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06001A85 RID: 6789 RVA: 0x000E4E3C File Offset: 0x000E323C
		public override Vector3 Position
		{
			get
			{
				return this.textMesh.transform.position;
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06001A86 RID: 6790 RVA: 0x000E4E64 File Offset: 0x000E3264
		// (set) Token: 0x06001A87 RID: 6791 RVA: 0x000E4E84 File Offset: 0x000E3284
		public override Color Color
		{
			get
			{
				return this.textMesh.color;
			}
			set
			{
				this.textMesh.color = value;
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06001A88 RID: 6792 RVA: 0x000E4E94 File Offset: 0x000E3294
		// (set) Token: 0x06001A89 RID: 6793 RVA: 0x000E4EB4 File Offset: 0x000E32B4
		public override string Text
		{
			get
			{
				return this.textMesh.text;
			}
			set
			{
				this.textMesh.text = value;
			}
		}

		// Token: 0x170003CB RID: 971
		// (set) Token: 0x06001A8A RID: 6794 RVA: 0x000E4EC3 File Offset: 0x000E32C3
		public override float Size
		{
			set
			{
				this.textMesh.fontSize = Mathf.RoundToInt(value * WorldFeatureTextMesh_Legacy.TextScaleFactor);
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06001A8B RID: 6795 RVA: 0x000E4EE0 File Offset: 0x000E32E0
		// (set) Token: 0x06001A8C RID: 6796 RVA: 0x000E4F05 File Offset: 0x000E3305
		public override Quaternion Rotation
		{
			get
			{
				return this.textMesh.transform.rotation;
			}
			set
			{
				this.textMesh.transform.rotation = value;
			}
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06001A8D RID: 6797 RVA: 0x000E4F1C File Offset: 0x000E331C
		// (set) Token: 0x06001A8E RID: 6798 RVA: 0x000E4F41 File Offset: 0x000E3341
		public override Vector3 LocalPosition
		{
			get
			{
				return this.textMesh.transform.localPosition;
			}
			set
			{
				this.textMesh.transform.localPosition = value;
			}
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x000E4F55 File Offset: 0x000E3355
		public override void SetActive(bool active)
		{
			this.textMesh.gameObject.SetActive(active);
		}

		// Token: 0x06001A90 RID: 6800 RVA: 0x000E4F69 File Offset: 0x000E3369
		public override void Destroy()
		{
			UnityEngine.Object.Destroy(this.textMesh.gameObject);
		}

		// Token: 0x06001A91 RID: 6801 RVA: 0x000E4F7C File Offset: 0x000E337C
		public override void Init()
		{
			GameObject gameObject = new GameObject("World feature name (legacy)");
			gameObject.layer = WorldCameraManager.WorldLayer;
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			this.textMesh = gameObject.AddComponent<TextMesh>();
			this.textMesh.color = new Color(1f, 1f, 1f, 0f);
			this.textMesh.anchor = TextAnchor.MiddleCenter;
			this.textMesh.alignment = TextAlignment.Center;
			this.textMesh.GetComponent<MeshRenderer>().sharedMaterial.renderQueue = WorldMaterials.FeatureNameRenderQueue;
			this.Color = new Color(1f, 1f, 1f, 0f);
			this.textMesh.transform.localScale = new Vector3(0.23f, 0.23f, 0.23f);
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x000E504B File Offset: 0x000E344B
		public override void WrapAroundPlanetSurface()
		{
		}
	}
}
