using UnitGenerator;

namespace TosCore
{
    internal static class UnitOfOptions
    {
        public const UnitGenerateOptions Default = UnitGenerateOptions.ArithmeticOperator | UnitGenerateOptions.ValueArithmeticOperator | UnitGenerateOptions.Comparable | UnitGenerateOptions.MinMaxMethod;
    }
}