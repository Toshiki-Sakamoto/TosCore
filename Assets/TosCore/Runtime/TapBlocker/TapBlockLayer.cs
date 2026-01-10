using System;

namespace TosCore.TapBlocker
{
    /// <summary>
    /// レイヤーごとのソート順と識別ラベルをまとめた値オブジェクト。
    /// </summary>
    public readonly struct TapBlockLayer : IEquatable<TapBlockLayer>
    {
        public string Name { get; }
        public int SortingOrder { get; }

        public TapBlockLayer(string name, int sortingOrder)
        {
            Name = string.IsNullOrWhiteSpace(name) ? "Default" : name;
            SortingOrder = sortingOrder;
        }

        public static TapBlockLayer Create(string name, int sortingOrder) => new(name, sortingOrder);

        public bool Equals(TapBlockLayer other) => SortingOrder == other.SortingOrder;

        public override bool Equals(object obj) => obj is TapBlockLayer other && Equals(other);

        public override int GetHashCode() => SortingOrder;

        public static bool operator ==(TapBlockLayer left, TapBlockLayer right) => left.Equals(right);

        public static bool operator !=(TapBlockLayer left, TapBlockLayer right) => !left.Equals(right);

        public override string ToString() => $"{Name} (Order:{SortingOrder})";
    }

}