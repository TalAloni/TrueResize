using System;
using System.Collections.Generic;
using System.Text;
using DiskAccessLibrary;
using DiskAccessLibrary.LogicalDiskManager;

namespace TrueCryptLibrary
{
    public class VolumeSelectionHelper
    {
        /// <summary>
        /// If a disk has more than one partition, the last one is the only one we'll be able to resize without moving partitions around
        /// </summary>
        public static Partition GetLastPartition(Disk disk)
        {
            MasterBootRecord mbr = MasterBootRecord.ReadFromDisk(disk);
            if (mbr == null)
            {
                return new RemovableVolume(disk);
            }
            else
            {
                if (!DynamicDisk.IsDynamicDisk(disk))
                {
                    List<Partition> partitions = BasicDiskHelper.GetPartitions(disk);
                    if (partitions.Count > 0)
                    {
                        return partitions[partitions.Count - 1];
                    }
                }
                return null;
            }
        }
    }
}
