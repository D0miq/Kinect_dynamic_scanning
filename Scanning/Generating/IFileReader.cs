namespace Generating
{
    /// <summary>
    /// An instance of the <see cref = "IFileReader"/> interface represents a reader.
    /// It should be able to read frame data from a file.
    /// </summary>
    /// <seealso cref="BinFileReader"/>
    public interface IFileReader
    {
        /// <summary>
        /// Reads data from a file and returns them as a frame.
        /// </summary>
        /// <param name="filePath">Path of the file.</param>
        /// <returns>The frame that holds frame data.</returns>
        Frame ReadFile(string filePath);
    }
}
