using Caliburn.Micro;
using DPFP;
using DPFP.Capture;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Biomet.ViewModels
{
    public class CaptureFingerViewModel : Screen, DPFP.Capture.EventHandler
    {
        private Capture _capturer;
        private string _report;
        private string _statusText;

        public string StatusText
        {
            get => _statusText;
            set => Set(ref _statusText, value);
        }

        public CaptureFingerViewModel()
        {
            Deactivated += CaptureFingerViewModel_Deactivated;
            Activated += CaptureFingerViewModel_Activated;
        }

        private void CaptureFingerViewModel_Activated(object sender, ActivationEventArgs e)
        {
            Init();
            Start();
        }

        private void CaptureFingerViewModel_Deactivated(object sender, DeactivationEventArgs e)
        {
            Stop();
        }

        public string Report
        {
            get => _report;
            private set => Set(ref _report, value);
        }

        protected virtual void Init()
        {
            try
            {
                _capturer = new Capture();				// Create a capture operation.

                if (null != _capturer)
                    _capturer.EventHandler = this;					// Subscribe for capturing events.
                else
                    SetPrompt("Can't initiate capture operation!");
            }
            catch
            {
                MessageBox.Show("Can't initiate capture operation!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected virtual void Process(DPFP.Sample Sample)
        {
            // Draw fingerprint sample image.
            DrawPicture(ConvertSampleToBitmap(Sample));
        }

        protected Bitmap ConvertSampleToBitmap(DPFP.Sample Sample)
        {
            SampleConversion Convertor = new DPFP.Capture.SampleConversion();  // Create a sample convertor.
            Bitmap bitmap = null;                                                           // TODO: the size doesn't matter
            Convertor.ConvertToPicture(Sample, ref bitmap);                                 // TODO: return bitmap as a result
            return bitmap;
        }

        private void DrawPicture(Bitmap bitmap)
        {
            OnUIThread(() =>
            {
                FPImageSource = Helpers.BitmapConverter.Convert(bitmap);
            });
        }

        private ImageSource _fpImageSource;

        public ImageSource FPImageSource
        {
            get => _fpImageSource;
            set => Set(ref _fpImageSource, value);
        }


        protected void Start()
        {
            if (null == _capturer)
                return;

            try
            {
                _capturer.StartCapture();
                SetPrompt("Using the fingerprint reader, scan your fingerprint.");
            }
            catch
            {
                SetPrompt("Can't initiate capture!");
            }
        }

        private void SetPrompt(string v)
        {
            Debug.WriteLine("promt: " + v);
        }

        protected void Stop()
        {
            if (null == _capturer)
                return;

            try
            {
                _capturer.StopCapture();
            }
            catch
            {
                SetStatus("Can't terminate capture!");
            }
        }

        public void OnComplete(object Capture, string ReaderSerialNumber, Sample Sample)
        {
            MakeReport("The fingerprint sample was captured.");
            SetPrompt("Scan the same fingerprint again.");
            Process(Sample);
        }

        protected void MakeReport(string v)
        {
            Report += DateTime.Now.ToString(CultureInfo.InvariantCulture) + " - " + v + "\n\n";
        }

        public void OnFingerGone(object capture, string readerSerialNumber)
        {
            MakeReport("The finger was removed from the fingerprint reader.");
        }

        public void OnFingerTouch(object Capture, string ReaderSerialNumber)
        {
            MakeReport("The fingerprint reader was touched.");
        }

        public void OnReaderConnect(object Capture, string ReaderSerialNumber)
        {
            MakeReport("The fingerprint reader was connected.");
        }

        public void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {
            MakeReport("The fingerprint reader was disconnected.");
        }

        public void OnSampleQuality(object Capture, string ReaderSerialNumber, DPFP.Capture.CaptureFeedback CaptureFeedback)
        {
            if (CaptureFeedback == DPFP.Capture.CaptureFeedback.Good)
                MakeReport("The quality of the fingerprint sample is good.");
            else
                MakeReport("The quality of the fingerprint sample is poor.");
        }

        protected DPFP.FeatureSet ExtractFeatures(DPFP.Sample Sample, DPFP.Processing.DataPurpose Purpose)
        {
            DPFP.Processing.FeatureExtraction Extractor = new DPFP.Processing.FeatureExtraction();  // Create a feature extractor
            DPFP.Capture.CaptureFeedback feedback = DPFP.Capture.CaptureFeedback.None;
            DPFP.FeatureSet features = new DPFP.FeatureSet();
            Extractor.CreateFeatureSet(Sample, Purpose, ref feedback, ref features);            // TODO: return features as a result?
            if (feedback == DPFP.Capture.CaptureFeedback.Good)
                return features;
            else
                return null;
        }

        protected void SetStatus(string status)
        {
            StatusText = status;
        }

    }
}
