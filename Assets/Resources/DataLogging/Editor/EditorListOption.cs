using UnityEditor;
using UnityEngine;
using System;

[Flags]
public enum EditorListOption {
	None = 0,
	ListSize = 1,
	ListLabel = 2,
	Default = ListSize | ListLabel
}
