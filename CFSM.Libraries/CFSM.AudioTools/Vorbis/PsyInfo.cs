namespace CFSM.AudioTools.Vorbis
{
    // psychoacoustic setup
    internal class PsyInfo
    {
        //int    athp;
        //int    decayp;
        //int    smoothp;
        //int    noisefitp;
        //int    noisefit_subblock;
        //float noisefit_threshdB;

        //float ath_att;

        //int tonemaskp;
        private float[] toneatt_125Hz = new float[5];
        private float[] toneatt_250Hz = new float[5];
        private float[] toneatt_500Hz = new float[5];
        private float[] toneatt_1000Hz = new float[5];
        private float[] toneatt_2000Hz = new float[5];
        private float[] toneatt_4000Hz = new float[5];
        private float[] toneatt_8000Hz = new float[5];

        //int peakattp;
        private float[] peakatt_125Hz = new float[5];
        private float[] peakatt_250Hz = new float[5];
        private float[] peakatt_500Hz = new float[5];
        private float[] peakatt_1000Hz = new float[5];
        private float[] peakatt_2000Hz = new float[5];
        private float[] peakatt_4000Hz = new float[5];
        private float[] peakatt_8000Hz = new float[5];

        //int noisemaskp;
        private float[] noiseatt_125Hz = new float[5];
        private float[] noiseatt_250Hz = new float[5];
        private float[] noiseatt_500Hz = new float[5];
        private float[] noiseatt_1000Hz = new float[5];
        private float[] noiseatt_2000Hz = new float[5];
        private float[] noiseatt_4000Hz = new float[5];
        private float[] noiseatt_8000Hz = new float[5];

        //float max_curve_dB;

        //float attack_coeff;
        //float decay_coeff;

        internal void free()
        {
        }
    }
}