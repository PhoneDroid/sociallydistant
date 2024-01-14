﻿#nullable enable

using System.Text.RegularExpressions;
using Shell.Common;
using TMPro;
using UI.PlayerUI;
using UnityEngine;
using UnityEngine.UI;
using UnityExtensions;
using Shell;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace UI.Widgets
{
	[ExecuteInEditMode]
	public class CompositeIconWidget : MonoBehaviour
	{
		[Header("Appearance")]
		[SerializeField]
		private CompositeIcon icon;

		[Header("UI")]
		[SerializeField]
		private TextMeshProUGUI? textIcon;

		[SerializeField]
		private Image? spriteIcon;

		private UiManager uiManager = null!;

		private void Awake()
		{
			this.AssertAllFieldsAreSerialized(typeof(CompositeIconWidget));
			this.MustGetComponentInParent(out uiManager);
		}

		public CompositeIcon Icon
		{
			get => icon;
			set
			{
				icon = value;
				UpdateIcon();
			}
		}
		
		private void Update()
		{
#if UNITY_EDITOR
			if (!EditorApplication.isPlaying)
				UpdateIcon();
#endif
		}

		private void UpdateIcon()
		{
			if (uiManager == null)
				return;
			
			// prefer sprites over text
			if (textIcon != null && (icon.spriteIcon != null || string.IsNullOrEmpty(icon.textIcon)))
			{
				textIcon.color = default;

				if (spriteIcon != null)
				{
					spriteIcon.color = icon.iconColor.AsUnityColor();
					spriteIcon.sprite = icon.spriteIcon;
				}
			}
			else
			{
				if (textIcon != null)
				{
					textIcon.color = icon.iconColor.AsUnityColor();
					textIcon.SetText(Regex.Unescape(icon.textIcon));
				}

				if (spriteIcon != null)
				{
					spriteIcon.color = default;
					spriteIcon.sprite = null;
				}
			}
		}
	}
}