#if (UNITY_WINRT || UNITY_WP8 || UNITY_PS3 || UNITY_WIIU) && !UNITY_EDITOR

using System;


namespace UnityEngine
{
    public class RPC : Attribute
    {
    }
}
#endif
