using System;
using Microsoft.SPOT;
using System.IO;

namespace IngenuityMicro.Radius
{
    public interface IFileSystem
    {
        void Initialize();
        bool IsFormatted { get; }
        void Format();
        void Mount();
        Stream Open(string fileName, FileMode fileMode);
        string[] GetFiles();
        void Delete(string filename);
    }
}
