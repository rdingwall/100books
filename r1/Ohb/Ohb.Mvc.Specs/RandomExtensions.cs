// ReSharper disable CheckNamespace
namespace System
// ReSharper restore CheckNamespace
{
    public static class RandomExtensions
    {
        public static long NextNonNegativeLong(this Random rg)
        {
            byte[] bytes = new byte[sizeof(long)];
            rg.NextBytes(bytes);
            // strip out the sign bit
            bytes[7] = (byte)(bytes[7] & 0x7f);
            return BitConverter.ToInt64(bytes, 0);
        }
    }
}