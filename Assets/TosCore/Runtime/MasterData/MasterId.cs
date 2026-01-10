using System;
using UnityEngine;

namespace TosCore.MasterData
{
    [Serializable]
    public struct MasterId<T> : IEquatable<MasterId<T>>
    {
        [SerializeField]
        private int _id;

        public MasterId(int id)
        {
            _id = id;
        }

        public static MasterId<T> None => default;

        public int ID => _id;

        public bool IsValid => _id != 0;

        public bool Equals(MasterId<T> other) => _id == other._id;

        public override bool Equals(object obj) => obj is MasterId<T> other && Equals(other);

        public override int GetHashCode() => _id.GetHashCode();

        public override string ToString() => _id.ToString();

        public static bool operator ==(MasterId<T> left, MasterId<T> right) => left.Equals(right);

        public static bool operator !=(MasterId<T> left, MasterId<T> right) => !left.Equals(right);
    }
}