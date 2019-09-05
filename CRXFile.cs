using System;
using System.IO;

namespace CRXExtractor
{
    class CRXFile
    {
        public char[] MagicNumber { get; set; }
        public bool IsMagicNumberValid { get; set; }
        public UInt32 Version { get; set; }
        public bool IsVersionSupported { get; set; }
        public UInt32 PubKeyLength { get; set; }
        public UInt32 SignatureLength { get; set; }
        public byte[] PubKey { get; set; }
        public byte[] Signature { get; set; }
        public byte[] ZipContents { get; set; }

        private CRXFile()
        {
            this.MagicNumber = new char[4];
            this.IsMagicNumberValid = false;
            this.IsVersionSupported = false;
        }

        public static CRXFile FromFile(string filePath)
        {
            CRXFile CRXData = new CRXFile();
            // Will throw exception if reading error
            using (BinaryReader br = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read)))
            {
                // Read MagicNumber
                for (int i = 0; i < 4; i++)
                {
                    CRXData.MagicNumber[i] = br.ReadChar();
                }
                // Verify MagicNumber
                if (CRXData.MagicNumber[0] != 'C') CRXData.IsMagicNumberValid = false;
                if (CRXData.MagicNumber[1] != 'r') CRXData.IsMagicNumberValid = false;
                if (CRXData.MagicNumber[2] != '2') CRXData.IsMagicNumberValid = false;
                if (CRXData.MagicNumber[3] != '4') CRXData.IsMagicNumberValid = false;
                // Read version
                CRXData.Version = br.ReadUInt32();
                // Verify version
                if (CRXData.Version == 2) CRXData.IsVersionSupported = true;
                // Read public key length
                CRXData.PubKeyLength = br.ReadUInt32();
                // Read signature length
                CRXData.SignatureLength = br.ReadUInt32();
                // Read public key
                CRXData.PubKey = new byte[CRXData.PubKeyLength];
                CRXData.PubKey = br.ReadBytes(Convert.ToInt32(CRXData.PubKeyLength));
                // Read signature
                CRXData.Signature = new byte[CRXData.PubKeyLength];
                CRXData.Signature = br.ReadBytes(Convert.ToInt32(CRXData.SignatureLength));
                // Read ZIP file
                long bytesLeft = br.BaseStream.Length - br.BaseStream.Position;
                CRXData.ZipContents = new byte[bytesLeft];
                CRXData.ZipContents = br.ReadBytes(Convert.ToInt32(bytesLeft));
            }
            return CRXData;
        }

    }
}
