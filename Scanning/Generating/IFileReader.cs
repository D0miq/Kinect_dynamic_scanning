namespace Generating
{
    /// <summary>
    /// 
    /// </summary>
    interface IFileReader
    {
        /// <summary>
        /// 
        /// </summary>
        void ReadFile(string filePath, out ushort[] depthData, out byte[] colorData);
    }
}
