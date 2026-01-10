using System;
using NUnit.Framework;

namespace TosCore.Attribute
{
    /// <summary> 指定したインターフェイスの派生型情報を選択可能にし、Serialize可能にする </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class SelectableSerializeReferenceAttribute : PropertyAttribute
    {
    }
}