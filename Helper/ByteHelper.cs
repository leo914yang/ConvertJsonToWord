namespace ConvertJsonToWord.Helper
{
    /// <summary>
    /// ByteHelper
    /// </summary>
    public class ByteHelper
    {
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 設置當前stream的位置為stream的起點 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        // byte[] 轉成 Stream
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }
    }
}
