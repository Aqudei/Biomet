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
    class FingerRegistrationViewModel : CaptureFingerViewModel
    {
        private Enrollment Enroller;

        public FingerRegistrationViewModel() : base()
        {

        }

        protected override void Init()
        {
            base.Init();
            Enroller = new DPFP.Processing.Enrollment();
            UpdateStatus();
        }

        protected override void Process(Sample Sample)
        {
            base.Process(Sample);

            DPFP.FeatureSet features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Enrollment);
            // Check quality of the sample and add to enroller if it's good
            if (features != null)
                try
                {
                    MakeReport("The fingerprint feature set was created.");
                    Enroller.AddFeatures(features);     // Add feature set to template.
                }
                finally
                {
                    UpdateStatus();
                    switch (Enroller.TemplateStatus)
                    {
                        case DPFP.Processing.Enrollment.Status.Ready:   // report success and stop capturing
                            SaveTemplate(Enroller.Template);
                            TryClose();
                            break;

                        case DPFP.Processing.Enrollment.Status.Failed:  // report failure and restart capturing
                            Enroller.Clear();
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
            using (var mem = new MemoryStream())
            {
                template.Serialize(mem);
                File.WriteAllBytes(Employee.EmployeeNumber, mem.ToArray());
            }
        }

        private void UpdateStatus()
        {
            SetStatus(String.Format("Fingerprint samples needed: {0}", Enroller.FeaturesNeeded));
        }

        public Employee Employee { get; internal set; }
    }
}
