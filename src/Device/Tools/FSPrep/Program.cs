using System;
using Microsoft.SPOT;
using IngenuityMicro.Radius.Hardware;
using System.IO;
using NetMF.IO;
using System.Reflection;
using System.Threading;

namespace FSPrep
{
    public class Program
    {
        private static TinyFileSystem _tfs;

        public static void Main()
        {
            Debug.Print("Radius Filesystem preparation tool");

            Debug.Print("Mount filesystem...");
            _tfs = Flash.SetUpTfs();
            if (!_tfs.CheckIfFormatted())
            {
                Debug.Print("Filesystem requires formatting...");
                _tfs.Format(); // takes quite awhile
                Debug.Print("Formatting complete.");
            }
            _tfs.Mount();

            Debug.Print("Clearing files...");
            var files = _tfs.GetFiles();
            foreach (var file in files)
            {
                _tfs.Delete(file);
            }

            Debug.Print("Installing apps...");
            TransferResource(Resources.BinaryResources.MainMenu, "MainMenu");
            TransferResource(Resources.BinaryResources.AnalogClock, "AnalogClock");

            //Debug.Print("Testing load and initialization of main menu...");
            //TestDynamicLoad("MainMenu");

            Thread.Sleep(Timeout.Infinite);
        }

        private static void TransferResource(Resources.BinaryResources id, string name)
        {
            Debug.Print("Transfering " + name + "...");
            var stream = _tfs.Open(name + ".pe", FileMode.OpenOrCreate);
            // truncate, in case it existed already
            stream.SetLength(0);
            var buffer = Resources.GetBytes(id);

            int offset = 0;
            do
            {
                int len = 512;
                if (len > buffer.Length - offset)
                    len = buffer.Length - offset;
                stream.Write(buffer, offset, len);
                offset += 512;
            } while (offset < buffer.Length);
            stream.Flush();
            stream.Close();
            Debug.Print("Transfer complete.");
        }

        private static void TestDynamicLoad(string name)
        {
            Debug.GC(true);

            using (var stream = _tfs.Open(name + ".pe", FileMode.Open))
            {
                byte[] assmbytes = new byte[stream.Length];
                stream.Read(assmbytes, 0, (int)stream.Length);

                var assm = Assembly.Load(assmbytes);
                var obj = AppDomain.CurrentDomain.CreateInstanceAndUnwrap(assm.FullName, name + ".Application");
                var type = assm.GetType(name + ".Application");
                MethodInfo mi = type.GetMethod("Initialize");

                mi.Invoke(obj, new object[] { null });
            }
        }
    }
}
