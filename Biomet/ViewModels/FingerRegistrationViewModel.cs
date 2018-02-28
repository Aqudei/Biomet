using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biomet.Models.Entities;
using DPFP;
using DPFP.Capture;
using DPFP.Processing;

namespace Biomet.ViewModels
{
    public class FingerRegistrationViewModel : CaptureFingerViewModel
    {
        private Enrollment _enroller;

        protected override void Init()
        {
            base.Init();
            _enroller = new Enrollment();
            UpdateStatus();
        }

        protected override void Process(Sample sample)
        {
            base.Process(sample);

            var features = ExtractFeatures(sample, DPFP.Processing.DataPurpose.Enrollment);
            // Check quality of the sample and add to enroller if it's good
            if (features == null) return;

            try
            {
                MakeReport("The fingerprint feature set was created.");
                _enroller.AddFeatures(features);     // Add feature set to template.
            }
            finally
            {
                UpdateStatus();
                switch (_enroller.TemplateStatus)
                {
                    case Enrollment.Status.Ready:   // report success and stop capturing
                        SaveTemplate(_enroller.Template);
                        TryClose();
                        break;

                    case Enrollment.Status.Failed:  // report failure and restart capturing
                        _enroller.Clear();
                        Stop();
                        UpdateStatus();
                        //OnTemplate(null);
                        Start();
                        break;
                }
            }

        }

        private void SaveTemplate(Template template)
        {
            if (template == null)
                throw new ArgumentNullException(nameof(template));

            using (var mem = new MemoryStream())
            {
                template.Serialize(mem);
                File.WriteAllBytes(Employee.EmployeeNumber, mem.ToArray());
            }
        }

        private void UpdateStatus()
        {
            SetStatus($"Fingerprint samples needed: {_enroller.FeaturesNeeded}");
        }

        public Employee Employee { get; internal set; }
    }
}
