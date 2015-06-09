using System;
using Microsoft.SPOT.Hardware;
using NetMF.IO;

namespace IngenuityMicro.Radius.Hardware
{
    public class Flash
    {
        /// <summary>
        /// Setup extrnal flash to act as a file system
        /// </summary>
        /// <returns>TinyFileSystem</returns>
        public static TinyFileSystem SetUpTfs()
        {
            var spiConfig = new SPI.Configuration(Pin.PB12, false, 0, 0, false, true, 12000, SPI.SPI_module.SPI1);
            var spi = new SPI(spiConfig);

            // Instantiate the block driver
            var driver = new MX25l3206BlockDriver(spi, Pin.PB4, 4);

            // Instantiate the file system passing the block driver for the underlying storage medium
            TinyFileSystem retVal = new TinyFileSystem(driver);

            return retVal;
        }
        /// <summary>
        /// Check if Flash is formatted for TFS use
        /// </summary>
        /// <param name="tfs">TinyFileSystem</param>
        /// <returns>bool</returns>
        public static bool CheckFlashIsFormatted(TinyFileSystem tfs)
        {
            return tfs.CheckIfFormatted();
        }
        /// <summary>
        /// Mount TFS
        /// </summary>
        /// <param name="tfs">TinyFileSystem</param>
        public static void Mount(TinyFileSystem tfs)
        {
            tfs.Mount();
        }
        /// <summary>
        /// Format Flash for TFS use
        /// </summary>
        /// <param name="tfs">TinyFileSystem</param>
        public static void FormatFlash(TinyFileSystem tfs)
        {
            tfs.Format();
        }
    }
}
