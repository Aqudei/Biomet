using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DPFP;
using DPFP.Verification;

namespace Biomet.ViewModels
{
    public class DTRViewModel : CaptureFingerViewModel
    {
        private Verification Verificator;

        protected override void Init()
        {
            base.Init();
            Verificator = new DPFP.Verification.Verification();

            LoadTemplates();
        }

        private Dictionary<string, Template> _templates = new Dictionary<string, Template>();

        private void LoadTemplates()
        {
            var files = Directory.GetFiles(Properties.Settings.Default.FPTEMPLATE_DIR);
            foreach (var f in files)
            {
                var templateBytes = File.ReadAllBytes(f);
                using (var mem = new MemoryStream(templateBytes))
                {
                    var template = new Template();
                    template.DeSerialize(mem);
                    _templates.Add(Path.GetFileNameWithoutExtension(f), template);
                }

            }
        }

        protected override void Process(Sample Sample)
        {
            base.Process(Sample);
            var features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Verification);
            if (features == null) return;

            var result = new DPFP.Verification.Verification.Result();
            foreach (var storedTemplate in _templates)
            {
                Verificator.Verify(features, storedTemplate.Value, ref result);
                UpdateStatus(result.FARAchieved);

                if (result.Verified)
                {
                    MessageBox.Show("[Temporary only for debug]\nYou are " + storedTemplate.Key);
                    return;
                }
            }
        }

        private void UpdateStatus(int fARAchieved)
        {
            Debug.WriteLine("Far Achieved: " + fARAchieved);
        }
    }
}
