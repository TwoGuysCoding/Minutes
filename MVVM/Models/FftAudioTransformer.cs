using MathNet.Numerics.IntegralTransforms;
using System.Numerics;

namespace Minutes.MVVM.Models
{
    internal static class FftAudioTransformer
    {
        public static double[] GetAudioLevels(IEnumerable<byte> audioBuffer, double additionalRandomNoise, int fftLength, float threshold)
        {
            var buffer = audioBuffer as byte[] ?? audioBuffer.ToArray();
            var audioLevelsFft = CalculateAudioLevelsFFT(buffer, fftLength);

            if (IsSilence(audioLevelsFft, threshold))
            {
                return CreateLowLevelArray(audioLevelsFft.Length);
            }

            var firstAudioLevelsHalf = new double[audioLevelsFft.Length / 2];
            var secondAudioLevelsHalf = new double[audioLevelsFft.Length / 2];
            Array.Copy(audioLevelsFft.ToArray(), 0, firstAudioLevelsHalf, 0, audioLevelsFft.Length / 2);
            Array.Copy(audioLevelsFft.ToArray(), audioLevelsFft.Length / 2, secondAudioLevelsHalf, 0,
                audioLevelsFft.Length / 2);

            Array.Sort(firstAudioLevelsHalf);
            Array.Sort(secondAudioLevelsHalf);

            Array.Reverse(secondAudioLevelsHalf);

            var newAudioLevels = firstAudioLevelsHalf.Concat(secondAudioLevelsHalf).ToArray();
            var random = new Random();
            const double pullUpAudioLevel = 0.6;
            const double pullDownAudioLevel = 0.8;
            const double maxGainFactor = 3;
            for (var i = 0; i < newAudioLevels.Length; i++)
            {
                switch (newAudioLevels[i])
                {
                    case < pullUpAudioLevel:
                        {
                            var difference = pullUpAudioLevel - newAudioLevels[i];

                            var newAudioLevel = newAudioLevels[i] + difference * random.NextDouble();
                            if (newAudioLevel > newAudioLevels[i] * maxGainFactor)
                                newAudioLevel = newAudioLevels[i] * maxGainFactor;
                            newAudioLevels[i] = newAudioLevel;
                            break;
                        }
                    case > pullDownAudioLevel:
                        {
                            var difference = (newAudioLevels[i] - pullUpAudioLevel) * newAudioLevels[i]; // The distance from the middle
                            var newAudioLevel = newAudioLevels[i] - difference;
                            newAudioLevels[i] = newAudioLevel;
                            break;
                        }
                }
            }

            return newAudioLevels.Select(level => level + additionalRandomNoise * random.NextDouble()).ToArray();
        }

        public static double[] CreateLowLevelArray(int length)
        {
            var zeroList = new List<double>();
            for (var i = 0; i < length; i++)
            {
                zeroList.Add(0.02);
            }
            return [.. zeroList];
        }

        // ReSharper disable once InconsistentNaming
        private static double[] CalculateAudioLevelsFFT(IEnumerable<byte> audioBuffer, int fftLength)
        {
            var samples = audioBuffer.Select(b => new Complex((b - 128) / 128.0, 0)).ToArray();

            // Pad the array with zeros to make its length a power of two
            var paddedLength = BitOperations.RoundUpToPowerOf2((uint)samples.Length);
            Array.Resize(ref samples, (int)paddedLength);

            // Perform FFT
            Fourier.Forward(samples, FourierOptions.NoScaling);

            // Calculate magnitudes of the FFT result
            var magnitudes = samples.Take(fftLength / 2).Select(c => c.Magnitude).ToArray();

            if (magnitudes.Length == 0)
            {
                return CreateLowLevelArray((int)paddedLength);
            }

            // Normalize magnitudes
            var maxMagnitude = magnitudes.Max();
            return magnitudes.Select(m => m / maxMagnitude).ToArray();
        }

        private static float CalculateRms(double[] audioLevels)
        {
            var samples = new double[audioLevels.Length];
            Array.Copy(audioLevels, samples, audioLevels.Length);

            var sumOfSquares = samples.Sum(sample => Math.Pow(sample, 2));

            return (float)(Math.Sqrt(sumOfSquares / samples.Length));
        }

        private static bool IsSilence(double[] audioLevels, float threshold)
        {
            var rms = CalculateRms(audioLevels);
            return rms < threshold;
        }
    }
}
