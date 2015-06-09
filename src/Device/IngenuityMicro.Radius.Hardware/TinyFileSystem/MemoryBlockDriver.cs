using System;
using Microsoft.SPOT;

namespace NetMF.IO
{
  /// <summary>
  /// Concreate implementation of IBlockDriver.  
  /// </summary>
  /// <remarks>
  /// This is implemented purely for testing purposes and therefore does not represent a realworld implementation.
  /// </remarks>
  public class MemoryBlockDriver : IBlockDriver
  {
    private byte[] _disk;
    private byte[] _wearCount;

    /// <summary>
    /// Creates an instance of the MemoryBlockDriver
    /// </summary>
    public MemoryBlockDriver()
    {
      _disk = new byte[DeviceSize];
      _wearCount = new byte[DeviceSize];
    }

    /// <summary>
    /// Full capacity of the device in bytes.
    /// </summary>
    public int DeviceSize { get { return 1024 * 32; } }

    /// <summary>
    /// The size in bytes of a sector on the device.
    /// </summary>  
    public int SectorSize { get { return 1024; } }

    /// <summary>
    /// The cluster size in bytes.
    /// </summary>    
    public ushort ClusterSize { get { return 256; } }

    /// <summary>
    /// Erases the entire device.
    /// </summary>
    public void Erase()
    {
      int count = DeviceSize / SectorSize;
      for (int i = 0; i < DeviceSize; i++)
      {
        _disk[i] = 0xff;
      }
    }

    /// <summary>
    /// Erases a sector on the device.
    /// </summary>
    /// <param name="sectorId">Sector to be erased.</param>
    public void EraseSector(int sectorId)
    {
      int address = sectorId * SectorSize;
      int count = SectorSize;
      for (int i = 0; i < count; i++)
      {
        _disk[address + i] = 0xff;
        _wearCount[address + i]++;
      }
    }

    /// <summary>
    /// Read a block of data from a cluster.
    /// </summary>
    /// <param name="clusterId">The cluster to read from.</param>
    /// <param name="data">The byte array to fill with the data read from the device.</param>
    public void Read(ushort clusterId, byte[] data)
    {
      Read(clusterId, 0, data, 0, data.Length);
    }

    /// <summary>
    /// Read a block of data from a cluster.
    /// </summary>
    /// <param name="clusterId">The cluster to read from.</param>
    /// <param name="clusterOffset">The offset into the cluster to start reading from.</param>
    /// <param name="data">The byte array to fill with the data read from the device.</param>
    /// <param name="index">The index into the array to start filling the data.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    public void Read(ushort clusterId, int clusterOffset, byte[] data, int index, int count)
    {
      int address = (clusterId * ClusterSize) + clusterOffset;
      if (address + count > DeviceSize)
      {
        throw new Exception("Past end of address space");
      }
      Array.Copy(_disk, address, data, index, count);
    }

    /// <summary>
    /// Write a block of data to a cluster.
    /// </summary>
    /// <param name="clusterId">The cluster to write to.</param>
    /// <param name="data">The byte array containing the data to be written.</param>
    /// <param name="index">The index into the array to start writting from</param>
    /// <param name="count">The number of bytes to write.</param>
    public void Write(ushort clusterId, byte[] data, int index, int count)
    {
      Write(clusterId, 0, data, index, count);
    }

    /// <summary>
    /// Write a block of data to a cluster.
    /// </summary>
    /// <param name="clusterId">The cluster to write to.</param>
    /// <param name="clusterOffset">The offset into the cluster to start writting to.</param>
    /// <param name="data">The byte array containing the data to be written.</param>
    /// <param name="index">The index into the array to start writting from</param>
    /// <param name="count">The number of bytes to write.</param>
    public void Write(ushort clusterId, int clusterOffset, byte[] data, int index, int count)
    {
      int address = (clusterId * ClusterSize) + clusterOffset;
      if (address + count > DeviceSize)
      {
        throw new Exception("Past end of address space");
      }

      for (int i = 0; i < count; i++)
      {
        _disk[address + i] &= data[index + i];
        _wearCount[address + i]++;
      }
    }
  }
}
