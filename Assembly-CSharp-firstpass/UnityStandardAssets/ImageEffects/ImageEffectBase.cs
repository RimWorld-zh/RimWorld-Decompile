using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x0200019B RID: 411
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("")]
	public class ImageEffectBase : MonoBehaviour
	{
		// Token: 0x04000806 RID: 2054
		public Shader shader;

		// Token: 0x04000807 RID: 2055
		private Material m_Material;

		// Token: 0x06000926 RID: 2342 RVA: 0x00013704 File Offset: 0x00011904
		protected virtual void Start()
		{
			if (!SystemInfo.supportsImageEffects)
			{
				base.enabled = false;
			}
			else if (!this.shader || !this.shader.isSupported)
			{
				base.enabled = false;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000927 RID: 2343 RVA: 0x00013750 File Offset: 0x00011950
		protected Material material
		{
			get
			{
				if (this.m_Material == null)
				{
					this.m_Material = new Material(this.shader);
					this.m_Material.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.m_Material;
			}
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0001379C File Offset: 0x0001199C
		protected virtual void OnDisable()
		{
			if (this.m_Material)
			{
				UnityEngine.Object.DestroyImmediate(this.m_Material);
			}
		}
	}
}
