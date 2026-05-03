

namespace DoohClick.Service.Shared.Helper
{
    public static class CodeGenerator
    {
        private static readonly Random _rng = new();

        public static string Generate(string prefix, string tenantCode, int randomLength = 6)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            string random = new string(
                Enumerable.Range(0, randomLength)
                          .Select(_ => chars[_rng.Next(chars.Length)])
                          .ToArray()
            );

            return $"{prefix.ToUpper()}-{tenantCode.ToUpper()}-{random}";
        }
    }
}
