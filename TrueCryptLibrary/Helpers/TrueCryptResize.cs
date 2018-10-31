using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Utilities;
using DiskAccessLibrary;
using DiskAccessLibrary.FileSystems.NTFS;

namespace TrueCryptLibrary
{
    public class TrueCryptResize
    {
        public static TrueCryptResizeStatus ExtendVolumeAndFileSystem(Disk disk, byte[] password, long additionalNumberOfSectors)
        {
            Volume partition = VolumeSelectionHelper.GetLastPartition(disk);
            TrueCryptVolume volume = new TrueCryptVolume(partition, password);
            if (volume.Header.IsValid && !volume.Header.IsSupported)
            {
                return TrueCryptResizeStatus.UnsupportedFormatVersion;
            }

            if (!volume.IsValidAndSupported)
            {
                return TrueCryptResizeStatus.InvalidDisk;
            }

            if (volume.IsHiddenVolume)
            {
                return TrueCryptResizeStatus.HiddenVolume;
            }

            NTFSVolume ntfsVolume;
            try
            {
                ntfsVolume = new NTFSVolume(volume);
            }
            catch (InvalidDataException)
            {
                return TrueCryptResizeStatus.UnsupportedFileSystem;
            }
            catch (NotSupportedException)
            {
                return TrueCryptResizeStatus.UnsupportedFileSystem;
            }

            TrueCryptResizeStatus status = ExtendVolume(disk, password, additionalNumberOfSectors);
            if (status == TrueCryptResizeStatus.Success)
            {
                // Reinitialize the partition / volume
                partition = VolumeSelectionHelper.GetLastPartition(disk);
                volume = new TrueCryptVolume(partition, password);
                ntfsVolume = new NTFSVolume(volume);
                long availableBytes = ntfsVolume.GetMaximumSizeToExtend();
                long availableSectors = availableBytes / disk.BytesPerSector;
                ntfsVolume.Extend(availableSectors);

                return TrueCryptResizeStatus.Success;
            }
            else
            {
                return status;
            }
        }

        /// <summary>
        /// Extend the partition and TrueCrypt volume
        /// </summary>
        public static TrueCryptResizeStatus ExtendVolume(Disk disk, byte[] password, long additionalNumberOfSectors)
        {
            Partition partition = VolumeSelectionHelper.GetLastPartition(disk);
            TrueCryptVolume volume = new TrueCryptVolume(partition, password);

            if (!volume.IsValidAndSupported)
            {
                return TrueCryptResizeStatus.InvalidDisk;
            }

            if (volume.IsHiddenVolume)
            {
                return TrueCryptResizeStatus.HiddenVolume;
            }

            long availableBytes;
            if (partition is MBRPartition || partition is GPTPartition)
            {
                availableBytes = ExtendHelper.GetMaximumSizeToExtendPartition(partition);
            }
            else // Removable volume
            {
                availableBytes = additionalNumberOfSectors * volume.BytesPerSector;
            }

            TrueCryptHeader header = volume.Header;
            long oldBackupHeaderOffset = (long)(header.MasterKeyScopeOffset + header.MasterKeyEncryptedAreaSize);
            long newBackupHeaderOffset = (long)(header.MasterKeyScopeOffset + header.MasterKeyEncryptedAreaSize + (ulong)availableBytes);
            header.MasterKeyEncryptedAreaSize += (ulong)availableBytes;
            header.VolumeSize += (ulong)availableBytes;
            byte[] headerBytes = header.GetBytes(password);
            // Read backup header group from old end of the partition
            byte[] backupHeaderGroupBytes = partition.ReadSectors(oldBackupHeaderOffset / disk.BytesPerSector, TrueCryptVolume.VolumeHeaderGroupLength / disk.BytesPerSector);
            // Updated the backup header (using the stored salt)
            header.Salt = ByteReader.ReadBytes(backupHeaderGroupBytes, 0, 64);
            byte[] backupHeaderBytes = header.GetBytes(password);
            Array.Copy(backupHeaderBytes, 0, backupHeaderGroupBytes, 0, backupHeaderBytes.Length);

            if (partition is MBRPartition || partition is GPTPartition)
            {
                long availableSectors = availableBytes / disk.BytesPerSector;
                ExtendHelper.ExtendPartition((Partition)partition, availableSectors);
                // Reinitialize the partition
                partition = VolumeSelectionHelper.GetLastPartition(disk);
            }

            // Write backup header to the new end of the partition
            partition.WriteSectors(newBackupHeaderOffset / disk.BytesPerSector, backupHeaderGroupBytes);

            // Destroy the old backup header (to prevent decryption of the volume after the password has changed)
            byte[] temp = new byte[TrueCryptVolume.VolumeHeaderGroupLength];
            new Random().NextBytes(temp);
            partition.WriteSectors(oldBackupHeaderOffset / disk.BytesPerSector, temp);

            // Write an updated header
            partition.WriteSectors(0,  headerBytes);

            return TrueCryptResizeStatus.Success;
        }

        public static void FillAllocatedSpaceWithData(DiskImage disk, long oldSize, ref long bytesWritten)
        {
            long startOffset = oldSize;
            long startSector = startOffset / disk.BytesPerSector;
            long sectorCount = disk.TotalSectors - (oldSize / disk.BytesPerSector);
            FillAllocatedSpaceWithData(disk, startSector, sectorCount, ref bytesWritten);
        }

        public static void FillAllocatedSpaceWithData(DiskImage disk, long startSector, long sectorCount, ref long bytesWritten)
        {
            const int DirectTransferSizeLBA = 2048; // 1 MB (assuming 512-byte sectors)
            byte[] data = new byte[DirectTransferSizeLBA * disk.BytesPerSector];
            Random random = new Random();
            long sectorIndex = startSector;
            while (sectorIndex < startSector + sectorCount)
            {
                long sectorsToWrite = startSector + sectorCount - sectorIndex;
                if (sectorsToWrite < DirectTransferSizeLBA)
                {
                    data = new byte[sectorsToWrite * disk.BytesPerSector];
                }
                random.NextBytes(data);
                disk.WriteSectors(sectorIndex, data);
                sectorIndex += DirectTransferSizeLBA;
                bytesWritten += data.Length;
            }
        }
    }
}
