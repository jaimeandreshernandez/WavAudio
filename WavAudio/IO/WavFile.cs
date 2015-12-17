using System;
using System.IO;

namespace WavAudio.IO
{
    public class WavFile : WavDataFile
    {
        public FileInfo FileInfo { private set; get; }

        public WavFile() {

        }

        public WavFile(string file) {

            // Make sure the file actually exists.
            if (File.Exists(file)) {
                // Make sure it's actually a wav file.
                if (file.EndsWith(".wav")) {

                    // Get the file's information.
                    this.FileInfo = new FileInfo(file);

                    var buffer = File.ReadAllBytes(file);

                    using (var memory = new MemoryStream(buffer)) {
                        using (var reader = new BinaryReader(memory)) {

                            // Read the header
                            this.hGroupID = reader.ReadChars(4);
                            this.dwFileLength = reader.ReadUInt32();
                            this.sRiffType = reader.ReadChars(4);

                            // Read the format chunk
                            this.fGroupID = reader.ReadChars(4);
                            this.dwChunkSize = reader.ReadUInt32();
                            this.wFormatTag = reader.ReadUInt16();
                            this.wChannels = reader.ReadUInt16();
                            this.dwSamplesPerSecond = reader.ReadUInt32();
                            this.dwAvgBytesPerSec = reader.ReadUInt32();
                            this.wBlockAlign = reader.ReadUInt16();
                            this.dwBitsPerSample = reader.ReadUInt16();

                            // Read the data chunk
                            this.dGroupID = reader.ReadChars(4);
                            this.dwLength = reader.ReadUInt32();

                            if (this.dwBitsPerSample == 16) {
                                if ((memory.Length - memory.Position) % (this.dwBitsPerSample / 8) == 0) {
                                    this.sData = new short[(memory.Length - memory.Position) / (this.dwBitsPerSample / 8)];

                                    for (int i = 0; i < sData.Length; i++) {
                                        this.sData[i] = reader.ReadInt16();
                                    }

                                }
                            }
                        }
                    }

                } else {
                    Console.WriteLine("File is not a .wav file!");
                }
            } else {
                Console.WriteLine("File does not exist!");
            }
        }

        public void Save(string path) {
            using (var memory = new MemoryStream()) {
                using (var writer = new BinaryWriter(memory)) {

                    writer.Write(this.hGroupID);
                    writer.Write(this.dwFileLength);
                    writer.Write(this.sRiffType);

                    writer.Write(this.fGroupID);
                    writer.Write(this.dwChunkSize);
                    writer.Write(this.wFormatTag);
                    writer.Write(this.wChannels);
                    writer.Write(this.dwSamplesPerSecond);
                    writer.Write(this.dwAvgBytesPerSec);
                    writer.Write(this.wBlockAlign);
                    // Bits per sample is the problem
                    writer.Write(this.dwBitsPerSample);

                    writer.Write(this.dGroupID);
                    writer.Write(this.dwLength);

                    if (this.dwBitsPerSample == 16 && this.dwLength % (this.dwBitsPerSample / 8) == 0) {
                        foreach (var data in sData) {
                            writer.Write(data);
                        }
                    }

                    File.WriteAllBytes(path, memory.ToArray());
                }
            }
        }
    }
}
