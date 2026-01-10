using System;
using UnityEngine;

namespace TosCore
{
    /// <summary>
    /// System.Typeをシリアライズ可能にするラッパークラス
    /// </summary>
    [Serializable]
    public class SerializableType : ISerializationCallbackReceiver
    {
        [SerializeField] private string _assemblyQualifiedName;

        private Type _type;

        public SerializableType(Type type)
        {
            _type = type;
            _assemblyQualifiedName = type.AssemblyQualifiedName;
        }

        public static implicit operator Type(SerializableType serializableType) =>
            serializableType._type;

        public static bool operator ==(SerializableType serializableType, Type type) =>
            serializableType != null && serializableType._type == type;

        public static bool operator !=(SerializableType serializableType, Type type) => 
            !(serializableType == type);

        /// <summary>
        /// シリアライズ直前に再度対応
        /// </summary>
        public void OnBeforeSerialize()
        {
            if (_type != null)
            {
                _assemblyQualifiedName = _type.AssemblyQualifiedName;
            }
        }

        /// <summary>
        /// デシリアライズ直後
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void OnAfterDeserialize()
        {
            if (!string.IsNullOrEmpty(_assemblyQualifiedName))
            {
                _type = Type.GetType(_assemblyQualifiedName);
            }
            else
            {
                _type = null;
            }
        }

        public override string ToString() =>
            _type?.Name ?? "(None)";
    }
}