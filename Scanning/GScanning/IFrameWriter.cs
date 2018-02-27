namespace GScanning
{
    /// <summary>
    /// An instance of the <see cref="IFrameWriter"/> interface represents a writer.
    /// It should be able to write <paramref name="frame"/> into some kind of output stream.
    /// The declared method should be implemented as thread-safe to avoid some possible future errors during frame handling.
    /// </summary>
    /// <seealso cref="BinFrameWriter"/>
    /// <seealso cref="FrameHandler"/>
    public interface IFrameWriter
    {
        /// <summary>
        /// Writes the given <paramref name="frame"/> into output stream.
        /// </summary>
        /// <param name="frame">The given frame that is written into output stream.</param>
        void WriteFrame(Frame frame);
    }
}
