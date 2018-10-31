using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Utilities;
using DiskAccessLibrary;

namespace TrueCryptLibrary
{
    public class TrueCryptVolume : Volume
    {
        public const long VolumeHeaderOffset = 0;
        public const long HiddenVolumeHeaderOffset = 65536;
        public const int VolumeHeaderLength = 65536;
        public const int VolumeHeaderGroupLength = 131072; // 65536 + 65536

        private Volume m_volume;
        private byte[] m_password;
        private TrueCryptHeader m_header;
        private bool m_isHiddenVolume = false;

        public TrueCryptVolume(Volume volume, string password) : this(volume, Encoding.UTF8.GetBytes(password))
        {
        }

        public TrueCryptVolume(Volume volume, byte[] password)
        {
            m_volume = volume;
            m_password = password;
            //  TrueCrypt first attempts to decrypt the standard volume header using the entered password
            ReadHeader(VolumeHeaderOffset);
            if (!m_header.IsValid)
            {
                ReadHeader(HiddenVolumeHeaderOffset);
                if (m_header.IsValid)
                {
                    m_isHiddenVolume = true;
                }
            }

            if (!m_header.IsValid)
            {
                throw new InvalidDataException("Invalid TrueCrypt volume or incorrect password");
            }

            if (!m_header.IsSupported)
            {
                throw new NotSupportedException("Unsupported TrueCrypt volume format version");
            }
        }

        private void ReadHeader(long volumeHeaderOffset)
        {
            long volumeHeaderSectorIndex = volumeHeaderOffset / m_volume.BytesPerSector;
            byte[] headerBytes = m_volume.ReadSector(volumeHeaderSectorIndex);
            m_header = new TrueCryptHeader(headerBytes, m_password);
        }

        public override byte[] ReadSectors(long sectorIndex, int sectorCount)
        {
            CheckBoundaries(sectorIndex, sectorCount);

            long imageSectorIndex = sectorIndex + (long)m_header.MasterKeyScopeOffset / m_volume.BytesPerSector;
            byte[] sectors = m_volume.ReadSectors(imageSectorIndex, sectorCount);
            Parallel.For(0, sectorCount, delegate(int index){
                int offset = index * m_volume.BytesPerSector;
                XTSHelper.XTSChainDecrypt(m_header.AlgorithmChain, (ulong)(imageSectorIndex + index), sectors, offset, m_volume.BytesPerSector, sectors, offset);
            });
            return sectors;
        }

        public override void WriteSectors(long sectorIndex, byte[] data)
        {
            CheckBoundaries(sectorIndex, data.Length / this.BytesPerSector);

            int sectorCount = data.Length / m_volume.BytesPerSector;
            long imageSectorIndex = sectorIndex + (long)m_header.MasterKeyScopeOffset / m_volume.BytesPerSector;
            byte[] encryptedData = new byte[data.Length];
            Parallel.For(0, sectorCount, delegate(int index)
            {
                int offset = index * m_volume.BytesPerSector;
                XTSHelper.XTSChainEncrypt(m_header.AlgorithmChain, (ulong)(imageSectorIndex + index), data, offset, m_volume.BytesPerSector, encryptedData, offset);
            });
            m_volume.WriteSectors(imageSectorIndex, encryptedData);
        }

        public override long Size
        {
            get
            {
                return (long)m_header.MasterKeyEncryptedAreaSize;
            }
        }

        public override int BytesPerSector
        {
            get
            {
                if (m_header.FormatVersion >= 5)
                {
                    // Only physical disks can have SectorSize != 512
                    return (int)m_header.SectorSize;
                }
                else
                {
                    return m_volume.BytesPerSector;
                }
            }
        }

        public override List<DiskExtent> Extents
        {
            get
            {
                return m_volume.Extents;
            }
        }

        public TrueCryptHeader Header
        {
            get
            {
                return m_header;
            }
        }

        public bool IsHiddenVolume
        {
            get
            {
                return m_isHiddenVolume;
            }
        }
    }
}
