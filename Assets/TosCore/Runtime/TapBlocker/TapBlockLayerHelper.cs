using System;

namespace TosCore.TapBlocker
{
    /// <summary>
    /// プロジェクト独自の列挙型を TapBlockLayer に変換するためのヘルパー。
    /// </summary>
    public static class TapBlockLayerHelper
    {
        /// <summary>
        /// 列挙値の整数値をソート順に、その ToString をラベルにして <see cref="TapBlockLayer"/> を生成する。
        /// </summary>
        public static TapBlockLayer FromEnum<TEnum>(TEnum value) where TEnum : Enum
        {
            var order = Convert.ToInt32(value);
            var name = $"{typeof(TEnum).Name}.{value}";
            return new TapBlockLayer(name, order);
        }
    }

}