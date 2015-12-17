using System;
using WavAudio.IO;

namespace WavAudio
{
    public static class Program
    {
        public static void Main(string[] args) {
            var file = new WavFile(Console.ReadLine());
            Console.ReadKey();
        }
    }
}
