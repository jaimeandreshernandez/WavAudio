    public class WavFile
    {
        // Header file
        public char[] hGroupID = new char[4];
        public uint dwFileLength;
        public char[] sRiffType = new char[4];

        // Format chunk
        public char[] fGroupID = new char[4];
        public uint dwChunkSize;
        public ushort wFormatTag;
        public ushort wChannels;
        public uint dwSamplesPerSecond;
        public uint dwAvgBytesPerSec;
        public ushort wBlockAlign;
        public uint dwBitsPerSample;

        // Data chunk
        public char[] dGroupID = new char[4];
        public uint dwChunkSize;
        public byte[] bData;
        public short[] sData;
        public float[] fData;

        // len = samplespersec * wchannels * secondsofaudio 
    }
