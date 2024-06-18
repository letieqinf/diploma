namespace Tusur.Practices.Server.Extensions
{
    public static class ByteArrayExtensions
    {
        public static int IndexOf(this byte[] array, byte[] sub)
        {
            if (sub.Length == 0)
                return 0;

            for (var i = 0; i < array.Length - sub.Length; i++)
            {
                if (Match(array, sub, i))
                    return i;
            }

            return -1;
        }

        private static bool Match(byte[] array, byte[] sub, int start)
        {
            if (sub.Length + start > array.Length)
                return false;

            for (var i = 0; i < sub.Length; i++)
            {
                if (sub[i] != array[i + start])
                    return false;
            }

            return true;
        }

        public static byte[]? Replace(this byte[] array, byte[] from, byte[] to)
        {
            int subIndex = array.IndexOf(from);
            if (subIndex == -1)
                return null;

            var result = new byte[array.Length - from.Length + to.Length];
            Buffer.BlockCopy(array, 0, result, 0, subIndex);
            Buffer.BlockCopy(to, 0, result, subIndex, to.Length);
            Buffer.BlockCopy(array, subIndex + from.Length, result, subIndex + to.Length, array.Length - (subIndex + from.Length));

            return result;
        }
    }
}
